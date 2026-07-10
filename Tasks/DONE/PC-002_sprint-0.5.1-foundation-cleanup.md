# Task Template

Copy this file into [Tasks/TODO/](../TODO/) as `PC-XXX_short-title.md` when a task is created. See [Documentation/AI/TASK_WORKFLOW.md](../../Documentation/AI/TASK_WORKFLOW.md).

---

## Task ID
PC-002

## Title
Sprint 0.5.1 — Foundation Cleanup

## Status
DONE

## Priority
Medium

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- Task PC-001 (Sprint 0.5 Production Foundation) — DONE, approved by Yoav 2026-07-11

## Related Documents
- [Tasks/DONE/PC-001_sprint-0.5-production-foundation.md](PC-001_sprint-0.5-production-foundation.md)

## Objective

Remove Sprint 0.5 abstractions that have no real consumer today, per the stated philosophy: "Architecture should emerge from real gameplay needs, not from anticipated future needs."

## Background

PC-001's technical review flagged several abstractions (`GameManager`, `GameDataSO`, `IDamageable`, `IHealth`, `IWeapon`, `SceneLoader.LoadSceneAsync`) as having zero current consumers, justified only by anticipated Sprint 1 needs. Yoav requested a short cleanup sprint before Sprint 1 begins to remove whatever doesn't earn its place today.

## Requirements

- Re-evaluate each of the 7 named items against: required for Sprint 1? real consumer today? concrete problem solved today? what's lost if removed?
- Remove anything that fails that test, without silently dropping the acceptance criteria PC-001 already satisfied (Bootstrap → PrototypeScene must keep working)
- No new systems, no gameplay, no expanded architecture

## Acceptance Criteria

- [x] `GameManager` removed; its one real responsibility (triggering the initial scene load) absorbed into `SceneLoader`
- [x] `GameDataSO` removed (zero subclasses existed)
- [x] `IDamageable`, `IHealth`, `IWeapon` removed (zero consumers existed)
- [x] `SceneLoader.LoadSceneAsync` removed (zero call sites, discarded `AsyncOperation`)
- [x] `SceneLoader` kept, now the project's only manager
- [x] Bootstrap → PrototypeScene flow still verified working in Play Mode
- [x] Zero compile errors, zero console warnings after cleanup

## Test Procedure

1. Open `Assets/Scenes/Bootstrap/Bootstrap.unity`, confirm the `SceneLoader` GameObject has no orphaned/missing-script components.
2. Enter Play Mode, confirm the active scene switches to `PrototypeScene` automatically.
3. Confirm zero errors/warnings in the Console throughout.
4. Exit Play Mode, confirm the Bootstrap scene is unchanged on disk.

## Files Allowed to Edit

- `Assets/Systems/Managers/SceneLoader.cs`
- `Assets/Scenes/Bootstrap/Bootstrap.unity`
- Deletion of: `Assets/Systems/Managers/GameManager.cs`, `Assets/ScriptableObjects/GameDataSO.cs`, `Assets/Systems/Interfaces/IDamageable.cs`, `Assets/Systems/Interfaces/IHealth.cs`, `Assets/Systems/Interfaces/IWeapon.cs`
- `Tasks/DONE/PC-001_sprint-0.5-production-foundation.md` (review notes/approval only)
- `Tasks/DONE/PC-002_sprint-0.5.1-foundation-cleanup.md`
- `CHANGELOG.md`

## Files Forbidden to Edit

- Everything not listed above. Folder structure from PC-001 is unchanged — `Systems/Interfaces/` and `ScriptableObjects/` remain as empty production folders; this cleanup is scoped to the 7 named code items, not folder layout.

## Out of Scope

- Gameplay, new systems, expanded architecture, preparing for future features
- Folder restructuring

## Risks

- **`Systems/Interfaces/` and `ScriptableObjects/` are now empty folders.** Left in place since folder structure wasn't in scope for this cleanup; if Sprint 1 doesn't need them quickly, worth revisiting whether empty production folders should persist.
- **Deleting these five files required treating Yoav's Sprint 0.5.1 spec as explicit, specific authorization for asset deletion** (normally outside Claude's independent authority per CLAUDE_RULES) — flagged here for the record, not because there's doubt about the authorization.

## Implementation Report

Implemented by Claude on 2026-07-11 on branch `feature/sprint-0.5-production-foundation`, uncommitted. Per-item analysis (required for Sprint 1 / real consumer / problem solved / what's lost) delivered in chat, agreeing with all five recommended removals — no disagreement found on technical grounds. `GameManager`'s only real behavior (loading `PrototypeScene` on start) was moved into `SceneLoader` so PC-001's Bootstrap acceptance criterion keeps passing with one fewer class. Verified via Play Mode that Bootstrap still loads PrototypeScene with zero console errors/warnings after removal. One incidental defect found and fixed during cleanup: the Bootstrap scene file retained an orphaned "missing script" `MonoBehaviour` block referencing the deleted `GameManager` after the component browser stopped surfacing it — removed directly from the scene YAML and reloaded from disk to confirm the fix. Not committed — awaiting Yoav's review.

## Review Notes

Yoav indicated the technical cleanup was "approved in principle" (2026-07-11), then completed an in-editor Game Director playtest and gave explicit final approval the same day. Per-item removal analysis (required for Sprint 1 / real consumer / problem solved / what's lost) delivered in chat for all 7 named items; no disagreement found on technical grounds.

## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: "I have completed the Game Director playtest. PC-001 and PC-002 are now explicitly approved." Explicit final approval given after in-editor playtest.

## Definition of Done Checklist

- [x] Acceptance criteria pass
- [x] Unity has no new compiler errors
- [x] Required testing was completed (Play Mode verification of Bootstrap flow after cleanup)
- [x] Yoav approved the result
- [x] Technical review was completed (per-item removal analysis delivered in chat 2026-07-11)
- [x] Relevant documentation was updated (CHANGELOG.md entry added; PC-001 review notes updated)
