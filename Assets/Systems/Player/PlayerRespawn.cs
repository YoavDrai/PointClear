using System.Collections;
using UnityEngine;
using PointClear.Combat;
using PointClear.Weapons;

namespace PointClear.Player
{
    /// <summary>
    /// PROTOTYPE-ONLY respawn behavior. This is not the final roguelite
    /// death design — it exists only to let Sprint 1 demonstrate a full
    /// damage -> death -> respawn loop.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private float respawnDelay = 2f;

        private Health health;
        private PlayerController playerController;
        private HitscanWeapon weapon;
        private Collider bodyCollider;
        private Renderer[] renderers;

        private void Awake()
        {
            health = GetComponent<Health>();
            playerController = GetComponent<PlayerController>();
            weapon = GetComponent<HitscanWeapon>();
            bodyCollider = GetComponent<Collider>();
            renderers = GetComponentsInChildren<Renderer>();
        }

        private void OnEnable()
        {
            health.Died += HandleDeath;
        }

        private void OnDisable()
        {
            health.Died -= HandleDeath;
        }

        private void HandleDeath()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SetAlive(false);

            yield return new WaitForSeconds(respawnDelay);

            if (spawnPoint != null)
            {
                transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }

            health.ResetHealth();
            SetAlive(true);
        }

        private void SetAlive(bool alive)
        {
            if (playerController != null)
            {
                playerController.enabled = alive;
            }

            if (weapon != null)
            {
                weapon.enabled = alive;
            }

            if (bodyCollider != null)
            {
                bodyCollider.enabled = alive;
            }

            foreach (Renderer r in renderers)
            {
                r.enabled = alive;
            }
        }
    }
}
