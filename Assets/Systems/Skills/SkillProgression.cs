using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.4: the central skill registry and allocation authority. Tracks
    /// each registered skill's current level (run-persistent) keyed by direct
    /// SkillDefinition asset reference — the stable Id on the definition is kept
    /// only for future save data, not used for runtime lookup.
    ///
    /// This class is fully skill-agnostic: it reads only SkillDefinition
    /// metadata (MaxLevel) and never names, switches on, or special-cases any
    /// individual skill. Skill behaviors read their own level via GetLevel and
    /// react to SkillLevelChanged; a new skill is added by registering its
    /// definition, with no change here.
    /// </summary>
    public class SkillProgression : MonoBehaviour
    {
        [Tooltip("Skills the player has access to this run. Each begins at its own SkillDefinition.StartingLevel.")]
        [SerializeField]
        private List<SkillDefinition> registeredSkills = new List<SkillDefinition>();

        [SerializeField]
        private SkillPoints skillPoints;

        private readonly Dictionary<SkillDefinition, int> levels = new Dictionary<SkillDefinition, int>();

        /// <summary>Raised after a successful allocation: (definition, new level).</summary>
        public event Action<SkillDefinition, int> SkillLevelChanged;

        public IReadOnlyList<SkillDefinition> RegisteredSkills => registeredSkills;

        private void Awake()
        {
            if (skillPoints == null)
            {
                skillPoints = GetComponent<SkillPoints>();
            }

            // Sprint 2.5: each skill starts at its OWN SkillDefinition.StartingLevel
            // (the former single global starting level was removed — a passive must
            // be able to start locked at 0 while active starters start at 1).
            // StartingLevel is already clamped to [0, MaxLevel] by the definition;
            // clamp again here defensively. Null and duplicate entries are skipped,
            // matching the existing registry safeguards.
            foreach (SkillDefinition definition in registeredSkills)
            {
                if (definition != null && !levels.ContainsKey(definition))
                {
                    levels[definition] = Mathf.Clamp(definition.StartingLevel, 0, definition.MaxLevel);
                }
            }
        }

        /// <summary>Current allocated level of a skill, or 0 if it is not registered.</summary>
        public int GetLevel(SkillDefinition definition)
        {
            if (definition != null && levels.TryGetValue(definition, out int level))
            {
                return level;
            }

            return 0;
        }

        public bool IsRegistered(SkillDefinition definition)
        {
            return definition != null && levels.ContainsKey(definition);
        }

        public bool CanAllocate(SkillDefinition definition)
        {
            return IsRegistered(definition)
                && levels[definition] < definition.MaxLevel
                && skillPoints != null
                && skillPoints.Available > 0;
        }

        /// <summary>
        /// Spends one Skill Point to raise one skill by one level. Validation
        /// order matters: unregistered and at-max are rejected BEFORE any point
        /// is spent, so a rejected allocation never costs a point.
        /// </summary>
        public bool TryAllocate(SkillDefinition definition)
        {
            if (!IsRegistered(definition))
            {
                return false;
            }

            if (levels[definition] >= definition.MaxLevel)
            {
                return false;
            }

            if (skillPoints == null || !skillPoints.TrySpend())
            {
                return false;
            }

            int newLevel = levels[definition] + 1;
            levels[definition] = newLevel;
            SkillLevelChanged?.Invoke(definition, newLevel);
            return true;
        }
    }
}
