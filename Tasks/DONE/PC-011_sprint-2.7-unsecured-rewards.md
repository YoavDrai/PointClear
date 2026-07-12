## Task ID
PC-011

## Title
Sprint 2.7 — Unsecured Rewards & Mission Risk (Cluster B)

## Status
DONE (2026-07-12) — Game Director approved after hands-on Play Mode review (Option B, physical currency drop). Confirmed: drop/collect, unsecured→banked on success, loss on failure, banked persistence, HUD legibility (after the round-1 layout fix), and the round-2 pickup-radius game-feel (configurable, no vacuum). Event-driven cleanup kept minimal by GD decision (no `OperationController` change for the unreachable InProgress→Return path). All deterministic checks pass; zero console errors; no forbidden files and no 2.6 lifecycle scripts touched.

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- PC-010 (Sprint 2.6 Minimal Operation Wrapper) — DONE (`OperationController` with `OperationStarted/Succeeded/Failed` events, `ExtractionPoint`, kill-quota, `EnemySpawner.EnemyKilled`)
- Existing `Health.Died`, `EnemyXPReward` (XP already immediate + retained — untouched)

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster B → Sprint 2.7
- [DECISIONS.md](../../DECISIONS.md) DEC-018 (XP/Loot separation — currency is physical loot), DEC-019 (mission risk / unsecured rewards), DEC-016 (no mission resets the character)
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Mission Risk Philosophy

## Objective
Add the smallest observable version of **secure-or-lose value** to the Operation: enemies drop one simple **physical** currency pickup on death; collecting it adds to an **Unsecured** wallet; Operation **Success banks** it, **Failure loses** it; **Banked** carries across Operations in the session. XP retention is already satisfied (immediate grant) and is not touched. Makes DEC-019 real in code.

## Player emotion
"I have something to lose now." Extraction becomes a *bank* button; once it opens, **extract-now vs. keep-fighting-for-more** is a real choice. First honest taste of greed-vs-safety in the single-Operation structure.

## Approved player flow (GD)
- Start Operation → Unsecured = 0.
- Enemy genuine death → drops one physical pickup at its position.
- Player walks over pickup → collects its fixed value into Unsecured.
- Kill quota opens extraction; enemies may keep spawning.
- After extraction opens: extract now (bank collected) **or** keep fighting for more (risking all Unsecured).
- Success → banks all collected Unsecured. Failure → loses all Unsecured. Banked unchanged. Uncollected pickups are never banked.

