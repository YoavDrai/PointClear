using UnityEngine;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.4: the shared, data-driven descriptor for one skill. Holds only
    /// common metadata and progression info the allocation core needs — a
    /// stable Id (for future save data), a display name, the category, and the
    /// maximum level. It deliberately contains NO per-skill gameplay values and
    /// NO universal stat table: each skill's typed, skill-relevant upgrade data
    /// (damage per rank, radius per rank, ...) lives with that skill's own
    /// behavior instead. Adding a new skill is mostly authoring a new asset of
    /// this type plus its behavior — the allocation system never changes.
    ///
    /// The runtime uses direct references to these assets as registry keys;
    /// the string Id exists only for future save/load, not for runtime lookup.
    /// </summary>
    [CreateAssetMenu(fileName = "SkillDefinition", menuName = "Point Clear/Skill Definition")]
    public class SkillDefinition : ScriptableObject
    {
        [Tooltip("Stable identifier for future save data. Never reused or renamed once shipped.")]
        [SerializeField]
        private string id;

        [SerializeField]
        private string displayName;

        [SerializeField]
        private SkillType skillType = SkillType.Active;

        [Tooltip("Highest rank this skill can reach via Skill Point allocation.")]
        [Min(1)]
        [SerializeField]
        private int maxLevel = 3;

        [Tooltip("Rank this skill begins at in a fresh run. 0 = locked (must be allocated). Active starter skills use 1; passives typically 0.")]
        [Min(0)]
        [SerializeField]
        private int startingLevel = 0;

        public string Id => id;
        public string DisplayName => string.IsNullOrEmpty(displayName) ? name : displayName;
        public SkillType SkillType => skillType;
        public int MaxLevel => Mathf.Max(1, maxLevel);

        // Sprint 2.5: initial progression state only — not a gameplay value.
        // Read-only at runtime and always clamped to [0, MaxLevel], so an
        // asset misconfigured above MaxLevel can never produce an invalid rank.
        public int StartingLevel => Mathf.Clamp(startingLevel, 0, MaxLevel);
    }
}
