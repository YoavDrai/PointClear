using UnityEngine;
using PointClear.Operations;

namespace PointClear.Utilities
{
    /// <summary>
    /// Sprint 2.6: minimal prototype HUD (OnGUI) for the Operation lifecycle —
    /// shows the current state, the objective/timer, the outcome, and dev
    /// Start / Return-to-Ready controls. Prototype only, matching DebugHud and
    /// SkillAllocationHud; not production UI.
    ///
    /// Sprint 2.7: also shows the CurrencyWallet — Unsecured (at risk this run)
    /// vs. Banked (secured) — so Success (Unsecured → Banked) and Failure
    /// (Unsecured lost) read clearly.
    /// </summary>
    public class OperationHud : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        [SerializeField]
        private CurrencyWallet wallet;

        private GUIStyle titleStyle;

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
            }

            // Right column, stacked BELOW the Skills panel (which occupies
            // Screen.width-320 .. y 10-270) so the two prototype HUDs don't overlap.
            const float width = 310f;
            GUILayout.BeginArea(new Rect(Screen.width - width - 10f, 285f, width, 220f), GUI.skin.box);

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
                    GUILayout.Label(operation.ExtractionOpen
                        ? "EXTRACTION: OPEN — reach the exit!"
                        : "EXTRACTION: LOCKED — clear the quota");
                    break;

                case OperationState.Succeeded:
                    GUILayout.Label("SUCCESS — extracted.");
                    if (GUILayout.Button("Return to Ready"))
                    {
                        operation.ReturnToReady();
                    }
                    break;

                case OperationState.Failed:
                    GUILayout.Label("FAILED — you died.");
                    if (GUILayout.Button("Return to Ready"))
                    {
                        operation.ReturnToReady();
                    }
                    break;
            }

            if (wallet != null)
            {
                GUILayout.Space(6f);
                GUILayout.Label($"Unsecured (at risk): {wallet.Unsecured}");
                GUILayout.Label($"Banked (secured): {wallet.Banked}");
            }

            GUILayout.EndArea();
        }
    }
}
