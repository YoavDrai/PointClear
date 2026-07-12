## Task ID
PC-008

## Title
Sprint 2.4 — Persistent Skill Points & Allocation

## Status
DONE (2026-07-11) — Yoav completed the hands-on Play Mode review and approved (points awarded on level-up; [+] enable/disable correct; allocation spends one and ranks up; Q Fracture Bolt damage scales; E Detonation Field radius scales; max-level and zero-point rejections correct; existing systems intact; no unexpected console errors). Technical-review box left unchecked (no Technical Director present); DONE on the Game Director's explicit authority, flagged — same handling as PC-003/PC-004/PC-006/PC-007/BUG-001.

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- Task PC-004 (Sprint 2.0–2.2 progression) — DONE (`PlayerXP`, `PlayerLevel` with `LevelUp(int)` event)
- Task PC-007 (Sprint 2.3 Active Skills) — DONE (`FractureBolt`, `DetonationField`)
- Sprint 2.4 Design Review — approved 2026-07-11 (this task implements it)

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster A → Sprint 2.4
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Progression Philosophy (XP unlocks build potential — levels and skill points)
- [DECISIONS.md](../../DECISIONS.md) DEC-020 (Level-Up Grants Persistent Build Potential Only)
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../../Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)

## Objective

Introduce the first Skill Point economy and a **data-driven** skill-allocation system, architected to scale to many skills (active, and later passive/ultimate/trees/synergies) without rewriting the core. Wire the two existing Active Skills as rank consumers so leveling up produces a real, spendable build choice — realizing DEC-020 in code.

## Persistence terminology (important)

"Persistent" here means **run-persistent**: Skill Points and allocated skill levels are retained throughout the current gameplay run — they are not reset between activations, level-ups, or scene ticks. **Disk save/load, persistence across game restarts, and meta-progression are explicitly OUT OF SCOPE** for this sprint (the architecture is designed to allow them later; they are not implemented here).

## Approved architecture (from the design review)

- **`SkillDefinition`** (ScriptableObject) — *shared metadata & progression only*: stable `Id`, `DisplayName`, `SkillType` (Active/Passive/Ultimate), `MaxLevel`. The core allocation system reads only these. It contains **no** per-skill gameplay fields and **no universal stat struct**.
- **Per-skill typed upgrade data lives with each skill's own behavior**, as clearly-typed serialized fields relevant to that skill (e.g. Fracture Bolt: damage per rank; Detonation Field: explosion radius per rank). No shared "every field for every skill" table.
- **`SkillPoints`** (MonoBehaviour on Player) — the wallet. Earns 1 point per level gained (from `PlayerLevel.LevelUp`), exposes `Available` and a change event, never goes negative.
- **`SkillProgression`** (MonoBehaviour on Player) — the registry. Reference-keyed (`SkillDefinition` → level; stable `Id` retained for future save). Initializes registered skills to their starting level, validates + performs allocation, fires `SkillLevelChanged(definition, level)`. Contains **no** knowledge of Fracture Bolt / Detonation Field, **no** string-based switch, **no** per-skill conditions.
- **Skill behaviors** remain responsible for their own gameplay; they read their current level from `SkillProgression` and apply their own typed per-rank data.
- **Minimal allocation UI** — prototype `OnGUI`, no polish.

## Starting-level decision (approved)

Both existing Active Skills **start at Level 1** and remain immediately usable (Q / E unchanged from Sprint 2.3). Skill Points raise them from Level 1 toward their configured `MaxLevel`. Level 0 remains *supported* by the architecture for future locked/unlearned skills but **unlocking is not implemented this sprint**.

## Requirements

- `SkillType` enum: `Active`, `Passive`, `Ultimate` (only `Active` used this sprint).
- `SkillDefinition` SO with `Id`, `DisplayName`, `SkillType`, `MaxLevel`; two assets created (`FractureBolt`, `DetonationField`) with stable ids (e.g. `fracture_bolt`, `detonation_field`).
- `SkillPoints`: subscribes to `PlayerLevel.LevelUp`; `Available` int, `+1` per level crossed (so a multi-level XP award grants one point per level); `Changed` event; a spend operation that refuses when `Available == 0` (never negative).
- `SkillProgression`: serialized list of registered `SkillDefinition`s + a `startingLevel` (=1); `GetLevel(def)`; `TryAllocate(def)` with validation order that checks *registered* and *below max* **before** spending a point; `SkillLevelChanged(def, level)` event; exposes the registered list for the UI.
- `TryAllocate` rules: reject if definition null/unregistered, if `Available == 0`, or if already at `MaxLevel`; otherwise spend exactly one point and raise exactly one level.
- `FractureBolt` and `DetonationField` each: reference their `SkillDefinition`, hold a typed per-rank array (Fracture Bolt → primary damage per rank; Detonation Field → explosion radius per rank), read current level from `SkillProgression`, recompute on `SkillLevelChanged`, and apply the value (defensively clamped to the array). Each shows a **meaningful per-rank gameplay change**.
- Minimal UI: available points + per-skill `Name Lv X/Max [Allocate]`, allocate disabled at 0 points or max level.
- Player prefab wired with `SkillPoints` + `SkillProgression`; skill behaviors reference their definitions.

