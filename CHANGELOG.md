# Point Clear — Changelog

Dated log of documentation, project-foundation, and prototype changes.

Format: `YYYY-MM-DD — Summary`

---

## 2026-07-11 — Task PC-003 (Sprint 1.3): Continuous Enemy Spawning & Crowd Scalability Prototype (approved)

- Added authoritative active-enemy counting to `EnemyAI` (`ActiveCount`/`PeakActiveCount`, owned by `OnEnable`/`OnDisable`, correct regardless of death/external destroy/scene unload), increased the separation buffer 8→32 slots after proving the original truncates under dense clustering.
- Added a single `EnemySpawner` (timer-paced, 0.5s interval, steady-state replacement, never bulk-spawns, never exceeds target) and 8 round-robin boundary spawn points with a player safety-distance check.
- Added diagnostic-only crowd metrics to `DebugHud` (active/target/peak count, spawn interval, approximate FPS).
- Ran identical crowd tests at 20/50/100 enemies: 20 and 50 held cleanly with zero console errors and no separation-buffer truncation (**50 enemies is the validated Phase 1 crowd target**). At 100 enemies the population still held exactly at target with zero errors, but the separation buffer truncated (44 true neighbors vs. 32 slots) and several enemy pairs became effectively stacked — a geometric limit of the fixed-radius attack ring at high convergence, not a spawner or counting defect. Per the sprint's own criteria this does not fail the sprint since 50 remains fully stable.
- Object pooling remains deferred until profiling demonstrates a current need — `Instantiate`/`Destroy` used throughout all three tiers with no allocation-related errors or stalls.
- **Yoav explicitly approved Sprint 1.3** ("Sprint 1.3 Approved"). This approval covers Sprint 1.3 only — PC-003's overall Game Director Approval (spanning Sprint 1/1.1/1.2 too) remains unrecorded pending its own explicit sign-off. Not staged, committed, merged, or pushed. Task file: [Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md](Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md).

## 2026-07-11 — Task PC-003 (Sprint 1.2): Combat Space & Enemy Behaviour (implementation, awaiting review)

- Yoav's first full playtest flagged two highest-priority issues: enemies stacking/forming "trains" while chasing, and the arena being too small. No new gameplay systems, progression, XP, or Dash added.
- **Enemy separation**: `EnemyAI` now blends seek-toward-player with a boids-style separation force computed via `Physics.OverlapSphereNonAlloc` against nearby enemies only (1.75-unit radius, 8-slot static non-alloc buffer) — spatially bounded, so cost scales with local crowd density, not total enemy count. No NavMesh, no new physics layer. The existing "don't push into the player" guard was reworked to clamp the *resulting* position radially rather than the step size, since separation can steer enemies off the direct line to the player.
- **Arena expansion**: ground scaled 4x to 40×40 (from 10×10), boundary walls moved to ±20 to match. Two enemy spawn points relocated and two more added (four symmetric corners); enemy count raised from 2 to 4 to give separation something to demonstrate. Added 6 simple greybox obstacles (crates, rocks, short walls), kept low enough to preserve isometric camera sightlines.
- Verified separation via direct algorithm invocation and an independent pure-math replication of the formula, plus one full simulated run showing 4 enemies converge from their corners into a clean diamond formation (~2.83 units apart) stopping exactly at `attackRange` (2.00) from the player — no stacking, no single-file line.
- **Known limitation**: enemies do not navigate around the new obstacles (no obstacle-avoidance term was in scope for this task, and none was added) — a kinematic enemy on a straight path crossing an obstacle will currently clip through it visually.
- Encountered the same Play Mode frame-loop stall as Sprint 1.1 (Editor losing OS focus during a long automated session); this time it also caused the player to briefly tunnel through the ground once (recovered by resetting its position) and revealed that `Rigidbody.MovePosition` on a kinematic body only applies during a real physics step — worked around via direct/reflection-based verification. Not a gameplay defect.
- Not committed. Task file: [Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md](Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md). Awaiting review.

