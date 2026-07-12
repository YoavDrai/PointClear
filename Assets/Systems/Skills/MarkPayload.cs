namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.5: the small typed payload a FractureBolt shard receives at
    /// spawn time telling it whether — and with what parameters — to apply a
    /// Detonation Mark on hit. This is the ONLY channel by which the passive
    /// interaction reaches the projectile: the shard consumes this value and
    /// never queries PassiveEffects, SkillProgression, SkillDefinition, or
    /// DetonationField. Deliberately a plain struct, not a callback/delegate or
    /// a general on-hit framework.
    ///
    /// The parameters are resolved by the firing coordinator (FractureBolt)
    /// from the player's current Detonation Field rank; they are never fabricated
    /// by the projectile.
    /// </summary>
    public struct MarkPayload
    {
        public bool ApplyMark;
        public float Duration;
        public float Radius;
        public float Damage;

        public static MarkPayload None => new MarkPayload { ApplyMark = false };

        public static MarkPayload Of(float duration, float radius, float damage)
        {
            return new MarkPayload
            {
                ApplyMark = true,
                Duration = duration,
                Radius = radius,
                Damage = damage
            };
        }
    }
}
