using System.Collections.Generic;
using UnityEngine;

namespace PointClear.Player
{
    /// <summary>
    /// Sprint 2.0's only supported stats — extend this enum when a new
    /// stat has a real, immediate consumer, not before.
    /// </summary>
    public enum StatType
    {
        Damage,
        FireRate,
        MoveSpeed
    }

    /// <summary>
    /// Minimal base + modifier stat container for Phase 2's progression
    /// foundation.
    ///
    /// effectiveValue = max(0, (base + sum(additive)) * (1 + sum(multiplicative)))
    ///
    /// Multiplicative modifiers are stored as deltas from 1 (+20% = 0.2),
    /// summed together and applied once — not chained sequentially. Base
    /// values are set once by the owning component and never mutated by a
    /// modifier. No modifier removal, respec, temporary modifiers, or
    /// advanced stacking rules — out of scope for this sprint.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        private readonly Dictionary<StatType, float> baseValues = new Dictionary<StatType, float>();
        private readonly Dictionary<StatType, List<float>> additiveModifiers = new Dictionary<StatType, List<float>>();
        private readonly Dictionary<StatType, List<float>> multiplicativeModifiers = new Dictionary<StatType, List<float>>();

        public void SetBase(StatType stat, float value)
        {
            baseValues[stat] = value;
        }

        public void AddAdditiveModifier(StatType stat, float amount)
        {
            GetOrCreateList(additiveModifiers, stat).Add(amount);
        }

        public void AddMultiplicativeModifier(StatType stat, float delta)
        {
            GetOrCreateList(multiplicativeModifiers, stat).Add(delta);
        }

        public float GetValue(StatType stat)
        {
            float baseValue = baseValues.TryGetValue(stat, out float b) ? b : 0f;

            float additiveSum = 0f;
            if (additiveModifiers.TryGetValue(stat, out List<float> additiveList))
            {
                for (int i = 0; i < additiveList.Count; i++)
                {
                    additiveSum += additiveList[i];
                }
            }

            float multiplicativeSum = 0f;
            if (multiplicativeModifiers.TryGetValue(stat, out List<float> multiplicativeList))
            {
                for (int i = 0; i < multiplicativeList.Count; i++)
                {
                    multiplicativeSum += multiplicativeList[i];
                }
            }

            float effective = (baseValue + additiveSum) * (1f + multiplicativeSum);
            return Mathf.Max(0f, effective);
        }

        private static List<float> GetOrCreateList(Dictionary<StatType, List<float>> dict, StatType stat)
        {
            if (!dict.TryGetValue(stat, out List<float> list))
            {
                list = new List<float>();
                dict[stat] = list;
            }

            return list;
        }
    }
}
