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

        [Tooltip("Sprint 2.6: when true, the OperationController owns death (death = run failure), so this component does NOT auto-respawn. It instead exposes SetDefeated()/ResetPlayer() for the Operation to drive.")]
        [SerializeField]
        private bool operationControlled = false;

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
            if (operationControlled)
            {
                // Sprint 2.6: the OperationController turns player death into a
                // run-ending failure and drives SetDefeated()/ResetPlayer().
                return;
            }

            StartCoroutine(RespawnRoutine());
        }

        /// <summary>Sprint 2.6: freeze/hide the player on Operation failure.</summary>
        public void SetDefeated()
        {
            SetAlive(false);
        }

        /// <summary>
        /// Sprint 2.6: restore the player to full health at the spawn point on an
        /// Operation return-to-neutral. Resets the ENCOUNTER only — it never
        /// touches persistent progression (Level/Experience/Skill Points).
        /// </summary>
        public void ResetPlayer()
        {
            if (spawnPoint != null)
            {
                transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }

            health.ResetHealth();
            SetAlive(true);
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
