# Task ID
PC-004

## Title
Sprint 2.0 — Progression Foundation & Player Stats (includes Sprint 2.1 — Immediate XP Rewards, Sprint 2.2 — Level Progression)

## Status
DONE (2026-07-11) — approved by Yoav, functional Play Mode regression passed. One Definition of Done item remains unchecked: no separate Technical Director review occurred (no reviewer other than Yoav was present this session); moved to DONE on Yoav's explicit authority as Game Director regardless, per his direct instruction. See Game Director Approval and Definition of Done Checklist, below.

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- Task PC-003 (Sprint 1 First Playable Combat Prototype) — REVIEW (not yet DONE; this task's code was built on top of it on the same branch)

## Related Documents
- [Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md](PC-003_sprint-1-first-playable-combat-prototype.md)
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Progression Philosophy
- [DECISIONS.md](../../DECISIONS.md) DEC-018 (Experience and Loot Separation)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](../../Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Experience, § Upgrade Selection

## Objective

Build the smallest possible player-progression foundation — a generic stat container, immediate on-kill XP, and derived level tracking — with zero power granted by leveling yet, proving the architecture ahead of a future upgrade-selection system.

## Background

This work was implemented and committed directly to `feature/sprint-1-first-playable` without a task file, without CHANGELOG entries, and without a recorded Game Director approval — a process gap discovered and backfilled retroactively on 2026-07-11 during a documentation/architecture review session, after Yoav confirmed the code should be documented rather than discarded. The code itself already carried informal "Sprint 2.0/2.1/2.2" labels in its own doc comments, which this task file formalizes. No new code was written to produce this backfill — see Implementation Report for the exact commits it documents.

## Requirements

- A generic base+modifier stat container supporting additive and multiplicative modifiers, scoped to the stats the current prototype actually needs (Damage, FireRate, MoveSpeed) — no speculative stat types
- A single-player reference resolver, replacing duplicated tag lookups in `EnemyAI`/`EnemySpawner`
- An XP accumulator on the player, granting no level/stat logic itself
- Enemies grant a configured XP amount immediately and deterministically on death — no physical pickup, no RNG (per [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Progression Philosophy)
- Level is derived purely from cumulative XP (no independently stored level that can drift)
- A `LevelUp` event fires on level transitions but grants no stat bonus — upgrade/power delivery is explicitly out of scope pending a future sprint
- Temporary on-screen diagnostic XP/Level readout, clearly marked non-final

## Acceptance Criteria

- [x] (Sprint 2.0) `PlayerStats` supports `SetBase`/`AddAdditiveModifier`/`AddMultiplicativeModifier`/`GetValue` for `Damage`, `FireRate`, `MoveSpeed`; effective value formula is `max(0, (base + Σadditive) * (1 + Σmultiplicative))`
- [x] (Sprint 2.0) `PlayerReference.Instance` resolves the local player `Transform`, resets between Play sessions via `RuntimeInitializeOnLoadMethod`
- [x] (Sprint 2.0) `EnemyAI`/`EnemySpawner`/`HitscanWeapon` updated to use `PlayerReference` instead of tag lookups where applicable
- [x] (Sprint 2.1) `PlayerXP.AddXP` accumulates XP and fires `XPChanged`; rejects non-positive amounts
- [x] (Sprint 2.1) `EnemyXPReward` grants a configured, fixed XP value to the local player immediately on `Health.Died` — no physical object, no RNG, no loot/gold/inventory logic
- [x] (Sprint 2.1) Temporary `XPDiagnosticDisplay` shows current XP, isolated from `DebugHud`
- [x] (Sprint 2.2) `PlayerLevel.CurrentLevel` is derived on every XP change from `PlayerXP.CurrentXP` via `PlaceholderXpCurve` (placeholder formula: `baseXpPerLevel * level`), not stored independently
- [x] (Sprint 2.2) `LevelUp` event fires once per level gained, including multiple levels in one XP grant
- [x] (Sprint 2.2) No stat bonus of any kind is granted by leveling — verified no `PlayerStats` call exists anywhere in the level-up path
- [x] (Sprint 2.2) `XPDiagnosticDisplay` extended to show current level and progress bar toward next level
- [x] Zero compile errors introduced by any of the above (verified twice: read-only console check 2026-07-11, and again via full Play Mode regression below)
- [x] No loot, inventory, upgrade selection, skills, passives, or build-layer content added — leveling intentionally grants no power yet
- [x] Enemy death grants XP correctly, at the configured rate, through real gameplay (not just direct method calls) — verified in regression below
- [x] Multi-level progression works correctly against the placeholder curve formula — verified in regression below
- [x] Leveling grants no automatic player-power increase — verified in regression below
- [x] The 20-enemy population target (`EnemySpawner.targetActiveCount`) remains stable under sustained kill pressure — verified in regression below

## Test Procedure

**Executed 2026-07-11 via Unity MCP, PrototypeScene, Play Mode.** Deterministic frame-stepping (`EditorApplication.Step()` while paused, `Time.timeScale` variation) was used instead of relying on real-time waits, to avoid the Play Mode frame-loop stall documented in PC-003 Sprint 1.1/1.2/1.3 (Editor losing OS focus during long automated sessions). Full method-level detail below; all steps passed.

1. **Baseline (before any XP):** Entered Play Mode. `PlayerReference.Instance` resolved correctly. `PlayerStats.GetValue()` recorded as Damage=10, FireRate=6, MoveSpeed=6. `PlayerXP.CurrentXP=0`, `PlayerLevel.CurrentLevel=1`. `EnemyAI.ActiveCount=5` (matches `EnemySpawner.initialActiveTarget`). Console: 0 entries of any kind.
2. **Population ramp stability:** Advanced simulated time (paused + stepped) past the ramp's three 25s increases. Result: `EnemyAI.ActiveCount=20`, `PeakActiveCount=20`, `EnemySpawner.CurrentTarget=20` — matches the configured `targetActiveCount` exactly, never exceeded it. Console: still 0 entries.
3. **XP-on-death and multi-level progression:** Repeatedly located all live `EnemyAI` instances and called `Health.TakeDamage(9999f)` on each (triggering `Health.Died` → `EnemyXPReward` → `PlayerXP.AddXP`), interleaved with stepped frames so `EnemySpawner` replaced the dead enemies, for a total of **114 kills**. Result: `PlayerXP.CurrentXP=114` (exactly 1 XP/kill, matching `EnemyXPReward.xpValue=1`), `PlayerLevel.CurrentLevel=7`. Cross-checked against `PlaceholderXpCurve`'s own formula (cumulative XP to reach level *N* = `5·N·(N-1)/2`): level 7 requires 105 XP, level 8 requires 140 XP — at 114 XP the character should be level 7 with 9 XP into the level and 35 XP required for the next. `PlayerLevel.XPIntoCurrentLevel=9`, `XPRequiredForNextLevel=35` — **exact match**, confirming `LevelUp` fired correctly across all six intervening level transitions in one accumulation pass, not just the first.
4. **No automatic power increase:** Re-read `PlayerStats.GetValue()` after reaching Level 7 (114 XP, 114 kills). Result: Damage=10, FireRate=6, MoveSpeed=6 — **identical to the Step 1 baseline**. No code path between `PlayerLevel`/`PlayerXP` and `PlayerStats` exists; confirmed both by static inspection (already noted in Acceptance Criteria) and now by this live measurement.
5. **Spawner recovery/stability after sustained kill pressure:** Advanced further stepped frames after the 114-kill burst. Result: `EnemyAI.ActiveCount` recovered to and held at 20, `PeakActiveCount` still 20 (never exceeded target even under repeated rapid kills). Console: still 0 entries.
6. **Console check (final):** Read all console output (errors, warnings, logs) across the entire session from Play Mode entry to exit. **0 entries of any kind** — zero errors, zero warnings, no repeating-warning pattern to evaluate because nothing was logged at all.
7. Exited Play Mode. Confirmed `PrototypeScene.isDirty == false` afterward — no unintended scene edits persisted (Play Mode changes are discarded by Unity on stop, as expected).

**Not covered by this regression pass:** `XPDiagnosticDisplay`'s on-screen rendering was not visually inspected (only the underlying `PlayerXP`/`PlayerLevel` values it reads were verified); `PlayerController`/`HitscanWeapon` gameplay feel was not re-tested (out of scope for a progression-system regression).

## Files Allowed to Edit

- `Assets/Systems/Player/` (`PlayerStats.cs`, `PlayerReference.cs`, `PlayerController.cs`)
- `Assets/Systems/Progression/` (`PlayerXP.cs`, `PlayerLevel.cs`, `PlaceholderXpCurve.cs`)
- `Assets/Systems/Enemies/` (`EnemyXPReward.cs`, `EnemyAI.cs`, `EnemySpawner.cs`)
- `Assets/Systems/Weapons/HitscanWeapon.cs`
- `Assets/Systems/Utilities/XPDiagnosticDisplay.cs`, `DebugHud.cs`
- `Assets/Prefabs/Player/`, `Assets/Prefabs/Enemies/`
- `Assets/Scenes/Prototype/PrototypeScene.unity`
- `Tasks/DONE/PC-004_sprint-2.0-2.2-progression-foundation.md` (path updated 2026-07-11 as the task moved REVIEW → DONE)
- `CHANGELOG.md`

## Files Forbidden to Edit

- Everything not listed above, including `DECISIONS.md`, `GAME_PILLARS.md`, `DESIGN_DNA.md`, `VISION.md`, `ROADMAP.md`, `PROJECT_BIBLE.md`, `GLOSSARY.md`

## Out of Scope

- Upgrade selection, skill points, passives, mutations, relics, weapon variety, loot, inventory, equipment — any actual power delivered by leveling (deferred to a future sprint)
- XP curve balancing (the formula is explicitly a placeholder)
- Multiplayer XP-credit rules (single local player only; `EnemyXPReward` documents this as the one place a future credit rule would change)

## Risks

- **No task file existed before this backfill.** Per [Documentation/AI/CLAUDE_RULES.md](../../Documentation/AI/CLAUDE_RULES.md), this violates "No hidden or undocumented project changes" and the Definition of Done. Flagged explicitly rather than silently corrected.
- **Resolved 2026-07-11:** `ROADMAP.md` was not updated when this work landed. Corrected in the same session — Phase 2 ("Vertical Slice") status changed to "In Progress" against its own listed goals, Phase 1 annotated to clarify this work doesn't count toward it, and a stronger terminology note added distinguishing Phase numbering from Sprint numbering (they are not meant to map to each other).
- **Resolved 2026-07-11:** `PlayerLevel`'s own doc comment named an intended "Sprint 2.3: upgrade selection" granting stat bonuses through `PlayerStats`. Superseded by Game Director decision — Sprint 2.3 is redirected to an Active Skills prototype (no stat picker), with persistent Skill Point allocation tied to `PlayerLevel` planned for the sprint after. `PlayerLevel.cs`'s doc comment is now stale on this point and should be updated when Sprint 2.3 lands.
- Diagnostic UI (`XPDiagnosticDisplay`) is explicitly prototype-only and will need replacement once real UI exists, same as `DebugHud`.

## Implementation Report

Code was implemented and committed directly by Claude across three commits on `feature/sprint-1-first-playable`, prior to this backfill:

- `64ceb5f` — **Sprint 2.0**: `PlayerStats` (base+modifier container), `PlayerReference` (single-player resolver replacing tag lookups in `EnemyAI`/`EnemySpawner`/`HitscanWeapon`).
- `398fb22` — **Sprint 2.1**: `PlayerXP` (accumulator), `EnemyXPReward` (grants configured XP immediately on enemy death via `Health.Died`), `XPDiagnosticDisplay` (temporary on-screen XP readout).
- `bbff4d3` — **Sprint 2.2**: `PlaceholderXpCurve` (placeholder linear-per-level formula), `PlayerLevel` (derives level purely from `PlayerXP.CurrentXP`, fires `LevelUp`, grants no stat bonus), `XPDiagnosticDisplay` extended with level + progress bar.

This task file and the corresponding `CHANGELOG.md` entries were written retroactively on 2026-07-11 to close the documentation gap — no gameplay code was changed as part of writing this file.

## Review Notes


## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: Explicitly approved in chat, individually, for all three sprints: *"I approve the overall direction... Sprint 2.0–2.2 backfill... record my explicit approval of Sprint 2.0, Sprint 2.1, and Sprint 2.2."* Preceded by: *"Your analysis is correct... update the Sprint 2.0-2.2 backfill documentation so it accurately records the completed and approved work."* Approval covers Sprint 2.0 (`PlayerStats`, `PlayerReference`), Sprint 2.1 (`PlayerXP`, `EnemyXPReward`), and Sprint 2.2 (`PlaceholderXpCurve`, `PlayerLevel`) as implemented, and the persistence model clarified in the same discussion: Character Level, total XP, and earned Skill Points are persistent character progression within a Season (see [DECISIONS.md](../../DECISIONS.md) DEC-020) — not a per-Operation temporary leveling loop. Explicitly not a claim that any save/disk-persistence system exists yet — see DEC-020's boundaries.

## Definition of Done Checklist

- [x] Acceptance criteria pass — independently re-verified 2026-07-11 via full Play Mode regression (see Test Procedure)
- [x] Unity has no new compiler errors (verified twice: read-only console check, and 0 console entries of any kind throughout the full regression run)
- [x] Required testing was completed — fresh functional Play Mode regression run 2026-07-11, all checks passed (see Test Procedure)
- [x] Yoav approved the result — explicit, per-sprint (Sprint 2.0, 2.1, 2.2 individually named)
- [ ] Technical review was completed — **not performed.** No Technical Director (per `PROJECT_BIBLE.md` § Team Roles, this project's "ChatGPT" role) was party to this session. Moved to DONE regardless, on Yoav's explicit direct instruction as Game Director and final approver — flagged here rather than silently checked.
- [x] Relevant documentation was updated (this task file + CHANGELOG.md entries)
