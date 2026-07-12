## Task ID
PC-014

## Title
Sprint 2.11 — First Meaningful Enemy Variety: the Stalker

## Status
SUPERSEDED (2026-07-12) — The Stalker was implemented and verified, but its hands-on playtest revealed the distance-keeping behavior reads as a false "ranged enemy" promise with no felt purpose ("why is it running away?"). Per the Game Director, the enemy was **replaced** rather than re-theorized. Sprint 2.11 pivoted to rapid-prototyping three enemies (Charger/Empowerer/Surrounder) and then to a **kill-driven composition ramp** that introduces enemies progressively over an operation (see CHANGELOG 2026-07-12). This task is archived as exploration history; `StalkerAI.cs`/`Stalker.prefab` remain in the repo but are no longer spawned. Not approved, not DONE.

## Priority
High

## Owner
Claude (implementation) — design decisions owned by Yoav (Game Director)

## Reviewer
Yoav (Game Director)

## Dependencies
- Cluster A build content (hitscan weapon, Fracture Bolt, Detonation Field, Volatile Fracture, Detonator Module) — the Stalker must be answerable by these *existing* tools; no new player mechanic.
- Sprint 2.6 Operation (kill quota → extraction) — Stalkers count toward the quota; gating closure is the incompleteness lever.
- DEC-030 (Behavioral Divergence Check) — the Stalker is that experiment's measuring instrument; it must pose a genuinely different world-question with *multiple* soft answers.

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster C → Sprint 2.11 (now precedes 2.10)
- [DECISIONS.md](../../DECISIONS.md) DEC-024 (world asks questions; behavior not stats; soft counters; readability governor), DEC-030 (behavioral divergence)
- Non-canonical design lead: [Documentation/Vision/DESIGN_ARCHITECTURE_REVIEW_2026-07-12.md](../../Documentation/Vision/DESIGN_ARCHITECTURE_REVIEW_2026-07-12.md) ("reveal a boundary through incompleteness, not lethality")

## Objective
Introduce the first second enemy archetype — the **Stalker** — whose purpose is to ask a genuinely different *behavioral* question than the chaser and, by doing so, create the first **contrast** in which a build reads as an *identity* (a way of engaging) rather than a power level. Build the smallest enemy that produces the player experience below — nothing more.

## Player experience (design starts HERE, not from AI)
1. **What the player naturally tries:** what the game trained them to do — hold position, let it come into the kill zone, drop Detonation Field, harvest it. *"Enemies come to me and die in my space."*
2. **Why it fails:** it doesn't come. It stays at range, outside the self-centered Detonation Field, so the field cashes nothing and waiting resolves nothing. Because it counts toward the extraction quota, **the run won't close while it's alive.** The failure is *incompleteness* ("why won't this finish?"), never death.
3. **The realization:** *"It's not coming to me — I have to deal with it where it is,"* then *"…and I have things that reach."* The player goes looking and a dormant tool surfaces.
4. **The new behavior:** the player leaves the fortress and projects force **outward** — and *which* way depends on their build/temperament (reach with Fracture Bolt · mark-at-range with Volatile Fracture then trigger · precision-snipe · or commit and close). That fork is the first visible divergence between two players' builds — the DEC-030 signal.

## Player emotion
Productive friction → discovery. Mild "why won't this finish?" resolving into "oh — I go to it / I have tools that reach." **Curiosity, never fear. Incompleteness, never death.**

## Governing design rule (Game Director — explicit, authoritative)
**The Stalker may deny PASSIVE closure. It must never deny COMMITTED closure.**
- If the player stands in the center and waits, they must *fail* to finish the encounter.
- The moment the player consciously decides "I'm dealing with this enemy now," the encounter must resolve **reliably and without frustration.** The player must never feel they are chasing an annoying AI indefinitely.
This is a player-experience rule, not an AI rule, and it is a hard acceptance criterion (below).