## Acceptance Criteria

- [x] A normal single level-up grants exactly 1 Skill Point. Verified: +5 XP (level 1→2) → +1 point.
- [x] Crossing multiple levels from one XP award grants one point per level crossed. Verified: +100 XP crossed 5 levels (2→7) → +5 points.
- [x] Available points remain until spent; never negative. Verified: points held across level-ups; zero-point spend rejected, balance stayed 0.
- [x] A successful allocation reduces available points by exactly one and raises exactly one skill by exactly one level. Verified: allocate FB → points 6→5, FB level 1→2.
- [x] Allocation is rejected (no state change) when unregistered, when points are zero, or at `MaxLevel`. Verified: at-max FB → false, level 3→3, points 4→4; zero-point FB → false, level 1→1, points 0→0.
- [x] Both skills begin at Level 1 and are immediately usable (Q/E unchanged). Verified: start levels 1/1; both `Activate()` paths fire.
- [x] Fracture Bolt's per-rank value (damage) and Detonation Field's per-rank value (explosion radius) each change correctly at higher ranks. Verified: FB damage 40 (L1) → 70 (L3), applied to a target; DF explosion radius 3 (L1, victim@4.5 not caught) → 5 (L3, victim@4.5 caught).
- [x] `SkillProgression` contains no per-skill names, string switches, or individual-skill conditions; core reads only `SkillDefinition` metadata. Verified by inspection — registry keyed on `SkillDefinition` references, reads only `MaxLevel`.
- [x] Minimal allocation UI shows points and allocates; disables correctly (`SkillAllocationHud`, OnGUI; `CanAllocate` gates the button, "MAX" at cap).
- [x] Regression: weapon present, XP-on-kill (+1), level, enemy pursuit (reached 2.0), obstacle avoidance (reached), both Active Skills (FB fires, DF marks + chains — confirmed in isolation) all intact; `PlayerStats.Damage` unchanged (10).
- [x] Zero new Unity compiler errors or console warnings.
- [x] No Sprint 2.5 features (passives, trees, synergies, save/load, respec) added.

## Test Procedure

Play Mode in `PrototypeScene`, deterministic frame-stepping where useful:
1. Add XP for exactly one level → `SkillPoints.Available` increments by 1.
2. Add a large XP award crossing several levels → `Available` increases by the number of levels crossed.
3. Confirm points persist (run-persistent) across subsequent frames/level-ups until spent.
4. `TryAllocate(fractureBolt)` with a point → returns true, `Available −1`, Fracture Bolt level `+1`.
5. Drain points to 0, `TryAllocate` → returns false, no change.
6. Raise a skill to `MaxLevel`, `TryAllocate` → returns false, no change, `Available` unchanged.
7. Confirm both skills report Level 1 at start and fire on Q/E.
8. Fracture Bolt damage differs between rank 1 and a higher rank (measured on a target); Detonation Field explosion radius differs between ranks (measured by which enemies a detonation catches).
9. Regression battery (weapon fire, XP-on-kill, level, enemy chase, obstacle avoidance, Detonation chain).
10. Console: zero errors/warnings.

## Files Allowed to Edit

- `Assets/Systems/Skills/` (new: `SkillType.cs`, `SkillDefinition.cs`, `SkillPoints.cs`, `SkillProgression.cs`; modify `FractureBolt.cs`, `FractureBoltProjectile.cs`, `DetonationField.cs`)
- `Assets/Systems/Utilities/` (new minimal `SkillAllocationHud.cs`, or extend `DebugHud.cs`)
- `Assets/ScriptableObjects/Skills/` or `Assets/Skills/` (new `SkillDefinition` assets)
- `Assets/Prefabs/Player/Player.prefab` (add `SkillPoints`, `SkillProgression`, wire references)
- `Assets/Scenes/Prototype/PrototypeScene.unity` (only if wiring the UI/player instance requires it)
- `Tasks/**/PC-008_sprint-2.4-skill-points-and-allocation.md`
- `CHANGELOG.md`, `ROADMAP.md` (status sync)

## Files Forbidden to Edit

- `PlayerXP.cs`, `PlayerLevel.cs` (consume `LevelUp`/`CurrentLevel`; do not modify — if a change seems required, stop and flag)
- `PlayerStats.cs` (untouched this sprint; its add-only limitation is a Sprint 2.5 concern for passives)
- `Health.cs`, `EnemyAI.cs`, and all documentation not listed above

## Out of Scope

