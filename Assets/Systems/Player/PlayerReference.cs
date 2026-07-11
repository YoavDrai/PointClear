using UnityEngine;

namespace PointClear.Player
{
    /// <summary>
    /// Single point of reference to the local player, replacing the
    /// duplicated GameObject.FindGameObjectWithTag("Player") lookups
    /// previously in EnemyAI and EnemySpawner. Explicitly single-player —
    /// this is not a registry and must not become a multiplayer
    /// abstraction. Reset via RuntimeInitializeOnLoadMethod so a stale
    /// reference can't survive between Play sessions.
    ///
    /// Consumers must resolve Instance lazily (e.g. in Update/FixedUpdate,
    /// not Awake) since Unity does not guarantee Awake() order across
    /// different GameObjects — by the time any object's first Update or
    /// FixedUpdate runs, every object's Awake (including this one) has
    /// already completed.
    /// </summary>
    public class PlayerReference : MonoBehaviour
    {
        public static Transform Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            Instance = null;
        }

        private void Awake()
        {
            Instance = transform;
        }

        private void OnDestroy()
        {
            if (Instance == transform)
            {
                Instance = null;
            }
        }
    }
}
