using UnityEngine;
using PointClear.Combat;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.7: drops one physical currency pickup when this enemy genuinely
    /// dies. Mirrors EnemyXPReward's pattern — it observes the existing public
    /// Health.Died event (Health.cs is not modified). Because Health.Died fires
    /// only on lethal damage and never on Destroy, enemies removed by Operation
    /// cleanup/reset drop nothing. Fixed, configurable amount — no RNG, rarity,
    /// drop table, or per-enemy reward data.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyCurrencyDrop : MonoBehaviour
    {
        [SerializeField]
        private GameObject pickupPrefab;

        [Tooltip("Fixed currency value this enemy drops on death.")]
        [Min(0)]
        [SerializeField]
        private int amount = 5;

        [Tooltip("Vertical offset so the drop sits on/above the ground.")]
        [SerializeField]
        private float dropHeight = 0.5f;

        private Health health;
        private bool subscribed;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            if (health != null && !subscribed)
            {
                health.Died += HandleDied;
                subscribed = true;
            }
        }

        private void OnDisable()
        {
            if (health != null && subscribed)
            {
                health.Died -= HandleDied;
                subscribed = false;
            }
        }

        private void HandleDied()
        {
            if (pickupPrefab == null)
            {
                return;
            }

            Vector3 position = transform.position + Vector3.up * dropHeight;
            GameObject drop = Instantiate(pickupPrefab, position, Quaternion.identity);
            if (drop.TryGetComponent(out CurrencyPickup pickup))
            {
                pickup.SetValue(amount);
            }
        }
    }
}
