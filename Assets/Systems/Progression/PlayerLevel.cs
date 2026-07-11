using System;
using UnityEngine;

namespace PointClear.Progression
{
    /// <summary>
    /// Sprint 2.2: derives the player's level purely from PlayerXP's
    /// cumulative total — no independent stored progress, so nothing can
    /// drift out of sync. Owns only: accumulate-derived level calculation,
    /// level-transition detection, and the LevelUp event. Grants no stat
    /// bonus of any kind — player power is unaffected by leveling until
    /// Sprint 2.3 introduces upgrade selection.
    ///
    /// The XP curve formula lives entirely in PlaceholderXpCurve, kept out
    /// of this class's logic — replacing the formula later means writing a
    /// new class and changing the one line in Awake() that constructs it,
    /// not editing the loop/event logic below. No interface here: there is
    /// only one implementation and one consumer today, so an abstraction
    /// would have nothing to earn its keep against — introduce one only
    /// when a second real curve implementation actually exists.
    /// </summary>
    [RequireComponent(typeof(PlayerXP))]
    public class PlayerLevel : MonoBehaviour
    {
        private const int MaxLevelSafetyCap = 10000;

        [SerializeField]
        private float baseXpPerLevel = 5f;

        public int CurrentLevel { get; private set; } = 1;
        public float XPIntoCurrentLevel { get; private set; }
        public float XPRequiredForNextLevel { get; private set; }

        public event Action<int> LevelUp;

        private PlayerXP playerXP;
        private PlaceholderXpCurve curve;

        private void Awake()
        {
            playerXP = GetComponent<PlayerXP>();
            curve = new PlaceholderXpCurve(baseXpPerLevel);
            Recalculate(playerXP.CurrentXP);
        }

        private void OnEnable()
        {
            playerXP.XPChanged += HandleXPChanged;
        }

        private void OnDisable()
        {
            playerXP.XPChanged -= HandleXPChanged;
        }

        private void HandleXPChanged(float currentXP)
        {
            Recalculate(currentXP);
        }

        private void Recalculate(float totalXP)
        {
            float xpConsumed = 0f;
            int level = 1;
            float requiredForLevel = curve.XPRequiredForLevel(level);

            while (requiredForLevel > 0f && totalXP - xpConsumed >= requiredForLevel && level < MaxLevelSafetyCap)
            {
                xpConsumed += requiredForLevel;
                level++;
                requiredForLevel = curve.XPRequiredForLevel(level);
            }

            XPIntoCurrentLevel = totalXP - xpConsumed;
            XPRequiredForNextLevel = requiredForLevel;

            int previousLevel = CurrentLevel;
            CurrentLevel = level;

            for (int lvl = previousLevel + 1; lvl <= level; lvl++)
            {
                LevelUp?.Invoke(lvl);
            }
        }
    }
}
