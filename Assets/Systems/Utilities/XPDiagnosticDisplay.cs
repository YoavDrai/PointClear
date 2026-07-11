using UnityEngine;
using PointClear.Progression;

namespace PointClear.Utilities
{
    /// <summary>
    /// TEMPORARY Sprint 2.1 diagnostic-only XP readout, isolated from
    /// DebugHud. Not the Phase 2 XP bar/level UI — that belongs to
    /// Sprint 2.2. Remove or replace once real progression UI exists.
    /// </summary>
    public class XPDiagnosticDisplay : MonoBehaviour
    {
        [SerializeField]
        private PlayerXP playerXP;

        private void OnGUI()
        {
            if (playerXP == null)
            {
                return;
            }

            GUI.Label(new Rect(10, 440, 300, 20), $"[Diagnostic] XP: {playerXP.CurrentXP:0}");
        }
    }
}
