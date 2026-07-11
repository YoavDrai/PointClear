using UnityEngine;
using PointClear.Combat;
using PointClear.Enemies;

namespace PointClear.Utilities
{
    /// <summary>
    /// PROTOTYPE/DEBUG-ONLY. On-screen control instructions, readable
    /// player HP, a brief red flash when the player takes damage, and
    /// Sprint 1.3 crowd-scalability metrics (active/target/peak enemy
    /// count, spawn interval, approximate FPS), all via legacy OnGUI.
    /// Not intended to survive past the prototype phase — remove once real
    /// UI exists.
    ///
    /// FPS is a diagnostic approximation only (smoothed over ~0.5s of
    /// unscaled time) — not a substitute for Profiler-measured frame time,
    /// and not reliable when sampled through an unattended automated
    /// session (see PC-003 Sprint 1.3 report).
    /// </summary>
    public class DebugHud : MonoBehaviour
    {
        [SerializeField]
        private Health playerHealth;

        [SerializeField]
        private EnemySpawner spawner;

        [SerializeField]
        private float damageFlashDuration = 0.15f;

        [SerializeField]
        private int maxEnemyHpLinesShown = 5;

        private GUIStyle controlsStyle;
        private GUIStyle hpStyle;
        private GUIStyle enemyHpStyle;
        private GUIStyle metricsStyle;
        private Texture2D flashTexture;
        private float flashTimer;

        private float fpsAccumulator;
        private int fpsFrameCount;
        private float fpsTimer;
        private float currentFps;

        private void Awake()
        {
            flashTexture = new Texture2D(1, 1);
            flashTexture.SetPixel(0, 0, Color.white);
            flashTexture.Apply();
        }

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.Damaged += HandlePlayerDamaged;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.Damaged -= HandlePlayerDamaged;
            }
        }

        private void HandlePlayerDamaged(float amount)
        {
            flashTimer = damageFlashDuration;
        }

        private void Update()
        {
            if (flashTimer > 0f)
            {
                flashTimer -= Time.deltaTime;
            }

            fpsAccumulator += 1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f);
            fpsFrameCount++;
            fpsTimer += Time.unscaledDeltaTime;
            if (fpsTimer >= 0.5f)
            {
                currentFps = fpsAccumulator / fpsFrameCount;
                fpsAccumulator = 0f;
                fpsFrameCount = 0;
                fpsTimer = 0f;
            }
        }

        private void OnGUI()
        {
            EnsureStyles();

            if (flashTimer > 0f)
            {
                Color previousColor = GUI.color;
                GUI.color = new Color(1f, 0f, 0f, 0.25f * (flashTimer / damageFlashDuration));
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), flashTexture);
                GUI.color = previousColor;
            }

            GUILayout.BeginArea(new Rect(10, 10, 380, 420));

            GUILayout.Label("WASD — Move", controlsStyle);
            GUILayout.Label("Mouse — Aim", controlsStyle);
            GUILayout.Label("Left Mouse Button — Shoot", controlsStyle);

            GUILayout.Space(12);

            if (playerHealth != null)
            {
                float pct = playerHealth.MaxHealth > 0f ? playerHealth.CurrentHealth / playerHealth.MaxHealth : 0f;
                hpStyle.normal.textColor = pct > 0.5f ? Color.green : (pct > 0.2f ? new Color(1f, 0.6f, 0f) : Color.red);
                GUILayout.Label($"Player HP: {playerHealth.CurrentHealth:0} / {playerHealth.MaxHealth:0}", hpStyle);
            }

            GUILayout.Space(12);
            GUILayout.Label("— Sprint 1.3 Crowd Metrics (diagnostic) —", metricsStyle);
            GUILayout.Label($"Active Enemies: {EnemyAI.ActiveCount} / {(spawner != null ? spawner.CurrentTarget.ToString() : "?")} (max {(spawner != null ? spawner.TargetActiveCount.ToString() : "?")})", metricsStyle);
            GUILayout.Label($"Peak Active: {EnemyAI.PeakActiveCount}", metricsStyle);
            GUILayout.Label($"Spawn Interval: {(spawner != null ? spawner.SpawnInterval.ToString("0.00") : "?")}s", metricsStyle);
            GUILayout.Label($"FPS (approx, diagnostic only): {currentFps:0}", metricsStyle);

            GUILayout.Space(8);

            Health[] allHealth = FindObjectsByType<Health>(FindObjectsInactive.Exclude);
            int enemyIndex = 0;
            int shown = 0;
            foreach (Health h in allHealth)
            {
                if (h == playerHealth)
                {
                    continue;
                }

                enemyIndex++;
                if (shown < maxEnemyHpLinesShown)
                {
                    GUILayout.Label($"Enemy {enemyIndex} HP: {h.CurrentHealth:0} / {h.MaxHealth:0}", enemyHpStyle);
                    shown++;
                }
            }

            int remaining = enemyIndex - shown;
            if (remaining > 0)
            {
                GUILayout.Label($"(+{remaining} more enemies not listed)", enemyHpStyle);
            }

            GUILayout.EndArea();
        }

        private void EnsureStyles()
        {
            if (controlsStyle != null)
            {
                return;
            }

            controlsStyle = new GUIStyle(GUI.skin.label) { fontSize = 14 };
            controlsStyle.normal.textColor = Color.white;

            hpStyle = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };

            enemyHpStyle = new GUIStyle(GUI.skin.label) { fontSize = 12 };
            enemyHpStyle.normal.textColor = new Color(1f, 0.6f, 0.6f);

            metricsStyle = new GUIStyle(GUI.skin.label) { fontSize = 14, fontStyle = FontStyle.Bold };
            metricsStyle.normal.textColor = Color.cyan;
        }
    }
}
