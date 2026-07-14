using UnityEngine;

namespace PointClear.Combat
{
    /// <summary>
    /// Permanent gameplay rule (PC-017): the player's own abilities never damage
    /// the player — no self-damage, no friendly fire from player skills. Enemy
    /// attacks remain fully dangerous; only PLAYER-dealt ability damage is filtered.
    ///
    /// This is the single shared guard every current and future player skill routes
    /// its damage through, so the rule stays consistent everywhere. The player is
    /// identified by the "Player" tag (enemies are untagged). A future skill that is
    /// *deliberately* designed around self-damage would call Health.TakeDamage
    /// directly, intentionally bypassing this guard.
    ///
    /// Not a damage framework — a two-method guard. It is the smallest way to make
    /// the "no self-damage" rule uniform across the disposable greybox skills.
    /// </summary>
    public static class PlayerAbilityDamage
    {
        /// <summary>True when this Health belongs to the local player (never hurt by player abilities).</summary>
        public static bool IsPlayer(Health target) => target != null && target.CompareTag("Player");

        /// <summary>Applies player-ability damage to the target unless it is the player.</summary>
        public static void Apply(Health target, float amount)
        {
            if (target != null && !target.CompareTag("Player"))
            {
                target.TakeDamage(amount);
            }
        }
    }
}
