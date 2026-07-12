## Task ID
PC-013

## Title
Sprint 2.9 — Minimal Loot & Equipment Foundation: the Detonator Module (Cluster C open)

## Status
DONE (2026-07-12) — Game Director approved after a final hands-on playtest: the readability fix resolves the multi-kill problem — the module is immediately recognizable even in large multi-kills (beacon + height offset + visual distinction), with no unnecessary systems added. The mechanical interaction was approved in the prior review. All functional + regression checks pass with zero console errors. Committed as a single change (`feat: add first secure-or-lose weapon module`); pushed by Yoav via GitHub Desktop. The Cluster B Decision Gate remains **intentionally open** — this sprint is the experiment that informs it — and no gate verdict is recorded until Yoav calls it.

## Review Round 1 — Readability Fix (2026-07-12)

**Reported:** during near-simultaneous multi-kills, the fourth-kill module drop was hard to identify — cyan didn't stand out, its origin location was unclear, and it could be buried among currency coins / corpses / effects.

**Investigation (before changing anything), all five checks:**
1. **Spawns exactly once?** Yes. `droppedThisRun` + `killsThisRun >= dropOnKill` fire once; `EnemyKilled` fires once per genuine death. Deterministic same-frame 6-kill test: modules stayed at 1.
2. **`LastKillPosition` reliably the 4th enemy?** Yes at the moment of the drop — module spawned at the 4th enemy's captured position `(7, y, 7)`.
3. **Same-frame deaths overwriting the stored position before spawn?** **No — no race.** The dropper reads `LastKillPosition` and `Instantiate`s **synchronously inside the 4th kill's `EnemyKilled` invocation**, before the 5th/6th deaths run. Proof: after all 6 same-frame kills, `spawner.LastKillPosition` had advanced to the 6th enemy `(11,1,11)` but the module **stayed** at the 4th's `(7,…,7)`.
4. **Spawning inside another pickup/corpse?** **Yes — this was the root cause.** The module dropped at the *same world position* as the 4th enemy's `CurrencyPickup` (both at `enemyPos + 0.5`) and both were 0.5-scale spheres → **nearest coin-to-module distance = 0.00**, so the cyan module sat perfectly inside a gold coin amid the pile.
5. **Cyan material/scale readable?** No — non-emissive, identical shape/size to the coin.

**Root cause:** co-location + identical shape/scale + a similar-family matte tint. Not a spawn/position/count bug.

