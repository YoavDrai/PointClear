## Task ID
PC-009

## Title
Sprint 2.5 — Second Build Layer: Passives + Volatile Fracture (first cross-skill interaction)

## Status
DONE (2026-07-12) — Game Director approved after hands-on playtest. Implemented and Play-Mode-verified by Claude; all acceptance criteria pass, zero console errors on the correct prefab. A per-skill `StartingLevel` core change was required mid-sprint (discovered blocker, Game-Director-approved — see Files Allowed). Committed and pushed on approval; the Cluster A Decision Gate is evaluated separately (see Roadmap/Changelog).

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- Task PC-007 (Sprint 2.3 Active Skills) — DONE (`FractureBolt`, `FractureBoltProjectile`, `DetonationField`, `DetonationMark`)
- Task PC-008 (Sprint 2.4 Skill Points & Allocation) — DONE (`SkillType`, `SkillDefinition`, `SkillPoints`, `SkillProgression`)
- Sprint 2.5 Design Review + final content spec — approved 2026-07-11

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster A → Sprint 2.5
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Build Philosophy point 13 (Expanding the Interaction Space)
- [DECISIONS.md](../../DECISIONS.md) DEC-013 (layered build system — Passives layer), DEC-020
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../../Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)

## Objective

Introduce the second build layer (Passives) reusing the Sprint 2.4 skill architecture, and deliver the project's first genuine cross-skill interaction: **Volatile Fracture** — while allocated, Fracture Bolt's shards apply Detonation Field's Mark to enemies they hit, using the player's current Detonation Field rank data. This is the first proof that Expanding the Interaction Space (CORE_PHILOSOPHY point 13) is real in code, and it makes the Cluster A Decision Gate evaluable.

## Approved content specification

**Exactly one passive.** No filler.

**Volatile Fracture**
- `SkillType.Passive`, `MaxLevel 1`, starts at rank 0 (locked).
- Rank 0: no effect. Rank 1: Fracture Bolt shards apply a Detonation Mark to enemies they hit.
- The mark uses the player's **current Detonation Field rank** parameters (duration, explosion radius, explosion damage) resolved **at shard spawn time** — identical to a Detonation-applied mark at that moment. Investing in both skills amplifies the interaction; the passive itself holds no numeric values.
- Interacts with: Fracture Bolt (shards become mark-appliers) and Detonation Field (supplies mark data; its death-detonation/chain is the payoff).
- Build-identity rationale: changes how the two skills combine (spread marks → trigger the cascade), a real point-spend decision and the seed of a combo build — not a numeric upgrade.

## Approved architecture (must follow exactly)

- Passive = a `SkillDefinition` asset (`SkillType.Passive`, MaxLevel 1) registered in the existing `SkillProgression`; ranked via the existing `SkillPoints`. **No new currency, registry, synergy engine, callback/delegate system, or universal on-hit framework.**
- **`PassiveEffects`** (new, Player-side): reads Volatile Fracture's rank from `SkillProgression`, recomputes on `SkillLevelChanged`, exposes a typed `VolatileFractureActive` flag. Owns passive-relevant data; no universal struct.
- **`DetonationField`**: add read-only getters exposing its current-rank mark parameters (duration, current explosion radius, explosion damage). No behavior change.
- **`FractureBolt`** is the coordinator: on activation, if `VolatileFractureActive` AND its `DetonationField` reference is available, it builds a **typed mark payload** `{ applyMark, duration, radius, damage }` from Detonation Field's current-rank data and passes it to each spawned shard via a typed setter; otherwise `{ applyMark = false }`.
- **`FractureBoltProjectile`**: stores the optional typed payload; on enemy hit, if `applyMark`, get-or-adds a `DetonationMark` on the enemy and calls its existing `Apply(duration, radius, damage)`. The projectile **must not** query `PassiveEffects`, `SkillProgression`, `SkillDefinition`, or `DetonationField` — it consumes only the spawn-time payload.
- **Mark refresh/replace:** reuse the existing `DetonationMark.Apply` semantics intentionally (refreshes expiry + overwrites radius/damage, subscribes to `Health.Died` only once — no stacking, no duplicate subscription). Same path `DetonationField.Activate` uses.
- **Edge case (Volatile Fracture active but Detonation Field unavailable):** do not apply a mark; use no fallback gameplay values; emit at most one development warning, guarded by a one-shot bool on `FractureBolt` (not per shot/hit). A correctly configured Player prefab produces zero warnings.
- **`PlayerStats` remains untouched.**
- UI: extend `SkillAllocationHud` to list the passive alongside the actives (it is already a `SkillDefinition` in `RegisteredSkills`).

