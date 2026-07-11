## Task ID
PC-007

## Title
Sprint 2.3 — Active Skill System Validation (Fracture Bolt + Detonation Field)

## Status
DONE (2026-07-11) — Yoav playtested Sprint 2.3 in Unity and approved ("Result: Approved... both active skills behave as expected"). Technical-review box left unchecked (no Technical Director present); DONE on the Game Director's explicit authority, flagged — same handling as PC-003/PC-004/PC-006.

## Priority
High

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- Task PC-003 (Sprint 1 combat prototype) — DONE (`Health`, `EnemyAI`, `HitscanWeapon`, `PlayerController`, `EnemySpawner`, arena)
- Task PC-004 (Sprint 2.0–2.2 progression) — DONE (`PlayerReference`, `PlayerStats`, `PlayerXP`, `PlayerLevel`)

## Related Documents
- [ROADMAP.md](../../ROADMAP.md) Phase 2 § Cluster A → Sprint 2.3
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Build Philosophy (Discovery, Expanding the Interaction Space)
- [DESIGN_DNA.md](../../DESIGN_DNA.md) § Build Philosophy (good vs. bad upgrades; propagation chains)
- [DECISIONS.md](../../DECISIONS.md) DEC-013 (layered build system — Active Skills layer), DEC-017 (no fixed classes)
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../../Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)

## Objective

Prove that Point Clear can support mechanically diverse Active Skills as a real system — not one weapon with variations — by building the first two concrete, mechanically distinct Active Skills: **Fracture Bolt** (a ranged, aimed, piercing/splitting projectile) and **Detonation Field** (a self-centered area skill that marks enemies and detonates them on death). Both are always available and bound to fixed inputs; there is no skill-point cost or selection screen yet.

## Background

Sprint 2.3 was redirected (from an earlier "upgrade selection / stat picker" idea) during the Phase 2 roadmap redesign to validate the **Active Skills** build layer (DEC-013). The two specific skills were explored in a design exercise and **approved by the Game Director on 2026-07-11** (Fracture Bolt + Detonation Field), chosen to be maximally distinct from each other and from the existing `HitscanWeapon`, and to be strong seeds for future build growth (propagation-style interactions). A Pre-Sprint Architecture Review was completed earlier and its conclusions are folded into Technical Notes, below.