**Smallest fix (mechanics unchanged; self-contained in the pickup + its dropHeight; no loot-label/rarity-beam/minimap/generic-VFX system):**
- **Vertical separation:** `WeaponModuleDropper.dropHeight` `0.5 → 1.2` so the module **floats above** the ground-level coin (now 0.70 above it, was 0.00), never buried. Still within the pickup's 1.75 attract radius, so walking under it collects (verified).
- **Unmistakable visual, in `WeaponModulePickup`:** bright saturated **cyan** body via a shared runtime URP/Lit material (same asset-free approach `DetonationMark` uses), a small **floating beacon** above the body (the "which location" cue), and a gentle **spin + beacon pulse/bob** (motion draws the eye; applied to rotation + the beacon child only, never the root position, so it can't affect attract/collect).
- **Emission rejected:** real `_EMISSION` on a runtime URP/Lit material rendered **magenta/pink** in this pipeline (confirmed on-screen) and wouldn't glow without a Bloom post-process anyway; a bright cyan base reads as "emissive" and renders correctly.

**Re-verified (deterministic + visual):** same-frame 6-kill test → exactly one module at the 4th's `(7,2.20,7)`, unmoved by later deaths; code-path material bright cyan, emission off; beacon present; collection from directly underneath works (6 ticks); full end-to-end (collect → early-detonate a marked enemy, near 100→75 → extract banks, `SecuredThisRun`) passes; **zero console errors.** Game-view screenshots confirm the module (floating cyan orb + beacon) is clearly distinct from a cluster of gold coins.

**Files touched this round:** `WeaponModulePickup.cs` (visual), `WeaponModuleDropper.cs` (`dropHeight` default) + the scene `WeaponModuleDropper` instance value `1.2`. No mechanics, lifecycle, or forbidden files changed.

## Module Lifecycle (Game-Director clarification, approved 2026-07-12 — authoritative)
- The Detonator Module **activates immediately** when picked up.
- Before extraction it is **Unsecured** (at risk).
- If the player **dies before extracting**, the module is **lost** and its effect disappears.
- If the player **extracts successfully**, the module becomes **Banked/Owned**.
- Once Banked/Owned, it remains **active across later Operations during the same Play Mode session** and is **no longer at risk**.
- If the player **already owns** the Detonator Module, **no additional module drops** in later Operations.
- Out of scope (do not add): inventory, module swapping, multiple modules, equipment UI, persistence across game restarts.
- Experiment shape held strictly to: **find it → use it → risk it → lose it or secure it.**

## Implementation constraints on the DetonationMark edit (Game-Director, approved 2026-07-12)
- Reuse the existing detonation implementation; early and death detonation share the **same explosion path**.
- The mark must **never detonate twice** (one-shot guard; whichever path fires first wins, the other no-ops).
- **Safely remove the `Health.Died` subscription** after an early detonation.
- Do **not** change Detonation Field's ownership of marking or its existing behavior beyond exposing the entry point.
- Do **not** introduce a generic trigger/effect framework.

## Priority
High

## Owner
Claude (implementation) — design decisions owned by Yoav (Game Director)

## Reviewer
Yoav (Game Director)

## Dependencies
- PC-011 (Sprint 2.7, DONE) — the module reuses the secure/lose pattern (`CurrencyWallet` + physical-pickup + `OperationStarted/Succeeded/Failed` reactor).
- PC-009 (Sprint 2.5, DONE) — `DetonationMark`, `DetonationField`, and the Volatile Fracture interaction the module's verb plugs into.
- Cluster A (2.3–2.5, DONE) — the module must interact with existing build content (Build Content Alignment gate, [REVIEW_CHECKLIST.md](../../Documentation/AI/REVIEW_CHECKLIST.md)).

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster C → Sprint 2.9
- [DECISIONS.md](../../DECISIONS.md) DEC-018 (loot is physical), DEC-019 (unsecured until banked; loot lost on failure), DEC-013 (build layers), DEC-021 (finite build budget), DEC-025 (combat feel / kinesthetic legibility)

## Objective
Prove DEC-018 end-to-end for one build layer: a **Weapon Module** that is acquired through **loot** (physically dropped in a run), **carried** unsecured, **secured** on extraction or **lost** on death, and whose effect **interacts with existing build content** rather than adding a flat stat. This is the smallest meaningful build-changing item — not a prototype of the final equipment system.

## Player emotion
**Fear of loss** — "I picked up something that makes me stronger *right now*, and I don't want to die before I bank it."

## Experiment framing (why this sprint carries the Cluster B gate)
Cluster B's loop is mechanically complete but whether Mission Risk *feels* like anything is deliberately untested — currency alone is too cheap a stake. This sprint supplies the smallest stake that could work. Validation signals, **observed in playtest, not asserted**:
- (a) behavior changes while carrying the equipped module,
- (b) genuine tension on the way to extraction,
- (c) a real emotional response to losing it on death. **Rushing to secure it counts as validation.**

**Validation signal is NOT "does the player stay longer."** Fair-test caveats, both honored by the decisions below: the item must be genuinely build-changing (not a stat stick — DEC-024/soft-interaction), and there must be a real "carry it out alive" window (drop happens mid-fight, before extraction opens). Escalation (rising danger/reward for lingering) stays deferred and only reopens if a build-changing item ALONE fails to create fear-of-loss.

## Kickoff decisions (all DECIDED by the Game Director, 2026-07-12)
1. **Slot identity → Weapon layer.** The item is the first acquirable **Weapon Module** (DEC-013 Weapon layer). No eighth build layer, no temporary "Gear" concept. Not a prototype of the final equipment system — the smallest droppable/carryable/securable build-changing item.
2. **Interaction → the Detonator Module.** While equipped, a weapon hit on a **marked** enemy detonates the mark **early**, instead of waiting for the enemy to die. The weapon gains a new verb ("I am the trigger"); **Detonation Field remains the owner of marking.** Keep it simple and legible; balance via tuning, not extra mechanics.
3. **Acquisition → deterministic mid-run drop.** A fixed kill index (default the **4th** kill, of the `killQuota` 8) drops exactly one module. No RNG. Because 4 < the 8-kill extraction quota, every run that reaches extraction necessarily passed the drop and carried it — guaranteeing the carry window every playtest run.
4. **Equip & loss → immediate activation.** Auto-equip on pickup; the effect works for the **rest of that run**. **Death loses it; extraction banks it** permanently (session/in-memory). Once banked, the capability is active from the start of subsequent runs and is no longer at risk (DEC-019: banked = safe).

## Mechanic specification (exact)
**The Detonator verb.** While the module `IsActive`, every weapon hit on an enemy that currently carries a `DetonationMark` triggers that mark's detonation immediately (the same AoE burst that would happen on the enemy's death), then consumes the mark. The weapon's own hit damage is unchanged; the carrier enemy is not killed by the trigger (the blast excludes the carrier, exactly as the existing death-detonation does — the weapon shot is what damages the carrier). Detonation reuses the mark's already-stored radius/damage (whatever primed it — a Detonation Field cast or a Volatile Fracture shard), so no new tuning and the DF-rank / VF scaling carries through unchanged.

