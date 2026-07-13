using System;
using UnityEngine;

namespace PointClear.FrontEnd
{
    /// <summary>
    /// PC-015: the single owner of which front-end screen is active. It is the
    /// one place that changes the current screen, so navigation and its guards
    /// live in exactly one spot.
    ///
    /// Stage 0 is the logic skeleton: the screen state machine + the re-entry
    /// guard (you cannot transition to the screen you are already on, and a
    /// transition cannot begin while one is in flight — the seed of the
    /// no-duplicate-load / no-double-confirm rule, UC15). Later stages attach
    /// the Canvas panels this toggles, the player-preset hand-off, and the
    /// Operation scene transition, and add fail-safe recovery (UC16).
    /// </summary>
    public class FrontEndFlow : MonoBehaviour
    {
        /// <summary>The currently active screen.</summary>
        public FrontEndScreen Current { get; private set; } = FrontEndScreen.MainMenu;

        /// <summary>True while a transition is being applied (guards re-entrancy).</summary>
        public bool IsTransitioning { get; private set; }

        /// <summary>Raised after the active screen changes: (previous, next).</summary>
        public event Action<FrontEndScreen, FrontEndScreen> ScreenChanged;

        /// <summary>
        /// Requests a transition to <paramref name="next"/>. Returns false (and
        /// changes nothing) when already on that screen or when a transition is
        /// already in flight — so a double-click or re-entrant call is a no-op.
        /// </summary>
        public bool ShowScreen(FrontEndScreen next)
        {
            if (IsTransitioning || next == Current)
            {
                return false;
            }

            IsTransitioning = true;
            try
            {
                FrontEndScreen previous = Current;
                Current = next;
                SessionContext.CurrentScreen = next;
                ScreenChanged?.Invoke(previous, next);
            }
            finally
            {
                IsTransitioning = false;
            }

            return true;
        }
    }
}
