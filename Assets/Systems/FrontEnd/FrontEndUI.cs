using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PointClear.Skills;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015: builds and drives the front-end Canvas layers in code, so the
    /// PrototypeScene footprint is a single GameObject (this one, with a
    /// FrontEndFlow) and the whole UI is reviewable in C#. It owns:
    ///  - one Screen-Space-Overlay canvas + an Input-System EventSystem,
    ///  - one opaque panel per Canvas screen (built by later stages too),
    ///  - visibility: exactly the active screen's panel is shown; in-arena
    ///    screens (Operation/Results) hide every front-end panel.
    ///
    /// Stages 1–5 build Main Menu, Options (placeholder), Credits, Character
    /// Creation, Starting Direction, World Map, and Results, with reliable
    /// navigation, a Return-to-Main-Menu confirmation (which reloads the scene for
    /// a fresh character), and guards. Combat coupling (input/HUD gating, applying
    /// the character, begin/return of the Operation, and reading run-result values)
    /// is delegated to CombatBridge.
    /// </summary>
    [RequireComponent(typeof(FrontEndFlow))]
    public class FrontEndUI : MonoBehaviour
    {
        private FrontEndFlow flow;
        private Canvas canvas;
        private readonly Dictionary<FrontEndScreen, GameObject> panels =
            new Dictionary<FrontEndScreen, GameObject>();

        // --- Character Creation working state ---
        private NameInputField nameInput;
        private Text nameWarning;
        private Button presetAButton;
        private Button presetBButton;
        private Image previewSwatch;
        private Text previewName;
        private CharacterPreset selectedPreset;
        private bool confirmInProgress;

        // --- Starting Direction / initial Skill allocation working state (PC-016) ---
        private RectTransform skillColumn;
        private Text startingPointsLabel;
        private Text directionHint;
        private bool directionConfirmInProgress;
        private bool startingSkillsBuilt;
        private readonly List<StartingSkillRow> startingSkillRows = new List<StartingSkillRow>();

        private sealed class StartingSkillRow
        {
            public SkillDefinition Skill;
            public Button Button;
        }

        // --- World Map / Operation hand-off state ---
        private Button mapNodeButton;
        private Text mapInfoText;
        private Text mapEnterHint;
        private bool mapNodeSelected;
        private bool enterInProgress;
        private CombatBridge combat;
        private bool combatInitialized;

        // --- Results / Return-to-Main-Menu state ---
        private Text resultsTitle;
        private Text resultsBody;
        private bool lastRunSuccess;
        private bool returnInProgress;
        private GameObject confirmDialog;
        private bool reloadInProgress;

        private void Awake()
        {
            flow = GetComponent<FrontEndFlow>();
            combat = gameObject.AddComponent<CombatBridge>();
            combat.OperationEnded += OnOperationEnded;

            UIFactory.EnsureEventSystem();
            canvas = UIFactory.CreateCanvas("FrontEndCanvas", transform);

            BuildMainMenu();
            BuildOptions();
            BuildCredits();
            BuildCharacterCreation();
            BuildStartingDirection();
            BuildWorldMap();
            BuildResults();
            BuildConfirmDialog();

            flow.ScreenChanged += HandleScreenChanged;
        }

        private void Start()
        {
            // FrontEndFlow initializes to MainMenu; apply that visibility once
            // (ScreenChanged only fires on an actual change).
            ApplyVisibility(flow.Current);
        }

        private void LateUpdate()
        {
            // Apply the initial combat gating once, in LateUpdate, so it runs AFTER
            // every other object's Start — notably OperationController.Start, which
            // re-enables the player via ResetPlayer. At the initial MainMenu this
            // leaves player input disabled and the combat HUDs hidden.
            if (!combatInitialized && combat != null)
            {
                combatInitialized = true;
                combat.SetCombatActive(flow.Current == FrontEndScreen.Operation);
            }
        }

        private void OnDestroy()
        {
            if (flow != null)
            {
                flow.ScreenChanged -= HandleScreenChanged;
            }
            if (combat != null)
            {
                combat.OperationEnded -= OnOperationEnded;
            }
        }

        private void HandleScreenChanged(FrontEndScreen previous, FrontEndScreen next)
        {
            ApplyVisibility(next);

            // Any screen change dismisses the Return-to-Main-Menu confirmation.
            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }

            // Focus the name field only while Character Creation is active, so
            // keyboard text goes to it and nowhere else.
            if (nameInput != null)
            {
                nameInput.SetFocused(next == FrontEndScreen.CharacterCreation);
            }
            if (next == FrontEndScreen.CharacterCreation)
            {
                RefreshCharacterUi();
            }
            if (next == FrontEndScreen.StartingDirection)
            {
                RefreshStartingSkills();
            }
            if (next == FrontEndScreen.WorldMap)
            {
                RefreshMapUi();
                if (mapEnterHint != null)
                {
                    mapEnterHint.text = MapHintSelect;
                    mapEnterHint.gameObject.SetActive(false);
                }
            }
            if (next == FrontEndScreen.Results)
            {
                RefreshResults();
            }

            // Stage 4: combat runs (player input + combat HUDs on) ONLY during an
            // Operation; every other screen keeps combat inert and hidden. When the
            // Operation screen opens, apply the character and begin the run.
            bool inOperation = next == FrontEndScreen.Operation;
            if (combat != null)
            {
                combat.SetCombatActive(inOperation);
                if (inOperation)
                {
                    combat.ApplyCharacter();
                    combat.BeginOperation();
                }
            }
        }

        /// <summary>Shows exactly the active screen's panel; hides all others.</summary>
        private void ApplyVisibility(FrontEndScreen screen)
        {
            foreach (KeyValuePair<FrontEndScreen, GameObject> entry in panels)
            {
                bool active = entry.Key == screen;
                if (entry.Value.activeSelf != active)
                {
                    entry.Value.SetActive(active);
                }
            }
        }

        private GameObject NewScreen(FrontEndScreen screen, string title, string subtitle)
        {
            GameObject panel = UIFactory.CreatePanel(canvas.transform, screen + "Panel", UIFactory.ScreenBg);
            panels[screen] = panel;

            Text titleLabel = UIFactory.CreateLabel(panel.transform, title, 72, UIFactory.TextPrimary,
                TextAnchor.MiddleCenter, FontStyle.Bold);
            RectTransform tr = titleLabel.rectTransform;
            tr.anchorMin = new Vector2(0.5f, 1f);
            tr.anchorMax = new Vector2(0.5f, 1f);
            tr.pivot = new Vector2(0.5f, 1f);
            tr.anchoredPosition = new Vector2(0f, -120f);
            tr.sizeDelta = new Vector2(1200f, 100f);

            if (!string.IsNullOrEmpty(subtitle))
            {
                Text sub = UIFactory.CreateLabel(panel.transform, subtitle, 28, UIFactory.TextMuted,
                    TextAnchor.MiddleCenter, FontStyle.Italic);
                RectTransform sr = sub.rectTransform;
                sr.anchorMin = new Vector2(0.5f, 1f);
                sr.anchorMax = new Vector2(0.5f, 1f);
                sr.pivot = new Vector2(0.5f, 1f);
                sr.anchoredPosition = new Vector2(0f, -210f);
                sr.sizeDelta = new Vector2(1200f, 50f);
            }

            return panel;
        }

        private void BuildMainMenu()
        {
            GameObject panel = NewScreen(FrontEndScreen.MainMenu, "POINT CLEAR", "prototype — first player journey");
            RectTransform column = UIFactory.CreateColumn(panel.transform, 420f, 24f, TextAnchor.MiddleCenter);

            UIFactory.CreateButton(column, "Play", 80f, UIFactory.AccentBg,
                () => flow.ShowScreen(FrontEndScreen.CharacterCreation));
            UIFactory.CreateButton(column, "Options", 80f, UIFactory.ButtonBg,
                () => flow.ShowScreen(FrontEndScreen.Options));
            UIFactory.CreateButton(column, "Credits", 80f, UIFactory.ButtonBg,
                () => flow.ShowScreen(FrontEndScreen.Credits));
        }

        private void BuildOptions()
        {
            GameObject panel = NewScreen(FrontEndScreen.Options, "Options", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 760f, 28f, TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(column,
                "Placeholder screen.\nSettings are not implemented in this prototype.",
                30, UIFactory.TextMuted, TextAnchor.MiddleCenter, FontStyle.Normal);
            UIFactory.CreateButton(column, "Back", 72f, UIFactory.ButtonBg,
                () => flow.ShowScreen(FrontEndScreen.MainMenu));
        }

        private void BuildCredits()
        {
            GameObject panel = NewScreen(FrontEndScreen.Credits, "Credits", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 760f, 28f, TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(column,
                "Point Clear — greybox prototype\n\nDesign / Game Director: Yoav\nImplementation: Claude (PC-015)",
                30, UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Normal);
            UIFactory.CreateButton(column, "Back", 72f, UIFactory.ButtonBg,
                () => flow.ShowScreen(FrontEndScreen.MainMenu));
        }

        private void BuildCharacterCreation()
        {
            GameObject panel = NewScreen(FrontEndScreen.CharacterCreation, "Create Character", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 720f, 16f, TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(column, "Name", 28, UIFactory.TextMuted, TextAnchor.MiddleLeft, FontStyle.Normal);

            nameInput = UIFactory.CreateNameInput(column, "Click here and type a name…", 72f, 24);
            nameInput.ValueChanged += OnNameChanged;

            nameWarning = UIFactory.CreateLabel(column, "Enter a name to continue.", 24,
                UIFactory.WarnText, TextAnchor.MiddleLeft, FontStyle.Bold);
            nameWarning.gameObject.SetActive(false);

            UIFactory.CreateLabel(column, "Appearance", 28, UIFactory.TextMuted, TextAnchor.MiddleLeft, FontStyle.Normal);
            presetAButton = UIFactory.CreateButton(column, "Preset A — Green", 64f, UIFactory.ButtonBg,
                () => SelectPreset(CharacterPreset.GreenA));
            presetBButton = UIFactory.CreateButton(column, "Preset B — Pink", 64f, UIFactory.ButtonBg,
                () => SelectPreset(CharacterPreset.PinkB));

            RectTransform previewRow = UIFactory.CreateRow(column, 60f, 16f);
            previewSwatch = UIFactory.CreateSwatch(previewRow, 52f);
            previewName = UIFactory.CreateLabel(previewRow, string.Empty, 28,
                UIFactory.TextPrimary, TextAnchor.MiddleLeft, FontStyle.Normal);

            UIFactory.CreateButton(column, "Confirm", 76f, UIFactory.AccentBg, OnConfirmCharacter);
            UIFactory.CreateButton(column, "Back", 60f, UIFactory.ButtonBg, OnBackFromCharacterCreation);

            // Initialize working state from whatever the session already holds:
            // empty on a fresh character; the last choices when navigating back.
            selectedPreset = SessionContext.Preset;
            nameInput.SetValue(SessionContext.CharacterName);
            RefreshCharacterUi();
        }

        private void OnNameChanged(string value)
        {
            if (nameWarning != null && SessionContext.IsNameValid(value))
            {
                nameWarning.gameObject.SetActive(false);
            }
            if (previewName != null)
            {
                previewName.text = "Name:  " + (SessionContext.IsNameValid(value) ? value.Trim() : "<none>");
            }
        }

        private void SelectPreset(CharacterPreset preset)
        {
            selectedPreset = preset;
            RefreshCharacterUi();
        }

        private void RefreshCharacterUi()
        {
            SetSelectableButtonState(presetAButton, "Preset A — Green", selectedPreset == CharacterPreset.GreenA);
            SetSelectableButtonState(presetBButton, "Preset B — Pink", selectedPreset == CharacterPreset.PinkB);
            if (previewSwatch != null)
            {
                previewSwatch.color = SessionContext.PresetColor(selectedPreset);
            }
            OnNameChanged(nameInput != null ? nameInput.Value : string.Empty);
        }

        private static void SetSelectableButtonState(Button button, string label, bool selected)
        {
            if (button == null)
            {
                return;
            }
            var image = button.GetComponent<Image>();
            if (image != null)
            {
                image.color = selected ? UIFactory.AccentBg : UIFactory.ButtonBg;
            }
            var text = button.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = label + (selected ? "  (selected)" : string.Empty);
            }
        }

        private void OnConfirmCharacter()
        {
            // Guard duplicate confirms: act only while actually on this screen and
            // not already mid-confirm (the flow's transition guard is the backstop).
            if (confirmInProgress || flow.Current != FrontEndScreen.CharacterCreation)
            {
                return;
            }

            string candidate = nameInput != null ? nameInput.Value : string.Empty;
            if (!SessionContext.IsNameValid(candidate))
            {
                if (nameWarning != null)
                {
                    nameWarning.gameObject.SetActive(true);
                }
                return;
            }

            confirmInProgress = true;
            SessionContext.CharacterName = candidate.Trim();
            SessionContext.Preset = selectedPreset;
            SessionContext.HasCharacter = true;
            flow.ShowScreen(FrontEndScreen.StartingDirection);
            confirmInProgress = false;
        }

        private void OnBackFromCharacterCreation()
        {
            // The typed name and selected preset live in the widgets (the panel is
            // hidden, not destroyed), so they are preserved on return. Nothing is
            // written to SessionContext until Confirm.
            flow.ShowScreen(FrontEndScreen.MainMenu);
        }

        // PC-016: the Starting Direction screen IS the character-start Skill Point
        // allocation step. It reuses SkillProgression through CombatBridge (no
        // second skill system); the offered skills are data-driven
        // (SkillDefinition.AvailableAtCharacterStart). The player may spend the 2
        // starting points now or confirm with them unspent (a weapon-only start is
        // valid) — Confirm is always allowed but must be pressed explicitly.
        private void BuildStartingDirection()
        {
            GameObject panel = NewScreen(FrontEndScreen.StartingDirection, "Starting Direction", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 840f, 16f, TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(column, "Choose where your build begins.", 32,
                UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Bold);
            UIFactory.CreateLabel(column,
                "Spend your starting Skill Points to shape your first build —\n" +
                "a starting vector, not a permanent class. You can also begin with\n" +
                "points unspent and invest them later as you level.",
                24, UIFactory.TextMuted, TextAnchor.MiddleCenter, FontStyle.Normal);

            startingPointsLabel = UIFactory.CreateLabel(column, "Skill Points: -", 28,
                UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Bold);

            // Skills are built lazily on first entry (SkillProgression is resolved
            // from the player at runtime); this empty column reserves their slot
            // between the points readout and the Confirm/Back buttons.
            skillColumn = UIFactory.CreateColumn(column, 820f, 10f, TextAnchor.MiddleCenter);

            directionHint = UIFactory.CreateLabel(column, string.Empty, 22,
                UIFactory.TextMuted, TextAnchor.MiddleCenter, FontStyle.Italic);
            directionHint.gameObject.SetActive(false);

            UIFactory.CreateButton(column, "Confirm", 76f, UIFactory.AccentBg, OnConfirmStartingDirection);
            UIFactory.CreateButton(column, "Back", 60f, UIFactory.ButtonBg, OnBackFromStartingDirection);
        }

        // Builds the per-skill allocate buttons once, then refreshes rank/points/
        // interactable state. Called on entry to the screen and after each spend.
        private void RefreshStartingSkills()
        {
            if (combat == null)
            {
                return;
            }

            IReadOnlyList<SkillDefinition> skills = combat.CharacterStartSkills();

            if (!startingSkillsBuilt && skillColumn != null)
            {
                foreach (SkillDefinition def in skills)
                {
                    SkillDefinition captured = def; // per-iteration capture for the closure
                    Button b = UIFactory.CreateButton(skillColumn, captured.DisplayName, 72f,
                        UIFactory.ButtonBg, () => OnAllocateStartingSkill(captured));
                    startingSkillRows.Add(new StartingSkillRow { Skill = captured, Button = b });
                }
                startingSkillsBuilt = true;
            }

            if (startingPointsLabel != null)
            {
                startingPointsLabel.text = "Skill Points: " + combat.SkillPointsAvailable;
            }

            foreach (StartingSkillRow row in startingSkillRows)
            {
                int level = combat.SkillLevel(row.Skill);
                bool atMax = level >= row.Skill.MaxLevel;
                SetButtonText(row.Button,
                    row.Skill.DisplayName + "   Lv " + level + "/" + row.Skill.MaxLevel + (atMax ? "   MAX" : "   +"));
                if (row.Button != null)
                {
                    row.Button.interactable = combat.CanAllocateSkill(row.Skill);
                }
            }

            // A gentle, non-blocking note when points remain — Confirm is still allowed.
            if (directionHint != null)
            {
                bool hasUnspent = combat.SkillPointsAvailable > 0;
                directionHint.text = hasUnspent
                    ? "You can spend your points now, or confirm and invest them later."
                    : string.Empty;
                directionHint.gameObject.SetActive(hasUnspent);
            }
        }

        private void OnAllocateStartingSkill(SkillDefinition def)
        {
            if (flow.Current != FrontEndScreen.StartingDirection || combat == null)
            {
                return;
            }
            combat.TryAllocateSkill(def);
            RefreshStartingSkills();
        }

        private void OnConfirmStartingDirection()
        {
            // Guard duplicate confirms: act only while on this screen and not
            // already mid-confirm (the flow's transition guard is the backstop).
            // Per the design, Confirm is always allowed — spending is optional.
            if (directionConfirmInProgress || flow.Current != FrontEndScreen.StartingDirection)
            {
                return;
            }

            directionConfirmInProgress = true;
            SessionContext.InitialAllocationConfirmed = true;
            flow.ShowScreen(FrontEndScreen.WorldMap);
            directionConfirmInProgress = false;
        }

        private static void SetButtonText(Button button, string text)
        {
            if (button == null)
            {
                return;
            }
            Text label = button.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.text = text;
            }
        }

        private void OnBackFromStartingDirection()
        {
            // Returns to Character Creation; that screen's widgets hold the
            // character draft and preserve it (nothing here clears it).
            flow.ShowScreen(FrontEndScreen.CharacterCreation);
        }

        private const string MapHintSelect = "Select the operation node first.";
        private const string MapInfo =
            "Operation:  Blacksite Sweep\n" +
            "Objective:  Clear the hostiles, then reach the Extraction Point.\n" +
            "Danger:  Moderate\n" +
            "Reward:  Salvage — unsecured until you extract.";

        private void BuildWorldMap()
        {
            GameObject panel = NewScreen(FrontEndScreen.WorldMap, "World Map", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 880f, 16f, TextAnchor.MiddleCenter);

            UIFactory.CreateLabel(column, "Select an operation node.", 26,
                UIFactory.TextMuted, TextAnchor.MiddleCenter, FontStyle.Normal);

            mapNodeButton = UIFactory.CreateButton(column, "Blacksite Sweep — Operation", 88f,
                UIFactory.ButtonBg, SelectMapNode);

            mapInfoText = UIFactory.CreateLabel(column, string.Empty, 26,
                UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Normal);
            mapInfoText.gameObject.SetActive(false);

            mapEnterHint = UIFactory.CreateLabel(column, MapHintSelect, 24,
                UIFactory.WarnText, TextAnchor.MiddleCenter, FontStyle.Bold);
            mapEnterHint.gameObject.SetActive(false);

            UIFactory.CreateButton(column, "Enter Operation", 78f, UIFactory.AccentBg, OnEnterOperation);
            UIFactory.CreateButton(column, "Return to Main Menu", 60f, UIFactory.ButtonBg, OnReturnToMainMenuRequested);

            mapNodeSelected = false;
            RefreshMapUi();
        }

        private void SelectMapNode()
        {
            mapNodeSelected = true;
            if (mapEnterHint != null)
            {
                mapEnterHint.gameObject.SetActive(false);
            }
            RefreshMapUi();
        }

        private void RefreshMapUi()
        {
            SetSelectableButtonState(mapNodeButton, "Blacksite Sweep — Operation", mapNodeSelected);
            if (mapInfoText != null)
            {
                mapInfoText.gameObject.SetActive(mapNodeSelected);
                if (mapNodeSelected)
                {
                    mapInfoText.text = MapInfo;
                    UIFactory.FitLabelHeight(mapInfoText);
                }
            }
        }

        private void OnEnterOperation()
        {
            // Guard duplicate entries: act only while on the World Map and not
            // already entering.
            if (enterInProgress || flow.Current != FrontEndScreen.WorldMap)
            {
                return;
            }
            if (!mapNodeSelected)
            {
                if (mapEnterHint != null)
                {
                    mapEnterHint.text = MapHintSelect;
                    mapEnterHint.gameObject.SetActive(true);
                }
                return;
            }
            if (combat == null || !combat.CanBeginOperation())
            {
                // Fail safe: stay on the World Map with a clear message rather than
                // stranding the player in an empty Operation screen (UC16).
                if (mapEnterHint != null)
                {
                    mapEnterHint.text = "Operation is not available right now.";
                    mapEnterHint.gameObject.SetActive(true);
                }
                return;
            }

            enterInProgress = true;
            flow.ShowScreen(FrontEndScreen.Operation);
            enterInProgress = false;
        }

        private void OnOperationEnded(bool success)
        {
            // The Operation reached a terminal state. Show the Results screen (which
            // only reads the existing result values). The return to neutral happens
            // when the player leaves Results for the World Map — never a restart.
            lastRunSuccess = success;
            flow.ShowScreen(FrontEndScreen.Results);
        }

        private void BuildResults()
        {
            GameObject panel = NewScreen(FrontEndScreen.Results, "Operation Complete", null);
            RectTransform column = UIFactory.CreateColumn(panel.transform, 820f, 16f, TextAnchor.MiddleCenter);

            resultsTitle = UIFactory.CreateLabel(column, string.Empty, 40,
                UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Bold);
            resultsBody = UIFactory.CreateLabel(column, string.Empty, 28,
                UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Normal);

            UIFactory.CreateButton(column, "Return to World Map", 78f, UIFactory.AccentBg, OnReturnToWorldMap);
        }

        private void RefreshResults()
        {
            if (combat == null || resultsTitle == null || resultsBody == null)
            {
                return;
            }

            string body;
            if (lastRunSuccess)
            {
                resultsTitle.text = "SUCCESS — Extracted";
                resultsTitle.color = new Color(0.55f, 0.85f, 0.55f, 1f);
                body = "Secured this run:  +" + combat.LastSecured + "\n";
            }
            else
            {
                resultsTitle.text = "FAILED — You Died";
                resultsTitle.color = UIFactory.WarnText;
                body = "Lost this run:  " + combat.LastLost + "\n";
            }
            body += "Banked total:  " + combat.Banked + "\n";
            body += "Retained —  Level " + combat.Level + ",   Skill Points " + combat.SkillPointsAvailable;
            resultsBody.text = body;
            UIFactory.FitLabelHeight(resultsTitle);
            UIFactory.FitLabelHeight(resultsBody);
        }

        private void OnReturnToWorldMap()
        {
            // Guard duplicate returns: act only from Results, once. ReturnToNeutral
            // is itself idempotent (no-op unless the run is terminal), so the
            // existing ReturnToReady lifecycle runs exactly once. No scene reload.
            if (returnInProgress || flow.Current != FrontEndScreen.Results)
            {
                return;
            }
            returnInProgress = true;
            if (combat != null)
            {
                combat.ReturnToNeutral();
            }

            // Block 1 iteration (PC-017): the improve-and-test beat —
            // Fight → Level Up → Extraction → Results → SPEND points → next Run.
            // After a SUCCESSFUL run, if the player has unspent Skill Points, route
            // through the existing allocation screen (StartingDirection) so they can
            // strengthen the build before re-entering. Shown ONLY when there are
            // points to spend (never empty friction); skipped on failure and at 0.
            bool offerAllocation = lastRunSuccess && combat != null && combat.SkillPointsAvailable > 0;
            flow.ShowScreen(offerAllocation ? FrontEndScreen.StartingDirection : FrontEndScreen.WorldMap);
            returnInProgress = false;
        }

        private void BuildConfirmDialog()
        {
            confirmDialog = new GameObject("ConfirmDialog",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            confirmDialog.transform.SetParent(canvas.transform, false);
            UIFactory.Stretch(confirmDialog.GetComponent<RectTransform>());
            // Opaque-enough dim that also blocks clicks to the World Map behind it.
            confirmDialog.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.72f);

            var box = new GameObject("Box", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            box.transform.SetParent(confirmDialog.transform, false);
            RectTransform boxRect = box.GetComponent<RectTransform>();
            boxRect.anchorMin = new Vector2(0.5f, 0.5f);
            boxRect.anchorMax = new Vector2(0.5f, 0.5f);
            boxRect.pivot = new Vector2(0.5f, 0.5f);
            boxRect.sizeDelta = new Vector2(780f, 400f);
            box.GetComponent<Image>().color = UIFactory.PanelBg;

            RectTransform column = UIFactory.CreateColumn(box.transform, 680f, 22f, TextAnchor.MiddleCenter);
            UIFactory.CreateLabel(column,
                "Return to Main Menu?\nThis discards your current character —\nLevel, Skill Points, and Banked currency.",
                28, UIFactory.TextPrimary, TextAnchor.MiddleCenter, FontStyle.Normal);
            UIFactory.CreateButton(column, "Discard & Return", 72f, UIFactory.AccentBg, OnConfirmMainMenu);
            UIFactory.CreateButton(column, "Cancel", 64f, UIFactory.ButtonBg, OnCancelMainMenu);

            confirmDialog.SetActive(false);
        }

        private void OnReturnToMainMenuRequested()
        {
            // Only meaningful from the World Map; open the confirmation modal.
            if (flow.Current != FrontEndScreen.WorldMap || confirmDialog == null)
            {
                return;
            }
            confirmDialog.SetActive(true);
        }

        private void OnCancelMainMenu()
        {
            // Remain on the World Map; all session state is preserved (nothing changed).
            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }
        }

        private void OnConfirmMainMenu()
        {
            // Guard duplicate confirms / multiple scene-reload requests.
            if (reloadInProgress)
            {
                return;
            }
            reloadInProgress = true;
            if (confirmDialog != null)
            {
                confirmDialog.SetActive(false);
            }
            // End the temporary character: clear session state, then reload the
            // active scene ONCE. The reload rebuilds the player and all progression
            // from scratch (Level 1 / 0 XP / 0 Skill Points / 0 Banked / no module),
            // a genuinely fresh character with no manual reset API on the progression
            // systems. The rebuilt FrontEndUI starts at Main Menu.
            SessionContext.Reset();
            UnityEngine.SceneManagement.Scene active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            UnityEngine.SceneManagement.SceneManager.LoadScene(active.buildIndex);
        }
    }
}