**Interaction proof (Build Content Alignment).** The module creates a genuinely new interaction across three existing systems: Weapon (the trigger) ↔ Detonation Field (marks + blast data) ↔ Volatile Fracture (shard-applied marks become shootable bombs). It does not add a flat number and does not replace any skill's identity. Existing content stays viable: without the module the weapon and skills behave exactly as before; with it, marked-enemy play gains a new option.

**Acquisition & carry.** On the run's Nth genuine kill (default 4), one physical `WeaponModulePickup` drops at that enemy's position. The player walks over it (same attract/collect feel as currency) → the module equips → `IsActive` true for the rest of the run. If the module is already owned (banked) this run, no drop occurs.

**Secure / lose (mirrors `CurrencyWallet` exactly, reacting to the existing Operation events — the 2.6 lifecycle is NOT modified):**
- `OperationStarted` → clear any uncollected module pickups; if not Banked, `Equipped = false`; reset the dropper's per-run counter and one-shot flag.
- `OperationSucceeded` → if `Equipped`, `Banked = true` (owned permanently, session); capture a read-only `SecuredThisRun`/status for the HUD.
- `OperationFailed` → `Equipped = false` (lost); `Banked` untouched.
- `IsActive => Equipped || Banked`.

**Legibility (HUD).** `OperationHud` shows a module line: not-owned (nothing/"—"), `EQUIPPED (at risk)` while carried unsecured, `SECURED` once banked; and the terminal results summary states whether the module was **secured this run** / **lost this run** (alongside the existing currency + retained-progression lines). Pickup is tinted a distinct non-gold color (cyan/"power") so it never reads as currency.

