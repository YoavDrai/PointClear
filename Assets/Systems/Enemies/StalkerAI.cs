using System.Collections;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// Sprint 2.11 — the Stalker. A distance-keeping harasser that asks a
    /// different question than the chaser (EnemyAI): "reach out and finish what
    /// won't come to your center." It holds a preferred distance band from the
    /// player — retreating when the player is inside it, approaching when far,
    /// holding otherwise — so it never enters the player's self-centered
    /// Detonation Field, and standing-and-waiting never finishes it.
    ///
    /// Governing rule (Game Director, PC-014): it may deny PASSIVE closure, never
    /// COMMITTED closure. Its move speed is deliberately clamped BELOW the player's
    /// (player base 6; this ≈ 4.5), so a player who commits to closing the distance
    /// always runs it down — it kites, it never escapes. It gets cornered against
    /// arena geometry via the same kinematic sweep-stop the chaser uses.
    ///
    /// Deliberately NOT the chaser: separate component so EnemyAI is untouched.
    /// This sprint has no cover/LoS-breaking and no ranged attack (its teeth are
    /// incompleteness — it counts toward the Operation quota, so a run cannot close
    /// while it is alive). Local steering only, no pathfinding.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class StalkerAI : MonoBehaviour
    {
        [Tooltip("Must stay BELOW the player's move speed (player base = 6) so a committed player can always close — the committed-closure rule (PC-014).")]
        [SerializeField]
        private float moveSpeed = 4.5f;

        [Header("Distance band")]
        [Tooltip("Preferred standoff distance. Kept above the Detonation Field mark radius (~5) so the Stalker never enters the player's self-centered field.")]
        [SerializeField]
        private float preferredDistance = 8f;

        [Tooltip("Retreat when the player is closer than this.")]
        [SerializeField]
        private float innerBand = 6.5f;

        [Tooltip("Approach when the player is farther than this. Between inner and outer, hold position.")]
        [SerializeField]
        private float outerBand = 9.5f;

        [Header("Separation (spread)")]
        [SerializeField]
        private float separationRadius = 2.5f;

        [SerializeField]
        private float separationWeight = 1.5f;

        [SerializeField]
        private float seekWeight = 1f;

        [Header("Obstacle blocking")]
        [SerializeField]
        private float obstacleSkin = 0.2f;

        [Header("Feedback")]
        [Tooltip("Distinct body tint so the Stalker reads instantly as a different kind of enemy.")]
        [SerializeField]
        private Color bodyColor = new Color(0.65f, 0.2f, 0.95f); // violet = "different"

        [SerializeField]
        private float hitFlashDuration = 0.1f;

        [SerializeField]
        private Transform player;

        private static readonly Collider[] SeparationBuffer = new Collider[32];
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private Health health;
        private Rigidbody rb;
        private Renderer bodyRenderer;
        private Coroutine flashRoutine;
        private bool subscribed;

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bodyRenderer = GetComponentInChildren<Renderer>();
            ApplyBodyTint();
        }

        private void OnEnable()
        {
            if (!subscribed)
            {
                health.Died += HandleDeath;
                health.Damaged += HandleDamaged;
                subscribed = true;
            }
        }

        private void OnDisable()
        {
            if (subscribed)
            {
                health.Died -= HandleDeath;
                health.Damaged -= HandleDamaged;
                subscribed = false;
            }
        }

        private void FixedUpdate()
        {
            if (player == null)
            {
                player = PlayerReference.Instance;
            }

            if (player == null || health.IsDead)
            {
                return;
            }

            Vector3 toPlayer = player.position - rb.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;
            Vector3 toPlayerDir = distance > 0.0001f ? toPlayer / distance : Vector3.zero;

            // Distance-band intent: retreat inside the band, approach outside it,
            // hold within it. Holding (not fleeing) is what keeps committed closure
            // reliable — a player who shoots or walks up always resolves it.
            Vector3 bandDirection;
            if (distance < innerBand)
            {
                bandDirection = -toPlayerDir; // retreat
            }
            else if (distance > outerBand)
            {
                bandDirection = toPlayerDir;  // approach
            }
            else
            {
                bandDirection = Vector3.zero; // hold
            }

            Vector3 separation = ComputeSeparation();
            Vector3 move = bandDirection * seekWeight + separation * separationWeight;

            // Always face the player (reads as "watching you", and keeps aim honest).
            if (toPlayerDir.sqrMagnitude > 0.0001f)
            {
                rb.MoveRotation(Quaternion.LookRotation(toPlayerDir));
            }

            if (move.sqrMagnitude < 0.0001f)
            {
                return; // holding in band
            }

            move.Normalize();
            float step = moveSpeed * Time.fixedDeltaTime;
            Vector3 intended = rb.position + move * step;
            intended = BlockAgainstObstacles(intended, move);
            rb.MovePosition(intended);
        }

        // Spread from other Stalkers so they ring the player rather than stacking.
        private Vector3 ComputeSeparation()
        {
            int count = Physics.OverlapSphereNonAlloc(
                rb.position,
                separationRadius,
                SeparationBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            Vector3 separation = Vector3.zero;
            int neighbors = 0;

            for (int i = 0; i < count; i++)
            {
                Collider other = SeparationBuffer[i];
                if (other == null || other.attachedRigidbody == rb)
                {
                    continue;
                }

                if (!other.TryGetComponent(out StalkerAI otherStalker) || otherStalker == this)
                {
                    continue;
                }

                Vector3 offset = rb.position - other.attachedRigidbody.position;
                offset.y = 0f;
                float d = offset.magnitude;
                if (d < 0.0001f)
                {
                    continue;
                }

                separation += offset.normalized / d;
                neighbors++;
            }

            if (neighbors > 0)
            {
                separation /= neighbors;
            }

            return separation;
        }

        // Kinematic MovePosition is not stopped by static colliders, so — exactly
        // like the chaser — sweep the collider and stop short of real obstacles.
        // This is what lets a retreating Stalker get CORNERED against arena walls,
        // guaranteeing committed closure. Enemies, the player, and floors are ignored.
        private Vector3 BlockAgainstObstacles(Vector3 intendedPosition, Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                return intendedPosition;
            }

            direction.Normalize();
            float moveDistance = Vector3.Distance(rb.position, intendedPosition);
            if (moveDistance <= 0.0001f)
            {
                return intendedPosition;
            }

            if (!rb.SweepTest(direction, out RaycastHit hit, moveDistance, QueryTriggerInteraction.Ignore))
            {
                return intendedPosition;
            }

            if (hit.collider == null
                || hit.collider.GetComponentInParent<StalkerAI>() != null
                || hit.collider.GetComponentInParent<EnemyAI>() != null
                || hit.collider.GetComponentInParent<PlayerController>() != null
                || hit.normal.y > 0.5f)
            {
                return intendedPosition;
            }

            float allowed = Mathf.Max(0f, hit.distance - obstacleSkin);
            Vector3 blocked = rb.position + direction * allowed;
            blocked.y = rb.position.y;
            return blocked;
        }

        private void ApplyBodyTint()
        {
            if (bodyRenderer == null)
            {
                return;
            }

            MaterialPropertyBlock block = new MaterialPropertyBlock();
            bodyRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, bodyColor);
            bodyRenderer.SetPropertyBlock(block);
        }

        // Brief white flash on damage — the only "juice" here, and it earns its
        // place: with no audio, it is the player's confirmation that a committed
        // hit is landing.
        private void HandleDamaged(float amount)
        {
            if (bodyRenderer == null)
            {
                return;
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            bodyRenderer.GetPropertyBlock(block);
            block.SetColor(BaseColorId, Color.white);
            bodyRenderer.SetPropertyBlock(block);

            yield return new WaitForSeconds(hitFlashDuration);

            block.SetColor(BaseColorId, bodyColor);
            bodyRenderer.SetPropertyBlock(block);
            flashRoutine = null;
        }

        private void HandleDeath()
        {
            Destroy(gameObject);
        }
    }
}
