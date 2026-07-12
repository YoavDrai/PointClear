# Point Clear — Roadmap

Status legend: **[APPROVED FACT]**, **[ASSUMPTION]**, **[PROPOSAL]**, **[UNRESOLVED]**. See also: [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md), [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [VISION.md](VISION.md), [DECISIONS.md](DECISIONS.md).

**Note — two separate numbering systems, do not conflate them:** the informal "Sprint 1", "Sprint 2.x" numbering used in day-to-day development (tracked in `Tasks/` and `CHANGELOG.md`) and this document's own Phase 0/1/2 numbering are unrelated schemes that happen to both use numbers. **"Sprint 2.x" is not "Phase 2"** — there is no rule that Sprint N maps to Phase N, and the two are not expected to reconcile into a single sequence. Each Phase's status below is assessed independently, against that Phase's own listed goals, regardless of which Sprint number is current. As of 2026-07-11: informal Sprint work has reached "Sprint 2.x" while, on their own separate criteria, Phase 0 is still in progress and Phase 1 has not started — this is expected under this model, not an inconsistency to resolve.

This roadmap describes phase intent, not detailed production promises. Later phases are tentative and will be broken down further as earlier phases complete.

This roadmap is intentionally iterative — a research plan, not a fixed production schedule (see [PROJECT_BIBLE.md](PROJECT_BIBLE.md) § Workflow). Each cluster below exists to answer a specific question. If playtesting does not confirm the answer, sprints may be repeated, expanded, split, or reordered — returning to an earlier sprint is a valid, expected outcome, not a deviation from the plan.

**Vision-consolidation note (2026-07-12):** the vision workshops ([Documentation/Vision/](Documentation/Vision/)) **validated** the cluster structure below rather than overturning it; the refinements are folded in, not a rewrite. Specifically: mission risk is the **Operation greed-vs-safety loop** (rewards accumulate unsecured as a run pushes deeper through Zones — DEC-009/010, DEC-019, DEC-025 attrition); enemy variety means **behavioral questions**, not stat variety (DEC-024); progression trends toward a **finite build budget**, not "max everything" (DEC-021); content is **authored questions / generated answers** (DEC-026); onboarding's bridge emotion is **directional possibility** (DEC-027). The large deferred systems (economy, trading, seasons, crafting, live-service, and all networking/persistence) remain **future phases** and are not pulled forward. See [DECISIONS.md](DECISIONS.md) DEC-021 through DEC-029. Per the Game Director, further design workshops open only when implementation surfaces a real question — the priority is a playable game whose documentation evolves with it.

---

## Phase 0 — Foundation and Pre-Production

**Status: In Progress** — **not completed**.

**[APPROVED FACT]** Goals:
- Repository and documentation foundation
- Game vision refinement
- Core gameplay loop definition — see [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
- Multiplayer requirements definition — unblocked now that Operation structure is defined (DEC-009–DEC-012); not yet written
- Networking technology evaluation
- Technical risk prototypes
- Initial scope definition — unblocked now that Operation structure is defined (DEC-009–DEC-012); not yet written

---

## Phase 1 — Technical Prototypes

**Status: Not Started** — none of this phase's own listed goals have been attempted. The single-player combat/progression prototyping tracked informally as "Sprint 1.x"/"Sprint 2.x" does not count toward this phase; none of it is networked or multiplayer. Flagged as a sequencing note, not a status change: single-player Vertical-Slice-shaped content (see Phase 2, below) is being built ahead of the multiplayer technical prototypes this phase covers.

**[PROPOSAL]** Potential prototypes:
- Two connected players
- Networked movement
- Server-authoritative enemy
- Networked damage
- Basic ability execution
- Enemy-count performance testing

---

## Phase 2 — Vertical Slice

**Status: In Progress** — assessed against this phase's own goals below, independent of "Sprint 2.x" numbering (see the Terminology Note at the top of this document). Not complete.

**[PROPOSAL]** Restructured 2026-07-11 around three research clusters, each ending in an explicit Decision Gate, rather than one flat checklist. Phase 2 is complete when Cluster C's gate passes — not when a fixed sprint count is reached.

Deferred out of all three clusters: cosmetic character creation, networking/co-op implementation, leaderboards, production art/UI, seasonal infrastructure, full itemization/crafting/economy/endgame-map systems. None of the sprints below are blocked by these.

### Cluster A — First Real Build

Exists to answer: can Point Clear create meaningful Build Identity at all?

**Sprint 2.3 — Active Skill System Validation**
- Status: DONE (PC-007, playtest-approved 2026-07-11) — Fracture Bolt (projectile) + Detonation Field (area) built; the Active Skills build layer is validated as a real system with two mechanically distinct entries.
- Question: Can Point Clear support mechanically diverse Active Skills as a real system — not just one weapon with variations? (Answered: yes.)
- Dependencies: `PlayerXP`/`PlayerLevel`/`PlayerStats` (Sprint 2.0–2.2, DONE); `Health`/`EnemyAI`/`HitscanWeapon` (Sprint 1.x, DONE).
- Why before 2.4: Skill Point allocation needs at least two real, mechanically distinct skills to allocate points into. Which specific skills those are is a design decision made at sprint kickoff, not a roadmap commitment.
- Playable loop at end: two or more Active Skills exist, demonstrably mechanically distinct from each other and from the existing weapon; both usable, no persistent choice yet.
- Out of scope: Skill Points, selection screen, second build layer, stat-card systems, physical knockback, gamepad input, committing to specific skill identities beyond what's needed to prove the system works.

**Sprint 2.4 — Persistent Skill Points & Allocation** — ✅ **DONE (PC-008, playtest-approved 2026-07-11).** Data-driven Skill Point wallet + allocation registry; both Active Skills are rank consumers (Fracture Bolt damage/rank, Detonation Field radius/rank). "Persistent" here = run-persistent (no disk save this sprint). DEC-020 is now realized in code.
- Question: Does leveling up grant a real, spendable, persistent resource that produces an actual build choice — is DEC-020 true in code, not just in a placeholder comment? (Answered: yes, run-persistent.)
- Dependencies: Sprint 2.3.
- Why before 2.5: this is the first point two players' builds can actually diverge (CORE_PHILOSOPHY point 7) — a second layer has nothing to diverge from without it.
- Playable loop at end: kill → level up → earn a Skill Point → spend it on one of the Sprint 2.3 skills → relative skill power is a real, in-session player choice.
- Out of scope: Second build layer, disk persistence, respec, UI polish.

**Sprint 2.5 — Second Build Layer**
- Status: **DONE (PC-009, playtest-approved 2026-07-12).** Second layer = **Passives** (reusing the Sprint 2.4 skill architecture). Flagship interaction **Volatile Fracture**: while allocated, Fracture Bolt shards apply a Detonation Mark using Detonation Field's current-rank data — the first real cross-skill interaction. A per-skill `StartingLevel` was added mid-sprint (discovered blocker, GD-approved) so passives can start locked at 0 while actives start at 1. This is the last Cluster A sprint; the Cluster A Decision Gate is now evaluable.
- Question: Does a second build layer deepen player expression and the interaction space — and which of the seven DEC-013 layers is the right one to build second? Passives is the leading candidate (already named in the Loadout MVP scope) but the choice is made at sprint kickoff, not fixed here. (Answered: Passives, interaction-first.)
- Dependencies: Sprint 2.4 (reuses Skill Points as the spend-currency, avoiding a premature second currency system).
- Why before Cluster B: the "improve the build" loop leg needs more than one lever to pull, or the loop will feel thin when tested end-to-end.
- Playable loop at end: a second build layer exists, with at least one piece of content that creates a genuine interaction with the Sprint 2.3 skills — the first proof that Expanding the Interaction Space (CORE_PHILOSOPHY § Build Philosophy) isn't just theoretical.
- Out of scope: Any layer beyond the second; a full tree/UI for it; more than a handful of entries.

**Cluster A Decision Gate**
- Question: Did we prove that Point Clear can create meaningful Build Identity?
- Decision: If yes → proceed to Cluster B. If no → return to Sprint 2.3, 2.4, or 2.5 (repeat, expand, split, or reorder as needed), improve the system, then repeat this validation before proceeding.
- Relationship to Sprint 2.10: this gate tests whether Build Identity exists *at all* — the earliest, cheapest checkpoint, before Mission Risk or Loot exist. Sprint 2.10 (in Cluster C) re-tests a related but distinct, later question: not "does Build Identity exist" but "does the build space remain healthy — genuinely divergent, not converging on one dominant build — once equipment and additional systems have been layered on top of it." Passing this gate is a prerequisite for reaching Sprint 2.10, but does not guarantee Sprint 2.10 will also pass.

---

### Cluster B — Mission Risk and Reward Loop

Exists to answer: does Mission Risk create meaningful tension and player decisions?

**Sprint 2.6 — Minimal Mission Wrapper**
- Status: **DONE (PC-010, playtest-approved 2026-07-12).** A minimal **Operation** lifecycle (`Ready → InProgress → Success | Failure → Ready`) wraps the arena: start a run, **clear a small enemy-kill quota** to unlock the **Extraction Point**, reach it to succeed, die to fail, return to a clean neutral state that resets the encounter (not the character — DEC-016). Kills are counted via `EnemySpawner` tracking its spawns and subscribing to the existing `Health.Died`. No rewards/Zones/objectives yet. This is the first Cluster B sprint; Sprints 2.7 (unsecured rewards) and 2.8 (results) attach to its `OperationSucceeded/Failed` events.
- Question: Can a "mission" exist as a bounded session with explicit start/success/failure conditions, wrapping the existing combat loop — proving the full Operation/Zone system (still largely `[UNRESOLVED]` per DEC-011) doesn't need to exist yet to test mission risk?
- Vision note: the intended emotional core is the **greed-vs-safety** decision — rewards found during a run stay *unsecured* until the player banks them, so pushing deeper raises both the reward and the cost of failure (DEC-019, DEC-025). Even the minimal wrapper should express "secure what I have, or risk more?", not just a win/lose flag.
- Dependencies: Existing `PrototypeScene`/combat loop (Sprint 1.x). Sequenced after Cluster A by priority, not by hard technical dependency.
- Why before 2.7: rewards can't be meaningfully secured or lost without a mission boundary to secure/lose them at.
- Playable loop at end: enter a bounded mission → fight with the Cluster A build → mission ends in explicit success (placeholder win condition) or failure (player death) → return to neutral state. No rewards yet.
- Out of scope: Loot, currency, rewards of any kind; Operations/Zones formal structure; Lobby/Party UI; results polish.

**Sprint 2.7 — Unsecured Rewards & Mission Risk**
- Status: **DONE (PC-011, playtest-approved 2026-07-12).** Enemies **drop a physical currency pickup** on genuine death; collecting it (with a small configurable pickup radius) fills an **Unsecured** wallet; extraction **Success banks** it, **Failure loses** it, **Banked** persists across Operations (session, in-memory). Extraction opening while enemies keep spawning creates the first extract-now-vs-fight-more choice. XP retention already satisfied (immediate grant) — untouched. Attaches to the 2.6 Operation events without modifying the lifecycle. Next: Sprint 2.8 (minimal results summary) closes the Cluster B loop.
- Question: Does DEC-019 (loot lost on failure, XP retained) hold up as a felt mechanic, not just a rule on paper?
- Dependencies: Sprint 2.6; `PlayerXP` (already auto-retains per DEC-018, no change needed).
- Why before 2.8: a results summary has nothing to summarize until secure/lose logic exists.
- Playable loop at end: enemies drop a single simple currency (unsecured) during the mission; success banks it permanently, failure loses it; XP gained during the mission is retained per a rule matching DEC-019 (exact retained percentage remains `[UNRESOLVED]` in `DECISIONS.md` — this sprint implements the mechanism, not the number).
- Out of scope: Equipment/itemization, rarity tiers, drop-table tuning, inventory UI.

**Sprint 2.8 — Minimal Results Summary**
- Status: **DONE (PC-012, playtest-approved 2026-07-12).** The `OperationHud` terminal states expand into a minimal results summary: **Secured this run** (Success) / **Lost this run** (Failure), **Banked total** (cumulative, safe), and **Character Progress Retained** (Level / Skill Points, read-only) — making DEC-019's secure/lose/retain distinction legible. Reuses `CurrencyWallet` (read-only `LastSecured`/`LastLost`) + existing Operation events; no lifecycle change. Approved after hands-on review + two extra regressions (terminal race, repeat-run stability). **Cluster B is now complete; a whole-cluster review precedes the Cluster B Decision Gate.**
- Question: Can the player actually see what happened — secured, lost, retained — so "improve the build → enter harder content" has something to act on?
- Dependencies: Sprint 2.7.
- Why before Cluster C: Cluster C validates the integration of Build + Mission Risk + Progression — unobservable without a way to see a loop's outcome.
- Playable loop at end: the full loop closes for the first time — enter → fight with a real build → find unsecured currency → succeed/fail → see what was secured/lost/retained → return with a persistently improved character.
- Out of scope: Production UI, leaderboard submission, run history beyond the single most recent result, cosmetic polish.

**Cluster B Decision Gate**
- Question: Did Mission Risk create meaningful tension and player decisions?
- Decision: If yes → proceed to Cluster C. If no → return to Sprint 2.6, 2.7, or 2.8, improve the system, then repeat this validation before proceeding.

---

### Cluster C — Integrated Vertical Slice

Exists to answer: would players actually want to repeat this gameplay loop?

**Sprint 2.9 — Minimal Loot & Equipment Foundation**
- Status: **DONE (PC-013, playtest-approved 2026-07-12).** First equipment slot = the **Weapon layer** (DEC-013), realized as the **Detonator Module**: dropped physically on a deterministic mid-run kill, auto-equipped (immediate power), **secured on extraction / lost on death** (DEC-019), banked = session-persistent + no longer at risk + no re-drop. Its effect is an **interaction, not a flat stat** — while equipped, a weapon hit on a *marked* enemy detonates the mark early (Weapon ↔ Detonation Field ↔ Volatile Fracture); Detonation Field still owns marking. First Cluster C sprint. **Note:** this sprint is the experiment that informs the still-open **Cluster B Decision Gate** (does Mission Risk create meaningful tension / fear-of-loss?) — that gate is evaluated separately by the Game Director and is **not** called by this entry.
- Question: Can a build layer actually be acquired through Loot rather than fixed at start — does DEC-018 work end to end for at least one equipment slot? (Answered: yes, for the Weapon layer.)
- Dependencies: Sprint 2.7 (equipment drops plug into the existing mission-risk pipeline); Cluster A (equipment needs existing build content to interact with — enforced by the Build Content Alignment gate in `Documentation/AI/REVIEW_CHECKLIST.md`).
- Why before 2.10: the build-validation check needs equipment in the mix to be a meaningful test of the full accumulated build space.
- Playable loop at end: one equipment slot, droppable, subject to the same secure/lose rule as currency, interacting with existing build content rather than adding a flat stat.
- Out of scope: Full itemization, multiple slots, rarity tiers, crafting, vendor/economy systems, real inventory UI.

**Sprint 2.10 — Build Validation & Convergence Check**
- Question: Now that equipment and additional systems have been layered on top of the Cluster A build space, does that space remain healthy — are players still creating genuinely divergent builds, or has everything converged on one dominant combination?
- Dependencies: Sprints 2.3–2.5 (full build-layer stack) + Sprint 2.9 (equipment) — the first point the build space is complete enough to be worth measuring.
- Why before 2.11: per Expanding the Interaction Space (CORE_PHILOSOPHY § Build Philosophy), adding more mechanics before verifying the existing space produces divergence would be growing vertically before confirming horizontal health first.
- What this sprint is: not content work — structured playtesting across multiple sessions/players, observing which builds naturally emerge. Not a one-time pass; repeatable.
- Decision: If divergence holds → proceed to Sprint 2.11. If convergence on one dominant build is found → do not proceed to new mechanics; return to expanding the interaction space among existing layers (not new layers/mechanics), then repeat this check before continuing.
- Out of scope: any new mechanic, layer, or content — measurement and rebalancing of what already exists only.

**Sprint 2.11 — First Meaningful Enemy Variety**
- Question: Does a second enemy archetype create a genuine new combat/skill-validation need — not variety for its own sake?
- Vision note: enemies are **questions the world asks the build** — a new enemy must threaten through *behavior* (a new problem to solve), not through more health/damage, and must be readable at a glance (DEC-024). "Justify your existence": if it doesn't force players to think differently, it doesn't belong.
- Dependencies: Cluster A's build content (the new enemy must specifically challenge something that exists); Sprint 2.10 passed.
- Why before 2.12: a boss drawing on an already-proven enemy mechanic is more coherent than one invented from nothing.
- Playable loop at end: at least one new enemy type that tests something the current build space doesn't already test.
- Out of scope: A full bestiary, Elite/Mini-Boss tiers, balancing pass.

**Sprint 2.12 — First Boss**
- Question: Does accumulated build depth produce a felt difference in how a boss fight plays out between different builds?
- Dependencies: Cluster A (build depth to test) + Sprint 2.7 (the boss sits inside a mission's risk, not as a standalone fight) + optionally Sprint 2.11.
- Why before 2.13: cheaper to discover "the boss isn't fun yet" against the placeholder arena than after authoring real content around it.
- Playable loop at end: one hard encounter inside the mission loop, genuinely shaped by build choices, resolving into the same success/failure/results flow as any other mission.
- Out of scope: Multiple bosses, Mini-Boss tier, boss-specific loot tables, scripted/cinematic sequences.

**Sprint 2.13 — Small Authored Mission/Map**
- Question: Does the full proven loop hold up in a real, designed space instead of the greybox arena?
- Dependencies: Everything above — deliberately last, so content authoring follows systems validation rather than leading it (DESIGN_DNA's MVP Rule).
- Why it's last: this is the Phase 2 exit point, not a dependency for anything further in this list.
- Playable loop at end: the full `CORE_GAMEPLAY_LOOP` sequence — enter → fight → find rewards → succeed/fail → secure/lose → improve build → re-enter, harder — inside an authored space.
- Out of scope: Multiple maps, semi-procedural generation (DEC-011 still unresolved), formal Operations/Zones structure, production art.

**Cluster C Decision Gate**
- Question: Would players actually want to repeat this gameplay loop?
- Decision: If yes → Phase 2 (Vertical Slice) is complete; proceed toward Phase 3 planning. If no → return to the relevant Cluster C sprint(s) — or to Cluster A or B, if the issue traces back further — improve the system, then repeat this validation before declaring Phase 2 complete.

---

## Later Phases (Tentative)

**[PROPOSAL]** — listed for awareness only, not scoped:

- Pre-Alpha
- Alpha
- Beta
- Season 0
- Season 1

---

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [VISION.md](VISION.md)
- [DECISIONS.md](DECISIONS.md)
- [CHANGELOG.md](CHANGELOG.md)
