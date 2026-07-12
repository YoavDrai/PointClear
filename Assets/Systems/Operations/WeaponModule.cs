using System;
using UnityEngine;
using PointClear.Combat;
using PointClear.Skills;
using PointClear.Weapons;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.9 (Cluster C): the first acquirable Weapon Module — the Detonator
    /// Module. While active, a weapon hit on a marked enemy detonates that mark
    /// EARLY (the same burst that would fire on the enemy's death) instead of
    /// waiting for the kill. Detonation Field still owns marking; this module only
    /// makes the weapon the trigger.
    ///
    /// Ownership mirrors CurrencyWallet's secure/lose pattern, reacting to the
    /// existing Operation lifecycle events — the 2.6 lifecycle is NOT modified:
    ///   - Equipped = acquired this run, at risk (Unsecured).
    ///   - Banked   = secured by a successful extraction; owned for the rest of the
    ///     Play Mode session and no longer at risk (DEC-019).
    ///   - IsActive = Equipped || Banked (the detonator verb is live).
    /// Death loses an unbanked module; extraction banks it. No inventory, module
    /// swapping, multiple modules, equipment UI, or cross-restart persistence.
    /// </summary>
    public class WeaponModule : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        [SerializeField]
        private HitscanWeapon weapon;

        public bool Equipped { get; private set; }
        public bool Banked { get; private set; }
        public bool IsActive => Equipped || Banked;

        /// <summary>Read-only, for the HUD terminal summary: whether the just-ended
        /// run secured (banked) or lost the module.</summary>
        public bool SecuredThisRun { get; private set; }
        public bool LostThisRun { get; private set; }

        /// <summary>Raised whenever ownership state changes (for the HUD).</summary>
        public event Action Changed;

        private bool operationSubscribed;
        private bool weaponSubscribed;

        private void Awake()
        {
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            if (weapon == null)
            {
                weapon = GetComponent<HitscanWeapon>();
            }
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Start()
        {
            // The OperationController / weapon may resolve later than Awake.
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            if (weapon == null)
            {
                weapon = GetComponent<HitscanWeapon>();
            }
            Subscribe();
        }

        /// <summary>Called by WeaponModulePickup when the player collects the drop.
        /// Immediate activation: the effect is live for the rest of this run.</summary>
        public void Equip()
        {
            if (IsActive)
            {
                return; // already owned this run (or banked) — no double-equip
            }

            Equipped = true;
            Changed?.Invoke();
        }

        private void HandleOperationStarted()
        {
            WeaponModulePickup.ClearAll();

            // A banked module stays owned/active across runs; an unbanked one does
            // not carry into a new run.
            if (!Banked)
            {
                Equipped = false;
            }

            SecuredThisRun = false;
            LostThisRun = false;
            Changed?.Invoke();
        }

        private void HandleOperationSucceeded()
        {
            // Newly secured only if it was carried unbanked into this run.
            if (Equipped && !Banked)
            {
                Banked = true;         // secured — owned for the session
                SecuredThisRun = true;
            }

            Equipped = false;          // folded into Banked; IsActive stays true via Banked
            WeaponModulePickup.ClearAll();
            Changed?.Invoke();
        }

        private void HandleOperationFailed()
        {
            LostThisRun = Equipped && !Banked;

            if (!Banked)
            {
                Equipped = false;      // lost; a banked module is untouched (DEC-019)
            }

            WeaponModulePickup.ClearAll();
            Changed?.Invoke();
        }

        // The weapon becomes the trigger: if the hit enemy is currently marked,
        // detonate that mark early. Detonation Field still owns marking; a lethal
        // hit's death-detonation and this early trigger are mutually idempotent
        // (DetonationMark's one-shot guard), so a lethal shot never double-explodes.
        private void HandleEnemyHit(Health enemyHealth)
        {
            if (!IsActive || enemyHealth == null)
            {
                return;
            }

            if (enemyHealth.TryGetComponent(out DetonationMark mark))
            {
                mark.DetonateEarly();
            }
        }

        private void Subscribe()
        {
            if (operation != null && !operationSubscribed)
            {
                operation.OperationStarted += HandleOperationStarted;
                operation.OperationSucceeded += HandleOperationSucceeded;
                operation.OperationFailed += HandleOperationFailed;
                operationSubscribed = true;
            }

            if (weapon != null && !weaponSubscribed)
            {
                weapon.EnemyHit += HandleEnemyHit;
                weaponSubscribed = true;
            }
        }

        private void Unsubscribe()
        {
            if (operation != null && operationSubscribed)
            {
                operation.OperationStarted -= HandleOperationStarted;
                operation.OperationSucceeded -= HandleOperationSucceeded;
                operation.OperationFailed -= HandleOperationFailed;
                operationSubscribed = false;
            }

            if (weapon != null && weaponSubscribed)
            {
                weapon.EnemyHit -= HandleEnemyHit;
                weaponSubscribed = false;
            }
        }
    }
}
