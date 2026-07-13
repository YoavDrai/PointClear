using System;
using UnityEngine;
using PointClear.Progression;

namespace PointClear.Skills
{
    /// <summary>
    /// Sprint 2.4: the Skill Point wallet. Earns one point for every character
    /// level gained (PlayerLevel.LevelUp fires once per level crossed, so a
    /// multi-level XP award correctly grants multiple points). Points are
    /// run-persistent — they last for the current play session and are only
    /// removed by spending; disk save/load is out of scope for this sprint.
    ///
    /// Spending goes through TrySpend, which never lets the balance go negative.
    /// The wallet knows nothing about individual skills; SkillProgression
    /// coordinates validation and spending.
    ///
    /// PC-016: a new character begins with a configurable pool of starting Skill
    /// Points (default 2) to invest before the first run. This grant fires
    /// exactly ONCE per component instance, in Awake, guarded by a flag — never
    /// on OnEnable/re-enable, Operation reset, death, respawn, or repeated init
    /// (those do not create a new instance and do not re-run Awake).
    ///
    /// TEMPORARY new-character model (no save system exists yet): a fresh
    /// component instance == a new character, because "New Character" is a scene
    /// reload producing fresh Level-1 state (DEC-031). So today a scene reload
    /// legitimately grants the starter — it IS a new character. The future
    /// save/load seam is LoadSavedBalance(): when persistence exists, the
    /// character loader calls it to restore a saved balance, which SETS (not
    /// adds) Available and suppresses the starter — making "an EXISTING character
    /// must not be re-granted the starter, even across a scene reload" true then,
    /// order-independently (it overwrites any starter already granted this load).
    /// </summary>
    [RequireComponent(typeof(PlayerLevel))]
    public class SkillPoints : MonoBehaviour
    {
        [Tooltip("Skill points granted per character level gained.")]
        [Min(0)]
        [SerializeField]
        private int pointsPerLevel = 1;

        [Tooltip("Skill points a NEW character starts with, granted once before the first run. Default 2.")]
        [Min(0)]
        [SerializeField]
        private int startingPoints = 2;

        public int Available { get; private set; }

        /// <summary>Raised whenever Available changes; argument is the new total.</summary>
        public event Action<int> Changed;

        private PlayerLevel playerLevel;

        // Guards the one-time starter grant / save restore. Instance field, so it
        // resets only when a new instance is created (a scene reload = a new
        // character today) — never on OnEnable, Operation reset, death, or respawn.
        private bool initialized;

        private void Awake()
        {
            playerLevel = GetComponent<PlayerLevel>();
            GrantStartingPointsOnce();
        }

        // Fires exactly once per instance. Any later call (e.g. a defensive
        // re-init) is a no-op, and a save restore that already ran suppresses it.
        private void GrantStartingPointsOnce()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            if (startingPoints > 0)
            {
                Available += startingPoints;
                Changed?.Invoke(Available);
            }
        }

        /// <summary>
        /// FUTURE SAVE/LOAD SEAM (no caller yet). When a save system exists, the
        /// character loader calls this to restore an EXISTING character's balance.
        /// It SETS Available to the saved value (never adds) and marks the wallet
        /// initialized, so the new-character starter is never duplicated — safe
        /// regardless of whether it runs before or after Awake's grant.
        /// </summary>
        public void LoadSavedBalance(int savedAvailable)
        {
            initialized = true;
            Available = Mathf.Max(0, savedAvailable);
            Changed?.Invoke(Available);
        }

        private void OnEnable()
        {
            playerLevel.LevelUp += HandleLevelUp;
        }

        private void OnDisable()
        {
            playerLevel.LevelUp -= HandleLevelUp;
        }

        private void HandleLevelUp(int newLevel)
        {
            if (pointsPerLevel <= 0)
            {
                return;
            }

            Available += pointsPerLevel;
            Changed?.Invoke(Available);
        }

        /// <summary>Spends one point if any are available. Returns false (and
        /// changes nothing) when the balance is zero — never goes negative.</summary>
        public bool TrySpend()
        {
            if (Available <= 0)
            {
                return false;
            }

            Available--;
            Changed?.Invoke(Available);
            return true;
        }
    }
}
