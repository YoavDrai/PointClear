## Task ID
BUG-001

## Title
Enemy Obstacle Blocking and Local Avoidance

## Type
Bug fix (combat-space behavior)

## Status
DONE (2026-07-11) — playtest-approved by Yoav ("Enemies now respect obstacles and actively navigate around the current prototype walls instead of remaining indefinitely stuck. The behavior is suitable for the current prototype scope."). Technical-review box left unchecked (no Technical Director present); DONE on the Game Director's explicit authority, flagged — same handling as PC-003/PC-004/PC-006/PC-007.

## Priority
High (affects core combat-space rules and enemy behavior)

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Discovered During
Extended gameplay review of the Sprint 2.3 build (PC-007). The Active Skills themselves were approved; this combat-space defect was found in the same session and fixed before starting Sprint 2.4.

## Related Documents
- [Tasks/DONE/PC-003_sprint-1-first-playable-combat-prototype.md](PC-003_sprint-1-first-playable-combat-prototype.md) — Sprint 1.2 added the greybox obstacles and recorded enemy pass-through as a known limitation at the time
- [Tasks/DONE/PC-007_sprint-2.3-active-skill-system.md](PC-007_sprint-2.3-active-skill-system.md) — the build under review when this was found

## The Original Bug (pass-through)
The player correctly collided with and was blocked by arena obstacles and boundary walls, but **enemies moved straight through those same obstacles**. Root cause: `EnemyAI.Awake()` sets the enemy `Rigidbody` to `isKinematic = true`, and `Rigidbody.MovePosition` on a **kinematic** body is not stopped by static colliders — it slides through them. The player's `Rigidbody` is dynamic (non-kinematic), so its `MovePosition` is blocked; that asymmetry produced the bug. Colliders (non-trigger `BoxCollider`s), layers (all `Default`), triggers, and the collision matrix were all already correct — the issue was purely kinematic movement.

## Why the Blocking-Only Fix Was Insufficient
The first correction added a collider **sweep-and-stop** before `MovePosition`: the enemy stopped just short of any obstacle instead of passing through. This fixed pass-through, but it was **passive** — an enemy pursuing a stationary player behind a wall pressed against the wall face and remained stuck there indefinitely (no lateral intent to go around), only freed if the player moved or another enemy nudged it to an edge. That violates the intended combat loop, where enemies continuously pursue the player.

## The Fix — Local Wall-Tangent Avoidance
Local steering, **no NavMesh** (unnecessary for the current single-obstacle prototype layout). The enemy stays kinematic; the hard sweep-stop remains as a final anti-pass-through safety.

- When the direct (seek + separation) path is blocked, the enemy derives the **tangent of the hit surface from the collision normal** and slides along it. Because the tangent is taken from the wall normal (not from a rotation of the toward-player direction), it stays **parallel to the wall regardless of where the enemy is along it** — so the enemy walks straight to the wall's edge instead of swinging back toward centre. (An earlier seek-relative angle-probe approach was tried and rejected precisely because it oscillated around the wall's midpoint as the player moved off-axis.)
- The chosen side is **latched for the whole traversal**; only a blocked tangent (inside corner) or the stuck-timer flips it. This is the anti-oscillation guarantee.
- A **position-based stuck timer** (keyed on actual movement, not distance-to-player — which is naturally flat while legitimately wall-sliding) flips the avoidance side if the enemy is genuinely jammed for `stuckTimeout` seconds.
- The enemy **returns to direct pursuit automatically** the instant the path clears past the obstacle's edge.
- A larger stop-gap (`obstacleSkin` 0.05 → 0.2) gives the side probe clearance so it doesn't graze the very wall it is hugging.
- Existing separation and the attack-range clamp are preserved (a light separation influence is retained while wall-following).

## Acceptance Tests Performed (Play Mode, deterministic frame-stepping)
- Enemy cannot pass through obstacles — verified (stops at wall surface; `passedThrough=false` in every scenario).
- Player stationary behind a **wide** wall — enemy goes around (max lateral 5.5, past the edge) and reaches the player (dist 2.00).
- Player stationary behind a **small** obstacle — reached in ~140 steps with a single lateral direction reversal (no jitter).
- **Several enemies** at the same wall — 4/4 reached; closest pair ever 0.75 (no severe stacking).
- **Player moving** around the obstacle while pursued — enemy followed around and caught up (dist 2.00).
- Open-space pursuit unchanged — ring at attackRange 2.00.
- Attack-range stopping correct — holds at 2.00.
- Enemy separation functional — open-space closest pair 2.83.
- Fracture Bolt and Detonation Field still work — 40 dmg + 2 shards; mark + chain detonation.
- Zero new console errors or warnings (confirmed after compile and after the full test session).
- Post-refactor re-verification (see below) reproduced all deterministic results identically.

## Known Limitation (concave geometry / future NavMesh)
This is **single-obstacle local steering, not global pathfinding**. It reliably rounds a single convex wall/obstacle — the current prototype's layout. A **concave trap** (U-shape / dead-end pocket) could still hold an enemy until the stuck-timer flips it out, and it will not compute a multi-obstacle route through a maze. The perfectly-symmetric dead-centre wide-wall case also shows run-to-run variance in how quickly it rounds the corner (it always reaches and never passes through, but timing varies). **If future maps introduce concave or densely packed geometry, that is the point to adopt a NavMesh** — recorded here as a deliberate future decision, not a surprise.

## Files Changed
- `Assets/Systems/Enemies/EnemyAI.cs` — only file. Added: kinematic-aware obstacle blocking (`BlockAgainstObstacles`), a shared sweep+filter (`TryGetObstacleAhead`, used by the hard clamp, the direction probe, and the avoidance check — introduced during the pre-finalize refactor to remove triplicated sweep logic), local wall-tangent avoidance (`ResolveMoveDirection`), and position-based stuck recovery (`UpdateStuckRecovery`). Serialized tunables: `obstacleSkin`, `avoidLookAhead`, `stuckTimeout`.

## Refactor Note (pre-finalize readability pass)
The correction added substantial logic to `EnemyAI.cs`. Before finalizing, the three places that each performed the same "sweep + filter out enemies/player/floor" were consolidated into one `TryGetObstacleAhead` helper (`BlockAgainstObstacles`, `IsDirectionBlocked`, and the avoidance probe now all call it). Behavior-neutral; re-verified in Play Mode after the refactor (deterministic scenarios reproduced identically, console clean).

## Game Director Approval
- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: Approved after a hands-on playtest of the updated avoidance. Suitable for current prototype scope; concave-geometry/NavMesh limitation noted and accepted.

## Definition of Done Checklist
- [x] Original pass-through bug fixed (enemies cannot move through obstacles)
- [x] Enemies actively navigate around single obstacles and reach the player when a route exists
- [x] No permanent stuck against ordinary prototype walls
- [x] Separation, attack-range stop, and open-space pursuit preserved
- [x] Fracture Bolt and Detonation Field still work
- [x] Unity has no new compiler errors or warnings
- [x] Yoav approved the result
- [ ] Technical review was completed — not performed (no Technical Director present); DONE on Game Director's explicit authority, flagged
- [x] Relevant documentation updated (this task file, CHANGELOG.md)
