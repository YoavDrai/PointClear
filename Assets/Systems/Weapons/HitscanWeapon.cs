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

        private PlayerController playerController;
        private InputAction shootAction;
        private float nextFireTime;
        private Coroutine trailRoutine;
        private Coroutine flashRoutine;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

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
                nextFireTime = Time.time + 1f / fireRate;
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

            if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
            {
                endPoint = hit.point;
                Health targetHealth = hit.collider.GetComponentInParent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
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
