using UnityEngine;

namespace PointClear.Benchmark
{
    /// <summary>
    /// TEMPORARY / THROWAWAY — Visual Identity Benchmark only.
    /// Drives the temporary Hollowed Thrall's Idle/Locomotion switch from how its
    /// root is actually moving, and adds cheap per-instance horde variation so a wave
    /// of spawned Thralls does not march in perfect lockstep.
    ///
    /// Read-only: it never touches EnemyAI movement, attack, health, physics, or any
    /// gameplay component. It only reads the root's world position (to detect motion)
    /// and writes to this visual's Animator (an IsMoving bool + a per-instance speed
    /// and start phase). Root motion stays off. Not shipped — this is a stand-in for
    /// the permanent locomotion system.
    /// </summary>
    [DisallowMultipleComponent]
    public class BenchmarkThrallAnimatorDriver : MonoBehaviour
    {
        [Tooltip("Animator to drive. Defaults to the Animator on this object.")]
        public Animator animator;

        [Tooltip("Transform whose movement is measured. Defaults to the parent (the enemy root).")]
        public Transform source;

        [Tooltip("Base Animator playback speed — the shamble cadence tuned against the enemy's move speed. Per-instance jitter is applied on top.")]
        [SerializeField] private float baseAnimatorSpeed = 1f;

        [Tooltip("Per-instance Animator speed variation (fraction). 0.10 = +/-10%.")]
        [SerializeField] private float speedJitter = 0.10f;

        [Tooltip("Root speed (units/sec) above which the Thrall is considered moving.")]
        [SerializeField] private float enterMoveSpeed = 0.08f;

        [Tooltip("Root speed (units/sec) below which the Thrall is considered stopped. Lower than enter => hysteresis.")]
        [SerializeField] private float exitMoveSpeed = 0.03f;

        private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
        private static readonly int LocomotionState = Animator.StringToHash("Locomotion");

        private Vector3 lastPos;
        private bool moving = true;

        private void Start()
        {
            if (animator == null) animator = GetComponent<Animator>();
            if (source == null) source = transform.parent != null ? transform.parent : transform;
            lastPos = source != null ? source.position : transform.position;

            if (animator != null)
            {
                // Horde variation: per-instance speed jitter + a randomized start phase
                // so simultaneously-spawned Thralls do not animate in unison.
                float j = 1f + Random.Range(-speedJitter, speedJitter);
                animator.speed = Mathf.Max(0.01f, baseAnimatorSpeed * j);
                animator.SetBool(IsMovingId, true);
                // Offset the locomotion cycle by a random phase and force-evaluate so the
                // offset actually lands (Play alone in Start is applied on the next update,
                // which can clobber it). Falls back silently if the state is absent.
                animator.Play(LocomotionState, 0, Random.value);
                animator.Update(0f);
            }
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

            if (animator != null) animator.SetBool(IsMovingId, moving);
        }
    }
}
