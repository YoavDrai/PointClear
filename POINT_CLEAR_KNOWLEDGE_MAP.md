# Point Clear — Knowledge Map

**Status: DRAFT — pending Game Director / Technical Director approval.**

This is not a design document. It contains no gameplay decisions. It is the blueprint of the documentation system itself — which knowledge domains exist, which document currently (or will) own each one, how they depend on each other, and which documents are sources of truth versus derived, living, or temporary. Every other document in this repository is downstream of the structure described here.

## Purpose

Point Clear's documentation is a knowledge base, not a pile of files. A *domain* owns a question the design needs a permanent, unambiguous answer to. A *document* is a container that currently holds part or all of one or more domains' answers. This map exists so that:

- Every piece of knowledge has exactly one canonical owner.
- No two documents assert conflicting or duplicated rules.
- Adding a new document is a deliberate act of assigning it a domain, not an ad hoc creation.
- The dependency graph between knowledge (not files) stays acyclic and legible.

## Legend

| Tag | Meaning |
|---|---|
| **SOURCE OF TRUTH** | The one canonical document for this knowledge. Any other document touching the same topic must reference it, not restate it. |
| **DERIVED** | Content is legitimately re-expressed from a Source of Truth for a different purpose (e.g. a checklist derived from a philosophy). Not duplication — a different artifact type. |
| **GENERATED** | Populated from a template, one instance per content entity (a class, a weapon, a task). Not hand-authored prose. |
| **LIVING** | Expected to grow indefinitely for the life of the project; never "finished." |
| **TEMPORARY** | Expected to be retired, archived, or substantially rewritten once its phase/purpose passes. |
| SHIPPED / IN REVIEW / PLANNED / BLOCKED / DEFERRED | Same status vocabulary as the document architecture proposal. |

## Knowledge Domains

| Code | Domain | Parent | Status |
|---|---|---|---|
| KD-01 | Identity & Philosophy | — (root) | Active |
| KD-02 | Governance & Process | — (root, sibling of KD-01) | Active |
| KD-03 | Core Loop & Session Structure | KD-01 | Active (in review) |
| KD-04 | Multiplayer & Co-op | KD-03 | Planned |
| KD-05 | Combat | KD-03 | Partially stubbed |
| KD-06 | Builds & Progression | KD-03 | Partially stubbed |
| KD-07 | Difficulty & Challenge | KD-03 | Partially stubbed |
| KD-08 | Death, Failure & Extraction | KD-03 | Partially stubbed, duplicated |
| KD-09 | Leaderboards & Competition | KD-01 | Fragmented across 2 docs |
| KD-10 | Seasons | KD-01 | Fragmented across 1 doc |
| KD-11 | Technical Architecture | KD-04 | Not started |
| KD-12 | Content Library (Classes/Skills/Weapons/Items/Enemies/Bosses/Maps/UI/Art/Audio) | KD-05 + KD-06 + KD-11 | Empty, correctly deferred |

## Domain Hierarchy

```
KD-01 Identity & Philosophy
 └─ KD-03 Core Loop & Session Structure
     ├─ KD-04 Multiplayer & Co-op
     │   └─ KD-11 Technical Architecture
     ├─ KD-05 Combat
     │   └─ KD-12 Content Library (partial parent)
     ├─ KD-06 Builds & Progression
     │   └─ KD-12 Content Library (partial parent)
     ├─ KD-07 Difficulty & Challenge
     └─ KD-08 Death, Failure & Extraction
KD-02 Governance & Process            (independent of KD-01 — process doesn't derive from game philosophy)
KD-09 Leaderboards & Competition      (child of KD-01 directly — competition is a philosophy-level concern, not loop-mechanical)
KD-10 Seasons                          (child of KD-01 directly, same reasoning)
```

No cycles: knowledge flows strictly downward from philosophy/process toward content. Verified by inspection — no domain's Source of Truth references a domain below it in this tree.

## Knowledge Topic Registry

