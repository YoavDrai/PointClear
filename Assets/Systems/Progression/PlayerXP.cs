using System;
using UnityEngine;

namespace PointClear.Progression
{
    /// <summary>
    /// Owns only the player's current accumulated XP total. No level
    /// thresholds, level calculation, or upgrade logic — that belongs to
    /// later Phase 2 sprints (2.2+).
    /// </summary>
    public class PlayerXP : MonoBehaviour
    {
        public float CurrentXP { get; private set; }

        public event Action<float> XPChanged;

        public void AddXP(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            CurrentXP += amount;
            XPChanged?.Invoke(CurrentXP);
        }
    }
}
