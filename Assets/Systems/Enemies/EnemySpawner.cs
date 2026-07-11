using UnityEngine;

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
    /// Behavior: ramps up to targetActiveCount at a fixed interval, then
    /// holds steady-state by spawning a replacement whenever the active
    /// count drops below target. Never intentionally exceeds target. Never
    /// bulk-spawns — at most one enemy per interval tick.
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

        public int TargetActiveCount => targetActiveCount;
        public float SpawnInterval => spawnInterval;

        private float nextSpawnTime;
        private int nextSpawnPointIndex;
        private bool warnedMissingPrefab;
        private bool warnedNoSpawnPoints;

        private void Awake()
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }
        }

        private void Update()
        {
            if (Time.time < nextSpawnTime)
            {
                return;
            }

            if (EnemyAI.ActiveCount >= targetActiveCount)
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

            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
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
