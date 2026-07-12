using UnityEngine;
using PointClear.Operations;
using PointClear.Progression;
using PointClear.Skills;
using PointClear.Enemies;

namespace PointClear.Utilities
{
    /// <summary>
    /// Sprint 2.6: minimal prototype HUD (OnGUI) for the Operation lifecycle —
    /// shows the current state, the objective/timer, the outcome, and dev
    /// Start / Return-to-Ready controls. Prototype only, matching DebugHud and
    /// SkillAllocationHud; not production UI.
    ///
    /// Sprint 2.7: also shows the CurrencyWallet — Unsecured (at risk this run)
    /// vs. Banked (secured).
    ///
    /// Sprint 2.8: the terminal states (Succeeded/Failed) expand into a minimal
    /// results summary — Secured/Lost this run, Banked total, and Character
    /// Progress Retained (Level / Skill Points, read-only). No separate panel.
    /// </summary>
    public class OperationHud : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        [SerializeField]
        private CurrencyWallet wallet;

        [Tooltip("Sprint 2.11: for the composition-ramp phase readout.")]
        [SerializeField]
        private EnemySpawner spawner;

        [Tooltip("Sprint 2.9: the Detonator Module — for the EQUIPPED/SECURED status line and the secured/lost-this-run summary.")]
        [SerializeField]
        private WeaponModule weaponModule;

        [Tooltip("Read-only, for the retained-progress line of the results summary.")]
        [SerializeField]
        private PlayerLevel playerLevel;

        [SerializeField]
        private SkillPoints skillPoints;

        private GUIStyle titleStyle;
        private GUIStyle headerStyle;

        private void Awake()
        {
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            if (wallet == null)
            {
                wallet = FindFirstObjectByType<CurrencyWallet>();
            }
            if (weaponModule == null)
            {
                weaponModule = FindFirstObjectByType<WeaponModule>();
            }
            if (spawner == null)
            {
                spawner = FindFirstObjectByType<EnemySpawner>();
            }
            if (playerLevel == null)
            {
                playerLevel = FindFirstObjectByType<PlayerLevel>();
            }
            if (skillPoints == null)
            {
                skillPoints = FindFirstObjectByType<SkillPoints>();
            }
        }

        private void OnGUI()
        {
            if (operation == null)
            {
                return;
            }

            if (titleStyle == null)
            {
                titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 16, fontStyle = FontStyle.Bold };
                headerStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            }

            // Right column, stacked BELOW the Skills panel (which occupies
            // Screen.width-320 .. y 10-270) so the two prototype HUDs don't overlap.
            const float width = 310f;
            GUILayout.BeginArea(new Rect(Screen.width - width - 10f, 285f, width, 310f), GUI.skin.box);

            GUILayout.Label($"OPERATION: {operation.State}", titleStyle);

            switch (operation.State)
            {
                case OperationState.Ready:
                    GUILayout.Label("Objective: start a run.");
                    if (GUILayout.Button("Start Operation"))
                    {
                        operation.StartOperation();
                    }
                    break;

                case OperationState.InProgress:
                    GUILayout.Label($"Kills: {operation.CurrentKills} / {operation.KillQuota}");
                    if (spawner != null)
                    {
                        GUILayout.Label($"Phase {spawner.CurrentPhaseIndex + 1}: {spawner.CurrentPhaseName}");
                    }
                    GUILayout.Label(operation.ExtractionOpen
                        ? "EXTRACTION: OPEN — reach the exit!"
                        : "EXTRACTION: LOCKED — clear the quota");
                    break;

                case OperationState.Succeeded:
                    DrawResultsSummary(true);
                    break;

                case OperationState.Failed:
                    DrawResultsSummary(false);
                    break;
            }

            // Live currency read-out only OUTSIDE the terminal summary
            // (in the terminal states the summary carries the currency values).
            if (operation.State == OperationState.Ready || operation.State == OperationState.InProgress)
            {
                if (wallet != null)
                {
                    GUILayout.Space(6f);
                    GUILayout.Label($"Unsecured (at risk): {wallet.Unsecured}");
                    GUILayout.Label($"Banked (secured): {wallet.Banked}");
                }

                if (weaponModule != null)
                {
                    GUILayout.Label($"Weapon Module: {ModuleStatus()}");
                }
            }

            GUILayout.EndArea();
        }

        // Sprint 2.8: the minimal end-of-run results summary, rendered in the
        // terminal states inside this same panel. Secured/Lost = this run's
        // result; Banked = cumulative currency already safe; Character Progress
        // Retained = run-persistent progression that was not removed.
        private void DrawResultsSummary(bool success)
        {
            GUILayout.Label(success ? "SUCCESS — Extracted" : "FAILED — You Died", titleStyle);

            if (wallet != null)
            {
                GUILayout.Label(success
                    ? $"Secured this run: +{wallet.LastSecured}"
                    : $"Lost this run: {wallet.LastLost}");
                GUILayout.Label($"Banked total: {wallet.Banked}");
            }

            // Sprint 2.9: whether the Detonator Module was secured or lost this run.
            if (weaponModule != null && (weaponModule.SecuredThisRun || weaponModule.LostThisRun))
            {
                GUILayout.Label(success
                    ? "Weapon Module: SECURED this run"
                    : "Weapon Module: LOST this run");
            }

            GUILayout.Space(4f);
            GUILayout.Label("Character Progress Retained", headerStyle);
            GUILayout.Label($"Level: {(playerLevel != null ? playerLevel.CurrentLevel.ToString() : "-")}");
            GUILayout.Label($"Skill Points: {(skillPoints != null ? skillPoints.Available.ToString() : "-")}");

            GUILayout.Space(4f);
            if (GUILayout.Button("Return to Ready"))
            {
                operation.ReturnToReady();
            }
        }

        // Sprint 2.9: the Detonator Module's ownership at a glance.
        private string ModuleStatus()
        {
            if (weaponModule.Banked)
            {
                return "SECURED";
            }
            return weaponModule.Equipped ? "EQUIPPED (at risk)" : "—";
        }
    }
}
