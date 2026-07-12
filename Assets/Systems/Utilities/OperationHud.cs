using UnityEngine;
using PointClear.Operations;

namespace PointClear.Utilities
{
    /// <summary>
    /// Sprint 2.6: minimal prototype HUD (OnGUI) for the Operation lifecycle —
    /// shows the current state, the objective/timer, the outcome, and dev
    /// Start / Return-to-Ready controls. Prototype only, matching DebugHud and
    /// SkillAllocationHud; not production UI.
    /// </summary>
    public class OperationHud : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        private GUIStyle titleStyle;

        private void Awake()
        {
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
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

            const float width = 300f;
            GUILayout.BeginArea(new Rect(Screen.width - width - 12f, 12f, width, 160f), GUI.skin.box);

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

            GUILayout.EndArea();
        }
    }
}
