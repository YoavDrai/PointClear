using System;
using UnityEngine;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.7 (Cluster B): the player's currency wallet and the sole owner of
    /// Unsecured (at-risk, this-run) and Banked (secured) currency. It reacts to
    /// the existing Operation lifecycle events — it does not modify the 2.6
    /// Operation scripts.
    ///
    /// - OperationStarted  → clear Unsecured (and defensively clear any pickups).
    /// - OperationSucceeded → Banked += Unsecured (exactly once), then clear
    ///   Unsecured; uncollected pickups are cleared (never banked).
    /// - OperationFailed   → clear Unsecured; Banked is untouched.
    ///
    /// Return-to-Ready needs no handler: a run only reaches Ready after a terminal
    /// event that already cleared Unsecured + pickups, and no drops occur after a
    /// terminal state. Banked is session/in-memory only — no spending, inventory,
    /// save/load, or economy here.
    /// </summary>
    public class CurrencyWallet : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        public int Unsecured { get; private set; }
        public int Banked { get; private set; }

        /// <summary>Raised whenever Unsecured or Banked changes (for the HUD).</summary>
        public event Action Changed;

        private bool subscribed;

        private void Awake()
        {
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
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
            // OperationController may resolve later than this component's Awake.
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            Subscribe();
        }

        /// <summary>Adds collected currency to the at-risk (Unsecured) pool.</summary>
        public void AddUnsecured(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Unsecured += amount;
            Changed?.Invoke();
        }

        private void HandleOperationStarted()
        {
            CurrencyPickup.ClearAll();
            Unsecured = 0;
            Changed?.Invoke();
        }

        private void HandleOperationSucceeded()
        {
            Banked += Unsecured;   // OperationSucceeded fires exactly once → banks once
            Unsecured = 0;
            CurrencyPickup.ClearAll();
            Changed?.Invoke();
        }

        private void HandleOperationFailed()
        {
            Unsecured = 0;         // lost; Banked untouched
            CurrencyPickup.ClearAll();
            Changed?.Invoke();
        }

        private void Subscribe()
        {
            if (operation == null || subscribed)
            {
                return;
            }

            operation.OperationStarted += HandleOperationStarted;
            operation.OperationSucceeded += HandleOperationSucceeded;
            operation.OperationFailed += HandleOperationFailed;
            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (operation == null || !subscribed)
            {
                return;
            }

            operation.OperationStarted -= HandleOperationStarted;
            operation.OperationSucceeded -= HandleOperationSucceeded;
            operation.OperationFailed -= HandleOperationFailed;
            subscribed = false;
        }
    }
}
