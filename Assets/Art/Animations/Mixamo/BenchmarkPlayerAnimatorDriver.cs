using UnityEngine;

namespace PointClear.Benchmark
{
    /// <summary>
    /// TEMPORARY / THROWAWAY — Visual Identity Benchmark only.
    /// Drives a 2D directional locomotion blend tree (Idle + Forward/Back/Strafe)
    /// from how the player root is actually moving, expressed RELATIVE to the
    /// player's facing (which PlayerController keeps pointed at the aim/mouse).
    ///
    /// Read-only: never touches movement, input, aiming, physics, or any gameplay
    /// component. Sampling runs in FixedUpdate to match physics movement
    /// (MovePosition, interpolation off), with a hysteresis dead-zone for a stable
    /// Idle/Locomotion switch and SmoothDamp on the blend direction so direction
    /// changes ease instead of snapping. Root motion stays off. Not shipped.
    /// </summary>
    [DisallowMultipleComponent]
    public class BenchmarkPlayerAnimatorDriver : MonoBehaviour
    {
        [Tooltip("Animator to drive. Defaults to the Animator on this object.")]
        public Animator animator;

        [Tooltip("Transform whose movement + facing are measured. Defaults to the parent (the player root).")]
        public Transform source;

        [Tooltip("Speed (units/sec) above which the player is considered moving.")]
        [SerializeField] private float enterMoveSpeed = 0.5f;

        [Tooltip("Speed (units/sec) below which the player is considered stopped. Lower than enter => hysteresis.")]
        [SerializeField] private float exitMoveSpeed = 0.15f;

        [Tooltip("Ease time (sec) for MoveX/MoveY to follow the movement direction. Higher = smoother, less snappy.")]
        [SerializeField] private float directionSmoothTime = 0.12f;

        private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
        private static readonly int MoveXId = Animator.StringToHash("MoveX");
        private static readonly int MoveYId = Animator.StringToHash("MoveY");

        private Vector3 lastPos;
        private bool moving;
        private Vector2 blendDir;   // smoothed local move direction (x = strafe, y = forward/back)
        private Vector2 blendVel;   // SmoothDamp velocity ref

        private void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (source == null) source = transform.parent != null ? transform.parent : transform;
            lastPos = source != null ? source.position : transform.position;
        }

        private void FixedUpdate()
        {
            if (source == null) return;

            Vector3 delta = source.position - lastPos;
            delta.y = 0f;
            lastPos = source.position;

            float dt = Time.fixedDeltaTime;
            float speed = dt > 0f ? delta.magnitude / dt : 0f;

            if (!moving && speed > enterMoveSpeed) moving = true;
            else if (moving && speed < exitMoveSpeed) moving = false;

            Vector2 target = Vector2.zero;
            if (moving && delta.sqrMagnitude > 1e-8f)
            {
                // World move direction -> player's local space (relative to aim facing).
                Vector3 local = source.InverseTransformDirection(delta.normalized);
                target = new Vector2(local.x, local.z); // x = strafe, y = forward/back
                if (target.sqrMagnitude > 1f) target.Normalize();
            }

            blendDir = Vector2.SmoothDamp(blendDir, target, ref blendVel, directionSmoothTime, Mathf.Infinity, dt);
            if (blendDir.sqrMagnitude > 1f) blendDir = blendDir.normalized; // clamp to the unit disk

            if (animator != null)
            {
                animator.SetBool(IsMovingId, moving);
                animator.SetFloat(MoveXId, blendDir.x);
                animator.SetFloat(MoveYId, blendDir.y);
            }
        }
    }
}