- Passive skills, Ultimate skills (enum value may exist; no behavior)
- Upgrade trees / branches / prerequisites
- Random level-up choice UI ("choose 1 of 3")
- Skill synergies / cross-skill interaction logic
- Disk save/load, cross-restart persistence, meta-progression
- Respec UI (the architecture allows free respec later; not built)
- `PlayerStats` modifier-removal rework
- Additional skills, loadout selection, balancing pass, UI polish, networking

## Risks

- **Architecture introduction:** this turns on the shared skill abstraction previously deferred — approved in the design review. Kept minimal (metadata SO + wallet + registry).
- **Behavior/definition consistency:** each behavior's per-rank array length should match its definition's `MaxLevel`; the behavior clamps defensively if not.
- **Ordering:** `SkillPoints` must subscribe to `LevelUp` before gameplay XP is awarded; at start (level 1, 0 XP) no points are granted, which is correct.
- **String ids** are for future save only; runtime keys on direct asset references to avoid typos.
- Keep intentionally simple: no trees/synergy/save this sprint.

## Implementation Report

Implemented by Claude 2026-07-11, branch `feature/sprint-1-first-playable`, uncommitted.

**Core (new, `PointClear.Skills`):**
- `SkillType.cs` — enum (Active/Passive/Ultimate; only Active used).
- `SkillDefinition.cs` — ScriptableObject holding shared metadata only (Id, DisplayName, SkillType, MaxLevel). No per-skill gameplay fields, no universal stat table.
- `SkillPoints.cs` — wallet; subscribes to `PlayerLevel.LevelUp`, +1 point per level crossed; `Available`, `Changed` event, `TrySpend()` that never goes negative.
- `SkillProgression.cs` — reference-keyed registry (`SkillDefinition` → level); `startingLevel` init; `GetLevel`, `CanAllocate`, `TryAllocate` (validates registered + below-max **before** spending), `SkillLevelChanged` event, `RegisteredSkills`. Fully skill-agnostic — no skill names, no string switch.

**Behaviors as rank consumers (typed per-rank data lives here, not in the core):**
- `FractureBolt.cs` — references its `SkillDefinition` + `SkillProgression`; holds `damagePerLevel` (`{40,55,70}`); reads level, recomputes on `SkillLevelChanged`, applies via new `FractureBoltProjectile.SetDamage`.
- `DetonationField.cs` — holds `explosionRadiusPerLevel` (`{3,4,5}`); scales the detonation radius by rank.
- `FractureBoltProjectile.cs` — added `SetDamage` (primary bolt only; shards keep prefab damage).

**UI:** `SkillAllocationHud.cs` (prototype OnGUI) — available points + per-skill `Name Lv X/Max [+]`, button gated by `CanAllocate`, "MAX" at cap.

**Assets/wiring:** two `SkillDefinition` assets in `Assets/ScriptableObjects/Skills/` (`FractureBolt` id `fracture_bolt`, `DetonationField` id `detonation_field`, both MaxLevel 3); `Player.prefab` gained `SkillPoints` + `SkillProgression` (registered both skills, startingLevel 1); skill behaviors wired to their definitions + progression; `SkillAllocationHud` added to the scene HUD host and wired to the player's wallet/registry.

**Data-design compliance:** the constraint (no universal per-level struct) is honored — `SkillDefinition` carries only metadata/progression; each skill's typed upgrade array lives on its own behavior. The core references definitions directly and keeps the string Id only for future save.

**Persistence:** run-persistent (in-memory for the session). No disk save/load — out of scope, per PC-008.

**Untouched (per Files Forbidden):** `PlayerXP`, `PlayerLevel`, `PlayerStats`, `Health`, `EnemyAI`.

**Testing:** full Play Mode verification via deterministic stepping — see Acceptance Criteria (all pass); zero console errors/warnings. Real Q/E keyboard allocation via the on-screen panel is the remaining hands-on confirmation for Yoav's review (input bindings/behaviors verified; UI logic code-reviewed).

## Review Notes

Yoav's hands-on Play Mode review confirmed every acceptance item: Skill Points awarded on level-up; [+] buttons enable/disable correctly; allocation reduces the wallet by one and raises the selected rank; Fracture Bolt (Q) damage scales by rank; Detonation Field (E) radius scales by rank; no upgrade past Max Level; allocation rejected at zero points; existing combat/enemy/XP/level/movement/weapon systems intact; no unexpected console errors. This completes the hands-on confirmation that was the one item outstanding from the automated verification.

## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: Approved after a hands-on Play Mode review of the full allocation loop and both skills' per-rank scaling. Realizes DEC-020 in code (run-persistent).

## Definition of Done Checklist

- [x] Acceptance criteria pass (Play Mode verified + Yoav's hands-on review)
- [x] Unity has no new compiler errors (0 errors, 0 warnings)
- [x] Required testing was completed (automated functional + regression, plus Yoav's hands-on playtest)
- [x] Yoav approved the result
- [ ] Technical review was completed — not performed (no Technical Director present); DONE on the Game Director's explicit authority, flagged
- [x] Relevant documentation was updated (this task file, CHANGELOG.md, ROADMAP.md)