The two skills are deliberately **not** wired to interact with each other in this sprint (e.g. Fracture Bolt does not apply Detonation Field's mark). That first cross-skill interaction belongs to Sprint 2.5 (Second Build Layer + First Real Interaction), and is explicitly out of scope here.

## Requirements

### Fracture Bolt (Projectile skill)
- Bound to a new `FractureBolt` input action (keyboard, `Q`).
- On activation (respecting its own cooldown), fires a projectile from the player toward the current aim point (reuse `PlayerController.AimWorldPoint`).
- The projectile travels in a straight line; on hitting its first enemy it deals full damage, then **splits into two shards** that continue outward at symmetric angles (±`splitAngle`) from the travel direction.
- Shards deal reduced damage, have a shorter lifetime/range, are destroyed on their first enemy hit, and **do not split again** (no recursion).
- Projectile and shards self-destroy on lifetime expiry or on leaving the arena (whichever first).
- Owns its own cooldown; no shared cooldown abstraction (per the architecture review — not enough evidence for a shared abstraction yet).

### Detonation Field (Area skill)
- Bound to a new `DetonationField` input action (keyboard, `E`).
- On activation (respecting its own cooldown), applies a **Mark** to every enemy within `markRadius` of the player (self-centered — no separate targeting/reticle this sprint).
- A Marked enemy that **dies within `markDuration` seconds** (from any damage source) triggers a secondary **explosion** at its position: an area-of-effect damage burst to enemies within `explosionRadius`. The Mark expires silently if the enemy survives past `markDuration`.
- Explosions may chain (an explosion that kills another Marked enemy triggers that enemy's explosion) — this is the intended "set up, then chain" payoff. Chains terminate because each enemy's `Health.Died` fires at most once.
- Re-marking an already-marked enemy refreshes its expiry rather than stacking.
- Owns its own cooldown; no shared cooldown abstraction.

### Shared
- No physical knockback (per architecture review — the enemy is kinematic; knockback would require an unjustified `EnemyAI` change).
- No skill points, no selection/allocation UI, no second build layer, no stat-card system.
- Placeholder visuals only, consistent with the existing greybox style (primitives + the existing `Assets/Art/Materials/VFX_Bright.mat`).
- Temporary on-screen cooldown readiness readout added to the existing prototype `DebugHud` (not final UI).
- Keyboard-only bindings (no gamepad this sprint).

## Acceptance Criteria

- [x] `FractureBolt` and `DetonationField` input actions exist in the Player action map (Move/Aim/Shoot/FractureBolt/DetonationField); no other actions added. Verified: both actions resolved and enabled at runtime (Q, E).
- [x] Fires a Fracture Bolt toward the aim point; on first enemy hit deals full damage (40) and spawns exactly two shards. Verified: target 100→60, 2 shards spawned; component's own `Activate()` spawns a bolt.
- [x] Shards deal reduced damage (20), do not split, and are destroyed on first hit or lifetime expiry. Verified: target 100→80, projectile count 1→0 on hit (no re-split).
- [x] Fracture Bolt respects its own cooldown. Verified: after firing, `IsReady=false`, `CooldownRemaining≈1.5s`.
- [x] Marks all enemies within `markRadius`; a marked enemy killed within `markDuration` produces an explosion damaging nearby enemies; an unmarked/out-of-range enemy does not. Verified: in-radius A/B marked, far enemy not marked; killing marked A damaged neighbor.
- [x] Explosion chains work and always terminate. Verified: killing marked A (explosion 100) killed full-HP marked neighbor B in turn; no loop/stall (bounded by `Health.Died` firing once per enemy).
- [x] Detonation Field respects its own cooldown. Verified: after activation, `IsReady=false`, `CooldownRemaining≈6s`. Mark-expiry verified: killing an expired-mark enemy produced no explosion (neighbor HP unchanged).
- [x] The two skills are mechanically distinct from each other and from `HitscanWeapon` (ranged aimed splitting projectile vs. self-centered set-up/payoff area).
- [x] `DebugHud` shows both skills' cooldown readiness (READY / remaining seconds) and the Q/E control lines.
- [x] No cross-skill interaction implemented (Fracture Bolt does not apply Marks) — reserved for Sprint 2.5.
- [x] Zero new Unity compiler errors and zero new console warnings (checked after compile and after the full Play Mode test session).
- [x] No out-of-scope systems added (no skill points, selection UI, second layer, knockback, gamepad); `PlayerStats` unchanged by skills (Damage=10, MoveSpeed=6 before/after).

## Test Procedure

Play Mode in `PrototypeScene`:
1. **Fracture Bolt basic:** press `Q` aimed at a single enemy → enemy takes full damage; two shards spawn and travel onward at symmetric angles.
2. **Fracture Bolt line-up:** align multiple enemies; a bolt + its shards hit several enemies (proving the "line them up" reward).
3. **Fracture Bolt cooldown:** rapid `Q` presses fire only at the cooldown interval.
4. **Detonation Field mark+detonate:** walk into a cluster, press `E`, then kill a marked enemy within `markDuration` → explosion damages neighbors. Kill an unmarked enemy → no explosion.
5. **Detonation Field chain:** mark a dense cluster, then kill one → observe chained explosions; confirm no error/stall and the scene remains stable.
6. **Detonation Field expiry:** mark an enemy, wait past `markDuration`, kill it → no explosion.
7. **Detonation Field cooldown:** rapid `E` presses activate only at the cooldown interval.
8. **Distinctness:** demonstrate both skills back-to-back; confirm they read as different mechanics.
9. **Console:** zero errors/warnings throughout.
10. **Regression:** existing movement, aiming, `HitscanWeapon` fire, enemy chase/separation, XP/leveling still work.

Deterministic frame-stepping (`EditorApplication.Step()` while paused) may be used to avoid the known Editor-focus Play Mode stall (see PC-003 notes).

## Files Allowed to Edit

- `Assets/Systems/Skills/` (new folder — `FractureBolt.cs`, `FractureBoltProjectile.cs`, `DetonationField.cs`, `DetonationMark.cs`, namespace `PointClear.Skills`)
- `Assets/Systems/Utilities/DebugHud.cs` (cooldown readout only)
- `Assets/Settings/InputSystem_Actions.inputactions` (add the two actions only)
- `Assets/Prefabs/` (new skill/projectile prefabs; `Player.prefab` to add skill components)
- `Assets/Scenes/Prototype/PrototypeScene.unity` (only if needed to wire the Player instance)
- `Tasks/**/PC-007_sprint-2.3-active-skill-system.md`
- `CHANGELOG.md`, `ROADMAP.md` (status sync on completion)

## Files Forbidden to Edit

- Everything else, including `Health.cs`, `EnemyAI.cs`, `PlayerStats.cs`, `PlayerXP.cs`, `PlayerLevel.cs` (skills must build on their existing public surfaces, not modify them — if a change to one seems required, stop and flag it), and all other documentation not listed above.

## Out of Scope

- Skill Points, skill allocation, any selection/upgrade UI (Sprint 2.4)
- Any second build layer (Sprint 2.5)
- Cross-skill interactions (Fracture Bolt applying Marks, etc. — Sprint 2.5)
- Physical knockback; any `EnemyAI` behavioral change
- A shared skill base class / interface / cooldown abstraction (no second real consumer justifies it yet)
- Gamepad bindings; final VFX/UI/audio
- Loot, equipment, enemy variety, bosses, maps (later Clusters)

## Technical Notes (from the Pre-Sprint Architecture Review)

- **Cooldowns:** each skill keeps its own private `nextReadyTime` gate (same shape as `HitscanWeapon` fire-rate and `EnemyAI.attackInterval`). Not extracted into a shared type — the Game Director explicitly declined that until real duplication with shared long-term behaviour appears.
- **Projectile collision:** use a per-`FixedUpdate` raycast along the movement step (`Physics.Raycast`, `QueryTriggerInteraction.Ignore`) rather than a trigger collider, to avoid tunnelling and match `HitscanWeapon`'s raycast approach. Resolve the hit `Health` via `GetComponentInParent<Health>()`.
- **Detonation Field query buffer:** its `Physics.OverlapSphere`/`NonAlloc` query must use its **own** buffer — never `EnemyAI.SeparationBuffer` (a shared static array reused every `EnemyAI.FixedUpdate`; borrowing it would corrupt separation mid-frame).
- **Mark model:** `DetonationMark` is a runtime `MonoBehaviour` added to a marked enemy, subscribing to that enemy's `Health.Died`. It records an expiry time and self-destroys on expiry; on death-before-expiry it spawns the explosion. Re-marking refreshes expiry. This keeps all mark logic out of `EnemyAI`/`Health`.
- **Chain termination:** guaranteed by `Health.TakeDamage`'s `if (IsDead) return;` guard — `Died` fires exactly once per enemy, so explosion chains are bounded by enemy count.
- **Input:** each skill component resolves its own `InputAction` in `OnEnable` via `InputSystem.actions.FindAction(...)`, matching `PlayerController`'s existing pattern (no central dispatcher).
- **Aim reuse:** Fracture Bolt reads `PlayerController.AimWorldPoint` for direction — no new targeting system. Detonation Field is self-centered, needing none.
- **`PlayerStats` note:** neither skill reads or writes `PlayerStats` this sprint (no stat scaling yet) — damage values are the skills' own serialized fields, kept data-driven, not hardcoded at call sites.

## Implementation Report

Implemented by Claude on 2026-07-11, branch `feature/sprint-1-first-playable`, uncommitted.

**Code (new folder `Assets/Systems/Skills/`, namespace `PointClear.Skills`):**
- `FractureBolt.cs` — Player component; reads the `FractureBolt` action (Q), private cooldown gate, spawns the bolt toward `PlayerController.AimWorldPoint`.
- `FractureBoltProjectile.cs` — projectile movement + per-`FixedUpdate` raycast collision (no trigger collider, no tunnelling). Primary bolt deals 40, splits into two shards at ±25°; shards (`canSplit=false`) deal 20, die on first hit, never re-split. Lifetime-based self-destruct.
- `DetonationField.cs` — Player component; reads the `DetonationField` action (E), private cooldown gate, self-centered `OverlapSphereNonAlloc` (own 64-slot buffer, never `EnemyAI.SeparationBuffer`) marking enemies in `markRadius`.
- `DetonationMark.cs` — runtime component added to a marked enemy; subscribes to that enemy's `Health.Died`; detonates (AoE 100 in `explosionRadius`) if the enemy dies before `markDuration` expires, else removes itself. Re-marking refreshes expiry. Chains terminate because `Health.Died` fires once per enemy.

**Assets:**
- Input Actions: added `FractureBolt` (`<Keyboard>/q`) and `DetonationField` (`<Keyboard>/e`) to the Player map; no other actions changed.
- New prefabs under `Assets/Prefabs/Skills/`: `FractureBoltProjectile` (sphere, 40 dmg, splits, references shard), `FractureShard` (smaller sphere, 20 dmg, no split, 0.6s life), `DetonationFieldVfx` (flat cylinder, scaled to radius at runtime). All use the existing `VFX_Bright.mat`; stray primitive colliders removed so projectiles/VFX never intercept shots or queries.
- `Player.prefab`: added `FractureBolt` (bolt prefab, Muzzle child as origin, 1.5s cd) and `DetonationField` (markRadius 5, markDuration 4, explosionRadius 3, explosionDamage 100, 6s cd, field VFX).
- `DebugHud.cs`: Q/E control lines + per-skill READY/cooldown readout.

**Tuning note:** `explosionDamage = 100` equals enemy max HP so the chain-detonation is reliably demonstrable — a prototype placeholder for validating the mechanic, **not** a balance decision (balance is explicitly out of scope; see CORE_PHILOSOPHY § Build Philosophy point 9).

**Files forbidden to edit were respected:** `Health.cs`, `EnemyAI.cs`, `PlayerStats.cs`, `PlayerXP.cs`, `PlayerLevel.cs` were **not** modified — the skills build only on their existing public surfaces (`Health.TakeDamage`/`Died`/`IsDead`, `PlayerController.AimWorldPoint`, `EnemyAI` as a type filter).

**Testing:** full Play Mode verification via deterministic frame-stepping (avoids the PC-003 Editor-focus stall). All acceptance criteria checked and passing; regression confirmed (weapon, XP-on-kill, level, PlayerStats all intact); **zero console errors/warnings** after compile and after the entire test session. Real sustained keyboard-input simulation was not used (unreliable in an unattended session, per PC-003) — the input **binding** was verified as resolved+enabled, and each skill's own activation code path was exercised directly; a human Q/E playtest is the remaining confirmation for Yoav's review.

## Review Notes

Yoav performed the QA playtest in Unity and approved: implementation feels solid, gameplay responsive, both active skills behave as expected. This completes the hands-on Q/E confirmation that was the one item outstanding from the automated verification.

## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: Approved after a hands-on Unity playtest of both skills ("QA Review Complete... Result: Approved"). Covers Fracture Bolt and Detonation Field as implemented, validating the Active Skills build layer for Cluster A.

## Definition of Done Checklist

- [x] Acceptance criteria pass (verified via Play Mode; see Acceptance Criteria)
- [x] Unity has no new compiler errors (0 errors, 0 warnings)
- [x] Required testing was completed (automated Play Mode functional + regression, plus Yoav's hands-on Q/E playtest)
- [x] Yoav approved the result
- [ ] Technical review was completed — not performed (no Technical Director present); DONE on the Game Director's explicit authority, flagged
- [x] Relevant documentation was updated (this task file, CHANGELOG.md, ROADMAP.md)
