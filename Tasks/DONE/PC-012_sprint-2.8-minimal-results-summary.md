## Task ID
PC-012

## Title
Sprint 2.8 — Minimal Results Summary (Cluster B close)

## Status
DONE (2026-07-12) — Game Director approved after hands-on Success/Failure review ("clear, readable, communicates exactly what it should"); the Secured/Banked/Retained distinction is easy to understand and the prototype HUD is sufficient. Two additional GD-requested regressions passed (rapid terminal race + repeat-run stability), zero console errors. No forbidden/lifecycle files touched.

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- PC-010 (Operation wrapper) — DONE (`OperationController`, `OperationSucceeded/Failed`, `OperationHud`)
- PC-011 (Unsecured rewards) — DONE (`CurrencyWallet` with `Unsecured`/`Banked` + event handlers)

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster B → Sprint 2.8
- [DECISIONS.md](../../DECISIONS.md) DEC-019 (mission risk — loot lost on failure, XP retained), DEC-016 (no mission resets the character)

## Objective
Close the Cluster B loop with the smallest honest **results summary**: at run end the player sees what they **secured**, **lost**, and **retained** — inside the existing `OperationHud` terminal view. No separate panel, no new UI system.

## Player emotion
**Clarity / consequence** — "I understand exactly what this run cost me or earned me." On Failure, the loss is legible but *not total* (character progress kept). Legibility of the outcome, not celebration.

## Approved presentation (exact)
Extend the existing `OperationHud` terminal-state section (keep the existing **Return to Ready** button). Show the currency `Unsecured/Banked` block only in Ready/InProgress; in terminal states show the summary instead.

**Success:**
```
SUCCESS — Extracted
Secured this run: +{LastSecured}
Banked total: {Banked}
Character Progress Retained
Level: {CurrentLevel}
Skill Points: {AvailableSkillPoints}
[Return to Ready]
```

**Failure:**
```
FAILED — You Died
Lost this run: {LastLost}
Banked total: {Banked}
Character Progress Retained
Level: {CurrentLevel}
Skill Points: {AvailableSkillPoints}
[Return to Ready]
```

Visual hierarchy must keep three concepts distinct: **Secured/Lost** = this run's result; **Banked** = cumulative currency already safe; **Character Progress Retained** = run-persistent progression not removed. Prototype OnGUI is sufficient; keep readable, no overlap with the Skills/Debug panels. Do **not** display "XP earned this run" (no XP delta is tracked).

## Architecture (approved constraints)
- **`CurrencyWallet`** gains read-only `LastSecured` and `LastLost`. Captured **inside the existing Success/Failure handlers before clearing `Unsecured`**; reset to 0 on `OperationStarted`. Success: `LastSecured = Unsecured`, `LastLost = 0`, bank once, clear Unsecured. Failure: `LastLost = Unsecured`, `LastSecured = 0`, clear Unsecured, Banked unchanged. CurrencyWallet remains the sole owner of currency result values. No run-history model, generic result struct, statistics system, or new event.
- **`OperationHud`** renders the summary in terminal states; reads `wallet.LastSecured/LastLost/Banked` and **read-only** `PlayerLevel.CurrentLevel` + `SkillPoints.Available`.
- No modification to `OperationController`/`ExtractionPoint`/`OperationState`, XP/Level/SkillPoints (read-only), Skills, Health, or EnemyAI.

## Scope — excluded (do not build)
Polished results screen, rewards animation/juice, inventory, equipment, loot rarity, spending, vendors, economy, save/load, run history, statistics tracking, multi-Operation expedition chaining, generic result-data structures, new events.

## Files Allowed to Edit
- **Modify:** `Assets/Systems/Operations/CurrencyWallet.cs`; `Assets/Systems/Utilities/OperationHud.cs`
- **Docs:** `CHANGELOG.md`, `ROADMAP.md`, `Tasks/**/PC-012_*.md`

## Files Forbidden to Edit
- `OperationController.cs`, `ExtractionPoint.cs`, `OperationState.cs`, `Health.cs`, `EnemyAI.cs`, `EnemyXPReward.cs`, `PlayerXP.cs`, `PlayerLevel.cs` (read-only), `PlayerStats.cs`, `SkillPoints.cs` (read-only), `SkillProgression.cs`, all other `Skills/`
- All documentation not listed above

## Risks & Edge Cases (verify)
- `Unsecured` is zeroed on the terminal event → `LastSecured`/`LastLost` MUST be captured inside the wallet handlers before zeroing.
- Success with zero Unsecured → `Secured this run: +0`. Failure with zero Unsecured → `Lost this run: 0`.
- A Success summary never shows a stale `LastLost`; a Failure summary never shows a stale `LastSecured` (each handler zeroes the other).
- Repeated Success/Failure cannot bank/lose/capture twice (terminal fires once — 2.6 guard).
- Return to Ready may leave captured values stored, but the terminal summary must no longer render (summary only in terminal states).
- Starting the next Operation resets both captured values to 0.
- HUD readable at the current Game view resolution; no overlap with Skills/Debug panels.

## Acceptance Criteria
- [x] Success shows `Secured this run = collected` (+15 in test), `Banked total = previous + secured` (0→15), retained Level/Skill Points; Return button present. Verified + screenshot.
- [x] Failure shows `Lost this run = unsecured` (5), `Banked total unchanged` (15), retained Level/Skill Points; Return button present. Verified + screenshot.
- [x] Secured-this-run and Banked-total are distinct, correctly labeled (`Secured this run: +N` vs `Banked total: M`). Verified.
- [x] Zero-Unsecured Success → `+0`. Verified (extract with 0 collected → LastSecured 0). (Zero-Unsecured Failure → `0` by symmetry — same handler path.)
- [x] Success never shows stale LastLost; Failure never shows stale LastSecured (each handler zeroes the other). Verified (post-success LastLost=0; post-failure LastSecured=0 with prior 15 cleared).
- [x] New Operation resets LastSecured/LastLost to 0; summary renders only in terminal states (currency block gated to Ready/InProgress). Verified.
- [x] `OperationController`/lifecycle unchanged; XP/Level/SkillPoints not modified (read-only, git-confirmed); no forbidden files touched.
- [x] Zero console errors (only benign URP render-info logs from screenshot capture); no out-of-scope systems.