## Architecture (approved constraints)
**New scripts (`PointClear.Operations`, `Assets/Systems/Operations/` — grouped with the other secure/lose reactors, no new "equipment system" namespace):**
- `WeaponModule.cs` (Player component) — owns `Equipped`/`Banked`/`IsActive` + read-only `SecuredThisRun`/`LostThisRun` for the HUD; reacts to the Operation events (mirrors `CurrencyWallet`); subscribes to the weapon's new `EnemyHit` event and, when `IsActive`, triggers the marked enemy's early detonation. Sole owner of module ownership state. `Equip()` called by the pickup.
- `WeaponModulePickup.cs` — physical trigger drop mirroring `CurrencyPickup` (pickup radius, attract, collect-once, self-destroy, static `ClearAll()`); on collect → `WeaponModule.Equip()`; cyan tint.
- `WeaponModuleDropper.cs` (scene component) — subscribes to `EnemySpawner.EnemyKilled`, counts kills this run, and on the Nth (serialized, default 4) instantiates exactly one `WeaponModulePickup` at the last kill position; guarded to drop at most once per run and never when the module is already owned; resets on `OperationStarted`.

**Minimal additive edits to existing files (each necessary, each flagged):**
- `HitscanWeapon.cs` — add `public event Action<Health> EnemyHit;`, invoked after the existing `TakeDamage`. No behavior change when unsubscribed. *(This is the Weapon-layer sprint; the weapon is legitimately in scope.)*
- `DetonationMark.cs` — add `public void DetonateEarly()` that reuses the existing private `Explode()` then removes the mark, plus a `detonated` one-shot guard so early-trigger and death-detonation are mutually idempotent (whichever fires first wins; the other no-ops). **⚠ This is a `Skills/` file — the one edit that crosses the forbidden-file line the last sprints held.** It is the minimum needed to make Decision 2 real: nothing but the mark can trigger the mark. Detonation Field's marking behavior is unchanged; only a new trigger entry point is added.
- `EnemySpawner.cs` — add `public Vector3 LastKillPosition { get; private set; }`, set from the dying enemy inside the existing `HandleTrackedEnemyDied` immediately before `EnemyKilled` fires, so the dropper drops at the true Nth-kill position without changing the parameterless `EnemyKilled` signature (which `OperationController`, a forbidden file, consumes). Additive only. *(EnemySpawner is prototype infra, modified in 2.6, not on the forbidden list.)*
- `OperationHud.cs` — add the module status line + secured/lost-this-run line to the terminal summary. (HUD is edited every sprint.)

**Prefab/scene wiring:** `Player.prefab` gains `WeaponModule`; a scene object (the Operation object) gains `WeaponModuleDropper`; new `WeaponModulePickup` prefab. No changes to `OperationController`/`ExtractionPoint`/`OperationState`, `Health`, `EnemyAI`, `EnemyXPReward`, `PlayerXP/Level/Stats`, `SkillPoints`, `SkillProgression`, `DetonationField`, `FractureBolt`, `CurrencyWallet`, or `CurrencyPickup`.

## Scope — excluded (do not build)
Full itemization, multiple slots, rarity tiers, affixes/RNG stats, drop tables, crafting, vendor/economy/spending, real inventory UI, disk/cross-session persistence, respec, a second module, a generic equipment/loot framework, new enemy archetypes (Sprint 2.11), escalation/attrition, any change to the Operation lifecycle.

## Files Allowed to Edit
- **New:** `Assets/Systems/Operations/WeaponModule.cs`, `WeaponModulePickup.cs`, `WeaponModuleDropper.cs`; `Assets/Prefabs/**/WeaponModulePickup.prefab` (+ .meta).
- **Modify (additive, minimal):** `Assets/Systems/Weapons/HitscanWeapon.cs`; `Assets/Systems/Skills/DetonationMark.cs` (⚠ flagged); `Assets/Systems/Enemies/EnemySpawner.cs`; `Assets/Systems/Utilities/OperationHud.cs`; `Assets/Prefabs/**/Player.prefab`; the scene object holding `WeaponModuleDropper`.
- **Docs:** `CHANGELOG.md`, `ROADMAP.md` (Sprint 2.9 status only — **not** the Cluster B gate section), `Tasks/**/PC-013_*.md`.

