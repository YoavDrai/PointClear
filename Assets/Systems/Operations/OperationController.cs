using System;
using UnityEngine;
using PointClear.Combat;
using PointClear.Enemies;
using PointClear.Player;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.6 (Cluster B): the minimal Operation wrapper. Turns the endless
    /// arena into a bounded run with an explicit start, a placeholder success
    /// condition (defeat a small enemy-kill quota, then reach the Extraction
    /// Point), and failure on player death — returning cleanly to a neutral
    /// Ready state.
    ///
    /// It owns ONLY the outer lifecycle and orchestrates existing systems through
    /// their public surfaces (EnemySpawner population + its EnemyKilled event,
    /// PlayerRespawn reset, the player's Health.Died). It builds no rewards,
    /// securing, results, Zones, or dynamic objectives — those attach later to
    /// the OperationStarted / OperationSucceeded / OperationFailed events this
    /// class exposes (Sprint 2.7+).
    ///
    /// A run reset restores the ENCOUNTER only (player health/position, enemy
    /// population, kill count); it never touches persistent progression — Level,
    /// Experience, and Skill Points survive a run reset untouched (DEC-016).
    /// </summary>
    public class OperationController : MonoBehaviour
    {
        [Header("Scene references")]
        [SerializeField]
        private EnemySpawner enemySpawner;

        [SerializeField]
        private PlayerRespawn playerRespawn;

        [Tooltip("The player's Health. Auto-resolved from PlayerReference if left empty.")]
        [SerializeField]
        private Health playerHealth;

        [SerializeField]
        private ExtractionPoint extractionPoint;

        [Header("Objective (placeholder — Sprint 2.6)")]
        [Tooltip("Enemy kills required before the Extraction Point opens. Prototype tuning.")]
        [Min(1)]
        [SerializeField]
        private int killQuota = 8;

        public OperationState State { get; private set; } = OperationState.Ready;
        public bool ExtractionOpen { get; private set; }
        public int CurrentKills { get; private set; }
        public int KillQuota => Mathf.Max(1, killQuota);

        /// <summary>Raised when a run begins (Ready → InProgress).</summary>
        public event Action OperationStarted;
        /// <summary>Raised exactly once when a run is won.</summary>
        public event Action OperationSucceeded;
        /// <summary>Raised exactly once when a run is lost (player death).</summary>
        public event Action OperationFailed;

        private bool healthSubscribed;
        private bool spawnerSubscribed;

        private void Awake()
        {
            if (enemySpawner == null)
            {
                enemySpawner = FindFirstObjectByType<EnemySpawner>();
            }

            if (extractionPoint == null)
            {
                extractionPoint = FindFirstObjectByType<ExtractionPoint>();
            }

            ResolvePlayer();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Start()
        {
            // References may not be resolvable until other Awakes have run;
            // resolve/subscribe once more, then establish the neutral Ready state.
            ResolvePlayer();
            Subscribe();
            EnterReady();
        }

        /// <summary>Dev control: begins a run from the Ready state.</summary>
        public void StartOperation()
        {
            if (State != OperationState.Ready)
            {
                return;
            }

            ClearEnemies();
            if (playerRespawn != null)
            {
                playerRespawn.ResetPlayer();
            }
            CloseExtraction();
            CurrentKills = 0;

            State = OperationState.InProgress;

            if (enemySpawner != null)
            {
                enemySpawner.BeginSpawning();
            }

            OperationStarted?.Invoke();
        }

        /// <summary>Dev control: returns to the neutral Ready state after a run.</summary>
        public void ReturnToReady()
        {
            EnterReady();
        }

        private void EnterReady()
        {
            if (enemySpawner != null)
            {
                enemySpawner.StopSpawning();
            }
            ClearEnemies();
            CloseExtraction();
            CurrentKills = 0;
            if (playerRespawn != null)
            {
                playerRespawn.ResetPlayer();
            }
            State = OperationState.Ready;
        }

        private void HandlePlayerDied()
        {
            if (State != OperationState.InProgress)
            {
                return;
            }

            State = OperationState.Failed;
            EndCombat();
            if (playerRespawn != null)
            {
                playerRespawn.SetDefeated();
            }
            OperationFailed?.Invoke();
        }

        private void HandleEnemyKilled()
        {
            // Ignore kills unless a run is live and the quota is not yet met —
            // this also drops any notifications after Success/Failure and
            // guarantees the Extraction Point unlocks exactly once.
            if (State != OperationState.InProgress || ExtractionOpen)
            {
                return;
            }

            CurrentKills++;
            if (CurrentKills >= KillQuota)
            {
                OpenExtraction();
            }
        }

        /// <summary>Called by ExtractionPoint when the player reaches an open exit.</summary>
        public void NotifyExtractionReached()
        {
            if (State != OperationState.InProgress || !ExtractionOpen)
            {
                return;
            }

            State = OperationState.Succeeded;
            EndCombat();
            OperationSucceeded?.Invoke();
        }

        private void EndCombat()
        {
            if (enemySpawner != null)
            {
                enemySpawner.StopSpawning();
            }
            ClearEnemies();
        }

        private void OpenExtraction()
        {
            ExtractionOpen = true;
            if (extractionPoint != null)
            {
                extractionPoint.SetOpen(true);
            }
        }

        private void CloseExtraction()
        {
            ExtractionOpen = false;
            if (extractionPoint != null)
            {
                extractionPoint.SetOpen(false);
            }
        }

        private void ResolvePlayer()
        {
            if (PlayerReference.Instance == null)
            {
                return;
            }

            if (playerHealth == null)
            {
                playerHealth = PlayerReference.Instance.GetComponent<Health>();
            }

            if (playerRespawn == null)
            {
                playerRespawn = PlayerReference.Instance.GetComponent<PlayerRespawn>();
            }
        }

        private void Subscribe()
        {
            if (playerHealth != null && !healthSubscribed)
            {
                playerHealth.Died += HandlePlayerDied;
                healthSubscribed = true;
            }

            if (enemySpawner != null && !spawnerSubscribed)
            {
                enemySpawner.EnemyKilled += HandleEnemyKilled;
                spawnerSubscribed = true;
            }
        }

        private void Unsubscribe()
        {
            if (playerHealth != null && healthSubscribed)
            {
                playerHealth.Died -= HandlePlayerDied;
                healthSubscribed = false;
            }

            if (enemySpawner != null && spawnerSubscribed)
            {
                enemySpawner.EnemyKilled -= HandleEnemyKilled;
                spawnerSubscribed = false;
            }
        }

        private static void ClearEnemies()
        {
            EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    Destroy(enemies[i].gameObject);
                }
            }
        }
    }
}
