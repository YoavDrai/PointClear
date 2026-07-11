## Task ID
PC-006

## Title
Point Clear Project Dashboard — Documentation & Progress Hub (v1, read-only)

## Status
DONE (2026-07-11) — approved by Yoav after a second manual review (live updates and the Documentation Markdown reader both verified). One Definition of Done item remains unchecked: no separate Technical Director review occurred (no reviewer other than Yoav was present); moved to DONE on Yoav's explicit authority as Game Director, per his direct instruction — flagged, not silently checked. See Game Director Approval and Definition of Done, below.

## Priority
Medium

## Owner
Claude

## Reviewer
Yoav (Game Director)

## Dependencies
- None — reads existing repository documentation only, no gameplay or Unity dependency

## Related Documents
- [ROADMAP.md](../../ROADMAP.md)
- [DECISIONS.md](../../DECISIONS.md)
- [CHANGELOG.md](../../CHANGELOG.md)
- [POINT_CLEAR_KNOWLEDGE_MAP.md](../../POINT_CLEAR_KNOWLEDGE_MAP.md)
- [Documentation/AI/REVIEW_CHECKLIST.md](../../Documentation/AI/REVIEW_CHECKLIST.md)

## Objective

Build a local, read-only web dashboard that acts as the project's permanent control center — a single place to see current phase/cluster/sprint, task board state, recent decisions, changelog activity, and document relationships — derived entirely from the repository's own markdown and git state. The dashboard must never become a second source of truth: markdown remains canonical, the dashboard only reads and visualizes it.

## Background

