using UnityEngine;
using PointClear.Combat;
using PointClear.Player;
using PointClear.Progression;

namespace PointClear.Enemies
{
    /// <summary>
    /// Sprint 2.1 (revised): grants a configured XP value immediately and
    /// deterministically when this enemy dies — no physical object, no
    /// RNG, no loot/gold/rarity/inventory logic. A separate component from
    /// EnemyAI, same pattern as EnemyAI's own hit-reaction handling and
    /// PlayerRespawn subscribing independently to Health's events.
    ///
    /// For this single-player prototype, XP is granted to the local
    /// player via PlayerReference. This is the only place the "who
    /// receives credit for a kill" rule lives — a future multiplayer
    /// credit rule (nearest player, damage contribution, etc.) only
    /// requires changing this component, not EnemyAI, Health, or PlayerXP.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class EnemyXPReward : MonoBehaviour
    {
        [SerializeField]
        private float xpValue = 1f;

        private Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.Died += HandleDeath;
        }

        private void OnDisable()
        {
            health.Died -= HandleDeath;
        }

        private void HandleDeath()
        {
            Transform player = PlayerReference.Instance;
            if (player == null)
            {
                return;
            }

            if (player.TryGetComponent(out PlayerXP playerXP))
            {
                playerXP.AddXP(xpValue);
            }
        }
    }
}
