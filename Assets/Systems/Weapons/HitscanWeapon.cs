using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Weapons
{
    /// <summary>
    /// Single prototype hitscan weapon. Fires toward the player's current
    /// aim point (not the character's visual facing, which is smoothed) so
    /// shots land exactly where the mouse points. Infinite ammo, no reload.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerStats))]
    public class HitscanWeapon : MonoBehaviour
    {
        [SerializeField]
        private float damage = 10f;

        [SerializeField]
        private float fireRate = 6f;

        [SerializeField]
        private float range = 50f;

        [SerializeField]
        private Transform muzzle;

        [SerializeField]
        private LineRenderer bulletTrail;

        [SerializeField]
        private GameObject muzzleFlash;

        [SerializeField]
        private float trailDuration = 0.05f;

        [SerializeField]
        private float muzzleFlashDuration = 0.05f;

        /// <summary>
        /// Sprint 2.9: raised after a shot deals damage to an enemy's Health, with
        /// that Health. Additive hook for the Weapon layer (the Detonator Module
        /// subscribes to trigger marks); no behavior change when nothing listens.
        /// </summary>
        public event Action<Health> EnemyHit;

        private PlayerController playerController;
        private PlayerStats stats;
        private InputAction shootAction;
        private float nextFireTime;
        private Coroutine trailRoutine;
        private Coroutine flashRoutine;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            stats = GetComponent<PlayerStats>();
            stats.SetBase(StatType.Damage, damage);
            stats.SetBase(StatType.FireRate, fireRate);

            if (bulletTrail != null)
            {
                bulletTrail.enabled = false;
            }

            if (muzzleFlash != null)
            {
                muzzleFlash.SetActive(false);
            }
        }

        private void OnEnable()
        {
            shootAction = InputSystem.actions.FindAction("Shoot");
        }

        private void Update()
        {
            if (shootAction.IsPressed() && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + 1f / stats.GetValue(StatType.FireRate);
            }
        }

        private void Fire()
        {
            Vector3 origin = muzzle != null ? muzzle.position : transform.position;
            Vector3 aimPoint = playerController.AimWorldPoint;
            Vector3 direction = aimPoint - origin;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = transform.forward;
            }
            else
            {
                direction.Normalize();
            }

            Vector3 endPoint = origin + direction * range;

            // QueryTriggerInteraction.Ignore: Sprint 2.1's trigger-collider XP
            // pickups must not block shots meant for enemies behind them.
            if (Physics.Raycast(origin, direction, out RaycastHit hit, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                endPoint = hit.point;
                Health targetHealth = hit.collider.GetComponentInParent<Health>();
                // No self-damage from player abilities (PC-017): the player's own
                // shot never hurts the player.
                if (targetHealth != null && !PlayerAbilityDamage.IsPlayer(targetHealth))
                {
                    targetHealth.TakeDamage(stats.GetValue(StatType.Damage));
                    EnemyHit?.Invoke(targetHealth);
                }
            }

            Debug.DrawLine(origin, endPoint, Color.red, 0.1f);
            ShowTrail(origin, endPoint);
            ShowMuzzleFlash();
        }

        private void ShowTrail(Vector3 start, Vector3 end)
        {
            if (bulletTrail == null)
            {
                return;
            }

            if (trailRoutine != null)
            {
                StopCoroutine(trailRoutine);
            }

            trailRoutine = StartCoroutine(TrailRoutine(start, end));
        }

        private IEnumerator TrailRoutine(Vector3 start, Vector3 end)
        {
            bulletTrail.SetPosition(0, start);
            bulletTrail.SetPosition(1, end);
            bulletTrail.enabled = true;
            yield return new WaitForSeconds(trailDuration);
            bulletTrail.enabled = false;
            trailRoutine = null;
        }

        private void ShowMuzzleFlash()
        {
            if (muzzleFlash == null)
            {
                return;
            }

            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(MuzzleFlashRoutine());
        }

        private IEnumerator MuzzleFlashRoutine()
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(muzzleFlashDuration);
            muzzleFlash.SetActive(false);
            flashRoutine = null;
        }
    }
}