## The Stalker — behavior (derived from the experience)
- **Keeps a preferred distance band** from the player (retreat if the player is inside it, approach if outside it, otherwise hold/drift). This alone produces the core failure.
- **Retreats at a speed ≤ the player's move speed**, so a committed player can *always* run it down — it denies *passive* closure, never *committed* closure.
- **Spawns as a small spread group** (reusing the existing separation so they don't clump) and **counts toward the extraction quota** — lazy single-target sniping is inefficient against a spread, and the run cannot close until they're dealt with.
- **Harmless / near-harmless this sprint.** Teeth are *incompleteness via the quota gate*, not damage. No enemy ranged attack in 2.11.
- **Distinct look** (color/silhouette) for instant readability.
- **No cover / line-of-sight-breaking this sprint** — cut as a sharpener, saved for a future iteration (see Scope excluded).

## Interactions with existing systems (all reused, nothing rebuilt)
- **Detonation Field** — its self-centered nature is exposed as a *boundary*: weak against spread distance-keepers unless the player closes or herds them.
- **Fracture Bolt** — reach + pierce/split finally has a natural, outward target.
- **Volatile Fracture** — the dormant one: mark a Stalker at range via shards (the field can't reach it), then detonate.
- **Detonator Module** — trigger the range-placed marks on the player's timing.
- **Hitscan** — still finishes it; rewards deliberate aim/repositioning.
- **Operation / quota / extraction** — Stalkers count toward the quota (via the existing `EnemySpawner.EnemyKilled` → `OperationController` path; the lifecycle is NOT modified).
- **XP / currency** — unchanged; reuse `EnemyXPReward` and `EnemyCurrencyDrop` as-is.

## Architecture / implementation plan
- **New `StalkerAI` (`PointClear.Enemies`)** — banded movement (retreat / approach / hold), retreat speed clamped ≤ player, lightweight separation for spread. **Separate component so the chaser (`EnemyAI`) is untouched.** Kinematic Rigidbody `MovePosition`, mirroring the chaser's movement discipline.
- **New `Stalker` prefab** — Capsule + kinematic Rigidbody + `Health` + `StalkerAI` + `EnemyXPReward` + `EnemyCurrencyDrop` + a distinct material tint.
- **`EnemySpawner` (additive)** — spawn a small configurable group of Stalkers when a run begins and **track them like chasers** so their deaths raise the existing `EnemyKilled` (counting toward the quota **without touching `OperationController`**). Chaser ramp unchanged.
- Distinct material via the same asset-free runtime-tint approach used elsewhere (no new art).

## Files Allowed to Edit
- **New:** `Assets/Systems/Enemies/StalkerAI.cs`; `Assets/Prefabs/Enemies/Stalker.prefab` (+ .meta).
- **Modify (additive):** `Assets/Systems/Enemies/EnemySpawner.cs` (spawn + track Stalkers); `Assets/Scenes/Prototype/PrototypeScene.unity` (assign the Stalker prefab + count on the spawner).
- **Docs:** `CHANGELOG.md`, `ROADMAP.md` (Sprint 2.11 status only), `Tasks/**/PC-014_*.md`.

## Files Forbidden to Edit
- `EnemyAI.cs` (chaser stays untouched), `Health.cs`, `OperationController.cs`, `ExtractionPoint.cs`, `OperationState.cs`
- `CurrencyWallet.cs`, `CurrencyPickup.cs`, `EnemyCurrencyDrop.cs`, `EnemyXPReward.cs`, `WeaponModule*.cs`
- `HitscanWeapon.cs`, all `Skills/`, `PlayerController.cs`, `PlayerRespawn.cs`, `PlayerStats.cs`, `PlayerXP.cs`, `PlayerLevel.cs`, `SkillPoints.cs`, `SkillProgression.cs`
- The Cluster B gate sections of ROADMAP/CHANGELOG; all canonical philosophy docs

## Acceptance Criteria
- [~] **Readability:** distinct violet tint applied at runtime + visibly keeps its distance (band verified). Runtime tint + distance-band confirmed programmatically; the *at-a-glance* read is reserved for hands-on.
- [x] **Passive closure denied:** **verified** — Stalkers hold a distance band (inside→retreat 4.0→6.6; in-band→hold 8.0→8.2; outside→approach 13.0→9.5), all settling into 6.5–9.5 and **never entering the center**; `innerBand 6.5 > Detonation Field markRadius 5`, so a waiting/field-harvesting player cannot finish them and the quota can't close while they live.
- [x] **COMMITTED closure guaranteed (GD rule — hard):** **verified both paths.** By *reaching*: a hit kills a Stalker (no dodge), counts toward the quota, drops its reward. By *closing*: a pursuer faster than the Stalker collapses the gap (8.0→0.3) — it cannot escape a committed chase — and `stalkerSpeed 4.5 < playerSpeed 6` guarantees the real player always catches it; the kinematic sweep-stop corners it against geometry. No infinite chase.
- [~] **Reveals an outward answer:** structurally present (Stalkers sit outside the field at range; Fracture Bolt/Volatile Fracture/repositioning reach them) — the *felt* discovery is reserved for hands-on.
- [~] **Detonation Field boundary felt:** structurally present (band 6.5–9.5 > markRadius 5, so the field can't reach them) — *felt* confirmation reserved for hands-on.
- [~] **Multiple soft answers:** by construction (reach / mark+trigger / snipe / close-and-brawl all work; nothing is immune; no hard counter) — the *divergence shakedown* is reserved for hands-on.
- [x] **Low lethality:** **verified** — the Stalker has no ranged attack this sprint; its teeth are the quota gate (incompleteness), not damage.
- [x] **Quota + rewards:** **verified** — a Stalker death raises `EnemyKilled` (quota 0→1) and drops a currency pickup (reward reused, unchanged).
- [x] **No regressions:** **git-confirmed** — `EnemyAI`, `OperationController`/`ExtractionPoint`/`OperationState`, `Health`, all `Skills/`, weapon, XP/Level/SkillPoints unchanged; only `EnemySpawner` (additive) + scene wiring touched.
- [x] **Zero console errors** — verified across all Play-Mode runs and on Play-Mode exit.

## Scope — excluded (do not build)
Cover / line-of-sight-breaking; any enemy ranged attack or projectile; Elite/Mini-Boss tiers; a second general AI framework; a boss; art / animation / audio; a balance pass beyond making the feel right; the formal Behavioral Divergence Check *run* itself (that is Sprint 2.10 — 2.11 only delivers the instrument and an informal shakedown). *(If playtest shows Stalkers are ignorable or trivially sniped, the first tuning levers to revisit — in a later iteration, not this sprint — are cover and a light ranged poke.)*

## Test / Playtest plan
- **Programmatic (Play Mode):** Stalker maintains its distance band (retreats when player near, approaches when far); retreat speed ≤ player (a player moving straight at it always closes); Stalkers spread (don't clump); Stalker death raises `EnemyKilled` and increments the quota; XP + currency drop on death; chaser/lifecycle/skills unmodified (git). Zero console errors.
- **Committed-closure check (the GD rule):** simulate a player committing (move straight at a Stalker) → it is caught and killed in bounded time; and reaching (bolt/mark) finishes it — never an unbounded chase.
- **Hands-on (Yoav):** readability at a glance; fortress-wait fails to finish; committing (reach or close) resolves cleanly and un-frustratingly; a dormant tool reveals its purpose; the Detonation Field boundary is felt; different builds/players answer differently (divergence shakedown).

## Implementation Report
Implemented by Claude 2026-07-12, branch `feature/sprint-1-first-playable`. **Uncommitted (in REVIEW).**

**New:** `StalkerAI.cs` (`PointClear.Enemies`) — kinematic distance-band movement (retreat inside `innerBand` 6.5, approach outside `outerBand` 9.5, hold between), `moveSpeed` 4.5 (clamped below the player's 6 — the committed-closure guarantee), lightweight separation from other Stalkers (spread), the same kinematic sweep-stop the chaser uses (so it corners against geometry), a distinct violet body tint, and a brief white damage flash (the only juice — damage confirmation with no audio). Chaser `EnemyAI` is untouched. `Stalker.prefab` — cloned from `Enemy.prefab` (keeps `Health`/`EnemyXPReward`/`EnemyCurrencyDrop` wiring) with `EnemyAI` swapped for `StalkerAI`.

**Modified (additive):** `EnemySpawner.cs` — spawns a small fixed Stalker group (`stalkerCount`, default 3) on `BeginSpawning`, tracks each so deaths raise the existing `EnemyKilled` (counting toward the quota **without touching `OperationController`**), and clears them on `StopSpawning`/next `BeginSpawning` (run-transition cleanup; `Destroy` doesn't fire `Health.Died`, so cleared Stalkers are never miscounted). `PrototypeScene` — assigned the Stalker prefab on the spawner.

**Confirmed unmodified (git):** `EnemyAI`, `OperationController`, `ExtractionPoint`, `OperationState`, `Health`, all `Skills/`, `HitscanWeapon`, `CurrencyWallet`/`WeaponModule`/pickups, XP/Level/Stats/SkillPoints.

**Testing (Play Mode, `execute_code` + editor frame-stepping):** spawn-3-at-distinct-points; distance-band retreat/hold/approach all settling into 6.5–9.5 and never < innerBand; `stalkerSpeed 4.5 < playerSpeed 6` and `innerBand 6.5 > markRadius 5`; shoot-to-kill counts toward the quota (0→1) and drops currency; committed pursuer collapses the gap (8.0→0.3); run-transition cleanup (0 Stalkers after ReturnToReady); zero console errors. A player-teleport harness artifact (a manually-driven dynamic player Rigidbody under-delivers speed while `PlayerController` co-drives) was identified and worked around with a faster-than-stalker charge — not a code issue.

## Known limitations / notes
- **Felt/subjective criteria reserved for hands-on** (readability at a glance, the fortress-fails→reach-out realization, the Detonation-Field-boundary feel, the divergence shakedown, "not frustrating") — deterministic checks cover the mechanics, not the emotion.
- **Stalkers may be ignorable if the player farms chasers to the quota.** Making them a *required* clear is deliberately out of MVP scope; if the playtest shows they're ignorable or trivially sniped, the first tuning levers (next iteration) are cover/LoS-breaking and a light ranged poke (both explicitly cut this sprint).
- Feel constants (band, retreat margin, group size) are playtest-tunable, not final.

## Review Notes — hands-on steps for Yoav
1. Start a run. A few **violet** enemies appear and **hang back at range** while the red chasers rush you — confirm they read instantly as a different kind of enemy.
2. Try to fight them the trained way: hold the middle, drop **Detonation Field (E)**, wait. Confirm it **doesn't finish them** (they stay outside the field) and the run won't close.
3. Now deal with one deliberately — **shoot it**, throw **Fracture Bolt (Q)**, mark it at range (Volatile Fracture) then trigger, or **walk it down**. Confirm any committed choice **resolves cleanly and never turns into an annoying endless chase** (the governing rule).
4. Notice whether a previously-pointless tool (reach / ranged marking / repositioning) suddenly has a purpose.
5. Confirm nothing regressed: chasers, Q/E skills, Volatile Fracture, the Detonator Module, XP/leveling, currency/extraction all behave as before.

## Game Director Approval
- [ ] Approved by Yoav
- Date:
- Notes:

## Definition of Done Checklist
- [ ] Acceptance criteria pass
- [ ] Unity has no new compiler errors
- [ ] Required testing was completed
- [ ] Yoav approved the result
- [ ] Technical review was completed
- [ ] Relevant documentation was updated
