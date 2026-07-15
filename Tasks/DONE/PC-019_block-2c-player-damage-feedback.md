# Task PC-019 — Milestone 1 / Block 2 (Combat Feel) / Block 2C: Player Damage Feedback

## Task ID
PC-019

## Title
Block 2C — Player Damage Feedback: a restrained greybox hit beat (body flash + danger vignette)
so getting hit registers instantly, without losing the selected character colour.

## Status
DONE — playtest-approved by the Game Director 2026-07-15. Technical-review box left
unchecked (no Technical Director present); DONE on the Game Director's explicit
authority, flagged (consistent with PC-007..018).

## Priority
High (Combat Feel sub-block, Milestone 1)

## Owner
Claude (implementation) / Yoav (direction + playtest approval)

## Reviewer
Yoav (Game Director)

## Dependencies / Related
- Milestone 1 design; Combat Feel review (Block 2 breakdown, chat-approved)
- Precedent: Block 2A Kill Feedback (PC-018) — the enemy hit-flash + death-beat vocabulary
- Front-end seam: `CombatBridge.ApplyCharacter` (applies the selected preset colour to a
  runtime material instance) — the fix must not fight this
- Pillars: Combat Is King, Arena Rhythm (readability governor), Gameplay Prototype
  Philosophy, The Golden Rule (fair, never "the game cheated")

## Objective
Make *getting hit* read instantly and fairly — "that hurt / I'm in danger" — without the
player reading a number, and without slowing the Arena rhythm. Close the player/enemy
feedback asymmetry: enemies already flash on hit and have a death beat; the player's only
damage feedback was a debug full-screen wash slated for deletion.

## What was delivered
- **`PlayerDamageFeedback`** (new, on the Player prefab): on `Health.Damaged`, two
  restrained greybox channels —
  - **Body hurt-flash** — ~0.1s **red** `_BaseColor` flash on the player mesh (mirrors the
    enemy hit-flash pattern). Red = "I got hit" vs. the white "I hit something" enemy flash.
    Colour-only, **no scale-pop** — the player is the visual/aim anchor.
  - **Screen-edge danger vignette** — ~0.35s red edge pulse (code-generated texture, no art
    asset), opacity scaled by current HP (0.25 full → 0.7 near death) so a low-HP hit reads
    as more dangerous. Draws over the play space at the edges only, keeping the centre legible.
- **`DebugHud`** — removed its debug full-screen red damage wash, its `Health.Damaged`
  subscription, and the flash texture. Now **text-only** (HP, controls, crowd metrics). Player
  hit-feedback now has a single, non-debug owner.
- Added `PlayerDamageFeedback` to the Player prefab.

## Character-colour preservation (playtest bug + fix)
The hurt flash paints red via a per-renderer `MaterialPropertyBlock` **override** and restores
by **clearing** that override — never by writing a cached colour. The selected preset colour
lives on a runtime material instance (`CombatBridge.ApplyCharacter`), which the flash never
touches, so the exact active colour returns after every hit and future runtime colour changes
are respected.

Bug found in playtest: the first hit snapped the body to the material default. Cause: an earlier
version cached `sharedMaterial._BaseColor` in `Awake` (the shared asset default, before
customization applied the preset) and restored *that* into the property block, permanently
masking the preset. Fixed by the restore-by-clear approach above (no cached colour at all).

## Acceptance Criteria
- [x] Getting hit reads instantly on the player body (red flash) without fighting the read of
      your own position/aim.
- [x] Screen-edge danger vignette reads as "in danger" without washing out the play space;
      stronger the lower your HP.
- [x] Selected character preset colour is preserved exactly across single, repeated, and
      overlapping hits (validated in-arena with Pink B).
- [x] Restrained per the juice ceiling — no camera shake, hitstop, or audio.
- [x] No new compiler/console errors.
- [x] Game Director hands-on playtest approved.

## Files changed
- New: `Assets/Systems/Player/PlayerDamageFeedback.cs`
- Modified: `Assets/Systems/Utilities/DebugHud.cs`
- Prefab: `Assets/Prefabs/Player/Player.prefab`
- Docs: `CHANGELOG.md`, this task file

## Out of scope (respected — later Combat Feel blocks / frozen decisions)
No camera shake, no hitstop (Block 2B — Hit Impact), no audio/rumble, no directional
"which enemy hit me" indicator, no i-frames/knockback (a mechanics change, not feedback),
no real HUD/health-bar rework, no damage numbers, no generic feedback framework.

## Notes
All tuning knobs (`bodyFlashColor`, `bodyFlashDuration`, `vignetteColor`, `vignetteDuration`,
`vignetteMinAlpha`, `vignetteMaxAlpha`) are serialized on `PlayerDamageFeedback` — greybox,
disposable. Automated Unity validation could confirm the clear-path and colour preservation
directly (reflection + material reads) but not the 0.1s coroutine restore live (an unfocused
Editor doesn't tick game time under MCP-driven testing); the 0.1s timing was confirmed in the
Game Director's focused hands-on playtest.
