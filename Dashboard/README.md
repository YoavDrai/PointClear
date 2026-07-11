# Point Clear — Project Dashboard

A local, read-only control center for the Point Clear repository. It shows current phase, cluster, sprint,
task board state, decisions, changelog activity, and document relationships — all derived directly from the
repository's own markdown and git state.

**Markdown remains the only canonical source.** This app never writes to any tracked file, never becomes a
second source of truth, and never runs a git write command. See [PC-006](../Tasks/DONE/PC-006_project-dashboard.md)
for the full task record, and [`server/git/readOnlyGit.ts`](server/git/readOnlyGit.ts), which structurally
contains no write command — not merely an unused one.

## Run it

```bash
cd Dashboard
npm install
npm run dev
```

Opens automatically at the printed `localhost` URL (default `http://localhost:5183`, falls back to the next
free port if that one's taken).

## How it works

- `server/` parses `ROADMAP.md`, `DECISIONS.md`, `CHANGELOG.md`, `Tasks/**/*.md`, and
  `POINT_CLEAR_KNOWLEDGE_MAP.md` into one in-memory `ProjectSnapshot` (see `server/model/types.ts`), using
  `remark`/`unified` to parse real markdown ASTs rather than regex-over-lines.
- A file watcher (`chokidar`, scoped to only the relevant markdown paths — never `Assets/`, `Library/`,
  `Temp/`, or `Logs/`) rebuilds the snapshot on every change and pushes it to every connected browser tab over
  a WebSocket. No polling, no manual refresh, no restart required.
- `server/git/readOnlyGit.ts` shells out to `git status`/`log`/`rev-parse`/`rev-list` only.
- The snapshot is never persisted to disk and never hand-edited — it is a pure, disposable function of the
  files on disk at the moment it's built.

## Commands

| Command | What it does |
|---|---|
| `npm run dev` | Starts the app with live reload |
| `npm run build` | Type-checks, then produces a production build in `dist/` |
| `npm run preview` | Serves the production build locally |
| `npm test` | Runs the parser unit tests (Vitest) |
| `npm run typecheck` | Type-checks without emitting |

## What v1 deliberately does not do

No editing of Tasks, Decisions, or Documentation from the UI; no git write operations; no database; no cloud
or external network calls; no authentication (single local user, localhost only). The parse → model → API
layering was designed so a future version could add editing without a rewrite — see the architecture review
in chat history and PC-006's task file — but none of that is implemented here.
