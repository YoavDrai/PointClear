using System;
using System.Collections.Generic;
using UnityEngine;
using PointClear.Operations;
using PointClear.Player;
using PointClear.Weapons;
using PointClear.Skills;
using PointClear.Progression;
using PointClear.Utilities;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015 Stage 4: the SINGLE seam between the front-end and the existing
    /// gameplay. It touches combat only through public surfaces already present
    /// on the baseline — it never duplicates or reimplements OperationController
    /// logic. Responsibilities:
    ///  - toggle player input + the OnGUI combat/dev HUDs together, so combat is
    ///    inert and hidden while a front-end screen is up, and live only during
    ///    an Operation;
    ///  - apply the session character (preset color + name) to the player;
    ///  - begin an Operation (OperationController.StartOperation) and reset it to
    ///    neutral on return (OperationController.ReturnToReady);
    ///  - re-raise OperationController's success/failure as one OperationEnded
    ///    event for the flow to react to.
    /// References are resolved lazily via existing lookups (PlayerReference,
    /// FindAnyObjectByType) and every access is null-guarded, so a missing piece
    /// fails safe instead of throwing.
    /// </summary>
    public class CombatBridge : MonoBehaviour
    {
        /// <summary>Raised once when the Operation ends; argument is true on success, false on failure.</summary>
        public event Action<bool> OperationEnded;

        private OperationController operation;
        private Transform player;
        private Behaviour[] inputBehaviours;
        private Behaviour[] hudBehaviours;
        private MeshRenderer[] playerRenderers;
        private CurrencyWallet wallet;
        private PlayerLevel playerLevel;
        private SkillPoints skillPoints;
        private SkillProgression progression;
        private bool subscribed;

        // Read-only run-result / progression values, owned entirely by the
        // existing systems — the front-end Results panel only displays these.
        public int LastSecured => wallet != null ? wallet.LastSecured : 0;
        public int LastLost => wallet != null ? wallet.LastLost : 0;
        public int Banked => wallet != null ? wallet.Banked : 0;
        public int Level => playerLevel != null ? playerLevel.CurrentLevel : 0;
        public int SkillPointsAvailable => skillPoints != null ? skillPoints.Available : 0;

        // PC-016: the character-start allocation step reaches SkillProgression only
        // through these accessors — the front-end never touches the skill system
        // directly, and no second allocation system is introduced. The offered set
        // is data-driven (SkillDefinition.AvailableAtCharacterStart).
        public IReadOnlyList<SkillDefinition> CharacterStartSkills()
        {
            Resolve();
            var list = new List<SkillDefinition>();
            if (progression != null)
            {
                foreach (SkillDefinition def in progression.RegisteredSkills)
                {
                    if (def != null && def.AvailableAtCharacterStart)
                    {
                        list.Add(def);
                    }
                }
            }
            return list;
        }

        public int SkillLevel(SkillDefinition def)
        {
            Resolve();
            return progression != null ? progression.GetLevel(def) : 0;
        }

        public bool CanAllocateSkill(SkillDefinition def)
        {
            Resolve();
            return progression != null && progression.CanAllocate(def);
        }

        public bool TryAllocateSkill(SkillDefinition def)
        {
            Resolve();
            return progression != null && progression.TryAllocate(def);
        }

        private void Resolve()
        {
            if (operation == null)
            {
                operation = FindAnyObjectByType<OperationController>();
                if (operation != null && !subscribed)
                {
                    operation.OperationSucceeded += HandleSucceeded;
                    operation.OperationFailed += HandleFailed;
                    subscribed = true;
                }
            }

            if (player == null)
            {
                // Prefer the canonical PlayerReference; fall back to a direct
                // lookup if that static is transiently null (e.g. resolved before
                // the player's Awake), so the bridge still binds and never leaves
                // input/character un-applied.
                Transform resolved = PlayerReference.Instance;
                if (resolved == null)
                {
                    PlayerController controller = FindAnyObjectByType<PlayerController>();
                    if (controller != null)
                    {
                        resolved = controller.transform;
                    }
                }
                if (resolved != null)
                {
                    player = resolved;
                    playerRenderers = player.GetComponentsInChildren<MeshRenderer>(true);
                    inputBehaviours = CollectInput(player);
                    wallet = player.GetComponent<CurrencyWallet>();
                    playerLevel = player.GetComponent<PlayerLevel>();
                    skillPoints = player.GetComponent<SkillPoints>();
                    progression = player.GetComponent<SkillProgression>();
                }
            }

            if (hudBehaviours == null)
            {
                hudBehaviours = CollectHuds();
            }
        }

        private static Behaviour[] CollectInput(Transform player)
        {
            var list = new List<Behaviour>();
            AddIfPresent(list, player.GetComponent<PlayerController>());
            AddIfPresent(list, player.GetComponent<HitscanWeapon>());
            AddIfPresent(list, player.GetComponent<FractureBolt>());
            AddIfPresent(list, player.GetComponent<DetonationField>());
            return list.ToArray();
        }

        private static Behaviour[] CollectHuds()
        {
            var list = new List<Behaviour>();
            AddIfPresent(list, FindAnyObjectByType<DebugHud>());
            AddIfPresent(list, FindAnyObjectByType<XPDiagnosticDisplay>());
            AddIfPresent(list, FindAnyObjectByType<SkillAllocationHud>());
            AddIfPresent(list, FindAnyObjectByType<OperationHud>());
            return list.ToArray();
        }

        private static void AddIfPresent(List<Behaviour> list, Behaviour behaviour)
        {
            if (behaviour != null)
            {
                list.Add(behaviour);
            }
        }

        /// <summary>Enables/disables player input + the combat/dev HUDs together.</summary>
        public void SetCombatActive(bool active)
        {
            Resolve();
            SetEnabled(inputBehaviours, active);
            SetEnabled(hudBehaviours, active);
        }

        private static void SetEnabled(Behaviour[] behaviours, bool value)
        {
            if (behaviours == null)
            {
                return;
            }
            for (int i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i] != null)
                {
                    behaviours[i].enabled = value;
                }
            }
        }

        /// <summary>Applies the session character to the player: preset color + name.</summary>
        public void ApplyCharacter()
        {
            Resolve();
            if (player == null)
            {
                return;
            }

            Color color = SessionContext.PresetColor(SessionContext.Preset);
            if (playerRenderers != null)
            {
                foreach (MeshRenderer renderer in playerRenderers)
                {
                    if (renderer == null)
                    {
                        continue;
                    }
                    Material mat = renderer.material; // runtime instance; URP Lit uses _BaseColor
                    if (mat.HasProperty("_BaseColor"))
                    {
                        mat.SetColor("_BaseColor", color);
                    }
                    if (mat.HasProperty("_Color"))
                    {
                        mat.SetColor("_Color", color);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(SessionContext.CharacterName))
            {
                // The player is found by tag / PlayerReference, never by name, so
                // renaming the object is a safe way to carry the character name in.
                player.gameObject.name = SessionContext.CharacterName;
            }
        }

        /// <summary>True when an Operation can be started (controller exists and is neutral).</summary>
        public bool CanBeginOperation()
        {
            Resolve();
            return operation != null && operation.State == OperationState.Ready;
        }

        /// <summary>Starts the existing Operation via its public API only.</summary>
        public void BeginOperation()
        {
            Resolve();
            if (operation != null)
            {
                operation.StartOperation();
            }
            else
            {
                Debug.LogWarning("[CombatBridge] No OperationController found — cannot begin operation.", this);
            }
        }

        /// <summary>Resets the Operation to its neutral Ready state (existing API).</summary>
        public void ReturnToNeutral()
        {
            Resolve();
            if (operation != null && operation.State != OperationState.Ready)
            {
                operation.ReturnToReady();
            }
        }

        private void HandleSucceeded()
        {
            OperationEnded?.Invoke(true);
        }

        private void HandleFailed()
        {
            OperationEnded?.Invoke(false);
        }

        private void OnDestroy()
        {
            if (operation != null && subscribed)
            {
                operation.OperationSucceeded -= HandleSucceeded;
                operation.OperationFailed -= HandleFailed;
                subscribed = false;
            }
        }
    }
}
