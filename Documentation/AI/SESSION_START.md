# Session Start — Reusable Prompt for Future Claude Sessions

Use this prompt verbatim (or paste its contents) at the start of any new Claude session working on Point Clear.

---

## Prompt

> Before doing anything else, follow the Point Clear session-start procedure:
>
> 1. **Read the required documents, in order:**
>    - `PROJECT_BIBLE.md`
>    - `GAME_PILLARS.md`
>    - `VISION.md`
>    - `ROADMAP.md`
>    - `DECISIONS.md`
>    - `CHANGELOG.md`
>    - `Documentation/AI/AI_ONBOARDING.md`
>    - `Documentation/AI/CLAUDE_RULES.md`
>    - The active task file (check `Tasks/IN_PROGRESS/` first, then `Tasks/TODO/` if none is in progress)
>
> 2. **Inspect the repository:** confirm the current file structure matches what the documentation describes. Note any drift.
>
> 3. **Inspect the active Unity project and scene:** check the current scene, hierarchy, build settings, installed packages, render pipeline, and input system configuration.
>
> 4. **Check the Console:** report any existing compiler errors or warnings before making any change.
>
> 5. **Check Git status and current branch:** report the branch name and any uncommitted or staged changes. Do not assume a clean working tree.
>
> 6. **Identify the current task:** state which task file is active, its status, and its stated scope (Objective, Requirements, Files Allowed/Forbidden to Edit, Out of Scope).
>
> 7. **Compare documentation against implementation:** explicitly report any disagreement between what the docs claim and what the project/repository/Unity project actually contain.
>
> 8. **Report your understanding:** summarize the above findings before proposing or taking any action.
>
> 9. **Make no modifications until explicitly authorized.** Do not edit files, scenes, assets, settings, or packages, and do not install/remove packages, rename folders, or delete assets, until the Game Director or Technical Director explicitly authorizes the specific action.

---

## Why This Exists

Per the Documentation Truth Rule ([PROJECT_BIBLE.md § 4](../../PROJECT_BIBLE.md#4-documentation-truth-rule) / DEC-007 in [DECISIONS.md](../../DECISIONS.md)), project knowledge must live in repository documentation, not only in chat history. This prompt ensures every new session re-establishes ground truth from the repository and the live Unity project rather than from assumptions or prior chat context.

## Related Documents

- [AI_ONBOARDING.md](AI_ONBOARDING.md)
- [CLAUDE_RULES.md](CLAUDE_RULES.md)
- [TASK_WORKFLOW.md](TASK_WORKFLOW.md)
- [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md)
