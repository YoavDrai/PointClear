# Review Checklist — Point Clear

Use this checklist during the "Technical review" and "Approval" steps of [TASK_WORKFLOW.md](TASK_WORKFLOW.md), and before marking any task DONE per [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md).

## Scope

- [ ] Implementation matches the task's stated **Objective** and **Requirements** exactly.
- [ ] Nothing outside **Files Allowed to Edit** was touched.
- [ ] Nothing listed under **Out of Scope** was implemented.
- [ ] No packages were installed or removed unless explicitly authorized in the task.
- [ ] No project settings, folder renames, or asset deletions occurred unless explicitly authorized in the task.

## Correctness

- [ ] Acceptance criteria all pass.
- [ ] Unity Console has no new compiler errors introduced by this task.
- [ ] Required testing (as defined in the task's Test Procedure) was completed.

## Engineering Principles

- [ ] Code is clean, readable, and uses small, focused components.
- [ ] No unnecessary abstractions were introduced.
- [ ] No hardcoded gameplay values where configuration would be appropriate.
- [ ] Networked state (if any) has clear ownership; competitive/leaderboard-sensitive logic remains server-authoritative in design intent even if networking is not yet implemented.
- [ ] Design is performance-aware for large enemy counts where relevant.

## Identity

- [ ] No content, terminology, assets, progression structure, or UI was copied from another commercial game.

## Documentation

- [ ] Relevant documentation under [Documentation/](../../Documentation/) or root-level docs was updated to reflect what changed.
- [ ] [CHANGELOG.md](../../CHANGELOG.md) has an entry for this task.
- [ ] Any new decision is recorded in [DECISIONS.md](../../DECISIONS.md) using [Templates/DECISION_TEMPLATE.md](../../Templates/DECISION_TEMPLATE.md).

## Sign-off

- [ ] Technical review completed (ChatGPT).
- [ ] Game Director approval given (Yoav).
- [ ] Task file moved to [Tasks/DONE/](../../Tasks/DONE/).

## Related Documents

- [TASK_WORKFLOW.md](TASK_WORKFLOW.md)
- [CLAUDE_RULES.md](CLAUDE_RULES.md)
- [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md)
