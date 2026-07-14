# Task PC-018 — Milestone 1 / Block 2 (Combat Feel) / Block 2A: Kill Feedback

## Task ID
PC-018

## Title
Block 2A — Kill Feedback: a brief greybox death beat replacing instant enemy disappearance

## Status
DONE — playtest-approved by the Game Director 2026-07-13. Technical-review box left
unchecked (no Technical Director present); DONE on the Game Director's explicit
authority, flagged (consistent with PC-007..017).

## Priority
High (first sub-block of Block 2 — Combat Feel, Milestone 1)

## Owner
Claude (implementation) / Yoav (direction + playtest approval)

## Reviewer
Yoav (Game Director)

## Dependencies / Related
- Milestone 1 design; Combat Feel review (Block 2 breakdown, chat-approved)
- Pillars: Combat Is King, Arena Rhythm (readability governor), Gameplay Prototype
  Philosophy, The Golden Rule

## Objective
Make normal enemy kills feel *defeated* rather than *vanished* — a short, restrained,
readable death beat — without slowing the Arena rhythm, and without disturbing any
kill rewards or Operation progression.

## What was delivered
- **`EnemyDeathBeat`** (new, one per enemy prefab): on `Health.Died` it stops the
  enemy (disables its `IEnemyBehaviour` driver + all colliders — no more move/attack/
  collide/re-hit), plays a ~0.15s white-flash **pop→collapse**, then destroys the object.
- **`IEnemyBehaviour`** (new, empty marker): implemented by EnemyAI/Charger/Surrounder/
  Empowerer so the beat can stop any enemy's AI generically. Not a framework.
- The four AIs no longer self-destruct on death (removed their `Died → Destroy`);
  EmpowererAI keeps `OnDisable → RevertAll` so buffs clear the instant it dies.
- `EnemyDeathBeat` added to Enemy/Charger/Empowerer/Surrounder prefabs.

## Exactly-once guarantee
All rewards (XP, currency, kill counter, Operation quota, mark detonation) fire on
`Health.Died` via their own subscribers. `Health` raises `Died` exactly once; the
death beat only adds the visual + teardown and a `beating` guard. `EnemyAI.ActiveCount`
is still decremented once (on the AI's `OnDisable`, fired when the beat disables it).

## Acceptance Criteria
- [x] Instant disappearance replaced by a short pop/collapse (~0.15s), readable at horde scale.
- [x] Enemy cannot move/attack/collide/be-re-hit during the beat.
- [x] XP, kill count, drops, Operation progression trigger exactly once (double-kill grants nothing extra).
- [x] Empowerer buff clears on death.
- [x] Multiple simultaneous deaths stable and readable.
- [x] No new compiler/console errors.
- [x] Game Director hands-on playtest approved.

## Files changed
- New: `Assets/Systems/Enemies/EnemyDeathBeat.cs`, `Assets/Systems/Enemies/IEnemyBehaviour.cs`
- Modified: `Assets/Systems/Enemies/EnemyAI.cs`, `ChargerAI.cs`, `SurrounderAI.cs`, `EmpowererAI.cs`
- Prefabs: `Assets/Prefabs/Enemies/Enemy.prefab`, `Charger.prefab`, `Empowerer.prefab`, `Surrounder.prefab`
- Docs: `CHANGELOG.md`, this task file

## Out of scope (respected — later Combat Feel blocks)
No ragdoll/gore/gibs, no particle/animation systems, no audio, no camera shake, no
hitstop (Block 2B), no per-enemy/explosion emphasis (Blocks 2B/2D), no generic feedback
framework.

## Notes
`duration` (0.15s) and `popScale` (1.2) are serialized on `EnemyDeathBeat` for playtest
tuning — greybox, disposable.
