using System;
using System.Collections.Generic;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// Block 1 (PC-017) — Enemy Variety, onboarding by Run.
    ///
    /// The spawner now separates its two escalation axes (the change Block 1 exists
    /// to make):
    ///  - ENEMY-TYPE INTRODUCTION is cross-RUN: each Run has its own composition
    ///    (RunPlan), so the onboarding introduces ONE new question per Run —
    ///    Run 1 Walker (move &amp; shoot) → Run 2 +Charger (dodge a telegraph) →
    ///    Run 3 +Surrounder (keep moving) → Run 4 +Empowerer (priority target).
    ///    Each new type gets a clean low-density debut, then folds into the mix.
    ///  - DENSITY / INTENSITY is within-RUN: each Run's Phases are authored as an
    ///    Arena-Rhythm curve (pressure → lull → build), never a flat wall.
    ///
    /// The Run advances by one on each BeginSpawning() (i.e. each Operation start),
    /// clamped to the last authored Run. This is a minimal per-Run composition
    /// SELECTOR only — NOT the deferred Run-Cycle / Difficulty-Tier Controller (no
    /// tier scaling, no boss cadence, no success-gated progression). `startRunNumber`
    /// lets a playtester jump straight to a Run.
    ///
    /// Kills are tracked (EnemyKilled → OperationController quota) exactly as before;
    /// injected specials are cleared on StopSpawning. Numbers here are greybox and
    /// playtest-tunable (DEC-036) — not locked values.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Serializable]
        public class Phase
        {
            public string name = "Phase";
            [Tooltip("Kills required to ENTER this phase (kill-driven escalation, within the Run).")]
            public int killsToEnter = 0;
            [Tooltip("Active Walker (chaser) population maintained during this phase. Drop it for a lull, raise it for pressure.")]
            public int chaserTarget = 3;
            [Tooltip("One-time special enemies injected when this phase begins.")]
            public int chargersToInject = 0;
            public int empowerersToInject = 0;
            public int surroundersToInject = 0;
            [Tooltip("Advance to the next phase after this many seconds even without enough kills (safety backstop).")]
            public float timeBackstop = 45f;
        }

        [Serializable]
        public class RunPlan
        {
            public string name = "Run";
            [Tooltip("The within-Run rhythm curve: ordered phases (pressure/lull/build) using only this Run's eligible enemy types.")]
            public Phase[] phases;
        }

        [SerializeField] private GameObject enemyPrefab;   // the Walker (chaser)
        [SerializeField] private GameObject chargerPrefab;
        [SerializeField] private GameObject empowererPrefab;
        [SerializeField] private GameObject surrounderPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float spawnInterval = 0.5f;
        [SerializeField] private Transform player;
        [SerializeField] private float playerSafetyDistance = 8f;

        [Header("Onboarding — per-Run composition (leave empty to use code defaults)")]
        [SerializeField] private RunPlan[] runPlans;

        [Tooltip("Playtest helper: which Run to start on (1-based). 1 = the first onboarding Run.")]
        [Min(1)]
        [SerializeField] private int startRunNumber = 1;

        public float SpawnInterval => spawnInterval;
        public bool SpawningEnabled => spawningEnabled;
        public int KillCount => killCount;
        public int CurrentTarget => CurrentPhase != null ? CurrentPhase.chaserTarget : 0;
        public int TargetActiveCount { get { int m = 0; if (activePhases != null) foreach (var p in activePhases) m = Mathf.Max(m, p.chaserTarget); return m; } }
        public int CurrentPhaseIndex => currentPhaseIndex;
        public string CurrentPhaseName => CurrentPhase != null ? CurrentPhase.name : "-";

        /// <summary>Block 1: the currently active Run's name, for the HUD readout.</summary>
        public string CurrentRunName => (runPlans != null && currentRunIndex >= 0 && currentRunIndex < runPlans.Length) ? runPlans[currentRunIndex].name : "-";

        /// <summary>Sprint 2.9: world position of the most recently killed tracked enemy,
        /// captured before EnemyKilled fires (for loot drops without changing the event).</summary>
        public Vector3 LastKillPosition { get; private set; }

        /// <summary>Sprint 2.6: raised once per genuine death of a spawned enemy (drives the quota).</summary>
        public event Action EnemyKilled;

        private float nextSpawnTime;
        private int nextSpawnPointIndex;
        private bool spawningEnabled;
        private int killCount;
        private int currentPhaseIndex;
        private int currentRunIndex;   // Run currently active (for display)
        private int nextRunIndex;      // Run to use on the next BeginSpawning
        private float phaseEnterTime;
        private bool warnedMissingPrefab;
        private bool warnedNoSpawnPoints;
        private Phase[] activePhases;
        private readonly Dictionary<Health, Action> trackedEnemies = new Dictionary<Health, Action>();
        private readonly List<GameObject> spawnedSpecials = new List<GameObject>();

        private Phase CurrentPhase => (activePhases != null && currentPhaseIndex >= 0 && currentPhaseIndex < activePhases.Length) ? activePhases[currentPhaseIndex] : null;

        private void Awake()
        {
            EnsureRunPlans();
            nextRunIndex = Mathf.Clamp(startRunNumber - 1, 0, Mathf.Max(0, RunCount - 1));
        }

        private int RunCount => runPlans != null ? runPlans.Length : 0;

        // Code-default onboarding: one new question per Run, each Run an Arena-Rhythm
        // curve (pressure → lull → build) with the new type debuting at low density.
        // Greybox / playtest-tunable — NOT locked (DEC-036).
        private void EnsureRunPlans()
        {
            if (runPlans != null && runPlans.Length > 0) return;
            // Retuned for a ~10-kill extraction quota, "variety before density":
            // Walker concurrency stays LOW and roughly FLAT across Runs (the prior
            // Runs' pressure stays familiar); each Run's escalation is the ONE new
            // enemy, debuting early (by ~kill 2) so it is taught within the short
            // Run, with a breather beat for Arena Rhythm. Greybox / playtest-tunable.
            runPlans = new RunPlan[]
            {
                new RunPlan { name = "Run 1 — Move & Shoot", phases = new[]
                {
                    new Phase { name = "Warm-Up",  killsToEnter = 0, chaserTarget = 2, timeBackstop = 15f },
                    new Phase { name = "Breather", killsToEnter = 4, chaserTarget = 1, timeBackstop = 12f },
                    new Phase { name = "Ease",     killsToEnter = 7, chaserTarget = 2, timeBackstop = 999f },
                }},
                new RunPlan { name = "Run 2 — Read the Telegraph", phases = new[]
                {
                    new Phase { name = "Warm-Up",       killsToEnter = 0, chaserTarget = 2, timeBackstop = 12f },
                    new Phase { name = "Charger Debut", killsToEnter = 2, chaserTarget = 1, chargersToInject = 1, timeBackstop = 15f },
                    new Phase { name = "Breather",      killsToEnter = 5, chaserTarget = 2, timeBackstop = 12f },
                    new Phase { name = "Hold",          killsToEnter = 8, chaserTarget = 2, chargersToInject = 1, timeBackstop = 999f },
                }},
                new RunPlan { name = "Run 3 — Keep Moving", phases = new[]
                {
                    new Phase { name = "Warm-Up",          killsToEnter = 0, chaserTarget = 2, timeBackstop = 12f },
                    new Phase { name = "Surrounder Debut", killsToEnter = 2, chaserTarget = 1, surroundersToInject = 2, timeBackstop = 15f },
                    new Phase { name = "Breather",         killsToEnter = 5, chaserTarget = 2, timeBackstop = 12f },
                    new Phase { name = "Hold",             killsToEnter = 8, chaserTarget = 2, chargersToInject = 1, surroundersToInject = 1, timeBackstop = 999f },
                }},
                new RunPlan { name = "Run 4 — Priority Target", phases = new[]
                {
                    new Phase { name = "Warm-Up",         killsToEnter = 0, chaserTarget = 3, timeBackstop = 12f },
                    new Phase { name = "Empowerer Debut", killsToEnter = 2, chaserTarget = 3, empowerersToInject = 1, timeBackstop = 18f },
                    new Phase { name = "Breather",        killsToEnter = 6, chaserTarget = 2, timeBackstop = 12f },
                    new Phase { name = "Peak",            killsToEnter = 8, chaserTarget = 3, chargersToInject = 1, empowerersToInject = 1, timeBackstop = 999f },
                }},
            };
        }

        public void BeginSpawning()
        {
            UnsubscribeAllTracked();
            ClearSpecials();
            EnsureRunPlans();

            // Select this Run's composition, then advance the pointer for the next Run
            // (clamped to the last authored Run — no wrap, no tier scaling).
            currentRunIndex = Mathf.Clamp(nextRunIndex, 0, Mathf.Max(0, RunCount - 1));
            activePhases = RunCount > 0 ? runPlans[currentRunIndex].phases : null;
            nextRunIndex = Mathf.Min(currentRunIndex + 1, Mathf.Max(0, RunCount - 1));

            killCount = 0;
            spawningEnabled = true;
            nextSpawnTime = Time.time;
            EnterPhase(0);
        }

        public void StopSpawning()
        {
            spawningEnabled = false;
            UnsubscribeAllTracked();
            ClearSpecials();
        }

        private void EnterPhase(int index)
        {
            currentPhaseIndex = index;
            phaseEnterTime = Time.time;
            Phase p = CurrentPhase;
            if (p == null) return;
            InjectSpecial(chargerPrefab, p.chargersToInject);
            InjectSpecial(empowererPrefab, p.empowerersToInject);
            InjectSpecial(surrounderPrefab, p.surroundersToInject);
        }

        private void InjectSpecial(GameObject prefab, int count)
        {
            if (prefab == null || count <= 0 || spawnPoints == null || spawnPoints.Length == 0) return;
            for (int i = 0; i < count; i++)
            {
                Transform sp = SelectSpawnPoint();
                if (sp == null) break;
                GameObject e = Instantiate(prefab, sp.position, sp.rotation);
                TrackEnemy(e);
                spawnedSpecials.Add(e);
            }
        }

        private void Update()
        {
            if (!spawningEnabled) return;
            if (player == null) player = PlayerReference.Instance;

            AdvancePhaseIfReady();

            if (Time.time < nextSpawnTime) return;
            Phase p = CurrentPhase;
            if (p == null) return;

            // Only Walkers use EnemyAI, so ActiveCount is the current Walker population.
            if (EnemyAI.ActiveCount >= p.chaserTarget) return;

            if (enemyPrefab == null)
            {
                if (!warnedMissingPrefab) { Debug.LogWarning("EnemySpawner: no Walker prefab assigned.", this); warnedMissingPrefab = true; }
                return;
            }
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                if (!warnedNoSpawnPoints) { Debug.LogWarning("EnemySpawner: no spawn points configured.", this); warnedNoSpawnPoints = true; }
                return;
            }

            nextSpawnTime = Time.time + spawnInterval;
            Transform spawnPoint = SelectSpawnPoint();
            if (spawnPoint == null) return;
            GameObject spawned = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            TrackEnemy(spawned);
        }

        // Kill-driven, with a per-phase time backstop. Cascades if kills already
        // clear several thresholds (each entered phase injects its specials once).
        private void AdvancePhaseIfReady()
        {
            if (activePhases == null) return;
            while (currentPhaseIndex < activePhases.Length - 1)
            {
                Phase next = activePhases[currentPhaseIndex + 1];
                bool byKills = killCount >= next.killsToEnter;
                bool byTime = Time.time - phaseEnterTime >= CurrentPhase.timeBackstop;
                if (byKills || byTime) EnterPhase(currentPhaseIndex + 1);
                else break;
            }
        }

        private void TrackEnemy(GameObject enemy)
        {
            if (enemy == null || !enemy.TryGetComponent(out Health health)) return;
            Action handler = null;
            handler = () => HandleTrackedEnemyDied(health, handler);
            health.Died += handler;
            trackedEnemies[health] = handler;
        }

        private void HandleTrackedEnemyDied(Health health, Action handler)
        {
            if (health != null)
            {
                health.Died -= handler;
                LastKillPosition = health.transform.position;
            }
            trackedEnemies.Remove(health);
            killCount++;
            EnemyKilled?.Invoke();
        }

        private void UnsubscribeAllTracked()
        {
            foreach (KeyValuePair<Health, Action> pair in trackedEnemies)
                if (pair.Key != null) pair.Key.Died -= pair.Value;
            trackedEnemies.Clear();
        }

        private void ClearSpecials()
        {
            for (int i = 0; i < spawnedSpecials.Count; i++)
                if (spawnedSpecials[i] != null) Destroy(spawnedSpecials[i]);
            spawnedSpecials.Clear();
        }

        private void OnDisable() { UnsubscribeAllTracked(); }

        private Transform SelectSpawnPoint()
        {
            for (int attempts = 0; attempts < spawnPoints.Length; attempts++)
            {
                Transform candidate = spawnPoints[nextSpawnPointIndex];
                nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;
                if (candidate == null) continue;
                if (player != null && Vector3.Distance(candidate.position, player.position) < playerSafetyDistance) continue;
                return candidate;
            }
            return null;
        }
    }
}