## 2026-07-11 — Task PC-003 (Sprint 1.1): Playability and Combat Feedback Fix (implementation, awaiting review)

- Yoav playtested Sprint 1 and reported movement/camera felt good but shooting was unclear, enemy contact felt like silent damage, and there wasn't enough visible feedback to understand combat. This pass fixes readability only — no new gameplay systems.
- Confirmed `Shoot` is bound to `<Mouse>/leftButton` (no change needed, verified against the Input Actions asset).
- Added `Health.Damaged` event so non-lethal hits can trigger reactions, not only death.
- `HitscanWeapon`: added a `LineRenderer` bullet trail and a muzzle-flash `GameObject`, both visible in the Game View — `Debug.DrawLine` alone was insufficient and is now a secondary aid only.
- `EnemyAI`: added a hit-reaction (color flash + scale pulse on taking damage) and an attack-tell (pulse on landing its melee hit); clamped its per-step movement so it can no longer overshoot past `attackRange` and push into the player.
- `DebugHud`: added always-visible on-screen control instructions (WASD/Mouse/LMB), larger color-coded player HP, and a brief red screen flash on player damage.
- Created `Assets/Art/Materials/VFX_Bright.mat` for the new trail/flash, wired into the Player prefab.
- Jump was explicitly not implemented. **[UNRESOLVED]** Jump versus Dash/Dodge movement option requires a future Game Director decision.
- Not committed. Task file: [Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md](Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md). Awaiting review.

## 2026-07-11 — Task PC-003: Sprint 1 First Playable Combat Prototype (implementation, awaiting review)

- Created branch `feature/sprint-1-first-playable` from `main`.
- Added `Aim` action (Vector2, mouse position) to `InputSystem_Actions.inputactions` — Player map is now Move/Aim/Shoot.
- Added `PlayerController` (`PointClear.Player`): Rigidbody-based WASD movement in `FixedUpdate` (frame-rate independent, diagonal normalized), mouse cursor projected onto the ground plane drives smooth rotation independently of movement direction.
- Added `Health` (`PointClear.Combat`): single concrete component (no interface) shared by player and enemy — `TakeDamage`, `Died` event, `ResetHealth`.
- Added `HitscanWeapon` (`PointClear.Weapons`): fires toward the player's live aim point (not the smoothed visual facing) on Shoot held, configurable fire rate and damage, infinite ammo, no reload, `Debug.DrawLine` shot feedback, single concrete weapon (no framework).
- Added `EnemyAI` (`PointClear.Enemies`): straight-line chase toward the player, stops at attack range, timed melee damage, self-destroys on `Health.Died` — no pathfinding (not required by this small open arena).
- Added `PlayerRespawn` (`PointClear.Player`), explicitly commented as prototype-only: disables controller/weapon/renderers on death, coroutine delay, teleports to a spawn point, resets health, re-enables.
- Added `IsometricCameraFollow` (`PointClear.Gameplay`): fixed-angle smooth follow via `SmoothDamp`, never rotates from input.
- Added `DebugHud` (`PointClear.Utilities`), marked prototype/debug-only: OnGUI player/enemy HP display.
- Built one Player prefab (Capsule, Rigidbody, Health, PlayerController, HitscanWeapon, PlayerRespawn, muzzle child) and one Enemy prefab (Capsule, Rigidbody, Health, EnemyAI); placed a player spawn point, two enemy spawn points, two enemy instances, and four boundary walls matching the existing 10x10 ground plane in PrototypeScene; configured the Main Camera with a fixed isometric rotation and the follow script.
- Verified zero compile errors and zero console warnings after fixing one deprecated-API warning (`FindObjectsByType` overload) during development.
- Playtested extensively in Play Mode: real simulated WASD input confirmed movement and diagonal normalization; mouse-aim ground-plane projection confirmed; hitscan weapon fire confirmed via direct invocation (correctly stopped at a wall, then dealt exact configured damage against a valid target); enemy chase/attack observed organically over an extended session; enemy death confirmed (`Destroy` on zero health); player death→disable→delayed respawn→full heal→re-enable confirmed, including multiple autonomous cycles with no soft-lock or errors.
- Sustained mouse-button-hold and precise cursor-position simulation proved unreliable once the Editor Game View has focus (real OS input overrides synthetic state) — a testing-tooling limitation noted in the task file, not a defect in the implementation.
- No XP, upgrades, build layers, loot, inventory, objectives, extraction, boss, multiplayer, networking, save system, leaderboards, or audio/VFX polish was added — out of scope for this sprint.
- Not committed. Task file: [Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md](Tasks/REVIEW/PC-003_sprint-1-first-playable-combat-prototype.md). Game Director approval intentionally not recorded — awaiting Yoav's explicit review and approval.

