using UnityEngine;
using UnityEngine.InputSystem;

namespace PointClear.Player
{
    /// <summary>
    /// Prototype player movement and mouse-aim rotation. WASD drives
    /// translation; the mouse cursor projected onto the ground plane drives
    /// rotation. The two are intentionally independent of each other.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 6f;

        [SerializeField]
        private float rotationSpeed = 12f;

        public Vector3 AimWorldPoint { get; private set; }

        private Rigidbody rb;
        private PlayerStats stats;
        private Camera mainCamera;
        private InputAction moveAction;
        private InputAction aimAction;
        private Vector2 moveInput;
        private bool hasAimPoint;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            mainCamera = Camera.main;

            stats = GetComponent<PlayerStats>();
            stats.SetBase(StatType.MoveSpeed, moveSpeed);
        }

        private void OnEnable()
        {
            moveAction = InputSystem.actions.FindAction("Move");
            aimAction = InputSystem.actions.FindAction("Aim");
        }

        private void Update()
        {
            moveInput = moveAction.ReadValue<Vector2>();
            UpdateAimPoint();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            ApplyRotation();
        }

        private void UpdateAimPoint()
        {
            if (mainCamera == null || aimAction == null)
            {
                return;
            }

            Vector2 mouseScreenPosition = aimAction.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

            if (groundPlane.Raycast(ray, out float distance))
            {
                AimWorldPoint = ray.GetPoint(distance);
                hasAimPoint = true;
            }
        }

        private void ApplyMovement()
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }

            float speed = stats.GetValue(StatType.MoveSpeed);
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }

        private void ApplyRotation()
        {
            if (!hasAimPoint)
            {
                return;
            }

            Vector3 lookDirection = AimWorldPoint - rb.position;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude < 0.0001f)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

        private void OnDrawGizmos()
        {
            if (!hasAimPoint)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(AimWorldPoint, 0.2f);
        }
    }
}