## Acceptance Criteria

- [x] Volatile Fracture exists as a `SkillDefinition` (Passive, MaxLevel 1), registered in `SkillProgression`, starting at rank 0. Verified (asset `skillType: Passive`, `maxLevel: 1`, `startingLevel: 0`; registered in the Player prefab's `SkillProgression`).
- [x] Rank 0: Fracture Bolt shards apply NO Detonation Mark. Verified (fresh start VF rank 0 → 0 marks).
- [x] Rank 1: Fracture Bolt shards apply a Detonation Mark to enemies they hit. Verified (stationary target hit by a shard → mark present; earlier misses were moving-enemy test artifacts).
- [x] The applied mark's parameters match the player's current Detonation Field rank; raising Detonation Field's rank changes the shard-applied mark accordingly. Verified (radius 3 at DF rank 1 → 5 at DF rank 3; duration/damage sourced from DF).
- [x] A shard-applied mark on an already-marked enemy refreshes/replaces it (no stacking, no duplicate `Health.Died` subscription). Verified (two shard marks → one `DetonationMark` component).
- [x] With Volatile Fracture active but Detonation Field unavailable: no mark applied, no fallback values, at most one dev warning (not per hit). Verified (5 resolves → 0 marks, exactly 1 warning).
- [x] Passive allocation obeys the same rules as actives (spend exactly one, reject at MaxLevel, reject at zero points, never negative). Verified (zero-point reject at fresh start; max-rank reject at rank 1; never negative).
- [x] `FractureBoltProjectile` does not reference `PassiveEffects`/`SkillProgression`/`SkillDefinition`/`DetonationField` — verified by inspection (only `MarkPayload`, `Health`, and applying `DetonationMark`).
- [x] `PlayerStats` untouched (confirmed unmodified in git); no new currency/registry/callback/synergy framework introduced (single typed `MarkPayload` struct only).
- [x] Regression: weapon, XP-on-kill, leveling, enemy pursuit (reached 2.0), obstacle avoidance (EnemyAI byte-unchanged per git — see note), both Active Skills + rank scaling, allocation, detonation chains all intact.
- [x] Zero Console errors or warnings with the correctly configured normal Player prefab (clean across the full fresh test session).
- [x] No out-of-scope features added.

**Note on obstacle-avoidance verification:** automated single-enemy avoidance runs returned "not reached in 400 steps" because the scene's `EnemySpawner` floods the arena during the window and crowds the attack-range ring, holding the tracked enemy just outside the strict 2.2 threshold. `EnemyAI.cs` is byte-for-byte unchanged from the approved BUG-001 commit (git confirms it is not in this changeset), so avoidance behavior is provably unaltered; open-space pursuit reached exactly 2.0 with this build.

## Test Procedure (deterministic)

Play Mode in `PrototypeScene`, deterministic stepping:
1. Volatile Fracture registered, starts rank 0.
2. Rank 0: fire Fracture Bolt at enemies; assert shards add NO `DetonationMark`.
3. Allocate a point to Volatile Fracture (rank 0→1, points −1); assert `PassiveEffects.VolatileFractureActive` true.
4. Rank 1: fire; assert shards add a `DetonationMark` to hit enemies; a subsequent kill detonates (chain reused).
5. Assert the shard-mark's parameters equal Detonation Field's current-rank values; raise Detonation Field's rank and confirm the shard-mark's radius/values change to match.
6. Pre-mark an enemy, then hit it with a shard: assert the mark is refreshed/replaced (one `DetonationMark`, single death subscription — no double detonation).
7. Remove/null the Detonation Field reference with Volatile Fracture active: assert no mark applied, exactly one warning across many shots.
8. Passive allocation rules: spend-one, max-rank reject (rank 1 is max), zero-point reject.
9. Regression battery (actives fire + scale, weapon, XP-on-kill, pursuit, avoidance, detonation chain, allocation).
10. Console: zero errors/warnings on the correct prefab.

## Files Allowed to Edit

- `Assets/Systems/Skills/` (new: `PassiveEffects.cs`, `MarkPayload.cs`; modify `FractureBolt.cs`, `FractureBoltProjectile.cs`, `DetonationField.cs`)
- `Assets/Systems/Utilities/SkillAllocationHud.cs` (show the passive)
- `Assets/ScriptableObjects/Skills/` (new `VolatileFracture` `SkillDefinition` asset; **modified** `FractureBolt.asset` + `DetonationField.asset` to set `StartingLevel = 1` — see discovered-blocker note below)
- `Assets/Prefabs/Player/Player.prefab` (add `PassiveEffects`, register the passive, wire refs)
- `Assets/Scenes/Prototype/PrototypeScene.unity` (only if scene wiring requires it)
- `Tasks/**/PC-009_sprint-2.5-passives-volatile-fracture.md`
- `CHANGELOG.md`, `ROADMAP.md`

### Discovered-blocker addition (approved 2026-07-11 — NOT part of the originally planned scope)
- **`SkillDefinition.cs`** and **`SkillProgression.cs`** — originally listed as Forbidden ("reuse as-is; if a change seems required, stop and flag"). During implementation, testing revealed that `SkillProgression`'s Sprint 2.4 single global `startingLevel` (=1) forced the passive to start at rank 1, conflicting with the approved "Volatile Fracture starts at rank 0." Implementation was stopped and the blocker flagged; the Game Director approved a minimal per-skill starting-level fix: `SkillDefinition` gains a read-only, clamped `StartingLevel` (0..MaxLevel); `SkillProgression` initializes each skill from its own `definition.StartingLevel` and the obsolete global field was removed. This was a discovered necessity, not original scope.

## Files Forbidden to Edit

- `PlayerStats.cs` (must remain untouched — confirmed unmodified)
- `PlayerXP.cs`, `PlayerLevel.cs`, `Health.cs`, `EnemyAI.cs` (confirmed unmodified)
- `SkillPoints.cs` (reused as-is; unmodified)
- `DetonationMark.cs` — **modified in review round 1 (Game-Director-requested)** to add the mark orb + death-burst prototype visuals; its `Apply` and detonation logic are otherwise unchanged. (Originally listed as reuse-only; the visual-feedback request made the change necessary.)
- All documentation not listed above

## Out of Scope

- Numeric/stat passives and any `PlayerStats` modifier-removal rework
- Additional passives beyond Volatile Fracture; a third build layer
- Upgrade trees/branches; random level-up choice UI
- General synergy engine / callback framework / universal on-hit system
- Save/load, cross-restart persistence, meta-progression, respec
- Ultimates, additional Active Skills, loadout selection, balancing pass, UI polish, networking
- Evaluating the Cluster A Decision Gate (that follows approval, separately)

## Risks

- `FractureBoltProjectile` decoupling — enforce via the typed spawn-time payload only.
- Mark-parameter sourcing timing — resolved at shard spawn from Detonation Field's live rank; guard for missing reference.
- Chain interactions under crowds — safe (`DetonationMark` guarantees one detonation per enemy) but verify.
- One-warning discipline — a single guarded bool, not per-hit logging.

## Implementation Report

Implemented by Claude 2026-07-11, branch `feature/sprint-1-first-playable`, uncommitted.

**New (`PointClear.Skills`):**
- `MarkPayload.cs` — plain typed struct `{ ApplyMark, Duration, Radius, Damage }` (+ `None`/`Of` helpers). The sole channel by which the interaction reaches a shard. Not a callback/delegate or general on-hit framework.
- `PassiveEffects.cs` — Player-side reader; exposes `VolatileFractureActive` (rank ≥ 1), recomputed on `SkillLevelChanged`, with a `Start()` safety re-read to eliminate start-up ordering fragility.

**Modified skill behaviors:**
- `FractureBolt.cs` — coordinator: `ResolveMarkPayload()` returns a payload only when `VolatileFractureActive` AND a `DetonationField` is available, sourcing duration/radius/damage from Detonation Field's current rank; sets it on the spawned primary; one-shot `warnedMissingDetonation` guard for the missing-field edge case.
- `FractureBoltProjectile.cs` — `SetMarkPayload`; primary forwards the payload to its shards (never marks itself); a shard, on hit, get-or-adds a `DetonationMark` and calls its existing `Apply`. Consumes only the payload — no passive/progression/definition/DetonationField queries.
- `DetonationField.cs` — added read-only `CurrentMarkDuration` / `CurrentExplosionRadius` / `CurrentExplosionDamage` getters (no behavior change).
- `SkillAllocationHud.cs` — row label now includes `SkillType`, so the passive shows as `Volatile Fracture (Passive) Lv 0/1`.

**Discovered-blocker core change (Game-Director-approved mid-sprint):**
- `SkillDefinition.cs` — added read-only `StartingLevel` (serialized, clamped to `[0, MaxLevel]`; initial progression state only, no gameplay value).
- `SkillProgression.cs` — initializes each registered skill from its own `definition.StartingLevel`; the obsolete single global `startingLevel` field was removed (no competing mechanism).

**Assets/wiring:** new `VolatileFracture` `SkillDefinition` (id `volatile_fracture`, Passive, MaxLevel 1, StartingLevel 0). Existing assets set explicitly: `FractureBolt` StartingLevel 1, `DetonationField` StartingLevel 1. `Player.prefab`: added `PassiveEffects` (wired to progression + the VF definition), registered VF in `SkillProgression`, wired `FractureBolt.passiveEffects` + `.detonationField`.

**Confirmed unmodified:** `PlayerStats`, `PlayerXP`, `PlayerLevel`, `Health`, `EnemyAI`, `SkillPoints`, `DetonationMark` (git-verified).

**Testing:** full deterministic Play Mode plan — see Acceptance Criteria (all pass); zero console errors/warnings on the correct prefab. Hands-on Q/E + on-screen allocate-button confirmation remains for Yoav (input/UI logic verified programmatically).

Not committed — awaiting Yoav's playtest and review.

## Review Notes

**Review round 1 (2026-07-11) — not approved: no visible feedback.** Yoav's hands-on review could not see the mark on an enemy or any explosion on a marked death (the enemy "just disappeared"), making the interaction unverifiable in normal play even though the damage logic was correct.

**Investigation:** confirmed there was *no* visual for the Detonation Mark and *no* visual for the death detonation (only Detonation Field's separate cast-disc VFX existed). Area damage *was* real in normal play (runs in `HandleDeath`, not test-only) — just invisible.

**Fix (minimal prototype visuals, added to `DetonationMark.cs`):** a small orange marker orb above any marked enemy, and a short-lived yellow burst sphere sized to the detonation radius (diameter = radius × 2) at the death position. Both are collider-less sphere primitives on a runtime-created shared URP/Lit material (tinted via MaterialPropertyBlock), and clean themselves up (marker with the mark/enemy, burst on a 0.35s timer). Because both the Detonation Field cast and the Volatile Fracture shards create a `DetonationMark` and call `Apply`, they share these visuals automatically. No VFX system, screen shake, audio, or UI added.

**Re-verified in Play Mode (all pass, zero console errors):** marker appears on both DF-cast and shard-applied marks (same visual); URP/Lit shader (no magenta); marker and burst have no colliders; enemy death destroys the enemy and its marker (1→0) and spawns a burst that auto-destroys (1→0); mark expiry removes the marker with no erroneous detonation; area damage hits an enemy inside the radius (100→0) and not one outside (100→100); burst size scales with Detonation Field rank (radius 3→5 ⇒ burst scale 6→10) alongside the gameplay radius; before Volatile Fracture is allocated, shard hits produce no marker. No accumulating scene objects.

(Still awaiting Yoav's hands-on re-review.)

## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-12
- Notes: Hands-on playtest passed — the mark, the death detonation, and the Volatile Fracture cross-skill interaction are now visible and behave as designed. Approved to DONE, commit, and push. Cluster A Decision Gate to be evaluated immediately after.

## Future VFX Architecture Note (agreed at approval, out of scope for this sprint)

The Sprint 2.5 detonation visuals in `DetonationMark.cs` are deliberately prototype-grade: each mark/burst is a `GameObject.CreatePrimitive(Sphere)` created at runtime, and each shard-applied mark is an `AddComponent<DetonationMark>` on the enemy. This is correct for proving legibility now, but it allocates and instantiates on combat hot paths and will not scale to the large enemy counts the project's engineering principles call for.

Agreed direction for a future maintenance/VFX sprint (not to be implemented until scoped and approved):
- Replace runtime primitive creation with a **pooled visual system** (pre-warmed marker + burst pools, reused rather than instantiated/destroyed per event).
- Consider **prefab-authored** marker/burst visuals on a proper URP VFX/material pipeline instead of runtime `CreatePrimitive` + `Shader.Find`.
- Keep the current shared-material + `MaterialPropertyBlock` tinting approach (already allocation-light) or move to a small set of pre-authored materials.
- No behavior change intended — this is a performance/architecture refactor of presentation only; gameplay (mark/detonate/chain) stays as-is.

Recorded here so the debt is tracked against a real trigger (rising enemy counts / a dedicated VFX pass), not lost.

## Definition of Done Checklist

- [x] Acceptance criteria pass (Play Mode verified)
- [x] Unity has no new compiler errors (0 errors, 0 warnings on correct prefab)
- [x] Required testing was completed (deterministic functional + regression; hands-on playtest passed 2026-07-12)
- [x] Yoav approved the result (2026-07-12)
- [ ] Technical review was completed — no Technical Director present; DONE on the Game Director's explicit authority, flagged (consistent with PC-007/PC-008)
- [x] Relevant documentation was updated (this task file, CHANGELOG.md, ROADMAP.md)
