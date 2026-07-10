# Task Workflow — Point Clear

How a feature moves from idea to completed task. See also [CONTRIBUTING.md](../../CONTRIBUTING.md), [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md), [REVIEW_CHECKLIST.md](REVIEW_CHECKLIST.md).

## The 12-Step Process

1. **Idea** — raised by anyone, in any form.
2. **Discussion** — clarified with the Game Director and Technical Director.
3. **Game Director approval** — Yoav approves pursuing the idea.
4. **Design and technical specification** — ChatGPT (Technical Director) produces the spec.
5. **Task creation** — a task file is created from [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md) in [Tasks/TODO/](../../Tasks/TODO/).
6. **Implementation by Claude** — the task file moves to [Tasks/IN_PROGRESS/](../../Tasks/IN_PROGRESS/) while Claude implements strictly within the task's stated scope.
7. **Unity playtest by Yoav** — the implementation is playtested in the editor/build.
8. **Technical review** — ChatGPT reviews the implementation against the spec and acceptance criteria; the task file moves to [Tasks/REVIEW/](../../Tasks/REVIEW/).
9. **Corrections if required** — loop back to step 6 as needed.
10. **Documentation update** — any relevant document under [Documentation/](../../Documentation/) or root-level docs are updated to reflect the change.
11. **Approval** — Game Director gives final approval.
12. **Task completion** — the task file moves to [Tasks/DONE/](../../Tasks/DONE/).

Tasks that are abandoned or superseded move to [Tasks/ARCHIVED/](../../Tasks/ARCHIVED/) instead of DONE.

## Definition of Done

A task is not DONE merely because code was written. All of the following must be true:

- [ ] Acceptance criteria pass
- [ ] Unity has no new compiler errors
- [ ] Required testing was completed
- [ ] Yoav approved the result
- [ ] Technical review was completed
- [ ] Relevant documentation was updated

## Task File Location by Status

| Status | Folder |
|---|---|
| Not started | [Tasks/TODO/](../../Tasks/TODO/) |
| Actively being implemented | [Tasks/IN_PROGRESS/](../../Tasks/IN_PROGRESS/) |
| Awaiting/undergoing review | [Tasks/REVIEW/](../../Tasks/REVIEW/) |
| Complete | [Tasks/DONE/](../../Tasks/DONE/) |
| Abandoned or superseded | [Tasks/ARCHIVED/](../../Tasks/ARCHIVED/) |

## Related Documents

- [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md)
- [REVIEW_CHECKLIST.md](REVIEW_CHECKLIST.md)
- [CLAUDE_RULES.md](CLAUDE_RULES.md)
- [CONTRIBUTING.md](../../CONTRIBUTING.md)
