# Task PC-017 — Milestone 1 / Block 1: Enemy Variety + Arena Loop Iteration

## Task ID
PC-017

## Title
Block 1 — Enemy Variety (one question per Run), gentle onboarding, between-run Skill allocation, no self-damage (DEC-038)

## Status
DONE — playtest-approved by the Game Director 2026-07-13. Technical-review box left
unchecked (no Technical Director present); DONE on the Game Director's explicit
authority, flagged (consistent with PC-007..016).

## Priority
High (first block of Milestone 1 — Arena Gameplay)

## Owner
Claude (implementation) / Yoav (direction + playtest approval)

## Reviewer
Yoav (Game Director)

## Dependencies / Related
- Milestone 1 design (Arena Rhythm + A Living Arena pillars, chat-approved)
- Gameplay Prototype Philosophy ([DESIGN_DNA.md](../../DESIGN_DNA.md))
- Frozen vision DEC-034/035/036/037; new decision **DEC-038** (no self-damage)

## Objective
Prove the four onboarding player-questions are legible and introduced one per Run,
make the early onboarding genuinely gentle, add the between-run improve-and-test
beat, and remove player self-damage — all greybox, disposable vehicles for the
permanent gameplay roles.

## What was delivered
- **Enemy Variety:** `EnemySpawner` split into per-Run type introduction + within-Run
  density (Arena-Rhythm curve). Onboarding: Run 1 Walker → 2 +Charger → 3 +Surrounder
  → 4 +Empowerer, each debuting at low density. Minimal per-Run composition selector
  (NOT the deferred Run-Cycle/Tier Controller).
- **Gentle onboarding:** global quota 25→10 (scene); walker pressure flat/low across
  Runs 1–3; escalation via variety, not density. Values greybox/tunable (DEC-036).
- **Between-run beat:** after a successful extraction with unspent Skill Points, route
  through the existing allocation screen (spend → next Arena); shown only when points
  are unspent; skipped on failure/0. XP cadence ~1 level/Run via the quota change.
- **No self-damage (DEC-038):** shared guard `Combat/PlayerAbilityDamage` routed
  through the weapon, Fracture Bolt, and Detonation Mark. Enemy attacks still dangerous.
- **Cleanup:** Empowerer reflection hack → `EnemyAI.SetMoveSpeed`; Stalker retired;
  HUD shows the current Run.

## Acceptance Criteria
- [x] Four questions legible and introduced one per Run (cumulative, clean debut).
- [x] Run 1 gentle/short ("I feel powerful"); Run 2 keeps Run 1's feel + only the Charger.
- [x] Between-run allocation shown only after success with unspent points; skipped otherwise.
- [x] ~1 level-up per early Run.
- [x] Player abilities never damage the player; enemy attacks still do; player damage still kills enemies.
- [x] No new compiler/console errors.
- [x] Game Director hands-on playtest approved.

## Files changed
- `Assets/Systems/Enemies/EnemySpawner.cs` (per-Run composition + rhythm; Run readout)
- `Assets/Systems/Enemies/EnemyAI.cs` (`SetMoveSpeed`)
- `Assets/Systems/Enemies/EmpowererAI.cs` (real buff, reflection removed)
- `Assets/Systems/Utilities/OperationHud.cs` (Run readout)
- `Assets/Systems/FrontEnd/FrontEndUI.cs` (between-run allocation routing)
- `Assets/Systems/Combat/PlayerAbilityDamage.cs` (new — no-self-damage guard)
- `Assets/Systems/Skills/DetonationMark.cs`, `Assets/Systems/Skills/FractureBoltProjectile.cs`, `Assets/Systems/Weapons/HitscanWeapon.cs` (routed through the guard)
- `Assets/Scenes/Prototype/PrototypeScene.unity` (extraction quota 25→10)
- Deleted: `Assets/Systems/Enemies/StalkerAI.cs` (+meta), `Assets/Prefabs/Enemies/Stalker.prefab` (+meta)
- Docs: `DECISIONS.md` (DEC-038), `CHANGELOG.md`, this task file

## Out of scope (respected)
- No OperationController architecture expansion, no Run-Cycle/Tier Controller, no new
  Skill Tree UI, no generic frameworks, no new enemy (ranged deferred to a later Tier),
  no XP-curve redesign.

## Notes
Minor known quirk: the allocation screen's "Back" button returns to Character Creation
(its original context) — harmless (character persists); use Confirm between Runs. Per-Run
extraction quota remains a future tuning knob (belongs with the deferred controller).