Approved via an architecture review and visual mockup presented in chat on 2026-07-11 (not a task file at the time — this task formalizes and tracks that approved work going forward, consistent with this project's practice of tracking all work, including non-gameplay tooling, through `Tasks/`). The mockup is not part of the shipped implementation; it validated visual direction only.

## Scope

**In scope (v1):**
- A local Vite + React + TypeScript app in `Dashboard/`, running via `cd Dashboard && npm install && npm run dev`.
- A backend (Vite plugin, same process/port) that parses `ROADMAP.md`, `DECISIONS.md`, `CHANGELOG.md`, `Tasks/**/*.md`, and `POINT_CLEAR_KNOWLEDGE_MAP.md` into a single in-memory `ProjectSnapshot`, rebuilt on file change and pushed to connected browser tabs over a WebSocket (no polling, no manual refresh).
- Read-only git status (branch, HEAD, ahead/behind, modified/untracked files).
- Pages: Main Dashboard, Roadmap, Tasks (board), Decisions (timeline), Knowledge Map (graph and tree views), Documentation (browsable tree).
- **Project Health**: a computed summary (parse-failure count, tasks blocked/in-review, unresolved-decision count, documentation coverage) surfaced on the Main Dashboard.
- **Global Search**: a single search entry point querying across parsed sprints, tasks, decisions, and changelog entries.
- **Activity Timeline**: a merged, chronologically-sorted feed of changelog entries and decisions, shown on the Main Dashboard.
- **Knowledge Map — Graph and Tree views**: two view modes over the same data (parsed from `POINT_CLEAR_KNOWLEDGE_MAP.md`'s own tables, not hand-authored), toggled by the user.
- Parser unit tests (Vitest) against fixture markdown for every parser.

**Explicitly out of scope for v1 (read-only boundary):**
- No writing to any markdown file, ever. No editing Tasks, no Decision creation, no Documentation editing.
- No git write operations of any kind (no commit, push, add, checkout, branch creation) — the git module only ever calls read commands (`status`, `log`, `rev-parse`, `branch --show-current`), and no write command is even implemented, not merely unused.
- No database, no cloud service, no external network calls.
- No authentication/multi-user concerns (single local user, localhost only).
- No moving Task files between folders from the UI (folder = status is read-only in v1).

## Requirements

- `Dashboard/` scaffold: `package.json`, `vite.config.ts`, `tsconfig.json`, `index.html`, `src/`, `server/`, `README.md`.
- Parsers built on `unified`/`remark` (AST-based, not regex-over-lines) for Roadmap, Decisions, Tasks, Changelog, and the Knowledge Map's registry tables.
- `chokidar` file watcher scoped to root `*.md`, `Documentation/**/*.md`, `Tasks/**/*.md` only — explicitly excluding `Assets/`, `Library/`, `Temp/`, `Logs/`, `node_modules/`.
- WebSocket push of the rebuilt snapshot on every debounced file-change batch.
- "Current sprint" derived from `Tasks/` folder contents (non-archived task referencing the highest sprint number, else the next unclaimed sprint in `ROADMAP.md`'s sequence) — labeled as inferred in the UI, not presented as fact.
- Unparseable sections must render as a visible "couldn't parse this" state, never blank or silently wrong data.

## Acceptance Criteria

- [x] `cd Dashboard && npm install && npm run dev` starts the app with no errors and serves it on localhost — verified twice, including after bug fixes
- [x] Main Dashboard fields all confirmed present in the live `/api/snapshot` response and wired into `MainDashboard.tsx`
- [x] Project Health section — confirmed computed (`parseFailures: 0, tasksInReview: 1, unresolvedDecisions: 20, documentationWritten: 19`) against live data, not hardcoded
- [~] Global Search — implemented and code-reviewed against the live snapshot shape; not clicked through interactively in a browser this session
- [x] Activity Timeline — confirmed live in-browser; merged changelog feed renders and updates
- [x] Roadmap page — confirmed live: 3 clusters (A/B/C), 11 sprints (2.3–2.13), Cluster A's gate correctly attached and readable
- [x] Tasks page — confirmed live: 6 tasks, columns match actual `Tasks/` folder contents exactly (TODO: PC-005, IN_PROGRESS: PC-006, REVIEW: PC-003, DONE: PC-001/002/004)
- [x] Decisions page — confirmed live: 20 decisions, 20 unresolved items, both parsed from `DECISIONS.md`
- [~] Knowledge Map — confirmed 50 registry rows parsed live from `POINT_CLEAR_KNOWLEDGE_MAP.md`; Graph/Tree toggle implemented and code-reviewed, not clicked through interactively
- [x] Documentation page — **fixed & verified in-browser (2nd review pass):** 19 docs render as clickable buttons; clicking one loads and renders its Markdown (headings, lists, links, tables) via a read-only, path-validated `/api/doc` endpoint. Traversal / non-`.md` / absolute-path requests all rejected.
- [x] Live file-edit-to-UI-refresh — **fixed & verified end-to-end in a real browser (2nd review pass):** editing CHANGELOG.md with the server and browser running updated the Activity Timeline within ~1s without a reload; reverting it removed the entry live. Root cause was chokidar v4 dropping glob support (watch patterns resolved to 0 paths). See Implementation Report addendum.
- [x] No write git command exists anywhere in `server/git/` — mechanically re-verified by grep, only read subcommands present
- [ ] Parser tests exist for every parser and pass
- [ ] `npm run build` succeeds

## Test Procedure

1. `npm install` in `Dashboard/` — verify clean exit.
2. `npm test` — run Vitest parser suite against fixtures, record pass/fail counts.
3. `npm run build` — verify a production build succeeds with no type errors.
4. `npm run dev` — start the dev server, fetch `/api/snapshot`, confirm it returns real, non-empty data matching the repository's actual current state (branch name, task counts, DEC count, etc.), then stop the server.
5. Manual verification of live-update behavior is deferred to Yoav's own session (editing a file while the dev server runs, watching the browser update) — not fully exercisable in this automated pass.

## Files Allowed to Edit

- `Dashboard/**` (new folder, all contents)
- `.gitignore` (add `Dashboard/node_modules/`, `Dashboard/dist/`)
- `Tasks/**/PC-006_project-dashboard.md` (this file, moving TODO → IN_PROGRESS → DONE)
- `CHANGELOG.md`
- `README.md` (add the Dashboard to the repository-layout and start-here references — a permanent tool, so the top-level index should point at it)

## Files Forbidden to Edit

- Everything else, including all other root-level docs, `Assets/`, and any other task file

## Out of Scope

- Editing Tasks, Decision creation, Documentation editing, git write helpers, analytics, multi-user/auth — all explicitly deferred, architecture reserves room for them without a rewrite (see Future Extensibility in the approved architecture review)
- Visual/UX polish beyond what's needed to match the approved mockup's layout and token system

## Risks

- Parsing is coupled to the current prose conventions in `ROADMAP.md`/`DECISIONS.md`/task files — future changes to those conventions require matching parser updates.
- "Current sprint" is inferred, not stated anywhere in the repository, and can be wrong if a task file doesn't clearly reference its sprint number.
- Live-update behavior (file watch → WebSocket push → UI refresh) is difficult to fully verify without an interactive browser session watching a running dev server across a file edit in real time; verified structurally and via a single snapshot fetch, not a full live-edit cycle, in this pass.

## Implementation Report

Implemented 2026-07-11. `Dashboard/` scaffolded end to end: a Vite + React + TypeScript frontend, a Vite-plugin backend (parsers, read-only git, chokidar watcher, WebSocket push), all six pages, global search, Project Health, Activity Timeline, and Knowledge Map Graph/Tree views.

Two real bugs were found and fixed via live smoke-testing against the actual repository, not just fixture tests — worth recording since they're exactly the kind of thing fixtures alone wouldn't have caught:
1. `roadmapParser`'s Phase-goals collector was absorbing every Cluster's and Sprint's bullet list into `Phase.goals`, not just the phase's own initial list — caught by inspecting a live `/api/snapshot` response. Fixed by tracking whether a Cluster heading has been seen yet within the current phase; regression test added.
2. `inferFocus`'s "current sprint" heuristic picked "Sprint 1.1" as current — sourced from a parenthetical mention inside PC-003's title, even though PC-004 (DONE) already covers the more advanced Sprint 2.0–2.2. Root cause was twofold: only the first sprint-number match per title was read, and range notation ("Sprint 2.0-2.2") wasn't expanded to its end value. Fixed by collecting all refs (with range expansion) across all non-archived tasks and picking the highest one whose owning task isn't DONE, falling through to the next unclaimed roadmap sprint otherwise. Three regression tests added, including the exact PC-003/PC-004 scenario.

Final verification against the live repository: current focus correctly resolves to Sprint 2.3 — Active Skill System Validation, Cluster A, with the Cluster A Decision Gate correctly attached.

### Addendum — 2nd review pass (2026-07-11), two issues Yoav found and both now fixed

**Issue 1 — live updates were not working (fixed).** Root cause: `chokidar` v4.0.3 removed built-in glob support, so the watcher's patterns (`*.md`, `Documentation/**/*.md`, `Tasks/**/*.md`) resolved to **zero** watched paths — confirmed by direct probe (`w.getWatched()` returned `{}`). The initial snapshot worked because it's a direct build call, not watcher-driven, which exactly matched Yoav's symptom (only updated on restart). Fix: `server/watcher.ts` rewritten to watch real directory trees (`Documentation/`, `Tasks/`) plus root-level `.md` files enumerated at startup (chokidar can't glob them) plus `.git/HEAD`; events filtered to `.md`/`HEAD` in the handler. Added the full requested debug logging: watched absolute paths at boot, every add/change/unlink, and each rebuild+broadcast with entry count and client count. Verified end-to-end in a real browser: edited CHANGELOG.md → Activity Timeline updated within ~1s with no reload; reverted → entry disappeared live. Server log confirmed the fs event → rebuild → broadcast-to-2-clients chain each time.

**Issue 2 — Documentation page click did nothing (was unfinished; now implemented).** It was an unfinished feature, not a regression — v1 shipped the tree but not the reader, even though the original feature request said "open Markdown directly." Implemented: a read-only, path-validated `/api/doc?path=` endpoint (rejects traversal, non-`.md`, and absolute paths — all three probed and rejected) plus a two-pane Documentation page that renders the selected file's Markdown with `react-markdown` + `remark-gfm` (no raw-HTML pass-through, so no injection surface). Verified in-browser rendering CORE_PHILOSOPHY.md with correct headings, lists, inline links, and tables. Acceptance criteria updated accordingly.

Both fixes: typecheck clean, 12/12 tests pass (roadmap Phase-goals-leak and focus range-notation regressions still covered), `npm run build` succeeds. Not staged, committed, or pushed.

## Review Notes

(Awaiting Yoav's second manual review.)

## Game Director Approval

- [x] Approved by Yoav
- Date: 2026-07-11
- Notes: Approved after two manual review passes. First pass found two issues (live updates not working; Documentation page not clickable); both were fixed and the second manual review passed ("The second manual review passed. Please finalize PC-006."). Approval covers the v1 read-only dashboard as built.

## Definition of Done Checklist

- [x] Acceptance criteria pass — all re-verified this session; live-update and Documentation-viewer confirmed in a real browser (2nd review pass). Global Search and the Knowledge Map Graph/Tree toggle remain code-verified only, not clicked through — noted, not blocking.
- [x] `npm run build` has no errors
- [x] Required testing was completed (12/12 tests pass; live end-to-end update test + doc-endpoint security probes run in-browser; Yoav's own second manual review passed)
- [x] Yoav approved the result
- [ ] Technical review was completed — **not performed.** No Technical Director (the "ChatGPT" role per `PROJECT_BIBLE.md` § Team Roles) was party to this session. Moved to DONE on Yoav's explicit instruction as Game Director and final approver — flagged rather than silently checked, same handling as PC-004.
- [x] Relevant documentation was updated (this task file, CHANGELOG.md, README.md)