## 2026-07-11 — Sprint 0.5 / 0.5.1 approved: Unity production foundation established

- Yoav completed the Game Director playtest and gave explicit final approval for both PC-001 (Sprint 0.5 Production Foundation) and PC-002 (Sprint 0.5.1 Foundation Cleanup).
- Both task files moved `Tasks/REVIEW/` → `Tasks/DONE/`, Game Director Approval recorded, all Definition of Done checkboxes completed.
- Final validation re-confirmed before commit: zero Unity Console errors/warnings, Bootstrap → PrototypeScene Play Mode flow verified, `git diff --check` clean, all markdown links resolve.
- This closes Phase 0's Unity production-foundation milestone: folder structure, `Bootstrap`/`PrototypeScene` scenes, `SceneLoader` manager, and the trimmed Input Actions setup are now the committed baseline for Sprint 1 (First Playable Prototype).

## 2026-07-11 — Task PC-002: Sprint 0.5.1 Foundation Cleanup (implementation, awaiting review)

- Removed `GameManager` — its only real responsibility (triggering the initial scene load) had zero justification beyond spec compliance; folded that one line into `SceneLoader.Start()` so the Bootstrap → PrototypeScene flow keeps working with one fewer class.
- Removed `GameDataSO`, `IDamageable`, `IHealth`, `IWeapon` — all had zero consumers; per the stated philosophy ("architecture should emerge from real gameplay needs, not anticipated future needs"), these will be re-introduced in Sprint 1 alongside their first concrete implementation instead of pre-built now.
- Removed `SceneLoader.LoadSceneAsync` — unused, zero call sites, its `AsyncOperation` result was discarded.
- Kept `SceneLoader` — the project's only manager now — and its synchronous `LoadScene`.
- Fixed an orphaned "missing script" `MonoBehaviour` block left in `Bootstrap.unity` referencing the deleted `GameManager`, which the component browser wasn't surfacing after deletion; removed directly from the scene file and reloaded from disk.
- Re-verified zero compile errors/warnings and the Bootstrap → PrototypeScene Play Mode flow after all removals.
- `Systems/Interfaces/` and `ScriptableObjects/` folders retained (empty) — folder structure was out of scope for this cleanup.
- Correction (2026-07-11, same day): PC-001 had been incorrectly marked DONE with a recorded Game Director approval that was never explicitly given — moved back to `Tasks/REVIEW/` and the approval claim removed. Process rule reinforced: never record Game Director approval unless Yoav explicitly grants it. PC-002 also moved to `Tasks/REVIEW/` (was `IN_PROGRESS`). Both tasks await explicit final approval. Not committed.

## 2026-07-11 — Task PC-001: Sprint 0.5 Production Foundation (implementation, awaiting review)

