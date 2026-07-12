using System;
using System.Collections.Generic;
using UnityEngine;
using PointClear.Combat;
using PointClear.Player;

namespace PointClear.Enemies
{
    /// <summary>
    /// PROTOTYPE-ONLY continuous enemy spawner for Sprint 1.3's crowd
    /// scalability validation. Owns spawn timing, spawn-point selection, and
    /// the configured population target. Does NOT own the active-enemy
    /// count — that is authoritative on EnemyAI itself
    /// (see EnemyAI.ActiveCount) so counting stays correct regardless of
    /// how an enemy becomes inactive.
    ///
    /// Behavior: the allowed active-enemy target ramps up gradually from
    /// initialActiveTarget to targetActiveCount (the configured maximum),
    /// increasing by targetIncreaseAmount every targetIncreaseInterval
    /// seconds — this is a population ceiling ramp, not a wave system.
    /// Once at (or above) the current ramped target, the spawner holds
    /// steady-state by spawning a replacement whenever the active count
    /// drops below it. Never intentionally exceeds the current ramped
    /// target. Never bulk-spawns — at most one enemy per interval tick.
    ///
    /// To reproduce Sprint 1.3's original fixed-target test methodology
    /// (immediate 20/50/100 target, no ramp), set initialActiveTarget equal
    /// to targetActiveCount — the ramp then starts already at the maximum
    /// and every subsequent tick is a clamped no-op.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject enemyPrefab;

        [SerializeField]
        private Transform[] spawnPoints;

        [SerializeField]
        private int targetActiveCount = 20;

        [SerializeField]
        private float spawnInterval = 0.5f;

        [SerializeField]
        private Transform player;

        [SerializeField]
        private float playerSafetyDistance = 8f;

        [Header("Population Ramp")]
        [SerializeField]
        private int initialActiveTarget = 5;

        [SerializeField]
        private int targetIncreaseAmount = 5;

        [SerializeField]
        private float targetIncreaseInterval = 25f;

        public int TargetActiveCount => targetActiveCount;
        public float SpawnInterval => spawnInterval;
        public int CurrentTarget => currentTarget;
        public bool SpawningEnabled => spawningEnabled;
        public int KillCount => killCount;

        /// <summary>
        /// Sprint 2.6: raised exactly once for each genuine death of an enemy
        /// this spawner instantiated (a player kill, via the enemy's existing
        /// Health.Died). Enemies removed by an Operation reset/clear are
        /// destroyed, which does NOT fire Health.Died, so they are never counted.
        /// </summary>
        public event Action EnemyKilled;

        private float nextSpawnTime;
        private int nextSpawnPointIndex;
        private bool warnedMissingPrefab;
        private bool warnedNoSpawnPoints;
        private int currentTarget;
        private float nextRampTime;
        private bool spawningEnabled;
        private int killCount;
        private readonly Dictionary<Health, Action> trackedEnemies = new Dictionary<Health, Action>();

        /// <summary>
        /// Sprint 2.6: enable spawning and reset the population ramp to its
        /// initial target — called when an Operation begins. Until this is
        /// called, the spawner is idle (the arena stays calm in the neutral
        /// Ready state).
        /// </summary>
        public void BeginSpawning()
        {
            UnsubscribeAllTracked();
            killCount = 0;
            spawningEnabled = true;
            currentTarget = Mathf.Clamp(initialActiveTarget, 0, targetActiveCount);
            nextRampTime = Time.time + targetIncreaseInterval;
            nextSpawnTime = Time.time;
        }

        /// <summary>Sprint 2.6: stop spawning — called when an Operation ends.
        /// Defensively unsubscribes from any surviving tracked enemies so no
        /// kill can be counted after the encounter has stopped.</summary>
        public void StopSpawning()
        {
            spawningEnabled = false;
            UnsubscribeAllTracked();
        }

        private void OnDisable()
        {
            UnsubscribeAllTracked();
        }

        /// <summary>Sprint 2.6: begin tracking one spawned enemy — subscribe to
        /// its existing Health.Died so a genuine death raises EnemyKilled once.</summary>
        private void TrackEnemy(GameObject enemy)
        {
            if (enemy == null || !enemy.TryGetComponent(out Health health))
            {
                return;
            }

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
            }

            trackedEnemies.Remove(health);
            killCount++;
            EnemyKilled?.Invoke();
        }

        private void UnsubscribeAllTracked()
        {
            foreach (KeyValuePair<Health, Action> pair in trackedEnemies)
            {
                if (pair.Key != null)
                {
                    pair.Key.Died -= pair.Value;
                }
            }

            trackedEnemies.Clear();
        }

        private void Awake()
        {
            currentTarget = Mathf.Clamp(initialActiveTarget, 0, targetActiveCount);
            nextRampTime = Time.time + targetIncreaseInterval;
        }

        private void Update()
        {
            // Sprint 2.6: the Operation lifecycle owns when the arena is live.
            if (!spawningEnabled)
            {
                return;
            }

            if (player == null)
            {
                player = PlayerReference.Instance;
            }

            UpdateRamp();

            if (Time.time < nextSpawnTime)
            {
                return;
            }

            if (EnemyAI.ActiveCount >= currentTarget)
            {
                return;
            }

            if (enemyPrefab == null)
            {
                if (!warnedMissingPrefab)
                {
                    Debug.LogWarning("EnemySpawner: no enemy prefab assigned — spawning disabled.", this);
                    warnedMissingPrefab = true;
                }

                return;
            }

            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                if (!warnedNoSpawnPoints)
                {
                    Debug.LogWarning("EnemySpawner: no spawn points configured — spawning disabled.", this);
                    warnedNoSpawnPoints = true;
                }

                return;
            }

            nextSpawnTime = Time.time + spawnInterval;

            Transform spawnPoint = SelectSpawnPoint();
            if (spawnPoint == null)
            {
                // No valid (safe-distance) spawn point this tick — skip,
                // try again next interval. Do not force a spawn.
                return;
            }

            GameObject spawned = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            TrackEnemy(spawned);
        }

        /// <summary>
        /// Advances the current ramped target toward targetActiveCount by
        /// targetIncreaseAmount every targetIncreaseInterval seconds.
        /// Monotonically increasing and clamped to the configured maximum
        /// — never decreases, never exceeds it.
        /// </summary>
        private void UpdateRamp()
        {
            if (currentTarget >= targetActiveCount)
            {
                return;
            }

            if (Time.time < nextRampTime)
            {
                return;
            }

            currentTarget = Mathf.Min(currentTarget + targetIncreaseAmount, targetActiveCount);
            nextRampTime = Time.time + targetIncreaseInterval;
        }

        /// <summary>
        /// Round-robin selection with a safe skip for null entries and
        /// entries too close to the player. Bounded to spawnPoints.Length
        /// attempts so a fully-invalid list can't loop forever.
        /// </summary>
        private Transform SelectSpawnPoint()
        {
            for (int attempts = 0; attempts < spawnPoints.Length; attempts++)
            {
                Transform candidate = spawnPoints[nextSpawnPointIndex];
                nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;

                if (candidate == null)
                {
                    continue;
                }

                if (player != null && Vector3.Distance(candidate.position, player.position) < playerSafetyDistance)
                {
                    continue;
                }

                return candidate;
            }

            return null;
        }
    }
}