## Architecture (approved constraints — follow exactly)
- **`CurrencyWallet`** (Player component) — sole owner of `Unsecured`, `Banked`, and a `Changed` event. Subscribes safely (no duplicate subs) to the existing Operation events: `OperationStarted` → clear Unsecured (+ defensive pickup clear); `OperationSucceeded` → Banked += Unsecured **once** then clear Unsecured (+ clear pickups); `OperationFailed` → clear Unsecured, Banked unchanged (+ clear pickups). Banked is **session/in-memory only**. No spending/inventory/save/economy.
- **`EnemyCurrencyDrop`** (on the Enemy prefab, mirrors `EnemyXPReward`) — observes the existing public `Health.Died` (no `Health.cs` change); spawns exactly one pickup per genuine death; **does not** drop on cleanup-destroy (`Destroy` doesn't fire `Health.Died`); unsubscribes safely; fixed configurable `amount`; no RNG/rarity/tables/multipliers/per-enemy data.
- **`CurrencyPickup`** (on a simple pickup prefab) — trigger-based (does not block movement); collected only by the player; collected at most once; self-destroys on collection; clear prototype visual; a static `ClearAll()` destroys all live pickups (uncollected → not banked). No general pickup framework.
- **Pickup cleanup** — via the wallet's existing event handlers (`Started`/`Succeeded`/`Failed`) calling `CurrencyPickup.ClearAll()`. **Return-to-Ready is covered transitively**: a run only reaches Ready *after* a terminal event (Succeeded/Failed) that already cleared pickups + Unsecured; and no drops occur after a terminal state (spawner stopped, enemies cleared). **No modification to `OperationController`/`ExtractionPoint`/`OperationState`** — attach via public events only. If a real blocker requires touching them, STOP and report before changing scope.
- **`OperationHud`** — display **Unsecured** (at risk) and **Banked** (secured), so Success (Unsecured→Banked) and Failure (Unsecured lost) read clearly. Prototype text; no results screen.

## XP retention (DEC-019) — no work
XP is granted immediately on kill and persists; failure never removes it → "XP retained" already true. 2.7 implements the currency half only. A partial-retention *penalty* would require XP clawback — out of scope.

## Scope — explicitly EXCLUDED (do not build)
Results screen · currency spending · inventory · equipment/item generation · RNG amounts · drop tables · rarity · disk persistence · multi-Operation expedition chains · push-depth systems · any universal loot/reward/pickup/economy framework. Also: no change to the 2.6 Operation lifecycle scripts.

## Files Allowed to Edit
- **New:** `Assets/Systems/Operations/CurrencyWallet.cs`, `CurrencyPickup.cs`, `EnemyCurrencyDrop.cs`; a `CurrencyPickup` prefab under `Assets/Prefabs/`
- **Modify:** `Assets/Systems/Utilities/OperationHud.cs`; `Assets/Prefabs/Player/Player.prefab` (add `CurrencyWallet`); the Enemy prefab (add `EnemyCurrencyDrop` + assign pickup prefab); `Assets/Scenes/Prototype/PrototypeScene.unity` (wiring); `CHANGELOG.md`, `ROADMAP.md`, `Tasks/**/PC-011_*.md`

## Files Forbidden to Edit
- `Health.cs`, `EnemyAI.cs`, `EnemyXPReward.cs`, `PlayerStats/XP/Level`, all `Skills/`, `SkillPoints`, `SkillProgression`, and the 2.6 `OperationController.cs`/`ExtractionPoint.cs`/`OperationState.cs`
- All documentation not listed above

## Risks & Edge Cases
- Only *collected* currency banks; uncollected pickups cleared on run end (never banked).
- Pickups must not accumulate across runs (cleared on Started/Succeeded/Failed).
- Cleanup-destroyed enemies must not drop (relies on `Destroy` not firing `Health.Died` — verified in 2.6).
- Banked must never be erased by a reset; reset zeroes Unsecured only; DEC-016 progression untouched.
- Terminal precedence: bank exactly once; no bank-then-lose.
- Banked currency is not spendable yet (intended; not a bug).

## Acceptance Criteria
- [x] Start Operation → Unsecured 0; Banked unchanged. Verified.
- [x] Genuine enemy death drops exactly one pickup; cleanup-destroyed enemies drop none. Verified (kill → pickups 0→1; `DestroyImmediate` of a live enemy → pickups unchanged, no `Health.Died`).
- [x] Walking over a pickup adds its fixed value (5) to Unsecured once; pickup self-destroys; not re-collectable. Verified (collect → +5; second trigger on same pickup → no change).
- [x] Extraction Success → Banked += Unsecured **once**; Unsecured → 0. Verified (collected 15 → Banked 0→15, Unsecured 0).
- [x] Failure → Unsecured → 0; Banked unchanged. Verified (Unsecured 5→0, Banked stayed 15).
- [x] Return to Ready / new Operation → Unsecured 0; **Banked persists**; DEC-016 unchanged. Verified (Banked 15 across ReturnToReady + new StartOperation; SkillPoints/Level unchanged; XP not clawed back on failure).
- [x] Uncollected pickups cleared on run end; never banked; no accumulation. Verified (`CurrencyPickup.Active` → 0 after Success/Failure; only collected currency banked).
- [~] HUD legibly shows Unsecured and Banked. Implemented (OperationHud); on-screen confirmation pending Yoav's hands-on view.
- [x] 2.6 lifecycle unchanged; `OperationController`/`ExtractionPoint`/`OperationState` not modified (git-confirmed); attached via public events only.
- [x] Zero Console errors/warnings; no out-of-scope systems (no results screen, spend, inventory, RNG, rarity, drop tables, framework).

**Pending Yoav's hands-on playtest** (needs live frames/input the automation editor did not tick): physically collecting drops by walking over them (`OnTriggerEnter`), the extract-now-vs-keep-fighting choice after extraction opens, and the felt loss of dying with unsecured currency. Every code path was verified programmatically (drops via real `Health.Died`, collection via the real `OnTriggerEnter`, banking via the real Operation events).

## Test Procedure (deterministic Play Mode)
1. Start → assert Unsecured 0.
2. Spawn+kill enemies → assert one pickup per kill; cleanup-destroy an enemy → assert no pickup.
3. Collect pickups (move player onto them / simulate trigger) → assert Unsecured = N × value; pickups destroyed; not re-collectable.
4. Reach extraction → assert Banked += Unsecured once, Unsecured 0.
5. New run, accumulate, kill player → assert Unsecured 0, Banked unchanged, **XP/Level retained**.
6. ReturnToReady → assert Unsecured 0, Banked persists, no leftover pickups; DEC-016 intact.
7. Terminal precedence: near-simultaneous success/death → single outcome, wallet consistent.
8. Console clean.

## Implementation Report

Implemented by Claude 2026-07-12, branch `feature/sprint-1-first-playable`. **Uncommitted (in REVIEW).**

**New (`PointClear.Operations`):**
- `CurrencyWallet.cs` — Player component, sole owner of `Unsecured`/`Banked` + a `Changed` event. Subscribes to the existing `OperationController.OperationStarted/Succeeded/Failed` (safe, no duplicate subs). Started → clear Unsecured + `CurrencyPickup.ClearAll()`; Succeeded → `Banked += Unsecured` (once) then clear Unsecured + ClearAll; Failed → clear Unsecured + ClearAll (Banked untouched). Banked is session/in-memory only. **Does not modify `OperationController`** — attaches via events. Return-to-Ready needs no handler (a terminal event always precedes it and already cleared Unsecured + pickups; no drops occur after a terminal state).
- `CurrencyPickup.cs` — trigger-based drop; collected once, by the player only; grants its value to the wallet's Unsecured; self-destroys; static `ClearAll()` destroys all live pickups (uncollected → never banked). Gold-tinted via `MaterialPropertyBlock` (asset-free, same pattern as `DetonationMark`).
- `EnemyCurrencyDrop.cs` — on the Enemy prefab, mirrors `EnemyXPReward`; observes the existing public `Health.Died` (no `Health.cs` change); spawns one pickup per genuine death at the enemy's position; fixed configurable `amount` (5); no RNG/rarity/tables. Cleanup-destroyed enemies drop nothing (`Destroy` doesn't fire `Health.Died`).

