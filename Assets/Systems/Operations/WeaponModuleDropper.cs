using UnityEngine;
using PointClear.Enemies;

namespace PointClear.Operations
{
    /// <summary>
    /// Sprint 2.9: deterministically drops exactly one Weapon Module per run on a
    /// fixed kill index (default the 4th kill), so every playtest run guarantees
    /// the fear-of-loss experiment — no RNG. Because the drop index is below the
    /// Operation's extraction kill-quota, any run that reaches extraction has
    /// necessarily passed the drop and carried the module.
    ///
    /// Counts via the existing EnemySpawner.EnemyKilled signal (authoritative,
    /// post-increment) and drops at EnemySpawner.LastKillPosition so the module
    /// falls where the enemy died — no change to the parameterless EnemyKilled
    /// signature that OperationController consumes. Skips the drop entirely if the
    /// player already owns (banked) the module. Resets on OperationStarted.
    /// </summary>
    public class WeaponModuleDropper : MonoBehaviour
    {
        [SerializeField]
        private OperationController operation;

        [SerializeField]
        private EnemySpawner enemySpawner;

        [SerializeField]
        private WeaponModule playerModule;

        [SerializeField]
        private GameObject modulePickupPrefab;

        [Tooltip("Which kill of the run drops the module (1-based). Kept below the Operation kill quota so every extraction run carries it. Prototype tuning.")]
        [Min(1)]
        [SerializeField]
        private int dropOnKill = 4;

        [Tooltip("Sprint 2.9 review: the module floats ABOVE the ground-level currency coin (which drops at +0.5 from the same enemy) so it is never buried inside a coin/corpse. Kept within the pickup's attract radius so walking under it still collects.")]
        [SerializeField]
        private float dropHeight = 1.2f;

        private int killsThisRun;
        private bool droppedThisRun;
        private bool operationSubscribed;
        private bool spawnerSubscribed;

        private void Awake()
        {
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            if (enemySpawner == null)
            {
                enemySpawner = FindFirstObjectByType<EnemySpawner>();
            }
            if (playerModule == null)
            {
                playerModule = FindFirstObjectByType<WeaponModule>();
            }
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
            if (operation == null)
            {
                operation = FindFirstObjectByType<OperationController>();
            }
            if (enemySpawner == null)
            {
                enemySpawner = FindFirstObjectByType<EnemySpawner>();
            }
            if (playerModule == null)
            {
                playerModule = FindFirstObjectByType<WeaponModule>();
            }
            Subscribe();
        }

        private void HandleOperationStarted()
        {
            killsThisRun = 0;
            droppedThisRun = false;
        }

        private void HandleEnemyKilled()
        {
            if (droppedThisRun || modulePickupPrefab == null)
            {
                return;
            }

            // Already-owned module never drops again (Game-Director clarification).
            if (playerModule != null && (playerModule.Banked || playerModule.IsActive))
            {
                return;
            }

            killsThisRun++;
            if (killsThisRun < Mathf.Max(1, dropOnKill))
            {
                return;
            }

            droppedThisRun = true;

            Vector3 position = enemySpawner != null
                ? enemySpawner.LastKillPosition + Vector3.up * dropHeight
                : transform.position + Vector3.up * dropHeight;

            Instantiate(modulePickupPrefab, position, Quaternion.identity);
        }

        private void Subscribe()
        {
            if (operation != null && !operationSubscribed)
            {
                operation.OperationStarted += HandleOperationStarted;
                operationSubscribed = true;
            }

            if (enemySpawner != null && !spawnerSubscribed)
            {
                enemySpawner.EnemyKilled += HandleEnemyKilled;
                spawnerSubscribed = true;
            }
        }

        private void Unsubscribe()
        {
            if (operation != null && operationSubscribed)
            {
                operation.OperationStarted -= HandleOperationStarted;
                operationSubscribed = false;
            }

            if (enemySpawner != null && spawnerSubscribed)
            {
                enemySpawner.EnemyKilled -= HandleEnemyKilled;
                spawnerSubscribed = false;
            }
        }
    }
}
