using UnityEngine;
using PointClear.Skills;

namespace PointClear.Utilities
{
    /// <summary>
    /// PROTOTYPE/DEBUG-ONLY minimal Skill Point allocation panel (legacy OnGUI,
    /// no polish). Shows available points and, per registered skill, its rank
    /// and an Allocate button that calls SkillProgression.TryAllocate. The
    /// button is disabled when no points remain or the skill is at max level.
    /// Not the final skill UI — replace once real UI exists (Sprint 2.4 scope).
    /// </summary>
    public class SkillAllocationHud : MonoBehaviour
    {
        [SerializeField]
        private SkillPoints skillPoints;

        [SerializeField]
        private SkillProgression progression;

        private GUIStyle headerStyle;
        private GUIStyle rowStyle;

        private void OnGUI()
        {
            if (skillPoints == null || progression == null)
            {
                return;
            }

            EnsureStyles();

            GUILayout.BeginArea(new Rect(Screen.width - 320, 10, 310, 260));
            GUILayout.Label("— Skills (prototype) —", headerStyle);
            GUILayout.Label($"Skill Points: {skillPoints.Available}", headerStyle);
            GUILayout.Space(6);

            foreach (SkillDefinition definition in progression.RegisteredSkills)
            {
                if (definition == null)
                {
                    continue;
                }

                int level = progression.GetLevel(definition);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{definition.DisplayName} ({definition.SkillType})  Lv {level}/{definition.MaxLevel}", rowStyle, GUILayout.Width(210));

                bool canAllocate = progression.CanAllocate(definition);
                GUI.enabled = canAllocate;
                if (GUILayout.Button(level >= definition.MaxLevel ? "MAX" : "+", GUILayout.Width(70)))
                {
                    progression.TryAllocate(definition);
                }
                GUI.enabled = true;

                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }

        private void EnsureStyles()
        {
            if (headerStyle != null)
            {
                return;
            }

            headerStyle = new GUIStyle(GUI.skin.label) { fontSize = 14, fontStyle = FontStyle.Bold };
            headerStyle.normal.textColor = Color.cyan;

            rowStyle = new GUIStyle(GUI.skin.label) { fontSize = 13 };
            rowStyle.normal.textColor = Color.white;
        }
    }
}
