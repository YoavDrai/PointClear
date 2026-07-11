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

        [Header("Obstacle Avoidance")]
        // Gap kept between the enemy and an obstacle when blocked head-on.
        // Deliberately not tiny: the clearance is what lets the enemy slide
        // laterally along a wall it is pressed against without its side probe
        // grazing that same wall and reporting "blocked" (which would jam it).
        [SerializeField]
        private float obstacleSkin = 0.2f;

        // How far ahead the enemy checks for a blocking obstacle before it
        // decides the direct path is blocked and begins steering around it.
        // Larger than one step so avoidance starts before the enemy jams into
        // the wall, producing a smooth slide rather than a stop-then-turn.
        [SerializeField]
        private float avoidLookAhead = 1.6f;

        // If the enemy makes no real progress for this long while blocked, it
        // flips its avoidance side to escape a local dead-end (e.g. it committed
        // to the longer way around a wall).
        [SerializeField]
        private float stuckTimeout = 1.5f;

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

        // Obstacle-avoidance state.
        private bool avoiding;                 // currently steering around an obstacle
        private float avoidanceSign = 1f;      // +1 / -1 = which way along the wall
        private float lastProgressTime;        // last time real movement was made
        private Vector3 lastStuckCheckPosition; // position at that last-progress mark

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

            avoiding = false;
            lastProgressTime = Time.time;
            lastStuckCheckPosition = rb.position;

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

                Vector3 desiredDirection = seekDirection * seekWeight + separationDirection * separationWeight;
                desiredDirection = desiredDirection.sqrMagnitude > 0.0001f ? desiredDirection.normalized : seekDirection;

                UpdateStuckRecovery();
                Vector3 moveDirection = ResolveMoveDirection(seekDirection, separationDirection, desiredDirection);

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

                intendedPosition = BlockAgainstObstacles(intendedPosition);

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

        /// <summary>
        /// Sprint 2.3 fix (obstacle pass-through): the enemy Rigidbody is
        /// kinematic (set in Awake), and MovePosition on a kinematic body is NOT
        /// stopped by static colliders — so enemies previously slid straight
        /// through arena obstacles and boundary walls, while the player (a
        /// dynamic body) was already blocked. This sweeps the enemy's own
        /// collider along the intended move and, if it would enter a
        /// non-enemy / non-player collider, stops it just short.
        ///
        /// It intentionally ignores other enemies (separation already spaces
        /// them), the player (the attackRange clamp above already governs player
        /// proximity), and floor-like surfaces (upward-facing normals) so ground
        /// contact never blocks horizontal movement. This is deliberately NOT
        /// pathfinding: a blocked enemy stops against the obstacle rather than
        /// routing around it (see the known limitation noted in the task).
        /// </summary>
        private Vector3 BlockAgainstObstacles(Vector3 intendedPosition)
        {
            Vector3 delta = intendedPosition - rb.position;
            float distance = delta.magnitude;
            if (distance <= 0.0001f)
            {
                return intendedPosition;
            }

            Vector3 direction = delta / distance;
            if (TryGetObstacleAhead(direction, distance, out RaycastHit hit))
            {
                float allowed = Mathf.Max(0f, hit.distance - obstacleSkin);
                Vector3 blocked = rb.position + direction * allowed;
                blocked.y = rb.position.y;
                return blocked;
            }

            return intendedPosition;
        }

        /// <summary>
        /// Single sweep-and-filter used by every obstacle check: sweeps the
        /// enemy's collider along a horizontal <paramref name="direction"/> up
        /// to <paramref name="lookAhead"/> units and reports the first hit that
        /// is a real obstacle — i.e. not another enemy (separation handles
        /// spacing), not the player (the attackRange clamp governs player
        /// proximity), and not a floor-like surface (an upward-facing normal,
        /// so ground contact never blocks horizontal movement).
        /// </summary>
        private bool TryGetObstacleAhead(Vector3 direction, float lookAhead, out RaycastHit hit)
        {
            hit = default;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            direction.Normalize();
            if (!rb.SweepTest(direction, out hit, lookAhead, QueryTriggerInteraction.Ignore))
            {
                return false;
            }

            if (hit.collider == null || hit.collider.GetComponentInParent<EnemyAI>() != null || hit.collider.GetComponentInParent<PlayerController>() != null)
            {
                return false;
            }

            return hit.normal.y <= 0.5f;
        }

        /// <summary>Is a straight move along <paramref name="direction"/> blocked
        /// by an obstacle within <paramref name="lookAhead"/> units?</summary>
        private bool IsDirectionBlocked(Vector3 direction, float lookAhead)
        {
            return TryGetObstacleAhead(direction, lookAhead, out _);
        }

        /// <summary>
        /// Sprint 2.3 (obstacle avoidance): chooses the actual move direction.
        /// If the direct (seek + separation) path is clear, pursue normally. If
        /// it is blocked, slide along the obstacle using the tangent of the hit
        /// surface — which stays parallel to the wall regardless of where the
        /// enemy is along it, so the enemy walks straight to the wall's edge
        /// instead of swinging back toward centre. The chosen side is latched
        /// for the whole traversal (only the stuck-timer or a blocked tangent
        /// flips it), which is what prevents left/right oscillation. Returns to
        /// direct pursuit automatically once the path clears. Local steering
        /// only — no pathfinding/NavMesh (see task limitations).
        /// </summary>
        private Vector3 ResolveMoveDirection(Vector3 seekDirection, Vector3 separationDirection, Vector3 desiredDirection)
        {
            Vector3 probe = desiredDirection;
            probe.y = 0f;
            probe = probe.sqrMagnitude > 0.0001f ? probe.normalized : seekDirection;

            if (!TryGetObstacleAhead(probe, avoidLookAhead, out RaycastHit hit))
            {
                // Path clear — normal pursuit.
                avoiding = false;
                return desiredDirection;
            }

            // Blocked. Tangent along the wall from the hit normal (both flattened
            // to the ground plane) — stable along a flat wall, so no swing-back.
            Vector3 normal = hit.normal;
            normal.y = 0f;
            if (normal.sqrMagnitude < 0.0001f)
            {
                normal = -probe;
            }
            normal.Normalize();
            Vector3 tangent = Vector3.Cross(Vector3.up, normal);
            tangent.y = 0f;
            if (tangent.sqrMagnitude < 0.0001f)
            {
                return desiredDirection;
            }
            tangent.Normalize();

            // Choose the side ONCE, when first entering avoidance for this
            // obstacle: prefer the tangent that heads more toward the player;
            // for a dead-on wall (no preference) keep the current side. While
            // avoidance stays active the side is held (latched) — this is the
            // core anti-oscillation guarantee.
            if (!avoiding)
            {
                avoiding = true;
                float towardPlayer = Vector3.Dot(tangent, seekDirection);
                if (Mathf.Abs(towardPlayer) > 0.2f)
                {
                    avoidanceSign = Mathf.Sign(towardPlayer);
                }
            }

            Vector3 slide = tangent * avoidanceSign;

            // If the committed tangent is itself blocked (e.g. an inside corner),
            // try the other way; if both are blocked, fall back to desired and
            // let the hard clamp stop the enemy (stuck-timer will recover).
            if (IsDirectionBlocked(slide, avoidLookAhead))
            {
                Vector3 other = -slide;
                if (!IsDirectionBlocked(other, avoidLookAhead))
                {
                    avoidanceSign = -avoidanceSign;
                    slide = other;
                }
                else
                {
                    return desiredDirection;
                }
            }

            // Keep a little separation influence while wall-following so enemies
            // don't bunch, but keep the slide direction dominant.
            Vector3 blended = slide + separationDirection * (separationWeight * 0.5f);
            return blended.sqrMagnitude > 0.0001f ? blended.normalized : slide;
        }

        /// <summary>
        /// Tracks actual movement (not distance-to-player, which stays flat
        /// while legitimately sliding along a wall). If the enemy barely moves
        /// for stuckTimeout seconds — genuinely jammed, e.g. it committed to a
        /// dead-end side — it flips the avoidance side to try the other way.
        /// </summary>
        private void UpdateStuckRecovery()
        {
            if (Vector3.Distance(rb.position, lastStuckCheckPosition) > 0.25f)
            {
                lastStuckCheckPosition = rb.position;
                lastProgressTime = Time.time;
                return;
            }

            if (Time.time - lastProgressTime > stuckTimeout)
            {
                avoidanceSign = -avoidanceSign;
                lastProgressTime = Time.time;
                lastStuckCheckPosition = rb.position;
            }
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
