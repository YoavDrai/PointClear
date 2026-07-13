using System;
using UnityEngine;
using PointClear.Operations;

namespace PointClear.Progression
{
    /// <summary>
    /// Applies a configurable XP penalty when an Operation is failed by player
    /// death (DEC-032). Resolves DEC-019's previously-open "exact retained
    /// Experience amount on failure": levels and Skill Points are never lost —
    /// only progress into the CURRENT level is reduced.
    ///
    /// Penalty math (flat % of the current level's bar): the amount removed is
    /// penaltyFraction * (XP required for the current level). It is capped at
    /// XPIntoCurrentLevel so the total can never drop below the current level's
    /// floor — the character never de-levels. Example at penaltyFraction 0.2:
    /// Level 10 at 40% → Level 10 at 20%.
    ///
    /// This is a run-scoped RULE reacting to the existing Operation lifecycle,
    /// mirroring CurrencyWallet / WeaponModule: it subscribes to
    /// OperationFailed and never modifies the Operation scripts. Death OUTSIDE
    /// an Operation (e.g. the prototype respawn loop) does not trigger it — the
    /// penalty is part of the mission-risk loop, not raw death.
    ///
    /// Single-player scope (matches EnemyXPReward / PlayerReference): it applies
    /// to the local player it lives on. A future multiplayer rule for whose
    /// progression is penalised on a party wipe changes only this component.
    /// </summary>
    [RequireComponent(typeof(PlayerXP))]
    [RequireComponent(typeof(PlayerLevel))]
    public class DeathXPPenalty : MonoBehaviour
    {
        [Tooltip("Fraction of the current level's XP bar removed on Operation failure. 0.2 = lose 20% of a level's worth of progress. Levels are never lost. Configurable per DEC-032; not a balance decision.")]
        [Range(0f, 1f)]
        [SerializeField]
        private float penaltyFraction = 0.2f;

        [Tooltip("Scene Operation whose failure triggers the penalty. Auto-resolved if left empty.")]
        [SerializeField]
        private OperationController operation;

        /// <summary>XP removed by the most recent failure (0 if none yet, or if the
        /// player was at a level floor). Read-only, for the results summary HUD.</summary>
        public float LastPenaltyApplied { get; private set; }

        /// <summary>Raised after a penalty is applied; argument is the amount removed.</summary>
        public event Action<float> PenaltyApplied;

        private PlayerXP playerXP;
        private PlayerLevel playerLevel;
        private bool subscribed;

        private void Awake()
        {
            playerXP = GetComponent<PlayerXP>();
            playerLevel = GetComponent<PlayerLevel>();

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
            // The OperationController may resolve later than this component's Awake.
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            Subscribe();
        }

        private void HandleOperationFailed()
        {
            // Read the current level's boundaries at failure time. XP is still
            // untouched here — nothing in the failure/return path removes XP
            // except this component.
            float bar = playerLevel.XPRequiredForNextLevel;
            float intoLevel = playerLevel.XPIntoCurrentLevel;

            // Flat % of the level bar, capped so the character never de-levels.
            float loss = Mathf.Min(penaltyFraction * bar, intoLevel);

            LastPenaltyApplied = loss;

            if (loss > 0f)
            {
                playerXP.RemoveXP(loss);
            }

            PenaltyApplied?.Invoke(loss);
        }

        private void Subscribe()
        {
            if (operation == null || subscribed)
            {
                return;
            }

            operation.OperationFailed += HandleOperationFailed;
            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (operation == null || !subscribed)
            {
                return;
            }

            operation.OperationFailed -= HandleOperationFailed;
            subscribed = false;
        }
    }
}
