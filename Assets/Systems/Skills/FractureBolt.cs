using UnityEngine;
using UnityEngine.InputSystem;
using PointClear.Player;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.3 Active Skill (Projectile). On activation, fires a bolt from
    /// the player toward the current aim point. The bolt pierces its first
    /// enemy and splits into two angled shards — see FractureBoltProjectile.
    ///
    /// Owns its own cooldown gate and its own gameplay. Sprint 2.4: it is now a
    /// rank consumer — it reads its allocated level from SkillProgression and
    /// applies its own typed per-rank data (primary bolt damage per level). The
    /// progression core knows nothing about this skill; this class holds the
    /// skill-relevant upgrade values.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class FractureBolt : MonoBehaviour
    {
        [SerializeField]
        private GameObject boltPrefab;

        [SerializeField]
        private Transform muzzle;

        [SerializeField]
        private float cooldown = 1.5f;

        [Header("Skill Progression")]
        [SerializeField]
        private SkillDefinition definition;

        [SerializeField]
        private SkillProgression progression;

        [Tooltip("Primary bolt damage at each rank (index 0 = Level 1). Prototype tuning, not final balance.")]
        [SerializeField]
        private float[] damagePerLevel = { 40f, 55f, 70f };

        public float Cooldown => cooldown;
        public bool IsReady => Time.time >= nextReadyTime;
        public float CooldownRemaining => Mathf.Max(0f, nextReadyTime - Time.time);

        private PlayerController playerController;
        private InputAction activateAction;
        private float nextReadyTime;
        private float currentDamage;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            if (progression == null)
            {
                progression = GetComponent<SkillProgression>();
            }
        }

        private void OnEnable()
        {
            activateAction = InputSystem.actions.FindAction("FractureBolt");
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
            currentDamage = DamageForLevel(level);
        }

        private float DamageForLevel(int level)
        {
            if (damagePerLevel == null || damagePerLevel.Length == 0)
            {
                return 0f;
            }

            int index = Mathf.Clamp(level - 1, 0, damagePerLevel.Length - 1);
            return damagePerLevel[index];
        }

        private void Update()
        {
            if (activateAction != null && activateAction.WasPressedThisFrame() && IsReady)
            {
                Activate();
                nextReadyTime = Time.time + cooldown;
            }
        }

        private void Activate()
        {
            if (boltPrefab == null)
            {
                return;
            }

            Vector3 origin = muzzle != null ? muzzle.position : transform.position;
            Vector3 direction = playerController.AimWorldPoint - origin;
            direction.y = 0f;
            direction = direction.sqrMagnitude < 0.0001f ? transform.forward : direction.normalized;

            GameObject bolt = Instantiate(boltPrefab, origin, Quaternion.LookRotation(direction));
            if (bolt.TryGetComponent(out FractureBoltProjectile projectile))
            {
                projectile.SetDamage(currentDamage);
                projectile.Launch(direction);
            }
        }
    }
}
