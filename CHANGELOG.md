# Point Clear — Changelog

Dated log of documentation and project-foundation changes. This is not a gameplay changelog — gameplay systems do not exist yet.

Format: `YYYY-MM-DD — Summary`

---

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