## Files Forbidden to Edit
- `OperationController.cs`, `ExtractionPoint.cs`, `OperationState.cs`, `CurrencyWallet.cs`, `CurrencyPickup.cs`, `EnemyCurrencyDrop.cs`
- `Health.cs`, `EnemyAI.cs`, `EnemyXPReward.cs`, `PlayerXP.cs`, `PlayerLevel.cs`, `PlayerStats.cs`, `PlayerController.cs`, `PlayerRespawn.cs`
- `SkillPoints.cs`, `SkillProgression.cs`, `SkillDefinition.cs`, `DetonationField.cs`, `FractureBolt.cs`, `FractureBoltProjectile.cs`, `PassiveEffects.cs`, `MarkPayload.cs` (all `Skills/` **except** the single flagged `DetonationMark.DetonateEarly()` addition)
- The Cluster B Decision Gate sections of `ROADMAP.md`/`CHANGELOG.md` (gate stays open)
- All documentation not listed under Allowed

## Acceptance Criteria
- [x] Exactly one `WeaponModulePickup` drops on the run's Nth kill (default 4), at that enemy's position; none before, none after, at most one per run. **Verified:** no pickup at kills 1–3; exactly one at kill 4 at `(4.00, 0.50, 4.00)`; none through kill 8.
- [x] Walking over the pickup equips the module; `IsActive` becomes true mid-run; the pickup self-destroys and cannot be collected twice. **Verified:** `Collect` → `Equipped=True`, `IsActive=True`; guarded single-collection path.
- [x] While `IsActive`, a weapon hit on a marked enemy detonates the mark early (AoE burst + neighbor damage), consumes the mark, and does not kill the carrier by itself; an unmarked enemy hit behaves normally. **Verified:** near neighbor 100→75, far neighbor (outside radius) 100, carrier alive, mark consumed (proven via early-then-death no-re-detonation); unmarked hit is a no-op.
- [x] Early-detonation reuses the mark's stored radius/damage. **Verified:** radius 3 / damage 25 from `Apply` reproduced by the early trigger (mark carries whatever primed it — a DF cast or a Volatile-Fracture shard, both call `DetonationMark.Apply`). *(VF/DF-rank scaling flows through unchanged as the mark stores the applied values; hands-on Q/E + module combo reserved for Yoav.)*
- [x] Death during the run loses the module; extraction banks it (`Banked` true, `IsActive` stays true into subsequent runs); banked module no longer drops and is no longer at risk (DEC-019). **Verified:** unbanked death → `IsActive=False, LostThisRun=True`; success → `Banked=True, SecuredThisRun=True`; next run banked stays active, no re-drop, survives a death.
- [x] Without the module equipped/banked, weapon + skills behave exactly as before. **Verified:** inactive module ignores `EnemyHit`; weapon damage stat unchanged (10); effect fully gated by `IsActive`.
- [x] HUD shows EQUIPPED (at risk) vs SECURED, and the terminal summary states secured-this-run / lost-this-run. **Implemented in `OperationHud`; on-screen legibility reserved for Yoav's hands-on review.**
- [x] `OperationController`/lifecycle, currency scripts, and all forbidden files unchanged (git-confirmed); DEC-016 progression (Level/XP/Skill Points) intact across runs. **Verified:** Level/XP/SkillPoints/Banked unchanged across StartOperation/ReturnToReady; forbidden files not in the diff.
- [x] Death-detonation and early-detonation are mutually idempotent. **Verified:** death-then-early (neighbor 75, no double) and early-then-death (neighbor 75, no re-detonation) both single-detonate.
- [x] Zero console errors. **Verified** across all Play-Mode runs and on Play-Mode exit.

