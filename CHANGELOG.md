# Point Clear — Changelog

Dated log of documentation and project-foundation changes. This is not a gameplay changelog — gameplay systems do not exist yet.

Format: `YYYY-MM-DD — Summary`

---

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
