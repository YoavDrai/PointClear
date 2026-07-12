using System;
using UnityEngine;
using PointClear.Progression;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.4: the Skill Point wallet. Earns one point for every character
    /// level gained (PlayerLevel.LevelUp fires once per level crossed, so a
    /// multi-level XP award correctly grants multiple points). Points are
    /// run-persistent — they last for the current play session and are only
    /// removed by spending; disk save/load is out of scope for this sprint.
    ///
    /// Spending goes through TrySpend, which never lets the balance go negative.
    /// The wallet knows nothing about individual skills; SkillProgression
    /// coordinates validation and spending.
    /// </summary>
    [RequireComponent(typeof(PlayerLevel))]
    public class SkillPoints : MonoBehaviour
    {
        [Tooltip("Skill points granted per character level gained.")]
        [Min(0)]
        [SerializeField]
        private int pointsPerLevel = 1;

        public int Available { get; private set; }

        /// <summary>Raised whenever Available changes; argument is the new total.</summary>
        public event Action<int> Changed;

        private PlayerLevel playerLevel;

        private void Awake()
        {
            playerLevel = GetComponent<PlayerLevel>();
        }

        private void OnEnable()
        {
            playerLevel.LevelUp += HandleLevelUp;
        }

        private void OnDisable()
        {
            playerLevel.LevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp(int newLevel)
        {
            if (pointsPerLevel <= 0)
            {
                return;
            }

            Available += pointsPerLevel;
            Changed?.Invoke(Available);
        }

        /// <summary>Spends one point if any are available. Returns false (and
        /// changes nothing) when the balance is zero — never goes negative.</summary>
        public bool TrySpend()
        {
            if (Available <= 0)
            {
                return false;
            }

            Available--;
            Changed?.Invoke(Available);
            return true;
        }
    }
}
