# Point Clear

Multiplayer-first, persistent, seasonal, cooperative top-down Action ARPG. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) for the full identity statement.

**Current state:** Milestone 1 (Arena Loop Validation) — complete & playtest-approved · Milestone 2 (Visual Foundation) — in pre-production. (Roadmap production phases 0/1/2 and Sprint 2.x task numbering are unchanged — see [ROADMAP.md](ROADMAP.md).)
**Camera:** High top-down (slight perspective tilt where useful) — not isometric (DEC-003)
**Engine:** Unity 6000.5.3f1 (Universal Render Pipeline, Unity New Input System)
**Player count target:** 1–4 players
**Release platform:** Windows PC · **Technical scalability target:** Mobile 30 FPS on a future approved reference device (performance-discipline only; mobile release not approved — see [DECISIONS.md](DECISIONS.md) DEC-039)

This repository root **is** the Unity project root (`Assets/`, `Packages/`, `ProjectSettings/`, etc. live here alongside the project documentation below).

## Start Here

| Document | Purpose |
|---|---|
| [PROJECT_BIBLE.md](PROJECT_BIBLE.md) | Single source of truth: identity, team roles, workflow, engineering principles |
| [GAME_PILLARS.md](GAME_PILLARS.md) | The core pillars every design and implementation decision must serve |
| [VISION.md](VISION.md) | Current game vision in full |
| [ROADMAP.md](ROADMAP.md) | Phase plan and current status |
| [DECISIONS.md](DECISIONS.md) | Accepted decisions and unresolved open questions |
| [CHANGELOG.md](CHANGELOG.md) | Dated log of documentation and project changes |
| [CONTRIBUTING.md](CONTRIBUTING.md) | How work gets proposed, implemented, reviewed, and completed |

## Project Dashboard

A local, read-only web dashboard visualizes the project's current state — phase, cluster, sprint, tasks, decisions, changelog, and document relationships — derived live from the markdown files below (never a second source of truth). Run it with `cd Dashboard && npm install && npm run dev`; see [Dashboard/README.md](Dashboard/README.md).

## For AI Sessions

Any AI assistant (Claude or otherwise) working on this project must read [Documentation/AI/SESSION_START.md](Documentation/AI/SESSION_START.md) before making any changes.

## Repository Layout

```
PointClear/ (repo root = Unity project root)
├── Assets/, Packages/, ProjectSettings/, ...   (Unity project — see Unity docs, not covered here)
├── README.md, PROJECT_BIBLE.md, GAME_PILLARS.md, VISION.md,
│   ROADMAP.md, DECISIONS.md, CHANGELOG.md, CONTRIBUTING.md
├── Documentation/      Design & technical documentation by domain
│   └── AI/              AI collaborator onboarding and process docs
├── Tasks/              Task files by status (TODO, IN_PROGRESS, REVIEW, DONE, ARCHIVED)
├── Templates/          Reusable templates for tasks, systems, decisions, and content types
└── Dashboard/          Local read-only web dashboard over the docs above (Node tool, not part of the Unity build)
```

## Status

**Milestone 1 — Arena Loop Validation is complete and playtest-approved; Milestone 2 — Visual Foundation is in pre-production.** (Roadmap production phases 0/1/2 remain the phasing model — see [ROADMAP.md](ROADMAP.md).) Single-player **greybox prototype** systems exist and are being iterated — combat, progression (XP / Level / Skill Points), the Operation loop (secure-or-lose mission risk), Active Skills + Passives, a Weapon Module, the tuned four-enemy roster (Walker / Charger / Surrounder / Empowerer), and a **first complete front-end player journey** (Main Menu → Character Creation → Starting Direction → World Map → Operation → Results, PC-015). Networking, persistence, production art, and production commitments do **not** yet exist. The visual direction is captured in the frozen [Art Bible v1.0](Documentation/Vision/ART_BIBLE.md) and the approved [Visual Benchmark Plan](Documentation/Vision/POINT_CLEAR_VISUAL_BENCHMARK_PLAN.md); **no visual production (Phase 0) has begun.** See [ROADMAP.md](ROADMAP.md) and [DECISIONS.md](DECISIONS.md) for what is and is not decided.

**Current proving target (DEC-034/035/036):** the prototype is a deliberately compressed version of the full game, focused on proving one **Arena gameplay loop** is fun enough that players immediately want another Run — `Character → Skill allocation → Enter Arena → Fight → Extraction → Results → Re-enter → Repeat`, with **every 5th Run a Boss Run** and **gentle onboarding that escalates each Cycle**, the Difficulty Tier rising per Cycle — *before* expanding into Zones, multiple Operations/Arenas, story, or world progression (which remain the intact long-term vision). The Arena is bounded; the sequence of Runs/Tiers is infinite. Prototype terms **Arena/Run/Cycle/Difficulty Tier** do not replace the long-term **Map/Zone/Operation**. See [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) for the split.