**Pending Yoav's hands-on review** (needs live play): reading the Success and Failure summaries in normal play and confirming the three concepts (Secured/Lost this run · Banked total · Character Progress Retained) are legible and distinct. All values and rendering were verified programmatically + via game-view screenshots of both states.

## Test Procedure (deterministic Play Mode)
1. Start → collect N → extract (Success) → assert LastSecured=N, LastLost=0, Banked=prev+N, Unsecured=0.
2. New run → collect M → die (Failure) → assert LastLost=M, LastSecured=0, Banked unchanged, Unsecured=0.
3. New run → assert LastSecured/LastLost reset to 0.
4. Retained reads current Level/SkillPoints; assert they are unchanged by the summary (read-only; DEC-016).
5. Zero-value Success (extract with 0 collected) → LastSecured=0.
6. Repeated terminal events cannot double-bank/lose/capture.
7. Console clean; lifecycle scripts unmodified.

## Implementation Report

Implemented by Claude 2026-07-12, branch `feature/sprint-1-first-playable`. **Uncommitted (in REVIEW).** Two files changed; no new scripts/assets, no scene/prefab wiring (HUD auto-resolves references).

**`CurrencyWallet.cs`** — added read-only `LastSecured` / `LastLost` (wallet remains sole owner). Captured **inside the existing terminal handlers before clearing `Unsecured`**: `HandleOperationSucceeded` sets `LastSecured = Unsecured`, `LastLost = 0`, then banks once and clears Unsecured; `HandleOperationFailed` sets `LastLost = Unsecured`, `LastSecured = 0`, clears Unsecured (Banked untouched); `HandleOperationStarted` resets both to 0. No new event, no run-history/result struct.

**`OperationHud.cs`** — terminal states now render the summary via `DrawResultsSummary(bool success)`: outcome line (`SUCCESS — Extracted` / `FAILED — You Died`), `Secured this run: +{LastSecured}` or `Lost this run: {LastLost}`, `Banked total: {Banked}`, a bold **Character Progress Retained** header, `Level:` / `Skill Points:` (read-only `PlayerLevel.CurrentLevel` / `SkillPoints.Available`), and the existing `[Return to Ready]` button. The live `Unsecured/Banked` read-out now shows only in Ready/InProgress (the terminal summary carries the currency values). Panel height 220→260. Distinct labels keep Secured/Lost (this run) vs Banked (cumulative) vs Retained (persistent) separate.

**Confirmed unmodified (git):** `OperationController`, `ExtractionPoint`, `OperationState`, `Health`, `EnemyAI`, `EnemyXPReward`, `PlayerXP/Level/Stats`, `SkillPoints`, `SkillProgression`, all `Skills/`. `PlayerLevel`/`SkillPoints` are read-only.

**Testing:** deterministic Play-Mode harness (execute_code) — Success (LastSecured=collected, LastLost=0, Banked+=, Unsecured=0), Failure (LastLost=collected, LastSecured cleared, Banked unchanged), reset-on-start, zero-value success (+0), and no double-bank/capture (single-fire per 2.6 guard) all pass. Both summaries confirmed via game-view screenshots (correct layout, distinct concepts, no overlap with Skills/Debug panels). Zero code errors.

## Review Notes

**Hands-on approved (2026-07-12):** summary is clear and readable; the Secured-this-run / Banked-total / Character-Progress-Retained distinction is easy to understand; prototype HUD sufficient.

**Additional GD-requested regressions — both PASS, zero console errors:**
1. **Rapid terminal race** (Success & Failure same frame, both orderings): exactly one terminal wins (whichever fires first; the second is a no-op via the 2.6 `State != InProgress` guard). Order A (extract-then-death) → Succeeded, LastSecured=15, LastLost=0, Banked +15. Order B (death-then-extract) → Failed, LastLost=15, LastSecured=0, Banked unchanged. `LastSecured`/`LastLost` never both non-zero; currency never both banks and loses.
2. **Repeat-run stability** (Success, Success, Failure, Success): Banked accumulated only on success (15→30→45→**45**→60); `LastSecured`/`LastLost` reset to 0 at each run start and reflected only the current run (no leak, no stale UI); final total mathematically correct (60 = start 15 + three 15-currency successes; the failure's 15 excluded).

## Game Director Approval
- [x] Approved by Yoav
- Date: 2026-07-12
- Notes: Hands-on review passed; two additional regressions (terminal race, repeat-run stability) requested and passed with zero console errors. Approved to DONE and commit. Cluster B is now complete; a whole-cluster review precedes the Decision Gate (GD's direction).

## Definition of Done Checklist
- [x] Acceptance criteria pass (Play Mode verified — programmatic harness, screenshots, hands-on review, + 2 extra regressions)
- [x] Unity has no new compiler errors (clean compile, zero console errors)
- [x] Required testing was completed (deterministic harness + hands-on + race/stability regressions)
- [x] Yoav approved the result (2026-07-12)
- [ ] Technical review was completed — no Technical Director present; DONE on the Game Director's explicit authority, flagged (consistent with PC-007..011)
- [x] Relevant documentation was updated (task file, CHANGELOG.md, ROADMAP.md)
