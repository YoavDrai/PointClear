# Point Clear — Changelog

Dated log of documentation and project-foundation changes. This is not a gameplay changelog — gameplay systems do not exist yet.

Format: `YYYY-MM-DD — Summary`

---

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