| Domain | Topic | Canonical Owner (current or intended) | Status |
|---|---|---|---|
| KD-01 | Studio Identity & Mission | `PROJECT_BIBLE.md` §1 + `DESIGN_DNA.md` Purpose/Mission | SOURCE OF TRUTH |
| KD-01 | Design Pillars (operational checklist) | `GAME_PILLARS.md` | DERIVED from `DESIGN_DNA.md` |
| KD-01 | Feature Vision / Intent | `VISION.md` | SOURCE OF TRUTH |
| KD-01 | Terminology | `GLOSSARY.md` | SHIPPED, LIVING |
| KD-02 | Roles & Responsibilities | `PROJECT_BIBLE.md` §2 | SOURCE OF TRUTH |
| KD-02 | Feature/Task Workflow (12 steps) | Currently duplicated in `PROJECT_BIBLE.md` §3, `CONTRIBUTING.md`, `Documentation/AI/TASK_WORKFLOW.md` | **CONFLICT RISK — needs single owner** |
| KD-02 | AI Collaboration Rules | `Documentation/AI/CLAUDE_RULES.md` | SOURCE OF TRUTH |
| KD-02 | Review Standards | `Documentation/AI/REVIEW_CHECKLIST.md` | SOURCE OF TRUTH |
| KD-02 | Decision Log | `DECISIONS.md` | SOURCE OF TRUTH, LIVING |
| KD-02 | Roadmap / Phasing | `ROADMAP.md` | SOURCE OF TRUTH, LIVING (later phases TEMPORARY/tentative) |
| KD-02 | Change History | `CHANGELOG.md` | GENERATED (append-only), LIVING |
| KD-02 | Content Identity Rule (don't copy other games) | Currently duplicated in `PROJECT_BIBLE.md` §1, `CLAUDE_RULES.md`, `VISION.md` | **CONFLICT RISK — needs single owner** |
| KD-02 | Documentation Truth Rule (session read-order) | Currently duplicated in `PROJECT_BIBLE.md` §4, `CLAUDE_RULES.md` | **CONFLICT RISK — needs single owner** |
| KD-03 | Session Structure (Lobby/Party/Loadout) | `Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md` | SOURCE OF TRUTH |
| KD-03 | Run Sequence | `Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md` | SOURCE OF TRUTH |
| KD-03 | Player Emotional Arc | `DESIGN_DNA.md` "Player Journey"; `CORE_GAMEPLAY_LOOP.md` "Emotional Curve" now references it instead of restating | RESOLVED — single owner |
| KD-03 | Operation Definition | `GLOSSARY.md` (concise definition) + `Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md` (place/sequence in a run) | RESOLVED via DEC-009 — deliberate split, not duplication (same pattern as Extraction) |
| KD-03 | Connected Zones | `GLOSSARY.md` (definition) + `CORE_GAMEPLAY_LOOP.md` (sequence) | RESOLVED via DEC-010 |
| KD-03 | Semi-Procedural Variation | `DECISIONS.md` DEC-011 (approved boundary) + `CORE_GAMEPLAY_LOOP.md` § Zones (brief mention) | RESOLVED at high level; exact technique **[UNRESOLVED]** |
| KD-03 | Dynamic Objectives | `GLOSSARY.md` (definition) + `CORE_GAMEPLAY_LOOP.md` § Objectives (sequence) | RESOLVED via DEC-012 at high level; types/selection **[UNRESOLVED]** |
| KD-03 | MVP Scope Boundary | `SCOPE_MVP.md` | PLANNED — now unblocked (Operation gap resolved) |
| KD-04 | Co-op Requirements & Philosophy | `DESIGN_DNA.md` states the rule; `CORE_GAMEPLAY_LOOP.md` § Party now references it instead of restating | RESOLVED — single owner |
| KD-04 | Networking Requirements | `MULTIPLAYER_REQUIREMENTS.md` | PLANNED — now unblocked (Operation gap resolved) |
| KD-04 | Shared-State Ownership | Technical Architecture (future) | BLOCKED |
| KD-05 | Combat Philosophy | `DESIGN_DNA.md` "Combat Is King" | SOURCE OF TRUTH |
| KD-05 | Combat Feel | `DESIGN_DNA.md` "Combat Is King"; `CORE_GAMEPLAY_LOOP.md` § Combat now references it instead of restating | RESOLVED — single owner |
| KD-05 | Damage Model | Not written | BLOCKED |
| KD-05 | Targeting | Not written | BLOCKED |
| KD-05 | Enemy Interaction | Loop-sequencing stubs only | BLOCKED |
| KD-06 | Build Philosophy | `DESIGN_DNA.md` "Build Philosophy" | SOURCE OF TRUTH |
| KD-06 | Build Layer Structure (high-level) | `Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md` | RESOLVED at high level via DEC-013 (7 layers + interaction principle); exact rules **[UNRESOLVED]** |
| KD-06 | Upgrade Selection Mechanic | Loop stub, will move to Systems layer | BLOCKED — still needs `PROGRESSION_OVERVIEW.md` |
| KD-06 | Run vs. Account Progression | `PROGRESSION_OVERVIEW.md` | PLANNED |
| KD-07 | Difficulty Philosophy (mastery, fairness) | `DESIGN_DNA.md` "Difficulty" | SOURCE OF TRUTH |
| KD-07 | Difficulty Curve Mechanics (pacing) | `CORE_GAMEPLAY_LOOP.md` "Difficulty Curve" | SOURCE OF TRUTH — correctly separate from KD-07 philosophy; principle vs. mechanic, not duplication |
| KD-08 | Death & Failure Philosophy | Duplicated: `DESIGN_DNA.md` "Death" vs. `CORE_GAMEPLAY_LOOP.md` "Failure"/"Success" | **DUPLICATE + INCONSISTENT NAMING — still open, not addressed this pass (requires a `DESIGN_DNA.md` rename, which needs explicit Game Director sign-off — see report)** |
| KD-08 | Revive / Downed Mechanics | Not written | BLOCKED |
| KD-08 | Extraction Mechanics | `DESIGN_DNA.md` "Extraction" (philosophy); `CORE_GAMEPLAY_LOOP.md` § Extraction now sequence-only and references DNA | RESOLVED — correctly split, not duplication |
| KD-09 | Leaderboard Philosophy | `DESIGN_DNA.md` "Leaderboards" | SOURCE OF TRUTH |
| KD-09 | Categories & Metrics | Split across `DECISIONS.md` DEC-006 and `VISION.md` "Possible Leaderboard Statistics" | Fragmented, not yet conflicting — consolidate into `LEADERBOARDS_OVERVIEW.md` |
| KD-09 | Verification / Anti-Cheat | Not written | BLOCKED |
| KD-10 | Seasonal Philosophy | `DESIGN_DNA.md` "Seasons" | SOURCE OF TRUTH |
| KD-10 | Season Cadence & Structure | `DECISIONS.md` DEC-004/005 | SOURCE OF TRUTH (subject to validation) |
| KD-10 | Individual Season Specs | `Templates/SEASON_TEMPLATE.md` instances | GENERATED, DEFERRED |
| KD-11 | Networking Technology Evaluation | `NETWORKING_EVALUATION.md` | BLOCKED on KD-04 |
| KD-11 | Engine Architecture Overview | `ARCHITECTURE_OVERVIEW.md` | BLOCKED |
| KD-11 | Performance & Scale Requirements | `PROJECT_BIBLE.md` §5 (one line today) | Will need its own document once KD-11 matures |
| KD-12 | Classes / Skills / Weapons / Items / Enemies / Bosses / Maps / UI / Art / Audio | `Templates/*_TEMPLATE.md` instances | GENERATED, DEFERRED |

## Source of Truth Registry — Resolutions for Contested Knowledge

These are the concrete duplications found while building this map. Each needs exactly one owner going forward; the other location becomes a reference, not a restatement. Status column added after the DEC-009–013 documentation pass.

| Contested Knowledge | Currently Lives In | Recommended Sole Owner | Status |
|---|---|---|---|
| Player emotional arc | `DESIGN_DNA.md` "Player Journey" | `DESIGN_DNA.md` (Layer 1, stable) | **RESOLVED** — `CORE_GAMEPLAY_LOOP.md` § Emotional Curve now references it instead of restating |
| Death/failure philosophy | `DESIGN_DNA.md` "Death", `CORE_GAMEPLAY_LOOP.md` "Failure" + "Success" | `DESIGN_DNA.md`, section renamed "Death & Failure" | **STILL OPEN** — not addressed this pass; renaming `DESIGN_DNA.md` requires explicit Game Director approval before any edit (see report) |
| Combat feel adjectives | `DESIGN_DNA.md` "Combat Is King" | `DESIGN_DNA.md` | **RESOLVED** — `CORE_GAMEPLAY_LOOP.md` § Combat now references it instead of restating |
| Co-op composition freedom | `DESIGN_DNA.md` "Multiplayer Philosophy" | `DESIGN_DNA.md` | **RESOLVED** — `CORE_GAMEPLAY_LOOP.md` § Party keeps only the concrete Solo/Duo/Trio/Squad(4) statement and references DNA for the "why" |
| Extraction concept | `DESIGN_DNA.md` "Extraction" (why), `CORE_GAMEPLAY_LOOP.md` "Extraction" (sequence) | Split ownership, both legitimate | **RESOLVED** — Loop trimmed to sequence-only, references DNA for philosophy |
| Operation definition (new) | `GLOSSARY.md` (concise definition), `CORE_GAMEPLAY_LOOP.md` (place/sequence) | Split ownership, both legitimate — same pattern as Extraction | **RESOLVED from creation** — recorded via DEC-009 with the split intact, no restatement between them |
| Content Identity Rule | `PROJECT_BIBLE.md` §1, `CLAUDE_RULES.md`, `VISION.md` | `PROJECT_BIBLE.md` §1 | Still open — out of scope for this pass |
| Documentation Truth Rule (9-doc read order) | `PROJECT_BIBLE.md` §4, `CLAUDE_RULES.md` | `PROJECT_BIBLE.md` §4 | Still open — out of scope for this pass |
| 12-step Feature/Task Workflow | `PROJECT_BIBLE.md` §3, `CONTRIBUTING.md`, `TASK_WORKFLOW.md` | `Documentation/AI/TASK_WORKFLOW.md` | Still open — out of scope for this pass |
| "Golden Rule" | `DESIGN_DNA.md` (feature-evaluation heuristic), `CORE_GAMEPLAY_LOOP.md` (run-success test) | Both kept, disambiguated by name | **PARTIALLY RESOLVED** — `CORE_GAMEPLAY_LOOP.md` renamed to "The Golden Rule — Run Success Test"; `DESIGN_DNA.md`'s counterpart rename to "The Golden Rule — Feature Evaluation" still needs Game Director approval (see report) |

## Derived / Generated Documents

- `GAME_PILLARS.md` — DERIVED from `DESIGN_DNA.md` (operational checklist form of the same philosophy, different consumer: proposal-gating, not narrative reading).
- `CHANGELOG.md` — GENERATED, append-only.
- `Tasks/*.md` — GENERATED from `Templates/TASK_TEMPLATE.md`, one per task.
- Future `Documentation/Classes|Skills|Weapons|Enemies|Bosses|Seasons/*.md` — GENERATED from their respective templates, one per content entity.

## Living Documents

`DECISIONS.md`, `CHANGELOG.md`, `ROADMAP.md`, `GLOSSARY.md`, `Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md` (will keep absorbing approved layer-rule detail as it's decided), all `Documentation/Systems/*.md` (grow as systems are built out), all `Documentation/Technical/*.md`, and the entire Content Library (KD-12) — these never reach a final state by design.

## Temporary Documents

- `Tasks/*.md` — each task file is disposable once archived; the knowledge that matters survives in `CHANGELOG.md`/`DECISIONS.md`/whatever doc it updated, not in the task file itself.
- `SCOPE_MVP.md` — valid for the Phase 0–2 horizon only; expected to be retired or substantially rewritten once the vertical slice ships and scope questions change shape.
- The "Later Phases (Tentative)" section of `ROADMAP.md` — explicitly placeholder text pending real phase breakdown.

## Dependency Graph (Domain-Level)

```
KD-01 Identity & Philosophy
  └─▶ KD-03 Core Loop & Session Structure
        ├─▶ KD-04 Multiplayer & Co-op ──▶ KD-11 Technical Architecture
        ├─▶ KD-05 Combat ────────────────▶ KD-12 Content Library
        ├─▶ KD-06 Builds & Progression ──▶ KD-12 Content Library
        ├─▶ KD-07 Difficulty & Challenge
        └─▶ KD-08 Death, Failure & Extraction
  └─▶ KD-09 Leaderboards & Competition
  └─▶ KD-10 Seasons

KD-02 Governance & Process — independent, no gameplay-knowledge dependencies
```

**Circular dependency check: none found.** Every edge points from philosophy/process toward mechanics toward content, in one direction only. KD-02 is intentionally disconnected from the gameplay-knowledge tree — process knowledge and game-design knowledge should never need to reference each other's internals.

## Decision Architecture

Knowledge Architecture answers "where does this fact live?" Decision Architecture answers "why did this document need to exist at all, and what did it make possible?" Every document below exists because the team had to answer one specific question the game could not move forward without answering. This section defines, per document: the question it exists to answer, what it needs before it can be written, what it produces, which future decisions it unlocks or blocks, how we'll know it's done, who is allowed to change it, and what it alone is allowed to say.

**Lifecycle vocabulary** used below: **STABLE** (Game Director approval only — changes ripple widely, must be rare), **SEMI-STABLE** (Game Director approval, but Technical Director may propose amendments when implementation reveals a conflict), **VOLATILE** (expected to change often, by design — usually because it tracks a phase, a balance pass, or an evaluation still in progress).

### Layer 0 — Governance & Process

| Field | `PROJECT_BIBLE.md` | `README.md` | `CONTRIBUTING.md` |
|---|---|---|---|
| Primary Question | What is Point Clear, who is responsible for it, and how does work move from idea to done? | Where does a new reader start? | How does someone — human or AI — propose and land a change? |
| Required Inputs | None (root of Layer 0) | `PROJECT_BIBLE.md` | `PROJECT_BIBLE.md` §2–3 |
| Produced Knowledge | Identity facts, team roles, the 12-step workflow, engineering principles, Definition of Done | A map of where every other answer lives | The same workflow, restated for a contributor-facing audience |
| Unlocked Decisions | Every other document's right to exist | Nothing directly — it's a router | Whether a given change follows process correctly |
| Blocked Decisions | All of them, until this exists | None | None |
| Success Criteria | No reader should have to ask "who decides this?" | No reader should get lost finding a document | No contributor should have to ask "what do I do next?" |
| Lifecycle | STABLE | STABLE | STABLE |
| Source of Truth | Exclusive owner of: project identity fields, team roles, the 12-step workflow **(see conflict below)**, Definition of Done, Content Identity Rule **(see conflict below)** | Nothing — pure index, must not originate facts | Nothing — must reference `PROJECT_BIBLE.md`/`TASK_WORKFLOW.md`, not restate them |

| Field | `DECISIONS.md` | `ROADMAP.md` | `CHANGELOG.md` |
|---|---|---|---|
| Primary Question | What has the team already decided, and what remains explicitly open? | In what order and phase does work happen? | What changed, and when? |
| Required Inputs | `PROJECT_BIBLE.md` | `PROJECT_BIBLE.md`, `VISION.md` | Any document change |
| Produced Knowledge | An append-only ledger of accepted facts (DEC-001…) and a named list of unresolved questions | Phase goals and their status | A dated audit trail |
| Unlocked Decisions | Any downstream doc may cite a DEC-XXX as justification | Sequencing of which document gets written next | Historical reconstruction of intent |
| Blocked Decisions | Anything on the "Unresolved" list must not be assumed anywhere else | Later-phase content, until earlier phases close | None |
| Success Criteria | Every accepted fact has exactly one entry; nothing is decided twice | Every phase has explicit goals and an honest status | Every foundation-level change is traceable |
| Lifecycle | LIVING, append/amend only | LIVING, amend per phase | LIVING, append-only (GENERATED) |
| Source of Truth | Exclusive owner of: which facts are accepted vs. open | Exclusive owner of: phase sequencing and status | Exclusive owner of: the dated history of change |

| Field | `AI_ONBOARDING.md` | `CLAUDE_RULES.md` | `SESSION_START.md` | `TASK_WORKFLOW.md` | `REVIEW_CHECKLIST.md` | `Templates/*.md` |
|---|---|---|---|---|---|---|
| Primary Question | What must an AI collaborator know before touching anything? | What is Claude allowed to do without asking? | What must happen at the start of every session, before any action? | How does one task move from idea to DONE? | How do we know a task is actually finished? | What shape must a new instance of content-type X take? |
| Required Inputs | `PROJECT_BIBLE.md` | `PROJECT_BIBLE.md` §2 | `PROJECT_BIBLE.md`, `CLAUDE_RULES.md` | `PROJECT_BIBLE.md` §3, `CONTRIBUTING.md` | `TASK_WORKFLOW.md` | The relevant domain doc existing (e.g. `COMBAT_SYSTEM.md` before `WEAPON_TEMPLATE.md` is usable) |
| Produced Knowledge | Orientation summary + pointers | The boundary of independent action | A repeatable session-start checklist | The 5-folder task lifecycle + Definition of Done | A pass/fail checklist for review | A fixed skeleton for one content type |
| Unlocked Decisions | Whether a new AI session can act at all | Whether Claude proceeds or stops and flags | Whether a session's findings are trustworthy | Whether a task file is correctly placed | Whether a task can move to DONE | Whether a new class/skill/weapon/etc. doc can be created consistently |
| Blocked Decisions | Any action, until read | Any scope-expanding action | Any modification, until steps 1–9 complete | Task creation, until a spec exists | Task completion, until checklist passes | Content authoring, until template exists |
| Success Criteria | New session needs no other doc to get oriented | No ambiguity about what needs Game Director sign-off | Every session starts from verified ground truth, not assumption | No task is "kind of done" | No task reaches DONE with an unchecked box | Two authors of the same content type never disagree on structure |
| Lifecycle | STABLE | STABLE | STABLE | STABLE | STABLE | SEMI-STABLE (Technical Director may refine fields) |
| Source of Truth | Nothing new — pure orientation, references everything else | Exclusive owner of: Claude's approval boundary | Exclusive owner of: the session-start procedure itself (distinct from *what* to read, which `PROJECT_BIBLE.md` owns) | Exclusive owner of: the 12-step workflow **(conflict — see below)**, task folder lifecycle | Exclusive owner of: the DONE checklist | Exclusive owner of: field structure per content type |

**Conflict surfaced by this exercise:** `PROJECT_BIBLE.md` §3 and `TASK_WORKFLOW.md` both claim ownership of "the 12-step workflow," and `PROJECT_BIBLE.md` §1/§4 and `CLAUDE_RULES.md` both claim ownership of the Content Identity Rule and the Documentation Truth Rule's read-order list. This is the same duplication flagged in the Knowledge Map above, now visible from a second angle: two documents cannot both be the Source of Truth for the same question. Resolution already recorded in the Source of Truth Registry — `TASK_WORKFLOW.md` owns the workflow, `PROJECT_BIBLE.md` owns identity/read-order, everything else references.

### Layer 1 — Philosophy

| Field | `DESIGN_DNA.md` | `GAME_PILLARS.md` | `VISION.md` |
|---|---|---|---|
| Primary Question | Why does Point Clear exist, and what experience must every feature serve? | What is the checklist every proposal must pass? | What features does Point Clear intend to have? |
| Required Inputs | None (root of Layer 1) | `DESIGN_DNA.md` | `GAME_PILLARS.md`, `DESIGN_DNA.md` |
| Produced Knowledge | Five experience pillars, player emotional arc, build/death/difficulty/leaderboard/season/combat philosophy, "what we will never build" | A 10-item gating checklist | The intended feature list, party-size support, leaderboard category intent |
| Unlocked Decisions | Whether any feature is worth pursuing at all | Whether a specific proposal may proceed to spec | What `CORE_GAMEPLAY_LOOP.md` and every domain doc below it is allowed to contain |
| Blocked Decisions | Every design document | Feature approval | Any Layer 2+ document's scope |
| Success Criteria | No downstream doc should need to re-justify "why" — it can always cite here | Every proposal review is a checklist pass, not a debate | No reader should wonder "is X actually intended, or just possible?" |
| Lifecycle | STABLE | STABLE — "not expected to change casually" per its own text | SEMI-STABLE — individual items may move from PROPOSAL to APPROVED FACT as decisions land |
| Source of Truth | Exclusive owner of: player emotional arc **(currently duplicated — see resolution above)**, death/failure philosophy **(duplicated)**, combat feel philosophy **(duplicated, drifted)**, multiplayer philosophy **(duplicated)**, difficulty/leaderboard/season philosophy, "never build" list | Exclusive owner of: the 10 gating pillars | Exclusive owner of: the intended feature list, leaderboard category proposals |

### Layer 2 — Design Blueprints

| Field | `CORE_GAMEPLAY_LOOP.md` | `GLOSSARY.md` | `SCOPE_MVP.md` |
|---|---|---|---|
| Primary Question | What happens, in order, during a single run? | What does each Point-Clear-specific term mean? | What is actually in the first playable version, versus full vision? |
| Required Inputs | `DESIGN_DNA.md`, `GAME_PILLARS.md`, `VISION.md` | `CORE_GAMEPLAY_LOOP.md` (terms are coined there first) | `CORE_GAMEPLAY_LOOP.md`, `ROADMAP.md` |
| Produced Knowledge | Session structure, run sequence, upgrade/build/extraction/results stubs | Canonical definitions (Operation, Extraction, Elite, Loadout, etc.) | The explicit MVP/full-vision boundary |
| Unlocked Decisions | All Systems-layer specs; Multiplayer Requirements | Consistent terminology across every future doc | Which Systems docs get written first (Phase 2 target) |
| Blocked Decisions | Systems layer cannot be written without this | Nothing blocks on it, but drift risk grows without it | Systems prioritization is currently guesswork without it |
| Success Criteria | A reader can answer "what happens next" at any point in a run — **"What is an Operation?" is now answered (DEC-009); philosophy sections (Emotional Curve, Combat, Party, Extraction) now reference DESIGN_DNA.md instead of restating** | No two documents use a term differently | No one can mistake a full-vision feature for an MVP one |
| Lifecycle | SEMI-STABLE — structure is Game Director-approved; Technical Director may propose refinements as Systems docs reveal constraints | LIVING — grows every time a new term is coined | VOLATILE by design — expected to change every phase |
| Source of Truth | Exclusive owner of: session structure, run sequence, the Operation's place in a run, Zone/Objective sequencing | Exclusive owner of: term definitions, including Operation, Zone, Objective, Build Layer. Must never be redefined elsewhere | Exclusive owner of: what ships in v1 |

| Field | `MULTIPLAYER_REQUIREMENTS.md` | `PROGRESSION_OVERVIEW.md` | `LEADERBOARDS_OVERVIEW.md` | `SEASONS_OVERVIEW.md` |
|---|---|---|---|---|
| Primary Question | What must be true of the game for co-op to work? | How does a player get stronger, within a run and across runs? | What gets measured and compared between players? | What changes and resets between seasons? |
| Required Inputs | `CORE_GAMEPLAY_LOOP.md`, `DESIGN_DNA.md` (multiplayer philosophy) | `CORE_GAMEPLAY_LOOP.md`, `VISION.md` | `VISION.md`, `DECISIONS.md` (DEC-006) | `DESIGN_DNA.md`, `DECISIONS.md` (DEC-004/005) |
| Produced Knowledge | Co-op requirements independent of any specific networking tech | Run-scoped vs. account-scoped progression rules | Category list, candidate metrics | Season cadence, what persists vs. resets |
| Unlocked Decisions | `NETWORKING_EVALUATION.md`, `DEATH_REVIVE_SYSTEM.md`, `EXTRACTION_SYSTEM.md` | `UPGRADE_BUILD_SYSTEM.md`, Content Library structure | `RESULTS_TELEMETRY.md`, anti-cheat/verification model | Individual `SEASON_TEMPLATE.md` instances |
| Blocked Decisions | Networking technology choice must not be picked before this exists | Build system spec | Leaderboard verification approach | Any season content |
| Success Criteria | Every system doc can answer "does this need to account for co-op?" without guessing | No ambiguity between "this run's build" and "my account" | Primary metric is named, not just proposed | No season launches without a defined reset boundary |
| Lifecycle | SEMI-STABLE — amendable if Networking Evaluation finds a requirement infeasible | SEMI-STABLE | SEMI-STABLE | VOLATILE per-season, STABLE at the cadence level |
| Source of Truth | Exclusive owner of: co-op functional requirements | Exclusive owner of: progression-type distinction | Exclusive owner of: metric/category definitions | Exclusive owner of: season structure |

| Field | `Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md` |
|---|---|
| Primary Question | What are the high-level components a Point Clear build is made of? |
| Required Inputs | `DESIGN_DNA.md` (Build Philosophy), `DECISIONS.md` (DEC-013) |
| Produced Knowledge | The seven approved build layers and the interaction principle |
| Unlocked Decisions | Whether `Documentation/Systems/UPGRADE_BUILD_SYSTEM.md` has a structural foundation to build on once `PROGRESSION_OVERVIEW.md` also exists |
| Blocked Decisions | Exact per-layer rules, slot counts, acquisition, balance — all remain blocked pending future decisions |
| Success Criteria | No reader mistakes an illustrative example for approved content; the seven layers are named unambiguously |
| Lifecycle | LIVING — will absorb newly approved layer-rule decisions over time; SEMI-STABLE at the "which layers exist" level (Game Director approval to add/remove a layer) |
| Source of Truth | Exclusive owner of: the approved build-layer list and the interaction principle at high level |

### Layer 3 — Systems (not yet written; BLOCKED)

| Field | `COMBAT_SYSTEM.md` | `UPGRADE_BUILD_SYSTEM.md` | `DEATH_REVIVE_SYSTEM.md` | `EXTRACTION_SYSTEM.md` | `DIFFICULTY_ENEMY_TIERS.md` | `RESULTS_TELEMETRY.md` |
|---|---|---|---|---|---|---|
| Primary Question | What are the exact rules that make combat work? | What are the exact rules for how upgrades combine into a build? | What exactly happens when a player dies, and how are they revived? | What are the exact rules for surviving to evacuation? | What exactly distinguishes normal / Elite / Mini Boss / Boss? | What data is captured and shown at run end? |
| Required Inputs | `CORE_GAMEPLAY_LOOP.md`, `MULTIPLAYER_REQUIREMENTS.md` | `CORE_GAMEPLAY_LOOP.md`, `PROGRESSION_OVERVIEW.md` | `MULTIPLAYER_REQUIREMENTS.md` | `CORE_GAMEPLAY_LOOP.md`, `MULTIPLAYER_REQUIREMENTS.md` | `COMBAT_SYSTEM.md` | `LEADERBOARDS_OVERVIEW.md`, `PROGRESSION_OVERVIEW.md` |
| Produced Knowledge | Damage/targeting/feel rules | Upgrade-choice mechanics | Revive rules, downed state | Evacuation trigger and win condition | Enemy tier rules | Exact Results-screen data model |
| Unlocked Decisions | Weapon/Enemy content specs | Content Library build interactions | Solo-vs-co-op balance decisions | Multiplayer abandonment/griefing rules | Content Library enemy specs | Telemetry/backend requirements |
| Blocked Decisions | All Combat content | All Build content | All death-related balance | All Extraction balance | All Enemy/Boss content | Backend data schema |
| Success Criteria | A weapon/enemy designer never has to invent a combat rule | An upgrade designer never has to invent a selection rule | No ambiguity about solo vs. co-op death | No ambiguity about what ends a run successfully | No ambiguity about what makes an enemy "Elite" | No ambiguity about what a run's outcome record contains |
| Lifecycle | SEMI-STABLE design rules; VOLATILE numeric data (kept in Data/Configuration, not the design doc) | Same split | Same split | Same split | Same split | VOLATILE — likely to change with playtesting |
| Source of Truth | Exclusive owner of: combat rule design (not numbers) | Exclusive owner of: build-combination rules | Exclusive owner of: death/revive rules | Exclusive owner of: extraction rules | Exclusive owner of: tier distinctions | Exclusive owner of: results data model |

### Layer 4 — Technical (not yet written; BLOCKED)

| Field | `NETWORKING_EVALUATION.md` | `ARCHITECTURE_OVERVIEW.md` |
|---|---|---|
| Primary Question | Which networking technology can support Point Clear's multiplayer requirements? | How do the chosen systems and networking model fit together technically? |
| Required Inputs | `MULTIPLAYER_REQUIREMENTS.md` | `NETWORKING_EVALUATION.md`, all Layer 3 Systems docs |
| Produced Knowledge | A recommended/selected networking stack, recorded as a DEC-XXX | Overall technical architecture, ownership of networked state |
| Unlocked Decisions | Server-authoritative implementation of any system; hosting model | Actual Unity implementation tasks |
| Blocked Decisions | Every "server-authoritative" requirement in `PROJECT_BIBLE.md` §5 stays theoretical until this exists | Implementation tasks that span multiple systems |
| Success Criteria | DEC-XXX exists recording the chosen networking solution, closing the `DECISIONS.md` unresolved item | A new engineer can understand system boundaries without reading code first |
| Lifecycle | VOLATILE until decided, then STABLE (becomes a DEC-XXX) | SEMI-STABLE |
| Source of Truth | Exclusive owner of: which networking technology was chosen and why | Exclusive owner of: cross-system technical boundaries |

### Layer 5 — Content Library (empty; DEFERRED)

One shared Decision Model applies to every instance (a class, a weapon, an enemy, a boss, a map, a UI screen, an art/audio direction doc):

| Field | Answer |
|---|---|
| Primary Question | What is the exact specification of this one entity? |
| Required Inputs | The relevant Layer 3 Systems doc(s) + the relevant `Templates/*_TEMPLATE.md` |
| Produced Knowledge | A single content entity's full spec |
| Unlocked Decisions | Whether this entity can be implemented in Unity |
| Blocked Decisions | Implementation of this specific entity only — never blocks another entity |
| Success Criteria | An implementer needs no external context to build it |
| Lifecycle | VOLATILE — expected to change constantly during balancing; balance changes modify the Data/Configuration section only, never the Design Description prose |
| Source of Truth | Exclusive owner of: this one entity's specific values and behavior |

## Decision Dependency Graph

This is not the document graph from the Knowledge Map — it's the chain of *questions the game itself needs answered*, in the order reality forces them. Two independent graphs exist; they never cross, which is itself a useful finding: **process decisions (how the team works) never gate design decisions (what the game is), and vice versa.**

### Process Decision Graph (fully resolved — Layer 0 is shipped)

```
"Who is responsible for what?"
  → "How does an idea become a task?"
      → "How do we know a task is done?"
```

### Game Design Decision Graph (the one that matters going forward)

```
"Why does this game exist, and what must every feature do?"                [DESIGN_DNA — ANSWERED]
  ↓
"What is the checklist every idea must pass?"                              [GAME_PILLARS — ANSWERED]
  ↓
"What features do we intend to have?"                                      [VISION — ANSWERED]
  ↓
"What happens, in order, during a single run?"                             [CORE_GAMEPLAY_LOOP — IN REVIEW]
  ↓
"What is an Operation?"                                     ✅ ANSWERED — DEC-009. See GLOSSARY.md (definition)
  │                                                            + CORE_GAMEPLAY_LOOP.md (sequence).
  ├─▶ "What are Zones, and how do they connect?"              ✅ ANSWERED — DEC-010
  ├─▶ "Are Operations the same every run?"                     ✅ ANSWERED at high level — DEC-011 (semi-procedural;
  │                                                              exact generation technique remains [UNRESOLVED])
  ├─▶ "How are objectives chosen for a run?"                   ✅ ANSWERED at high level — DEC-012 (dynamic;
  │                                                              exact types/selection remain [UNRESOLVED])
  │
  ├─▶ "What ships in v1 vs. full vision?"                                  [SCOPE_MVP — PLANNED, now unblocked]
  │
  ├─▶ "What must be true for co-op to work?"                               [MULTIPLAYER_REQUIREMENTS — PLANNED, now unblocked]
  │     ↓
  │     ├─▶ "Which networking architecture supports that?"                [NETWORKING_EVALUATION — BLOCKED]
  │     │     ↓
  │     │     "How do systems and networking fit together?"               [ARCHITECTURE_OVERVIEW — BLOCKED]
  │     ├─▶ "What happens when a player dies, and how are they revived?"  [DEATH_REVIVE_SYSTEM — BLOCKED]
  │     └─▶ "What are the exact rules for surviving to evacuation?"       [EXTRACTION_SYSTEM — BLOCKED]
  │
  ├─▶ "How does a player get stronger, in-run and across runs?"           [PROGRESSION_OVERVIEW — PLANNED]
  │     ↓
  │     ├─▶ "What is a build made of, at a high level?"                   ✅ ANSWERED at high level — DEC-013,
  │     │                                                                    see BUILD_SYSTEM_OVERVIEW.md
  │     │     ↓
  │     │     "How do upgrades combine into a build, exactly?"            [UPGRADE_BUILD_SYSTEM — still BLOCKED,
  │     │                                                                    needs PROGRESSION_OVERVIEW.md too]
  │     └─▶ "What gets measured and compared between players?"           [LEADERBOARDS_OVERVIEW — PLANNED]
  │           ↓
  │           "What changes and resets between seasons?"                  [SEASONS_OVERVIEW — DEFERRED]
  │
  ├─▶ "What are the exact rules that make combat work?"                   [COMBAT_SYSTEM — BLOCKED]
  │     ↓
  │     "What exactly distinguishes normal / Elite / Mini Boss / Boss?"   [DIFFICULTY_ENEMY_TIERS — BLOCKED]
  │
  └─▶ "What data is captured and shown at the end of a run?"              [RESULTS_TELEMETRY — BLOCKED]
        (consumes: Progression + Leaderboards + Death/Extraction outcomes)

  ↓ (once Combat, Progression, and Multiplayer branches all resolve)
"What is the exact specification of this class / weapon / enemy / boss / map / etc.?"   [Content Library — DEFERRED]
```

"What does each term mean?" (`GLOSSARY.md`) is deliberately not in this chain — it's a cross-cutting, non-blocking reference node that consumes vocabulary from every branch above without gating any of them. It should grow continuously rather than wait its turn.

**Resolution note:** the root gap this graph previously exposed — "What is an Operation?" — is now answered via DEC-009 through DEC-012, recorded with the split ownership the Game Director specified: `GLOSSARY.md` owns the concise definitions, `CORE_GAMEPLAY_LOOP.md` owns their place and sequence in a run. `SCOPE_MVP.md` and `MULTIPLAYER_REQUIREMENTS.md` are now genuinely unblocked — both can reason about what a player deploys into. The next open branch point in this graph is `PROGRESSION_OVERVIEW.md`: it's required both to close the "in-run and across-run progression" branch and to unblock `UPGRADE_BUILD_SYSTEM.md`, which still cannot be written even though the build layers themselves are now named.

## Related

- [DESIGN_DNA.md](DESIGN_DNA.md)
- [GAME_PILLARS.md](GAME_PILLARS.md)
- [VISION.md](VISION.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [GLOSSARY.md](GLOSSARY.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) (in review)
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)
- [DECISIONS.md](DECISIONS.md)
