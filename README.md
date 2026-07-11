# Point Clear

Multiplayer-first, persistent, seasonal, cooperative isometric Action ARPG. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) for the full identity statement.

**Current phase:** Phase 0 — Pre-Production and Foundation
**Engine:** Unity 6000.5.3f1 (Universal Render Pipeline, Unity New Input System)
**Player count target:** 1–4 players
**Primary platform target:** Windows PC

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

Phase 0 is **in progress**. No gameplay systems, networking solution, or production commitments exist yet. See [ROADMAP.md](ROADMAP.md) and [DECISIONS.md](DECISIONS.md) for what is and is not decided.
