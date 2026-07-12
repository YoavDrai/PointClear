using UnityEngine;
using PointClear.Combat;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3: the moving projectile spawned by FractureBolt. Travels in a
    /// straight line; collision is detected by a per-FixedUpdate raycast along
    /// the movement step (not a trigger collider) so fast bolts can't tunnel
    /// through thin enemies — same raycast approach HitscanWeapon uses.
    ///
    /// On its first enemy hit a primary bolt deals full damage, then splits
    /// into two shards angled +/- splitAngle from its travel direction. Shards
    /// (canSplit = false) deal reduced damage, die on their first hit, and
    /// never split again, so there is no recursion.
    /// </summary>
    public class FractureBoltProjectile : MonoBehaviour
    {
        [SerializeField]
        private float speed = 22f;

        [SerializeField]
        private float damage = 20f;

        [SerializeField]
        private float lifetime = 2f;

        [SerializeField]
        private bool canSplit = true;

        [Header("Split (primary bolt only)")]
        [SerializeField]
        private GameObject shardPrefab;

        [SerializeField]
        private float splitAngle = 25f;

        private Vector3 direction;
        private float despawnTime;

        /// <summary>
        /// Sprint 2.4: lets the firing FractureBolt override this bolt's damage
        /// from its per-rank data before launch. Shards keep their own prefab
        /// damage (this is only called on the primary bolt).
        /// </summary>
        public void SetDamage(float value)
        {
            damage = value;
        }

        public void Launch(Vector3 travelDirection)
        {
            direction = travelDirection.normalized;
            despawnTime = Time.time + lifetime;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void FixedUpdate()
        {
            if (Time.time >= despawnTime)
            {
                Destroy(gameObject);
                return;
            }

            float step = speed * Time.fixedDeltaTime;
            Vector3 origin = transform.position;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, step, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Health target = hit.collider.GetComponentInParent<Health>();
                if (target != null && !target.IsDead)
                {
                    target.TakeDamage(damage);
                    OnEnemyHit(hit.point);
                    return;
                }
            }

            transform.position = origin + direction * step;
        }

        private void OnEnemyHit(Vector3 point)
        {
            if (canSplit && shardPrefab != null)
            {
                SpawnShard(Quaternion.Euler(0f, splitAngle, 0f) * direction, point);
                SpawnShard(Quaternion.Euler(0f, -splitAngle, 0f) * direction, point);
            }

            Destroy(gameObject);
        }

        private void SpawnShard(Vector3 shardDirection, Vector3 spawnPoint)
        {
            shardDirection.y = 0f;
            GameObject shard = Instantiate(shardPrefab, spawnPoint, Quaternion.LookRotation(shardDirection.normalized));
            if (shard.TryGetComponent(out FractureBoltProjectile projectile))
            {
                projectile.Launch(shardDirection.normalized);
            }
        }
    }
}
