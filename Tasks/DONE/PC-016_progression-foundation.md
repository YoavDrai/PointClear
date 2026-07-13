# Task PC-016 — Long-Term Progression Foundation (Death XP Penalty + Player-Driven Character-Start Skill Allocation)

## Task ID
PC-016

## Title
Death XP penalty (DEC-032) + player-driven character-start skill allocation (DEC-033)

## Status
DONE — playtest-approved by the Game Director 2026-07-13. Technical-review box left
unchecked (no Technical Director present); DONE on the Game Director's explicit
authority, flagged (consistent with PC-007..015).

## Priority
Medium

## Owner
Claude (implementation) / Yoav (direction + playtest approval)

## Reviewer
Yoav (Game Director) — no Technical Director present this session

## Dependencies
- Sprint 2.6 Operation lifecycle (`OperationController.OperationFailed`)
- `PlayerXP` / `PlayerLevel` (derived-level progression)
- Resolves the open number in DEC-019

## Related Documents
- [DECISIONS.md](../../DECISIONS.md) DEC-032 (this task's decision), DEC-016/018/019/020
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) points 21–24 (Persistence + Mission Risk)
- [CHANGELOG.md](../../CHANGELOG.md) 2026-07-13

## Objective
On Operation failure by player death, reduce only the player's progress into the
current level by a configurable flat fraction of that level's XP bar (default
20%), never removing a level, Skill Point, allocation, or Banked reward.

## Background
Game-Director-directed insert toward the long-term ARPG/extraction progression
vision. A design review found the permanent-vs-run separation the vision needs is
already built (permanent: PlayerXP→PlayerLevel→SkillPoints/SkillProgression;
run-scoped secure-or-lose: CurrencyWallet/WeaponModule on the Operation events).
The only genuinely missing piece was a failure XP penalty, which also resolves
DEC-019's previously-open "exact retained-Experience amount." This reverses the
prior "XP fully retained on failure" behaviour and is off the pre-existing
roadmap sequence, landed while the Cluster B gate is intentionally still open —
all flagged and Game-Director-approved to proceed (DEC-032).

## Requirements
- Penalty = `penaltyFraction * XPRequiredForCurrentLevel`, capped at current
  progress into the level (never de-levels). Default fraction 0.2, inspector-configurable, `[Range(0,1)]`.
- Triggers only on `OperationController.OperationFailed` (mission-risk death), not raw death outside an Operation.
- No modification to the Operation lifecycle scripts (subscribe only), matching CurrencyWallet/WeaponModule.
- Levels, total Skill Points, Skill Tree allocations, and Banked rewards untouched.
- The loss is visible in the failure results summary.
- New characters start with a configurable Skill Point pool (`SkillPoints.startingPoints`, default 2),
  granted exactly once per instance, never re-granted on OnEnable/reset/death/respawn/repeated init.
- A future save/load seam (`SkillPoints.LoadSavedBalance`) prevents duplicating the starter for an
  existing character; the temporary "fresh instance == new character" assumption is documented in code.

## Acceptance Criteria
- [ ] 40% progress → death → 20% (fraction 0.2); level unchanged.
- [ ] 15% progress → death → 0%; level unchanged (capped, no de-level).
- [ ] Death at 0% progress reduces nothing and does not de-level.
- [ ] Repeated deaths never de-level the player.
- [ ] Completed levels and already-awarded Skill Points remain retained.
- [ ] Success applies no penalty; XP/levels unchanged on extraction.
- [ ] Death outside an Operation applies no penalty.
- [ ] `OperationHud` failure summary shows "XP progress lost: -N" when a loss occurred.
- [ ] Starting Skill Points read 2 for a new character, granted only once.
- [ ] Operation reset and respawn do not re-grant the starting points.
- [ ] No new Unity compiler/console errors.
- [ ] Game Director hands-on playtest approves the feel.

## Unity Validation Status — PASS (2026-07-13, automated in-editor; hands-on playtest still pending)
Unity 6000.5.3f1, PrototypeScene. `DeathXPPenalty` added to `Assets/Prefabs/Player/Player.prefab`
(root `Player`, alongside PlayerXP/PlayerLevel/SkillPoints). Compile: **0 errors** (only pre-existing
project-wide `CS0618 FindFirstObjectByType` + one pre-existing `StalkerAI CS0414` warning). Runtime
validation drove the real components on the live player:
- Wiring: `startingPoints`=2, `Available` at play start=2, `penaltyFraction`=0.2, `operation` resolved
  + `subscribed`=true (to OperationFailed only), HUD's `deathXPPenalty` == the player's component.
- C1 40%→20% (pen 1.0); C2 15%→0% (pen 0.75, capped); C3 0%→0% (pen 0, no de-level);
  C4 60%→40%→20%→0%→0% across 4 deaths, stays L1 (never de-levels);
  C5 at L2 30%: level-up granted +1 SP (2→3), after death stays L2 (into 3.0→1.0, pen 2.0), SP stays 3
  (completed L1 + earned SP retained); C7 reset+respawn leaves SP at 3 (no re-grant).
