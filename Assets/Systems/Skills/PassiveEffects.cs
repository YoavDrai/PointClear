using UnityEngine;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.5: Player-side reader for Passive build-layer effects. Passives
    /// are ordinary SkillDefinitions (SkillType.Passive) allocated through the
    /// same SkillProgression / SkillPoints as Active Skills; this component
    /// translates their allocated rank into typed, queryable flags that skill
    /// coordinators read.
    ///
    /// Currently exposes one flag — VolatileFractureActive — recomputed on
    /// SkillLevelChanged (event-driven, no per-frame polling). This is the
    /// passive-relevant data owner; it holds no universal stat structure.
    /// </summary>
    public class PassiveEffects : MonoBehaviour
    {
        [SerializeField]
        private SkillProgression progression;

        [Tooltip("The Volatile Fracture passive definition (SkillType.Passive, MaxLevel 1).")]
        [SerializeField]
        private SkillDefinition volatileFracture;

        /// <summary>True while Volatile Fracture is allocated (rank >= 1).</summary>
        public bool VolatileFractureActive { get; private set; }

        private void Awake()
        {
            if (progression == null)
            {
                progression = GetComponent<SkillProgression>();
            }
        }

        private void OnEnable()
        {
            if (progression != null)
            {
                progression.SkillLevelChanged += HandleSkillLevelChanged;
            }
            Recalculate();
        }

        private void Start()
        {
            // Safety re-read: OnEnable can run before SkillProgression.Awake has
            // populated its registry (component/instantiation ordering). Start
            // runs after all Awakes, so this guarantees a correct initial value.
            // Subscription happened once in OnEnable — this does not re-subscribe.
            Recalculate();
        }

        private void OnDisable()
        {
            if (progression != null)
            {
                progression.SkillLevelChanged -= HandleSkillLevelChanged;
            }
        }

        private void HandleSkillLevelChanged(SkillDefinition changed, int newLevel)
        {
            if (changed == volatileFracture)
            {
                Recalculate();
            }
        }

        private void Recalculate()
        {
            VolatileFractureActive = progression != null
                && volatileFracture != null
                && progression.GetLevel(volatileFracture) >= 1;
        }
    }
}
