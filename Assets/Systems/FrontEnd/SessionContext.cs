using UnityEngine;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015: the front-end screens, in journey order. MainMenu / Options /
    /// Credits / CharacterCreation / StartingDirection / WorldMap are Canvas
    /// layers; Operation and Results are the in-arena states owned by the
    /// existing OperationController (the front-end only observes/drives them).
    /// </summary>
    public enum FrontEndScreen
    {
        MainMenu,
        Options,
        Credits,
        CharacterCreation,
        StartingDirection,
        WorldMap,
        Operation,
        Results
    }

    /// <summary>
    /// PC-015: the two greybox visual presets for the temporary character.
    /// A = green, B = pink — a simple recolor of the existing player capsule,
    /// deliberately NOT a male/female or final character model (out of scope).
    /// </summary>
    public enum CharacterPreset
    {
        GreenA = 0,
        PinkB = 1
    }

    /// <summary>
    /// PC-015: the smallest temporary session state carried between front-end
    /// screens and into an Operation, for ONE Play-Mode session.
    ///
    /// Deliberately a plain static — NOT a MonoBehaviour, DontDestroyOnLoad
    /// object, singleton service, save file, account model, or multiplayer
    /// registry. It survives the single PrototypeScene self-reload used as the
    /// New-Character reset boundary (statics persist across scene loads), and is
    /// wiped on domain reload via RuntimeInitializeOnLoadMethod so a stale
    /// character can never leak between Play sessions. It holds only what this
    /// vertical slice needs; future up-to-four-player support is not blocked —
    /// this is one explicit seam, easy to promote to a per-player holder later.
    /// </summary>
    public static class SessionContext
    {
        /// <summary>The confirmed character name (already validated non-blank).</summary>
        public static string CharacterName { get; set; }

        /// <summary>The confirmed visual preset.</summary>
        public static CharacterPreset Preset { get; set; }

        /// <summary>True once Character Creation has confirmed a character.</summary>
        public static bool HasCharacter { get; set; }

        /// <summary>True once the single starting node/direction is confirmed.</summary>
        public static bool StartingNodeConfirmed { get; set; }

        /// <summary>The screen the flow currently considers active.</summary>
        public static FrontEndScreen CurrentScreen { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnLoad()
        {
            Reset();
        }

        /// <summary>
        /// Clears the temporary character to a fresh state — the "New Character"
        /// reset (UC18). Resets ONLY this session-context data; it never touches
        /// runtime progression, which is reset by reloading the arena scene.
        /// </summary>
        public static void Reset()
        {
            CharacterName = string.Empty;
            Preset = CharacterPreset.GreenA;
            HasCharacter = false;
            StartingNodeConfirmed = false;
            CurrentScreen = FrontEndScreen.MainMenu;
        }

        /// <summary>UC3 helper: a name is confirmable only if non-blank once trimmed.</summary>
        public static bool IsNameValid(string candidate)
        {
            return !string.IsNullOrWhiteSpace(candidate);
        }

        /// <summary>The greybox body color for a preset (used to tint the player capsule).</summary>
        public static Color PresetColor(CharacterPreset preset)
        {
            return preset == CharacterPreset.PinkB
                ? new Color(0.93f, 0.35f, 0.66f, 1f)   // pink
                : new Color(0.35f, 0.85f, 0.45f, 1f);  // green
        }
    }
}
