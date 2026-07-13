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

        /// <summary>
        /// Removes XP from the cumulative total (used by the death penalty).
        /// This class stays deliberately level-agnostic — it clamps only at zero
        /// and never at a level boundary. The "never de-level" rule is enforced by
        /// the caller (DeathXPPenalty), which knows the current level's floor and
        /// caps the amount accordingly. Symmetric to AddXP: re-fires XPChanged so
        /// PlayerLevel recomputes the derived level from the new total.
        /// </summary>
        public void RemoveXP(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            CurrentXP = Mathf.Max(0f, CurrentXP - amount);
            XPChanged?.Invoke(CurrentXP);
        }
    }
}