- Created branch `feature/sprint-0.5-production-foundation` from `chore/project-foundation`.
- Created the production folder hierarchy under `Assets/`: `Art/{Animations,Materials,Models}`, `Audio/`, `Prefabs/{Player,Enemies,Environment,Weapons}`, `Scenes/{Bootstrap,Prototype}`, `Systems/{Core,Combat,Gameplay,Player,Weapons,Enemies,Managers,Interfaces,Utilities}`, `ScriptableObjects/`, `Resources/`.
- Created `Bootstrap.unity` (empty scene, holds a persistent `Managers` GameObject) and `PrototypeScene.unity` (Main Camera, Directional Light, Ground) — Bootstrap loads PrototypeScene on start via `SceneLoader`. Both added to Build Settings (Bootstrap index 0, PrototypeScene index 1); `SampleScene` removed from the build list but left on disk.
- Added `GameManager` and `SceneLoader` (namespace `PointClear.Managers`) — the only two managers created, per spec.
- Added `IDamageable`, `IHealth`, `IWeapon` (namespace `PointClear.Interfaces`) as pure contracts with no implementations, and `GameDataSO` (namespace `PointClear.ScriptableObjects`) as an abstract base ScriptableObject with only an identifying field — no gameplay data.
- Trimmed `InputSystem_Actions.inputactions` to a Move + Shoot placeholder Player action map (dropped Look/Interact/Crouch/Jump/Sprint/Previous/Next from the default Unity template) and relocated it to `Assets/Settings/`.
- Verified zero compiler errors and zero console warnings, including a Play Mode run of the Bootstrap → PrototypeScene flow.
- No gameplay, combat, enemies, UI, multiplayer, inventory, build system, save system, networking, or leaderboard code was added — out of scope for this sprint.
- Not committed. Task file: [Tasks/DONE/PC-001_sprint-0.5-production-foundation.md](Tasks/DONE/PC-001_sprint-0.5-production-foundation.md) (path updated 2026-07-11 as the task moved IN_PROGRESS → REVIEW → DONE). Awaiting Yoav's playtest and technical review.

## 2026-07-11 — Recorded Operation, Zone, Objective, and Build Layer decisions

- Added `DEC-009` through `DEC-013` to `DECISIONS.md`: Operation Definition, Connected Zones, Semi-Procedural Operations, Dynamic Objectives, and Layered Build System, each with approved statement, boundaries, and related unresolved questions. Extended the Unresolved Decisions list with the specific open sub-questions each decision leaves.
- Created `GLOSSARY.md` at the repo root: canonical, concise definitions for Operation, Zone, Objective, Dynamic Objective, Optional Objective, Deployment, Extraction, Run, Build, Build Layer, Mutation, Relic, Team Synergy, Temporary Operation Effect, Elite, Mini Boss, Boss, and Loadout. Resolves the "What is an Operation?" gap identified in `POINT_CLEAR_KNOWLEDGE_MAP.md`'s Decision Dependency Graph.
- Created `Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md`: the approved seven-layer build structure (Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, Temporary Operation Effects) and the interaction principle, with exact rules explicitly marked `[UNRESOLVED]`.
- Updated `Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md`: added Operation/Zone/Objective/Build Layer structure at sequence-appropriate detail; renamed its Golden Rule section to "Run Success Test" to disambiguate from `DESIGN_DNA.md`'s feature-evaluation Golden Rule; de-duplicated four sections (Emotional Curve, Combat, Party, Extraction) to reference `DESIGN_DNA.md` instead of restating its philosophy.
- Updated `VISION.md`: added Operation Zone structure, dynamic objectives, and layered build creation to the Intended Features list; added a pointer to DEC-009–DEC-013 and `GLOSSARY.md`.
- Updated `ROADMAP.md`: noted that "Multiplayer requirements definition" and "Initial scope definition" are now unblocked (Operation structure defined); Phase 0 remains **In Progress**, not marked complete.
- Updated `POINT_CLEAR_KNOWLEDGE_MAP.md`: marked the "What is an Operation?" node in the Decision Dependency Graph as answered; added Operation/Zone/Semi-Procedural/Dynamic Objective/Build Layer rows to the Knowledge Topic Registry; resolved five previously-flagged duplications (player emotional arc, combat feel, co-op composition, extraction, and the Operation split itself) in the Source of Truth Registry; left the Death/Failure philosophy duplication and the `DESIGN_DNA.md` Golden Rule rename explicitly open pending Game Director approval; added a Decision Model entry for `BUILD_SYSTEM_OVERVIEW.md`.
- Did not modify `DESIGN_DNA.md` — proposed renames (Death → "Death & Failure", Golden Rule → "Feature Evaluation") are reported, not applied.
- Did not create `Documentation/Systems/UPGRADE_BUILD_SYSTEM.md` or `Documentation/Multiplayer/MULTIPLAYER_REQUIREMENTS.md` — both remain correctly blocked/next-in-line per the Knowledge Map.
- No gameplay code, Unity scenes, assets, project settings, or packages were modified as part of this change. No commit was made.