**Modified:** `OperationHud.cs` — shows `Unsecured (at risk)` and `Banked (secured)`.

**Assets/wiring:** new `CurrencyPickup` prefab (`Assets/Prefabs/Operations/CurrencyPickup.prefab`, a small gold sphere, trigger collider). `Enemy.prefab` gained `EnemyCurrencyDrop` (pickup ref wired, amount 5). `Player.prefab` gained `CurrencyWallet`. HUD/wallet auto-resolve their references, so no scene wiring beyond prefab-instance propagation. Scene saved.

**Confirmed unmodified (git):** `Health`, `EnemyAI`, `EnemyXPReward`, `PlayerStats/XP/Level`, all `Skills/`, `SkillPoints`, `SkillProgression`, and the **2.6 lifecycle** (`OperationController`/`ExtractionPoint`/`OperationState`).

**Testing:** deterministic Play-Mode harness (via execute_code; automation editor doesn't tick frames, so drops were driven through real `Health.Died`, collection through the real `OnTriggerEnter`, banking through the real Operation events). Validated: drop-per-kill, cleanup-destroy-no-drop, collect-into-Unsecured, not-re-collectable, Success-banks-once, Failure-loses-Unsecured, pickups-cleared-on-run-end, Banked-persists-across-resets, and DEC-016 progression preserved (XP not clawed back on failure). Zero console errors.

## Review Notes

**Review round 1 (2026-07-12) — HUD overlap fix.** Yoav's hands-on view showed the Operation panel and the Skills panel drawing on top of each other (both were anchored top-right). Repositioned the `OperationHud` panel to the right column **below** the Skills panel (`Screen.width-320, y 285`, clear of DebugHud on the left and the Skills panel above). Verified in Play Mode via a game-view screenshot: the OPERATION panel (state, `Kills X/8`, `EXTRACTION: LOCKED/OPEN`, `Unsecured` / `Banked`) now reads cleanly with no overlap. Only `OperationHud.cs` changed; no gameplay logic touched.

**Review round 2 (2026-07-12) — pickup radius (game-feel).** Added a small, per-pickup attract-and-collect so the player needn't step exactly on each drop. `CurrencyPickup` gained a serialized `pickupRadius` (default **1.75 m**) and `attractSpeed` (default 8): while the player is within the radius the pickup moves toward them (`Vector3.MoveTowards`) and is collected within a 0.5 m `CollectDistance`. Bounded per-pickup — **no global vacuum/magnet, no framework**; the trigger remains a direct-contact fallback. Collection is funnelled through one guarded `Collect()` so the radius pull and the trigger cannot double-collect. Verified in Play Mode: attract pulls the pickup in (1.20 m → 1.04 m per tick) but does not collect until inside CollectDistance; radius-collect and trigger-collect each add exactly once (no double); a distant pickup (25 m) is **not** auto-collected; cleanup still clears pickups on Success, Failure, post-terminal Return-to-Ready, and defensive Start (all → 0). Only `CurrencyPickup.cs` changed; `OperationController` still untouched.

**Note on Return-to-Ready cleanup:** cleanup is event-driven (wallet reacts to `OperationStarted/Succeeded/Failed`). Return-to-Ready has no event of its own, but it is only reachable *after* a terminal state (the HUD exposes no Return button while InProgress), and terminal events already clear pickups — so Ready never holds pickups in normal play (verified). Making cleanup fire *literally* on Return-to-Ready would require adding an event to `OperationController`, which is deliberately not done (out of the approved scope; flagged rather than changed).

## Game Director Approval
- [x] Approved by Yoav
- Date: 2026-07-12
- Notes: Hands-on review passed. Drop/collect, secure-or-lose (banks on Success, lost on Failure), banked persistence, and DEC-016 all confirmed. HUD layout fixed in round 1 and now clear; the round-2 pickup radius "feels much better… without turning the pickup into a loot vacuum," configurable radius as wanted. Agreed to keep event-driven cleanup as-is (no `OperationController` change for the unreachable InProgress→Return-to-Ready path).

## Definition of Done Checklist
- [x] Acceptance criteria pass (Play Mode verified — programmatic harness + Game Director hands-on review)
- [x] Unity has no new compiler errors (clean compile, zero console errors)
- [x] Required testing was completed (deterministic harness + hands-on playtest, incl. two review rounds)
- [x] Yoav approved the result (2026-07-12)
- [ ] Technical review was completed — no Technical Director present; DONE on the Game Director's explicit authority, flagged (consistent with PC-007/008/009/010)
- [x] Relevant documentation was updated (task file, CHANGELOG.md, ROADMAP.md)
