# Point Clear — Project Bible

Status legend used throughout this document and all linked documentation: **[APPROVED FACT]**, **[ASSUMPTION]**, **[PROPOSAL]**, **[UNRESOLVED]**.

This document is the top-level reference for what Point Clear is, who is responsible for what, and how work moves from idea to done. For the project's permanent design principles, see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md). See also: [DESIGN_DNA.md](DESIGN_DNA.md), [GAME_PILLARS.md](GAME_PILLARS.md), [VISION.md](VISION.md), [ROADMAP.md](ROADMAP.md), [DECISIONS.md](DECISIONS.md).

---

## 1. Project Identity

**[APPROVED FACT]**

| Field | Value |
|---|---|
| Project name | Point Clear |
| Current phase | Phase 0 — Pre-Production and Foundation |
| Genre | Multiplayer-first, persistent, seasonal, cooperative isometric Action ARPG (see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Identity) |
| Player count | 1–4 players |
| Primary platform target | Windows PC |
| Engine | Unity 6000.5.3f1 |
| Render pipeline | Universal Render Pipeline (URP) |
| Input | Unity New Input System only |

Point Clear must be described by its own systems and identity. Do not use other commercial games as implementation specifications. Do not directly copy another game's content, terminology, assets, progression structure, or user interface.

---

## 2. Team Roles

**[APPROVED FACT]**

### Yoav — Founder and Game Director
- Product vision
- Gameplay direction
- Priorities
- Playtesting
- Final approval

### ChatGPT — Technical Director and Systems Designer
- System design
- Technical planning
- Architecture proposals
- Task specifications
- Review
- Documentation planning
- Identifying risks and inconsistencies

### Claude — Unity Implementation Developer
- Implementing approved tasks
- Editing Unity scenes and assets only when authorized
- Writing C# code only when authorized
- Testing implementation
- Reporting all changes

**Claude must not independently approve:**
- New gameplay features
- Package installation
- Architecture changes
- Folder renaming
- Asset deletion
- Project setting changes
- Scope expansion

See [Documentation/AI/CLAUDE_RULES.md](Documentation/AI/CLAUDE_RULES.md) for the full operating rules Claude follows in this project.

---

## 3. Workflow

**[APPROVED FACT]**

Every feature follows this process:

1. Idea
2. Discussion
3. Game Director approval
4. Design and technical specification
5. Task creation
6. Implementation by Claude
7. Unity playtest by Yoav
8. Technical review
9. Corrections if required
10. Documentation update
11. Approval
12. Task completion

A task is **not** DONE merely because code was written. A task can only move to DONE after:

- Its acceptance criteria pass
- Unity has no new compiler errors
- Required testing was completed
- Yoav approved the result
- Technical review was completed
- Relevant documentation was updated

Roadmaps and sprint sequences are research plans, not fixed production schedules. Each cluster of work exists to answer a specific question; if playtesting does not confirm the answer, sprints may be repeated, expanded, split, or reordered — returning to earlier work is a valid outcome, not a failure of process. See [ROADMAP.md](ROADMAP.md) for this principle applied to the current roadmap.

See [Documentation/AI/TASK_WORKFLOW.md](Documentation/AI/TASK_WORKFLOW.md) and [Templates/TASK_TEMPLATE.md](Templates/TASK_TEMPLATE.md).

---

## 4. Documentation Truth Rule

**[APPROVED FACT]**

Project knowledge must live in repository documentation, not only in chat history.

At the beginning of a future session, Claude must read:

1. [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
2. [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
3. [GAME_PILLARS.md](GAME_PILLARS.md)
4. [VISION.md](VISION.md)
5. [ROADMAP.md](ROADMAP.md)
6. [DECISIONS.md](DECISIONS.md)
7. [CHANGELOG.md](CHANGELOG.md)
8. [Documentation/AI/AI_ONBOARDING.md](Documentation/AI/AI_ONBOARDING.md)
9. [Documentation/AI/CLAUDE_RULES.md](Documentation/AI/CLAUDE_RULES.md)
10. The active task file

Claude must then inspect the current Unity project and report any disagreement between the documentation and the actual project state before implementing changes.

---

## 5. Engineering Principles

**[APPROVED FACT]**

- Multiplayer-first design
- Modular systems
- Data-driven configuration where appropriate
- Clear ownership of networked state
- Server-authoritative design for competitive and leaderboard-sensitive systems
- Clean and readable C#
- Small focused components
- No unnecessary abstractions
- No hardcoded gameplay values when configuration is appropriate
- Performance-aware design for large enemy counts
- No feature implementation outside the approved task
- No hidden or undocumented project changes

**[UNRESOLVED]** No networking solution has been chosen yet. The networking package and architecture selection is an open decision — see [DECISIONS.md](DECISIONS.md).

---

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [DESIGN_DNA.md](DESIGN_DNA.md)
- [GAME_PILLARS.md](GAME_PILLARS.md)
- [VISION.md](VISION.md)
- [ROADMAP.md](ROADMAP.md)
- [DECISIONS.md](DECISIONS.md)
- [CHANGELOG.md](CHANGELOG.md)
- [CONTRIBUTING.md](CONTRIBUTING.md)
- [Documentation/AI/AI_ONBOARDING.md](Documentation/AI/AI_ONBOARDING.md)
- [Documentation/AI/CLAUDE_RULES.md](Documentation/AI/CLAUDE_RULES.md)
