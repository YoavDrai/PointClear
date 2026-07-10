# Contributing to Point Clear

This document describes how work is proposed, implemented, reviewed, and completed on Point Clear. It applies equally to human and AI contributors. See also: [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [Documentation/AI/TASK_WORKFLOW.md](Documentation/AI/TASK_WORKFLOW.md).

## Roles

- **Yoav — Founder and Game Director**: product vision, gameplay direction, priorities, playtesting, final approval.
- **ChatGPT — Technical Director and Systems Designer**: system design, technical planning, architecture proposals, task specifications, review, documentation planning, risk identification.
- **Claude — Unity Implementation Developer**: implements approved tasks, edits Unity scenes/assets/C# only when authorized, tests implementation, reports all changes.

Full role detail: [PROJECT_BIBLE.md](PROJECT_BIBLE.md#2-team-roles).

## The Feature Workflow

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

A task is not DONE merely because code was written — see the Definition of Done in [Templates/TASK_TEMPLATE.md](Templates/TASK_TEMPLATE.md) and [Documentation/AI/TASK_WORKFLOW.md](Documentation/AI/TASK_WORKFLOW.md).

## Task Lifecycle

Tasks move through folders in [Tasks/](Tasks/) as their status changes:

`Tasks/TODO/` → `Tasks/IN_PROGRESS/` → `Tasks/REVIEW/` → `Tasks/DONE/` (or `Tasks/ARCHIVED/` if abandoned/superseded)

Every task file is created from [Templates/TASK_TEMPLATE.md](Templates/TASK_TEMPLATE.md).

## Boundaries for AI Contributors

An AI contributor (Claude) must not independently approve:

- New gameplay features
- Package installation
- Architecture changes
- Folder renaming
- Asset deletion
- Project setting changes
- Scope expansion

Full rules: [Documentation/AI/CLAUDE_RULES.md](Documentation/AI/CLAUDE_RULES.md).

## Documentation Expectations

- Repository documentation is the persistent source of project context (DEC-007, [DECISIONS.md](DECISIONS.md)).
- Any change to accepted project facts must be reflected in the relevant document and logged in [CHANGELOG.md](CHANGELOG.md).
- Use the status labels **[APPROVED FACT]**, **[ASSUMPTION]**, **[PROPOSAL]**, **[UNRESOLVED]** consistently so readers (human or AI) can tell what is settled from what is not.

## Related Documents

- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [Documentation/AI/TASK_WORKFLOW.md](Documentation/AI/TASK_WORKFLOW.md)
- [Documentation/AI/REVIEW_CHECKLIST.md](Documentation/AI/REVIEW_CHECKLIST.md)
- [Templates/TASK_TEMPLATE.md](Templates/TASK_TEMPLATE.md)
