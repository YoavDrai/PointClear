using UnityEngine;
using PointClear.Progression;

namespace PointClear.Utilities
{
    /// <summary>
    /// TEMPORARY diagnostic-only XP/Level readout, isolated from DebugHud.
    /// Not the final progression UI — remove or replace once real UI
    /// exists. PlayerLevel is resolved via GetComponent rather than a
    /// second serialized field since both components live on the same
    /// Player GameObject already referenced here.
    /// </summary>
    public class XPDiagnosticDisplay : MonoBehaviour
    {
        [SerializeField]
        private PlayerXP playerXP;

        private PlayerLevel playerLevel;

        private void Awake()
        {
            if (playerXP != null)
            {
                playerLevel = playerXP.GetComponent<PlayerLevel>();
            }
        }

        private void OnGUI()
        {
            if (playerXP == null)
            {
                return;
            }

            GUI.Label(new Rect(10, 440, 300, 20), $"[Diagnostic] XP: {playerXP.CurrentXP:0}");

            if (playerLevel == null)
            {
                return;
            }

            GUI.Label(new Rect(10, 460, 300, 20), $"[Diagnostic] Level: {playerLevel.CurrentLevel}");

            float pct = playerLevel.XPRequiredForNextLevel > 0f
                ? Mathf.Clamp01(playerLevel.XPIntoCurrentLevel / playerLevel.XPRequiredForNextLevel)
                : 0f;

            Rect barBackground = new Rect(10, 480, 200, 14);
            GUI.Box(barBackground, GUIContent.none);

            Color previousColor = GUI.color;
            GUI.color = Color.cyan;
            GUI.DrawTexture(new Rect(10, 480, 200 * pct, 14), Texture2D.whiteTexture);
            GUI.color = previousColor;
        }
    }
}
