using UnityEngine;
using PointClear.Combat;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3: the "marked" state Detonation Field applies to an enemy,
    /// added at runtime to the enemy GameObject. If the enemy dies before the
    /// mark expires, it detonates — an area-of-effect burst at its position.
    /// Otherwise the mark expires silently and removes itself.
    ///
    /// All mark logic lives here, subscribing to the enemy's existing
    /// Health.Died event — EnemyAI and Health are not modified. Explosion
    /// chains (an explosion killing another marked enemy, which detonates in
    /// turn) are intended and always terminate: Health.TakeDamage ignores
    /// damage once IsDead, so Died fires at most once per enemy.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class DetonationMark : MonoBehaviour
    {
        private static readonly Collider[] ExplosionBuffer = new Collider[64];

        private Health health;
        private float expiryTime;
        private float explosionRadius;
        private float explosionDamage;
        private bool subscribed;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        /// <summary>Marks (or re-marks, refreshing expiry) this enemy.</summary>
        public void Apply(float duration, float radius, float damage)
        {
            expiryTime = Time.time + duration;
            explosionRadius = radius;
            explosionDamage = damage;

            if (!subscribed)
            {
                health.Died += HandleDeath;
                subscribed = true;
            }
        }

        private void Update()
        {
            if (Time.time >= expiryTime)
            {
                Unsubscribe();
                Destroy(this);
            }
        }

        private void HandleDeath()
        {
            // Guard against a death arriving in the same frame the mark expired.
            if (Time.time >= expiryTime)
            {
                return;
            }

            Explode();
        }

        private void Explode()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                explosionRadius,
                ExplosionBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider other = ExplosionBuffer[i];
                if (other == null)
                {
                    continue;
                }

                // The dying enemy that triggered this explosion is excluded by
                // target != health; anything already dead is excluded by
                // !IsDead. A chained explosion on a killed marked neighbor
                // fires through its own DetonationMark.HandleDeath, not here.
                Health target = other.GetComponentInParent<Health>();
                if (target != null && target != health && !target.IsDead)
                {
                    target.TakeDamage(explosionDamage);
                }
            }
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (subscribed)
            {
                health.Died -= HandleDeath;
                subscribed = false;
            }
        }
    }
}
