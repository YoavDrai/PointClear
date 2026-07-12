using System;
using System.Collections.Generic;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// Sprint 2.11 — composition / pacing ramp. An Operation escalates through
    /// kill-driven PHASES: each phase maintains a chaser population and, on entry,
    /// injects a one-time group of special enemies. Enemy variety and density grow
    /// over the run (weak opening → building → intense) instead of starting fully
    /// active. Phase advance is driven by kills (earned, self-pacing, synced with
    /// the player's XP/level ramp) with a per-phase time backstop.
    ///
    /// Kills are tracked (EnemyKilled → OperationController quota) exactly as before.
    /// Chasers (EnemyAI) are cleared by OperationController.ClearEnemies; injected
    /// specials (their own AI) are cleared here on StopSpawning. Not a wave system —
    /// chasers are maintained continuously to the current phase's target.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [Serializable]
        public class Phase
        {
            public string name = "Phase";
            [Tooltip("Kills required to ENTER this phase (kill-driven escalation).")]
            public int killsToEnter = 0;
            [Tooltip("Active chaser population maintained during this phase.")]
            public int chaserTarget = 3;
            [Tooltip("One-time special enemies injected when this phase begins.")]
            public int chargersToInject = 0;
            public int empowerersToInject = 0;
            public int surroundersToInject = 0;
            [Tooltip("Advance to the next phase after this many seconds even without enough kills (safety backstop).")]
            public float timeBackstop = 45f;
        }

        [SerializeField] private GameObject enemyPrefab;   // the chaser
        [SerializeField] private GameObject chargerPrefab;
        [SerializeField] private GameObject empowererPrefab;
        [SerializeField] private GameObject surrounderPrefab;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float spawnInterval = 0.5f;
        [SerializeField] private Transform player;
        [SerializeField] private float playerSafetyDistance = 8f;

        [Header("Composition Ramp (Sprint 2.11) — leave empty to use code defaults")]
        [SerializeField] private Phase[] phases;

        public float SpawnInterval => spawnInterval;
        public bool SpawningEnabled => spawningEnabled;
        public int KillCount => killCount;
        public int CurrentTarget => CurrentPhase != null ? CurrentPhase.chaserTarget : 0;
        public int TargetActiveCount { get { int m = 0; if (phases != null) foreach (var p in phases) m = Mathf.Max(m, p.chaserTarget); return m; } }
        public int CurrentPhaseIndex => currentPhaseIndex;
        public string CurrentPhaseName => CurrentPhase != null ? CurrentPhase.name : "-";

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
        private float phaseEnterTime;
        private bool warnedMissingPrefab;
        private bool warnedNoSpawnPoints;
        private readonly Dictionary<Health, Action> trackedEnemies = new Dictionary<Health, Action>();
        private readonly List<GameObject> spawnedSpecials = new List<GameObject>();

        private Phase CurrentPhase => (phases != null && currentPhaseIndex >= 0 && currentPhaseIndex < phases.Length) ? phases[currentPhaseIndex] : null;

        private void Awake() { EnsurePhases(); }

        private void EnsurePhases()
        {
            if (phases != null && phases.Length > 0) return;
            phases = new Phase[]
            {
                new Phase { name = "Opening",   killsToEnter = 0,  chaserTarget = 3,  timeBackstop = 30f },
                new Phase { name = "Charger",   killsToEnter = 4,  chaserTarget = 5,  chargersToInject = 1, timeBackstop = 40f },
                new Phase { name = "Empowerer", killsToEnter = 10, chaserTarget = 7,  chargersToInject = 1, empowerersToInject = 1, timeBackstop = 45f },
                new Phase { name = "Peak",      killsToEnter = 18, chaserTarget = 10, chargersToInject = 1, empowerersToInject = 1, surroundersToInject = 2, timeBackstop = 999f },
            };
        }

        public void BeginSpawning()
        {
            UnsubscribeAllTracked();
            ClearSpecials();
            EnsurePhases();
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

            // Only chasers use EnemyAI, so ActiveCount is the current chaser population.
            if (EnemyAI.ActiveCount >= p.chaserTarget) return;

            if (enemyPrefab == null)
            {
                if (!warnedMissingPrefab) { Debug.LogWarning("EnemySpawner: no chaser prefab assigned.", this); warnedMissingPrefab = true; }
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
            if (phases == null) return;
            while (currentPhaseIndex < phases.Length - 1)
            {
                Phase next = phases[currentPhaseIndex + 1];
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
