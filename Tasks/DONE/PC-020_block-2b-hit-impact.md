# Task PC-020 — Milestone 1 / Block 2 (Combat Feel) / Block 2B: Hit Impact

## Task ID
PC-020

## Title
Block 2B — Hit Impact: tune the existing hit reaction and make it consistent across all
enemy prototypes (no new feedback system).

## Status
DONE — playtest-approved by the Game Director 2026-07-15. Technical-review box left
unchecked (no Technical Director present); DONE on the Game Director's explicit
authority, flagged (consistent with PC-007..019).

## Priority
High (Combat Feel sub-block, Milestone 1)

## Owner
Claude (implementation) / Yoav (direction + playtest approval)

## Reviewer
Yoav (Game Director)

## Dependencies / Related
- Milestone 1 design; Combat Feel review (Block 2 breakdown)
- Precedent: Block 2A Kill Feedback (PC-018, death beat), Block 2C Player Damage Feedback (PC-019)
- Pillars: Combat Is King, Arena Rhythm (readability governor), A Living Arena,
  Gameplay Prototype Philosophy, The Golden Rule

## Objective
Answer one question — "What should the player feel the instant a shot connects?" — by
determining whether the existing feedback channels are strong enough once tuned, rather
than adding a new channel. Goal: a crisp, fair hit read that is distinct from death,
telegraphs, muzzle/trail, and player-damage feedback, and consistent on every enemy.

## Review outcome (why this is a tuning pass, not a new system)
A full inventory of what fires on a successful hit found the arena already has hit
confirmation — the enemy **scale-pop** (reliable, colour-independent) plus a colour
**flash**, alongside muzzle flash, bullet trail, and the Block 2A death beat. The
weaknesses were (a) the flash was **white**, identical to the death beat and invisible on
the grey Walker after its first hit, and (b) the whole reaction only existed on the Walker.
Both are fixable by tuning/fixing existing cues — no spark, hitstop, shake, or audio.

## What was delivered
- **Tuning:** hit reaction is now **cyan `(0.4, 0.95, 1.0)`** with a **1.35× pop**
  (distinct from the 1.15× yellow attack pulse and the 1.2× white death pop, and from the
  player's red hurt flash and the yellow muzzle/trail).
- **Consistency:** new shared **`EnemyHitReaction`** component (one per enemy prefab)
  subscribes to `Health.Damaged`, flashes cyan + pops, then restores the **colour and
  scale that were live when the hit landed** — preserving each enemy's runtime tint and
  telegraph/aura state. Death-safe: ignores the killing blow, stops on `Health.Died`, and
  leaves death visuals to `EnemyDeathBeat`. Overlapping hits never capture the cyan/popped
  state as the restore target, so colour/scale never drift.
- **Correctness:** untinted enemies (grey Walker) restore to the shared material's grey,
  not a hardcoded white (previously left Walkers white after hit 1). Applied in both the
  shared reaction and the retained `EnemyAI` attack pulse.
- **Player-lookalike fix:** the Surrounder (a green Capsule, same mesh as the player's
  green preset) recoloured green → **purple `(0.6, 0.25, 0.85)`** — the smallest greybox
  change; no bad prefab/material reference existed. Set on the prefab's serialized
  `bodyColor`, which was shadowing the code default.
- `EnemyAI` keeps its attack pulse; its inline hit reaction and the dead `hitFlashDuration`
  field were removed.

## Acceptance Criteria
- [x] All four enemy types (Walker, Charger, Surrounder, Empowerer) show the cyan flash + 1.35× pop.
- [x] Every enemy restores its exact active runtime colour after being hit (Walker grey, Charger state colour, Surrounder purple, Empowerer gold, buffed Walkers their buff tint).
- [x] Charger telegraph states and Empowerer aura/buffs still work.
- [x] Death beat remains clean and distinct (no cyan/pop fighting the collapse).
- [x] No enemy resembles the player closely enough to confuse (purple Surrounder).
- [x] Rapid/overlapping hits stay readable with no colour or scale drift.
- [x] Hit read distinct from player-damage, death, telegraphs, muzzle flash, trail.
- [x] No new feedback system (no sparks/hitstop/shake/audio/damage numbers/new VFX/new assets).
- [x] Clean compile and console.
- [x] Game Director hands-on playtest approved.

## Files changed
- New: `Assets/Systems/Enemies/EnemyHitReaction.cs`
- Modified: `Assets/Systems/Enemies/EnemyAI.cs`, `Assets/Systems/Enemies/SurrounderAI.cs`
- Prefabs: `Assets/Prefabs/Enemies/Enemy.prefab`, `Charger.prefab`, `Surrounder.prefab`, `Empowerer.prefab`
- Docs: `CHANGELOG.md`, this task file

## Out of scope / gated (respected)
Hitstop, camera shake, audio, damage numbers, hit sparks, new VFX/systems/assets. A very
restrained hitstop experiment remains the only candidate *if* a future playtest shows hits
still read as weightless — an explicit, separate decision.

## Notes
Tuning knobs (`flashColor`, `flashDuration`, `popScale`) are serialized on
`EnemyHitReaction` — greybox, disposable. Automated Unity validation confirmed the flash
colour, 1.35× pop, and per-type restore target for all four enemies (reflection + material
reads); the 0.1s coroutine restore timing was confirmed in the Game Director's focused
hands-on playtest.
