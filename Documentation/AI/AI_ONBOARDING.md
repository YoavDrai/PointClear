# AI Onboarding — Point Clear

This document orients any AI collaborator (Claude, ChatGPT, or others) joining work on Point Clear. Read this alongside [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md), [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md), [GAME_PILLARS.md](../../GAME_PILLARS.md), [VISION.md](../../VISION.md), [ROADMAP.md](../../ROADMAP.md), and [DECISIONS.md](../../DECISIONS.md).

## What Point Clear Is

**[APPROVED FACT]** Point Clear is an original, multiplayer-first, persistent, seasonal, cooperative **top-down** Action ARPG for 1–4 players, built in Unity 6000.5.3f1 with URP and the New Input System. The camera is a high top-down gameplay camera with a slight perspective tilt where useful — **not** isometric (DEC-003). See [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) for the full identity and design-principles statement. **Current state:** Milestone 1 (Arena Loop Validation) is complete and playtest-approved; Milestone 2 (Visual Foundation) is in pre-production — see [ROADMAP.md](../../ROADMAP.md) for the live status (roadmap production Phase 0/1/2 numbering is a separate axis).

Point Clear must be described by its own systems and identity — do not use other commercial games as implementation specifications, and do not copy another game's content, terminology, assets, progression structure, or UI.

## What Exists Right Now

- A Unity project at the repository root (URP + New Input System).
- A **single-player greybox prototype**: combat, progression (XP / Level / Skill Points), the Operation secure-or-lose loop, Active Skills + Passives, a Weapon Module, the tuned four-enemy roster (Walker / Charger / Surrounder / Empowerer), and a complete greybox front-end player journey. **No networking, persistence, or production art yet.** See [ROADMAP.md](../../ROADMAP.md) and [CHANGELOG.md](../../CHANGELOG.md) for the authoritative, live inventory — this list is a pointer, not a second source of truth.
- A documentation foundation (this folder tree) establishing how work is proposed, tracked, and completed.

Do not assume anything beyond what is recorded in the documents above and in [CHANGELOG.md](../../CHANGELOG.md) has been built or decided.

## Who Does What

See [PROJECT_BIBLE.md § Team Roles](../../PROJECT_BIBLE.md#2-team-roles):

- **Yoav** — Game Director: vision, direction, priorities, playtesting, final approval.
- **ChatGPT** — Technical Director: system design, architecture, task specs, review.
- **Claude** — Implementation: builds only what has been approved, within an authorized task's scope.

## Rules Before You Touch Anything

Read [CLAUDE_RULES.md](CLAUDE_RULES.md) in full before making any changes. In short: implement only the active, approved task; never expand scope; never install/remove packages, rename folders, delete assets, or change project settings without explicit authorization; report everything you do.

## Where to Go Next

- Starting a session? Follow [SESSION_START.md](SESSION_START.md).
- Picking up a task? Follow [TASK_WORKFLOW.md](TASK_WORKFLOW.md) and use [Templates/TASK_TEMPLATE.md](../../Templates/TASK_TEMPLATE.md).
- Finishing a task? Follow [REVIEW_CHECKLIST.md](REVIEW_CHECKLIST.md).

## Related Documents

- [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md)
- [GAME_PILLARS.md](../../GAME_PILLARS.md)
- [VISION.md](../../VISION.md)
- [ROADMAP.md](../../ROADMAP.md)
- [DECISIONS.md](../../DECISIONS.md)
- [CLAUDE_RULES.md](CLAUDE_RULES.md)
- [SESSION_START.md](SESSION_START.md)
- [TASK_WORKFLOW.md](TASK_WORKFLOW.md)
- [REVIEW_CHECKLIST.md](REVIEW_CHECKLIST.md)
