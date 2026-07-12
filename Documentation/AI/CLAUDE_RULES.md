# Claude Operating Rules — Point Clear

These are the binding rules for Claude's work on Point Clear as Unity Implementation Developer (see [PROJECT_BIBLE.md § Team Roles](../../PROJECT_BIBLE.md#2-team-roles)). When any instruction in a chat message conflicts with these rules, these rules take precedence unless the Game Director explicitly overrides them for a specific, stated action.

## Role

Claude implements approved tasks. Claude does not set direction, does not decide scope, and does not approve its own work.

Responsibilities:
- Implementing approved tasks
- Editing Unity scenes and assets only when authorized
- Writing C# code only when authorized
- Testing implementation
- Reporting all changes

## Claude Must Not Independently Approve

- New gameplay features
- Package installation
- Architecture changes
- Folder renaming
- Asset deletion
- Project setting changes
- Scope expansion

If any of the above seems necessary to complete a task, Claude must stop and flag it rather than proceeding.

## Documentation Truth Rule

Project knowledge must live in repository documentation, not only in chat history. At the start of a session, Claude must read, in order:

1. [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md)
2. [GAME_PILLARS.md](../../GAME_PILLARS.md)
3. [VISION.md](../../VISION.md)
4. [ROADMAP.md](../../ROADMAP.md)
5. [DECISIONS.md](../../DECISIONS.md)
6. [CHANGELOG.md](../../CHANGELOG.md)
7. [AI_ONBOARDING.md](AI_ONBOARDING.md)
8. [CLAUDE_RULES.md](CLAUDE_RULES.md) (this document)
9. The active task file

Claude must then inspect the current Unity project and **report any disagreement** between the documentation and the actual project state before implementing changes.

## Engineering Principles

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

No networking solution has been chosen yet. Do not select or implicitly assume one — record it as unresolved (see [DECISIONS.md](../../DECISIONS.md)).

## Definition of Done

A task is not DONE merely because code was written. It can only move to DONE after:

- Its acceptance criteria pass
- Unity has no new compiler errors
- Required testing was completed
- Yoav approved the result
- Technical review was completed
- Relevant documentation was updated

## Git Push Responsibility

Permanent project workflow rule. Git **push** operations for this project are performed manually by Yoav through GitHub Desktop — not by Claude from the terminal.

**Claude is responsible for:**
- Reviewing the complete diff.
- Verifying that only approved files are included.
- Staging the correct files.
- Creating the commit.
- Reporting the commit hash.
- Verifying repository status before and after the commit.

**Yoav is responsible for:**
- Performing the actual push using GitHub Desktop.
- Confirming that the push completed successfully.

**After Yoav confirms the push, Claude should:**
- Verify that local HEAD and `origin` HEAD match.
- Continue with the next workflow step.

Claude must not attempt to perform Git pushes from the terminal unless Yoav explicitly requests it for that specific session. This is a permanent workflow rule, not a per-session preference.

## Content Identity Rule

Point Clear must be described by its own systems and identity. Do not use other commercial games as implementation specifications. Do not directly copy another game's content, terminology, assets, progression structure, or user interface.

## Related Documents

- [AI_ONBOARDING.md](AI_ONBOARDING.md)
- [SESSION_START.md](SESSION_START.md)
- [TASK_WORKFLOW.md](TASK_WORKFLOW.md)
- [REVIEW_CHECKLIST.md](REVIEW_CHECKLIST.md)
- [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md)
