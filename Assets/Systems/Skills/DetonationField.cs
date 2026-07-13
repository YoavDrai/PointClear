using UnityEngine;
using UnityEngine.InputSystem;
using PointClear.Combat;
using PointClear.Enemies;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3 Active Skill (Area). On activation, marks every enemy within
    /// markRadius of the player. A marked enemy that dies within markDuration
    /// detonates (see DetonationMark) — a "set up, then chain" payoff, distinct
    /// from Fracture Bolt's direct ranged damage.
    ///
    /// Self-centered: no separate targeting/reticle this sprint. Owns its own
    /// cooldown gate (not a shared abstraction — same rationale as FractureBolt).
    ///
    /// The marking query uses its OWN buffer, never EnemyAI.SeparationBuffer
    /// (a shared static array reused every EnemyAI.FixedUpdate — borrowing it
    /// would corrupt separation calculations mid-frame).
    /// </summary>
    public class DetonationField : MonoBehaviour
    {
        [SerializeField]
        private float markRadius = 5f;

        [SerializeField]
        private float markDuration = 4f;

        [SerializeField]
        private float explosionRadius = 3f;

        [SerializeField]
        private float explosionDamage = 25f;

        [SerializeField]
        private float cooldown = 6f;

        [SerializeField]
        private GameObject fieldVfxPrefab;

        [SerializeField]
        private float fieldVfxDuration = 0.4f;

        [Header("Skill Progression")]
        [SerializeField]
        private SkillDefinition definition;

        [SerializeField]
        private SkillProgression progression;

        [Tooltip("Explosion radius at each rank (index 0 = Level 1). Prototype tuning, not final balance.")]
        [SerializeField]
        private float[] explosionRadiusPerLevel = { 3f, 4f, 5f };

        public float Cooldown => cooldown;
        public bool IsReady => Time.time >= nextReadyTime;
        public float CooldownRemaining => Mathf.Max(0f, nextReadyTime - Time.time);

        // Sprint 2.5: read-only current-rank mark parameters, so a coordinator
        // (e.g. FractureBolt, for the Volatile Fracture passive) can apply a
        // mark identical to what this field would apply right now. Behavior is
        // unchanged — these just expose the same values Activate() already uses.
        public float CurrentMarkDuration => markDuration;
        public float CurrentExplosionRadius => currentExplosionRadius;
        public float CurrentExplosionDamage => explosionDamage;

        private static readonly Collider[] MarkBuffer = new Collider[64];

        private InputAction activateAction;
        private float nextReadyTime;
        private float currentExplosionRadius;

        private void Awake()
        {
            if (progression == null)
            {
                progression = GetComponent<SkillProgression>();
            }
        }

        private void OnEnable()
        {
            activateAction = InputSystem.actions.FindAction("DetonationField");
            if (progression != null)
            {
                progression.SkillLevelChanged += HandleSkillLevelChanged;
            }
            RecalculateFromLevel();
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
            if (changed == definition)
            {
                RecalculateFromLevel();
            }
        }

        private void RecalculateFromLevel()
        {
            int level = progression != null && definition != null ? progression.GetLevel(definition) : 1;
            currentExplosionRadius = RadiusForLevel(level);
        }

        private float RadiusForLevel(int level)
        {
            if (explosionRadiusPerLevel == null || explosionRadiusPerLevel.Length == 0)
            {
                return explosionRadius;
            }

            int index = Mathf.Clamp(level - 1, 0, explosionRadiusPerLevel.Length - 1);
            return explosionRadiusPerLevel[index];
        }

        private void Update()
        {
            if (activateAction != null && activateAction.WasPressedThisFrame() && IsReady && IsUnlocked())
            {
                Activate();
                nextReadyTime = Time.time + cooldown;
            }
        }

        // PC-016: an Active skill only fires once allocated to rank >= 1 — so a
        // character that has not invested in it (StartingLevel 0, unallocated)
        // genuinely cannot use it, making the character-start choice real. When
        // progression/definition are unresolved we treat it as usable (level 1),
        // matching RecalculateFromLevel's existing fallback so a misconfigured
        // prototype still fires rather than going silently dead.
        private bool IsUnlocked()
        {
            if (progression == null || definition == null)
            {
                return true;
            }
            return progression.GetLevel(definition) >= 1;
        }

        private void Activate()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                markRadius,
                MarkBuffer,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider other = MarkBuffer[i];
                if (other == null || !other.TryGetComponent(out EnemyAI _))
                {
                    continue;
                }

                if (!other.TryGetComponent(out Health health) || health.IsDead)
                {
                    continue;
                }

                DetonationMark mark = other.GetComponent<DetonationMark>();
                if (mark == null)
                {
                    mark = other.gameObject.AddComponent<DetonationMark>();
                }

                mark.Apply(markDuration, currentExplosionRadius, explosionDamage);
            }

            ShowFieldVfx();
        }

        private void ShowFieldVfx()
        {
            if (fieldVfxPrefab == null)
            {
                return;
            }

            GameObject vfx = Instantiate(fieldVfxPrefab, transform.position, Quaternion.identity);
            vfx.transform.localScale = new Vector3(markRadius * 2f, 0.1f, markRadius * 2f);
            Destroy(vfx, fieldVfxDuration);
        }
    }
}