## Test Procedure (deterministic Play Mode)
1. Start run → kill enemies; assert the pickup drops exactly on kill #N at the kill position, and not on kills 1..N-1 or N+1.
2. Collect it → assert `Equipped`/`IsActive` true, pickup destroyed, not re-collectable.
3. Shoot a marked (DF-cast) enemy non-lethally → assert early detonation (neighbors damaged, mark consumed, carrier alive); shoot an unmarked enemy → assert no detonation.
4. Volatile Fracture path: mark via a shard → shoot → assert early detonation with DF-rank-scaled radius.
5. Lethal shot on a marked enemy → assert exactly one detonation (no double).
6. Die with the module equipped → assert lost (next run `IsActive` false, no auto-drop of a banked module), Banked unchanged, Level/XP/SP retained.
7. Extract with the module equipped → assert Banked true, `IsActive` true at the next run's start, no new drop, no longer at risk on a subsequent death.
8. Regression with no module: weapon, Q/E skills, Volatile Fracture, XP/level, currency secure/lose, extraction all unchanged.
9. Console clean; forbidden/lifecycle files unmodified (git).

## Implementation Report

Implemented by Claude 2026-07-12, branch `feature/sprint-1-first-playable`. **Uncommitted (in REVIEW).**

**New scripts (`PointClear.Operations`):**
- `WeaponModule.cs` (on Player) — `Equipped`/`Banked`/`IsActive` + read-only `SecuredThisRun`/`LostThisRun`; mirrors `CurrencyWallet`'s reactor over the existing `OperationStarted/Succeeded/Failed` events (2.6 lifecycle untouched); subscribes to `HitscanWeapon.EnemyHit` and, when `IsActive`, calls `DetonationMark.DetonateEarly()` on a hit marked enemy. `Equip()` is the collect entry.
- `WeaponModulePickup.cs` — physical trigger drop mirroring `CurrencyPickup` (attract radius 1.75, collect-once, self-destroy, static `ClearAll()`); cyan tint; on collect → `WeaponModule.Equip()`.
- `WeaponModuleDropper.cs` (on the `Operation` object) — counts `EnemySpawner.EnemyKilled`, drops one pickup at `EnemySpawner.LastKillPosition` on kill `dropOnKill` (=4), at most once per run, never when already owned; resets on `OperationStarted`.

**Additive edits to existing files (diffs minimal, existing behavior preserved):**
- `HitscanWeapon.cs` — `+ event Action<Health> EnemyHit`, invoked after the existing `TakeDamage`. No behavior change unsubscribed.
- `DetonationMark.cs` — `+ bool detonated` one-shot guard; `HandleDeath` now routes through a shared private `Detonate()` (same `Explode()`); `+ public DetonateEarly()` = `Detonate()` then `Unsubscribe()` + `Destroy(this)`. Death and early paths share one guarded explosion path. Detonation Field's marking is unchanged.
- `EnemySpawner.cs` — `+ Vector3 LastKillPosition { get; private set; }` set in the existing `HandleTrackedEnemyDied` before `EnemyKilled` fires. Parameterless `EnemyKilled` signature (consumed by `OperationController`) unchanged.
- `OperationHud.cs` — module status line (`—` / `EQUIPPED (at risk)` / `SECURED`) in Ready/InProgress; `SECURED this run` / `LOST this run` in the terminal summary. Panel height 260→310.

**Wiring:** `Player.prefab` gained `WeaponModule`; `Operation` object gained `WeaponModuleDropper` with `modulePickupPrefab` → the new `WeaponModulePickup.prefab` (cyan sphere, trigger collider); scene saved. Auto-resolve (`FindAnyObjectByType`/`GetComponent`) covers the remaining refs at runtime.

**Confirmed unmodified (git diff):** `OperationController`, `ExtractionPoint`, `OperationState`, `CurrencyWallet`, `CurrencyPickup`, `EnemyCurrencyDrop`, `Health`, `EnemyAI`, `EnemyXPReward`, `PlayerXP/Level/Stats`, `PlayerController`, `PlayerRespawn`, `SkillPoints`, `SkillProgression`, `DetonationField`, `FractureBolt`, `FractureBoltProjectile`, `PassiveEffects`.