## 2026-07-11 — Added Core Gameplay Loop document

- Created `Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md`: the full run-to-run gameplay blueprint (Lobby → Party → Loadout → Operation → Exploration → Combat → Upgrades → Elite/Mini Boss/Boss → Extraction → Results → Permanent Progression), the emotional curve a run should produce, and the design-rules checklist every gameplay system must satisfy.
- Cross-linked it from `ROADMAP.md` (Phase 0 "Core gameplay loop definition" goal) and `VISION.md`.
- Reviewed against `DESIGN_DNA.md` and `GAME_PILLARS.md` for consistency; no conflicts found.
- No gameplay code, Unity scenes, assets, project settings, or packages were modified as part of this change.

## 2026-07-11 — Added Game Design DNA document

- Created `DESIGN_DNA.md` at the repo root: the philosophical foundation behind `GAME_PILLARS.md` and `VISION.md`, covering purpose, mission, vision, the five design pillars, player journey, build philosophy, multiplayer philosophy, difficulty, death, extraction, leaderboards, seasons, what will never be built, design principles, the MVP rule, the golden rule, and the studio motto.
- Cross-linked `DESIGN_DNA.md` from `PROJECT_BIBLE.md`, `GAME_PILLARS.md`, and `VISION.md`.
- No gameplay code, Unity scenes, assets, project settings, or packages were modified as part of this change.

## 2026-07-10 — Task PC-000: Project Documentation Foundation

- Created branch `chore/project-foundation`.
- Established the repository documentation scaffolding at the repo root: `README.md`, `PROJECT_BIBLE.md`, `GAME_PILLARS.md`, `VISION.md`, `ROADMAP.md`, `DECISIONS.md`, `CHANGELOG.md`, `CONTRIBUTING.md`.
- Created `Documentation/` domain folders (Gameplay, Systems, Multiplayer, Progression, Leaderboards, Seasons, Classes, Skills, Weapons, Items, Enemies, Bosses, Maps, UI, Art, Audio, Technical) and the `Documentation/AI/` process docs (`AI_ONBOARDING.md`, `CLAUDE_RULES.md`, `REVIEW_CHECKLIST.md`, `SESSION_START.md`, `TASK_WORKFLOW.md`).
- Created `Tasks/` status folders (TODO, IN_PROGRESS, REVIEW, DONE, ARCHIVED).
- Created `Templates/` (Task, System, Decision, Class, Skill, Weapon, Enemy, Boss, Season templates).
- Recorded initial accepted decisions DEC-001 through DEC-008 and the list of currently unresolved decisions in `DECISIONS.md`.
- Recorded Phase 0 / Phase 1 / Phase 2 roadmap intent in `ROADMAP.md`. Phase 0 is marked **In Progress**, not completed.
- No gameplay code, Unity scenes, assets, project settings, or packages were modified as part of this task.
- Pre-existing uncommitted changes to `Packages/manifest.json` and `Packages/packages-lock.json` (MCP for Unity package) were preserved, not committed, and not otherwise touched.

## Related Documents

- [ROADMAP.md](ROADMAP.md)
- [DECISIONS.md](DECISIONS.md)
