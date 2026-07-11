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
    /// Owns its own cooldown gate (same shape as HitscanWeapon's fire-rate and
    /// EnemyAI's attack interval). Deliberately NOT extracted into a shared
    /// cooldown/skill abstraction: two skills with different activation shapes
    /// don't yet justify one (Game Director decision — revisit when real
    /// duplication with shared long-term behaviour appears).
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

        public float Cooldown => cooldown;
        public bool IsReady => Time.time >= nextReadyTime;
        public float CooldownRemaining => Mathf.Max(0f, nextReadyTime - Time.time);

        private PlayerController playerController;
        private InputAction activateAction;
        private float nextReadyTime;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            activateAction = InputSystem.actions.FindAction("FractureBolt");
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
                projectile.Launch(direction);
            }
        }
    }
}