**Testing:** deterministic Play-Mode harness (`execute_code`) — drop-on-4th-at-position, no-drop 1–3, no-double-drop through 8, collect→equip, success-banks/failure-loses, banked-persists/no-redrop/not-at-risk, early detonation (radius-correct, carrier survives, consumed), unmarked no-op, inactive no-op, both detonation orderings idempotent, DEC-016 progression intact — all pass, zero console errors. Values-and-events verified programmatically; the **hands-on layer is reserved for Yoav** (see Review Notes).

## Known limitations / notes
- **Deferred-`Destroy` visual cleanup not frame-verified in the harness:** the Editor did not advance Play-Mode frames between scripted calls (`Time.frameCount` static), so `Destroy(this)`/`Destroy(gameObject)` (mark component, collected pickup) could not be observed flushing. Consumption is proven *synchronously* instead (early-then-death produces no second detonation → mark unsubscribed + guarded). In normal play (frames advancing) the mark orb and collected pickup disappear next frame, exactly as `CurrencyPickup` already does.
- **`FindFirstObjectByType` CS0618 deprecation warnings:** the new files follow the codebase's existing convention; the same warnings already blanket untouched files (`CurrencyWallet`, `OperationController`, `ExtractionPoint`). Zero errors. Project-wide migration to `FindAnyObjectByType` is out of scope (would touch forbidden files).
- **Banked = session/in-memory only** (no disk persistence) — matches `CurrencyWallet.Banked` and the approved scope.

## Review Notes — hands-on steps for Yoav
1. Enter Play Mode, Start Operation, and fight. On the **4th kill** a **bright cyan module floats above where the enemy died**, with a small pulsing **beacon marker above it** and a gentle spin — it should be unmistakable next to the gold currency coins, even in a multi-kill pile. Walk under/over it to collect (it's pulled in within ~1.75 m). HUD should flip to **Weapon Module: EQUIPPED (at risk)**. *Specifically re-check the multi-kill case that failed review: kill several enemies at once so the 4th lands in a cluster of coins, and confirm the module still reads clearly.*
2. With it equipped, cast **Detonation Field (E)** (or use a Volatile-Fracture shard via **Q**) to mark enemies, then **shoot a marked enemy** — it should detonate immediately (the burst) instead of waiting for it to die. Confirm this changes how you fight marked packs.
3. **Die** before extracting → results show **Weapon Module: LOST this run**, and the effect is gone on the next run.
4. Do a run where you pick it up and **extract** → results show **SECURED this run**; on later runs the HUD shows **SECURED**, the effect is active from the start, no new module drops, and dying no longer loses it.
5. Confirm nothing regressed before you ever pick it up (weapon, Q/E, Volatile Fracture, XP/level, currency secure/lose, extraction) and that the HUD reads clearly with no overlap.

## Game Director Approval
- [x] Approved by Yoav
- Date: 2026-07-12
- Notes: Approved after two hands-on playtests. Round 1 flagged a multi-kill readability problem; root-caused (co-location with the currency coin, not a spawn/position bug) and fixed with a height offset + floating beacon + cyan distinction, no new systems. Final playtest: the module is immediately recognizable even in large multi-kills. Mechanical interaction (weapon triggers marks early; secure-or-lose) approved in the prior round. DONE on the Game Director's explicit authority; technical-review box left unchecked (no Technical Director present), consistent with PC-007..012. Cluster B Decision Gate stays open — this sprint is its experiment; no gate verdict recorded yet.

## Definition of Done Checklist
- [x] Acceptance criteria pass (deterministic Play-Mode harness + hands-on playtest; readability re-verified via Game-view screenshots)
- [x] Unity has no new compiler errors (clean compile; zero console errors across all runs)
- [x] Required testing was completed (functional + regression + multi-kill readability + end-to-end)
- [x] Yoav approved the result (2026-07-12)
- [ ] Technical review was completed — no Technical Director present; DONE on the Game Director's explicit authority, flagged (consistent with PC-007..012)
- [x] Relevant documentation was updated (task file, CHANGELOG.md, ROADMAP.md)
