using System.Collections;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// Simple placeholder enemy AI: chases the player in a straight line
    /// blended with local separation from nearby enemies, stops at attack
    /// range, and deals timed melee damage. No pathfinding — not required
    /// by this prototype arena, and separation alone already prevents the
    /// "enemy train" problem a pure seek-only chase produces.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 3.5f;

        [SerializeField]
        private float attackRange = 2f;

        [SerializeField]
        private float attackDamage = 10f;

        [SerializeField]
        private float attackInterval = 1f;

        [SerializeField]
        private Transform player;

        [SerializeField]
        private float hitFlashDuration = 0.1f;

        [SerializeField]
        private float attackPulseDuration = 0.15f;

        [Header("Separation")]
        [SerializeField]
        private float separationRadius = 1.75f;

        [SerializeField]
        private float separationWeight = 1.5f;

        [SerializeField]
        private float seekWeight = 1f;

        // Sprint 1.3 buffer validation: the original 8-slot buffer truncates
        // under realistic dense local crowding. Two controlled cluster tests
        // (a pathological tight pack, and a physically-plausible
        // collider-touching hex pack — both centered on one subject enemy)
        // each found ~20 true neighbors within separationRadius once the
        // capsule collider's own radius is accounted for (OverlapSphere
        // tests collider volume, not just transform position, so the
        // effective query reach is separationRadius + colliderRadius).
        // 32 gives comfortable headroom above the ~20 observed without being
        // an arbitrarily large allocation — see PC-003 Sprint 1.3 report.
        private static readonly Collider[] SeparationBuffer = new Collider[32];

        /// <summary>
        /// Authoritative active-enemy count. Owned entirely by this
        /// component's lifecycle (OnEnable/OnDisable), not by the spawner
        /// and not by the Health.Died event — this is what keeps the count
        /// correct regardless of *why* an enemy becomes inactive (death,
        /// external Destroy(), scene unload, or manual SetActive(false)).
        /// Reset via RuntimeInitializeOnLoadMethod rather than relying on
        /// domain-reload-on-play, so a stale count can't survive into a new
        /// Play Mode session even if that Editor setting is ever disabled.
        /// </summary>
        public static int ActiveCount { get; private set; }

        public static int PeakActiveCount { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetActiveCount()
        {
            ActiveCount = 0;
            PeakActiveCount = 0;
        }

        private Health health;
        private Rigidbody rb;
        private Renderer bodyRenderer;
        private Vector3 baseScale;
        private float nextAttackTime;
        private Coroutine reactionRoutine;

        private void Awake()
        {
            health = GetComponent<Health>();
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            bodyRenderer = GetComponentInChildren<Renderer>();
            baseScale = transform.localScale;
        }

        private void OnEnable()
        {
            health.Died += HandleDeath;
            health.Damaged += HandleDamaged;

            ActiveCount++;
            if (ActiveCount > PeakActiveCount)
            {
                PeakActiveCount = ActiveCount;
            }
        }

        private void OnDisable()
        {
            health.Died -= HandleDeath;
            health.Damaged -= HandleDamaged;

            ActiveCount = Mathf.Max(0, ActiveCount - 1);
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
            float distanceToPlayer = toPlayer.magnitude;

            if (distanceToPlayer > attackRange)
            {
                Vector3 seekDirection = toPlayer.normalized;
                Vector3 separationDirection = ComputeSeparation();
                Vector3 moveDirection = seekDirection * seekWeight + separationDirection * separationWeight;

                if (moveDirection.sqrMagnitude > 0.0001f)
                {
                    moveDirection.Normalize();
                }
                else
                {
                    moveDirection = seekDirection;
                }

                float step = moveSpeed * Time.fixedDeltaTime;
                Vector3 intendedPosition = rb.position + moveDirection * step;

                // Separation can steer the enemy off the direct line to the
                // player, so clamp against the *resulting* distance rather
                // than the step size — this still guarantees the enemy can
                // never end a step closer than attackRange to the player,
                // regardless of how much separation bent the direction.
                Vector3 toPlayerFromIntended = intendedPosition - player.position;
                toPlayerFromIntended.y = 0f;
                float intendedDistance = toPlayerFromIntended.magnitude;

                if (intendedDistance < attackRange && intendedDistance > 0.0001f)
                {
                    Vector3 clampDirection = toPlayerFromIntended / intendedDistance;
                    intendedPosition = player.position + clampDirection * attackRange;
                    intendedPosition.y = rb.position.y;
                }

                rb.MovePosition(intendedPosition);
                rb.MoveRotation(Quaternion.LookRotation(moveDirection));
            }
            else if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackInterval;
            }
        }

        /// <summary>
        /// Boids-style separation: push away from nearby enemies, weighted
        /// by inverse distance so closer neighbors push harder. The
        /// Physics.OverlapSphereNonAlloc query is spatially bounded to
        /// separationRadius, so cost scales with local crowd density, not
        /// total enemy count — cheap even with many enemies on screen, and
        /// needs no pathfinding graph.
        /// </summary>
        private Vector3 ComputeSeparation()
        {
            // QueryTriggerInteraction.Ignore: Sprint 2.1 introduces trigger-collider
            // XP pickups into the arena; without this, pickups near an enemy would
            // consume slots in this fixed-size buffer alongside real neighbors.
            int count = Physics.OverlapSphereNonAlloc(
                rb.position,
                separationRadius,
                SeparationBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);
            Vector3 separation = Vector3.zero;
            int neighborCount = 0;

            for (int i = 0; i < count; i++)
            {
                Collider other = SeparationBuffer[i];
                if (other == null || other.attachedRigidbody == rb)
                {
                    continue;
                }

                if (!other.TryGetComponent(out EnemyAI otherEnemy) || otherEnemy == this)
                {
                    continue;
                }

                Vector3 offset = rb.position - other.attachedRigidbody.position;
                offset.y = 0f;
                float distance = offset.magnitude;
                if (distance < 0.0001f)
                {
                    continue;
                }

                separation += offset.normalized / distance;
                neighborCount++;
            }

            if (neighborCount > 0)
            {
                separation /= neighborCount;
            }

            return separation;
        }

        private void AttackPlayer()
        {
            if (player.TryGetComponent(out Health playerHealth))
            {
                playerHealth.TakeDamage(attackDamage);
            }

            PlayReaction(attackPulseDuration, 1.15f, Color.yellow);
        }

        private void HandleDamaged(float amount)
        {
            PlayReaction(hitFlashDuration, 1.2f, Color.white);
        }

        private void HandleDeath()
        {
            Destroy(gameObject);
        }

        private void PlayReaction(float duration, float scaleMultiplier, Color flashColor)
        {
            if (reactionRoutine != null)
            {
                StopCoroutine(reactionRoutine);
                transform.localScale = baseScale;
            }

            reactionRoutine = StartCoroutine(ReactionRoutine(duration, scaleMultiplier, flashColor));
        }

        private IEnumerator ReactionRoutine(float duration, float scaleMultiplier, Color flashColor)
        {
            MaterialPropertyBlock block = null;
            Color originalColor = Color.white;
            bool hasColor = false;

            if (bodyRenderer != null)
            {
                block = new MaterialPropertyBlock();
                bodyRenderer.GetPropertyBlock(block);
                hasColor = block.isEmpty == false;
                originalColor = block.GetColor("_BaseColor");
                block.SetColor("_BaseColor", flashColor);
                bodyRenderer.SetPropertyBlock(block);
            }

            transform.localScale = baseScale * scaleMultiplier;

            yield return new WaitForSeconds(duration);

            transform.localScale = baseScale;

            if (bodyRenderer != null)
            {
                block.SetColor("_BaseColor", hasColor ? originalColor : Color.white);
                bodyRenderer.SetPropertyBlock(block);
            }

            reactionRoutine = null;
        }
    }
}