- Success / non-Operation death apply no penalty by design (component subscribes to `OperationFailed`
  only — no `Died`/`Succeeded` subscription).
- HUD line: data path proven (ref resolves, `LastPenaltyApplied` populates, render guard `!success &&
  >0`); pixel-level confirmation is part of the hands-on playtest.
- Console: 0 errors before/after play mode.

Hands-on playtest: **PASSED (2026-07-13)** — GD confirmed the 40%→20% penalty, the
never-de-level floor, and retained levels/Skill Points.

## Addendum — Player-Driven Character-Start Skill Allocation (DEC-033)
Added at Game-Director direction after the penalty work: a new character no longer
starts with Active skills pre-chosen. See [DECISIONS.md](../../DECISIONS.md) DEC-033.

Decisions (GD): confirm allowed with points unspent (weapon-only start valid);
starting set is data-driven via `SkillDefinition.AvailableAtCharacterStart` (not
hardcoded); the StartingDirection screen becomes the allocation step (cosmetic node
replaced); non-start skills stay reachable via normal leveling (no hard lock — future
seam).

Changes: `SkillDefinition` (`AvailableAtCharacterStart` flag), `FractureBolt` +
`DetonationField` (rank≥1 activation gate), `CombatBridge` (start-skill accessors),
`SessionContext` (`InitialAllocationConfirmed`, replacing the vestigial
`StartingNodeConfirmed`), `FrontEndUI` (StartingDirection = allocation UI), and the
three skill assets (`FractureBolt`/`DetonationField` StartingLevel 1→0 +
AvailableAtCharacterStart true; `VolatileFracture` pinned false).

### Acceptance (addendum)
- [ ] Fresh character: both Active skills at Lv 0, unusable until allocated; Skill Points 2.
- [ ] Start step offers only `AvailableAtCharacterStart` skills (data-driven; passive excluded).
- [ ] Allocating raises rank, unlocks the skill, and spends points; refused at 0 points.
- [ ] First run cannot begin until the allocation step is explicitly confirmed; unspent points allowed.
- [ ] Non-start skills still reachable via later leveling (no regression to Volatile Fracture).
- [ ] Basic weapon still works from the start (weapon-only start is playable).
- [ ] Game Director hands-on playtest approves the feel.

### Unity validation (addendum) — PASS (2026-07-13, automated in-editor)
0 compile errors. Fresh character: FractureBolt/DetonationField Lv 0, both `IsUnlocked=false`,
SkillPoints 2. `CharacterStartSkills` = exactly {Fracture Bolt, Detonation Field}; Volatile
Fracture excluded. Allocate via `CombatBridge`: FB→1 (pts 2→1, unlocked), DF→1 (pts 1→0,
unlocked), 3rd allocate refused (0 pts). StartingDirection screen builds 2 allocation rows;
`OnConfirmStartingDirection` sets `InitialAllocationConfirmed=true` and advances to World Map.

Hands-on playtest: **PASSED (2026-07-13)** — GD confirmed fresh start = 2 points with both
actives at Lv 0 and unusable; start screen shows only configured start-available skills;
spend 0/1/2 and confirm all work; allocated skills fire, unallocated stay locked; Volatile
Fracture still reachable later through normal progression.

## Test Procedure
1. In Unity, add the `DeathXPPenalty` component to the Player GameObject (it
   requires `PlayerXP` + `PlayerLevel`, already present); assign the `OperationHud`
   `deathXPPenalty` reference (or leave for auto-resolve).
2. Enter Play, gain XP to reach a partial level (e.g. Level 2–3 mid-bar).
3. Start an Operation, note XP-into-level, then die. Confirm level unchanged and
   progress reduced by ~fraction·bar; confirm the "XP progress lost" line.
4. Repeat while at the very start of a level → confirm no de-level.
5. Repeat but extract successfully → confirm no penalty.

## Files Allowed to Edit
- `Assets/Systems/Progression/PlayerXP.cs`
- `Assets/Systems/Progression/DeathXPPenalty.cs` (new)
- `Assets/Systems/Utilities/OperationHud.cs`
- `DECISIONS.md`, `CHANGELOG.md`, `ROADMAP.md`, this task file

## Files Forbidden to Edit
- `OperationController.cs` and the rest of the Operation lifecycle
- `PlayerLevel.cs` core loop, `PlaceholderXpCurve.cs`
- Skills/, combat, enemy code — everything not listed above

## Out of Scope
- Save/load, inventory, stash, rarity, affixes, multiple characters, networking
- The "2 free starting Skill Points" and "skill tree opens on level up" flow items
  (noted as small follow-ups, not built here)
- XP-gain changes, curve tuning, or any balance decision on the fraction value
