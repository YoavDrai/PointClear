# Point Clear — Decisions

This document is the persistent record of accepted decisions and open questions. When a decision changes, update its entry rather than deleting history where practical, and log the change in [CHANGELOG.md](CHANGELOG.md). Permanent design principles live in [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) — this document records when and with what boundaries each was formally approved, not the principle itself. See also: [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [ROADMAP.md](ROADMAP.md), [GLOSSARY.md](GLOSSARY.md).

---

## Accepted Decisions

**[APPROVED FACT]**

### DEC-001 — Multiplayer-First
Point Clear is multiplayer-first.

### DEC-002 — Player Count
The game supports 1–4 players.

### DEC-003 — Camera Perspective
The camera perspective is isometric.

### DEC-004 — Seasonal Competition
The game is intended to support seasonal competition.

### DEC-005 — Season Duration (subject to validation)
The planned season duration is approximately four months and remains subject to validation.

### DEC-006 — Leaderboard Categories
Leaderboards should support separate solo, duo, trio, and four-player categories.

### DEC-007 — Documentation as Source of Truth
Repository documentation is the persistent source of project context.

### DEC-008 — Definition of Done
No task is complete before playtesting, review, approval, and documentation updates.

### DEC-009 — Operation Definition
An Operation is a complete mission session. It begins after the player or party leaves the Lobby and ends when the party succeeds, fails, or returns to the Lobby. An Operation includes deployment, multiple connected Zones, combat and exploration, objectives, build development during the run, escalating danger, a final boss phase, extraction, and results before returning to the Lobby. An Operation is not merely a map — it is the full playable mission structure.

**Important boundaries:** The canonical term definition lives in [GLOSSARY.md](GLOSSARY.md); the Operation's place and sequence in a run lives in [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md). This decision does not specify how many Operations exist, their length, or how they are selected.

**Related unresolved questions:** exact number and selection of Operations; Operation length/pacing targets.

### DEC-010 — Connected Zones
Each Operation is composed of multiple large connected Zones rather than a single linear corridor. A Zone may contain multiple routes, combat encounters, objectives, events, optional risks and rewards, secrets, Elite encounters, rewards, and transitions to later Zones. The connected-Zone structure exists to provide freedom and exploration while preserving controlled pacing and progression.

**Important boundaries:** Exact Zone count per Operation, Zone size, and transition mechanics are not approved.

**Related unresolved questions:** Zone count and size targets; transition mechanics.

### DEC-011 — Semi-Procedural Operations
Operations use a semi-procedural structure. The identity, overall structure, major landmarks, and authored design of an Operation remain recognizable across runs. Individual runs may vary through routes, objective placement, events, enemy composition, Elite encounters, rewards, secrets, optional challenges, and environmental conditions. This is not fully procedural world generation — the goal is authored quality combined with run-to-run variation.

**Important boundaries:** Exact procedural-generation techniques remain **[UNRESOLVED]**.

**Related unresolved questions:** procedural-generation techniques and tooling.

### DEC-012 — Dynamic Objectives
Operations use dynamic objectives. An Operation may select from multiple objective types and combinations for a given run rather than using a fixed set. Objectives may be mandatory, optional, risk-versus-reward, Zone-specific, cooperative, or required to unlock progression toward the final boss. Objectives should make co-op meaningful rather than functioning only as generic combat markers.

**Important boundaries:** Exact number, selection rules, objective types, and frequency remain **[UNRESOLVED]**.

**Related unresolved questions:** objective type catalog; selection and frequency rules.

### DEC-013 — Layered Build System
Point Clear uses a layered build system. A player's build is created through combinations of several system layers rather than being defined by one item or one skill. The currently approved high-level layers are: Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, and Temporary Operation Effects. A build is defined by interactions between layers; build decisions should change gameplay, not only increase numerical values.

**Important boundaries:** Exact rules, slot counts, acquisition methods, and balance/rarity systems for each layer are not approved. Specific weapons, skills, mutations, relics, or synergies discussed previously are illustrations only, not approved content.

**Related unresolved questions:** per-layer rules; slot counts; acquisition methods; balance and rarity systems.

### DEC-014 — Core Philosophy Adopted
[CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) is adopted as the project's highest-level, permanent design document — the root every other design document traces back to. It establishes build diversity and player agency as the project's central thesis, supersedes the prior informal framing of Point Clear as primarily a "Roguelite" (see DEC-015), and formalizes persistent character progression, the Experience/Loot separation, and mission risk as permanent rules (see DEC-016 through DEC-019).

**Important boundaries:** `CORE_PHILOSOPHY.md` states permanent principles only — it does not specify implementation or individual systems. Changing a principle within it is itself a decision requiring a new or amended DEC entry.

### DEC-015 — Genre Identity
Point Clear's genre is a persistent, seasonal, cooperative Action ARPG — not a Survivors-like, not a traditional roguelite, and not a clone of any existing ARPG. It takes inspiration from the endgame philosophy of games like Diablo and Path of Exile but must be described by its own systems and identity. This replaces the prior "Action Roguelite RPG" label used in `PROJECT_BIBLE.md` and `VISION.md`.

**Important boundaries:** This is a label and identity decision, not a mechanical specification. It does not itself define any system's rules.

### DEC-016 — Character Persistence
A player's character — level, Experience, equipped Weapons, Skills, and owned Equipment — persists across missions. No individual mission resets a character. ~~Only a new Season resets character progression.~~ **[UNDER REVISION — see DEC-028]:** the "only a new Season resets character progression" clause is under revision; what a Season resets vs. carries is deferred to a future Seasons workshop. What is locked: **characters never disappear.**

**Important boundaries:** Exact save/persistence technical implementation is not specified here — it is a future technical requirement, not yet in scope for any current sprint. The seasonal reset boundary is under revision (DEC-028).

### DEC-017 — No Fixed Classes
Point Clear has no predefined gameplay classes (no Warrior/Mage/Rogue-style archetypes). Character creation is cosmetic only — appearance, not gameplay. Gameplay identity is created entirely through build: equipment, skills, passives, and upgrades.

**Important boundaries:** This does not preclude future starting-loadout variety (e.g., different starting weapons) as long as it remains a build choice, not a locked class.

### DEC-018 — Experience and Loot Separation
Experience and Loot are permanently separate systems. Experience is automatic progression: enemy deaths grant it immediately, and it is never represented as a physical pickup. Loot is the physical, RNG-based reward — gold, currency, equipment, crafting materials, rare and legendary items. The two must never be merged into one mechanic.

**Important boundaries:** This decision does not specify loot tables, drop rates, rarity tiers, or itemization — all remain **[UNRESOLVED]** pending a future Loot/Itemization system design.

### DEC-019 — Mission Risk and Reward Security
Rewards earned during a mission (Loot, Gold, other in-mission rewards) are not permanently owned until the mission is completed successfully. Mission failure loses them. Failure must not erase all long-term progression — some earned Experience is retained regardless of outcome.

**Important boundaries:** The exact retained-Experience formula/percentage, and the exact definition of "mission failure" (e.g., party wipe vs. abandonment vs. timeout), are **[UNRESOLVED]**.

### DEC-020 — Level-Up Grants Persistent Build Potential Only
Character Level, total Experience, and earned Skill Points are persistent character progression within a Season (extends DEC-016) — they persist mission-to-mission and reset only at a new Season. Leveling up grants persistent build-unlocking currency (a Skill Point) to the character; it never grants a temporary in-mission stat bonus, and it does not pause the mission for an in-mission upgrade-selection screen. In-run power growth during an Operation instead comes from Loot, Relics and Mutations discovered during that run, and Temporary Operation Effects (DEC-013) — not from Level or Experience. This supersedes the "choose 1 of 3 upgrade selection tied to leveling" description previously in [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Upgrade Selection (now § In-Run Power Growth), which has been revised to match.

**Important boundaries:** This does not specify exact Skill-Point-to-Skill/Passive mechanics, acquisition rate, or respec rules — remain **[UNRESOLVED]** pending a future Skill Point Allocation sprint. This does not resolve whether Mutations, Relics, or Temporary Operation Effects persist between missions once found — those individual layer persistence rules remain **[UNRESOLVED]** (see [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)). This does not imply any save system has been implemented — persistence of Level, Experience, and Skill Points is the approved design model; the actual technical save/persistence implementation remains a separate, not-yet-scoped requirement (same boundary as DEC-016).

### DEC-021 — Finite Build Budget
A character's long-term progression (the skill tree layer) is a **finite** budget of permanent trade-offs, not an eventually-fillable checklist. A character can travel toward almost any direction, but can only own a small part of the total space at once; choosing one direction necessarily means giving up others. This is the mechanism behind [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) point 10 (meaningful choices close paths), and it is what makes build identity durable rather than converging on a single generalist. The starting archetype is a *starting vector*, not a class (extends DEC-017). Respec exists as *costly reinvention* (never a free meta-switch), and is more forgiving during early onboarding.

**Important boundaries:** The exact point budget, tree shape, respec cost curve, and archetype starting positions remain **[UNRESOLVED]**. The current prototype's "accumulate points, eventually max everything" trend is superseded by this decision as a design direction, not as an urgent implementation change.

### DEC-022 — Solo/Co-op Experience Parity and "Want, Not Need"
Solo and co-op are both first-class ways to play. Builds are self-sufficient — players **want** each other because together they create experiences that cannot exist alone, not because survival *requires* teammates (no mandatory roles; extends DESIGN_DNA § Multiplayer Philosophy). There is **no co-op-exclusive content or progression** — all content belongs to every player regardless of mode; co-op's distinct value is experiential (stories, interactions, moments). Parity is *experience* parity, not identical efficiency: **the efficiency gap between solo and co-op must stay small enough that players choose their mode for the experience they want, not because the game pressures them into the more efficient option.** A small, earned co-op delta is acceptable; players who cannot group must still reach all content, build any identity, and enjoy the full progression journey.

**Important boundaries:** Exact reward/scaling numbers, loot-sharing rules, and the push-or-bank *disagreement* resolution remain **[UNRESOLVED]** and are expected to be informed by playtesting. This constrains future Economy and Trading design (they must not push players into one mode).

### DEC-023 — Build Diversity Comes From Identity
Build diversity comes primarily from players identifying with their own build, not from party-composition incentives. The world's role is to **validate** the identities players have chosen, not to **dictate** them. Diversity is seduced, never enforced — a party of four identical builds always remains viable. The health target is **continued emotional validation** (every identity periodically feels special), not equal representation; no attach-able identity is permitted to *permanently* disappear. Division of responsibility: **Build Design** creates identities worth loving; **the World** continually expands the space of meaningful questions; **Live Service** ensures no identity quietly disappears.

**Important boundaries:** The identity-health mechanisms (metrics, thresholds, feedback loops, spotlight rotation) are **[UNRESOLVED]** and intentionally deferred until the game is live with real player data.

### DEC-024 — The World Asks the Questions (Soft Counters, Readability Governor)
Build diversity is validated by a diverse *problem-space*: enemies, mission objectives, environment, route, depth, and modifiers all pose "how does your build solve me?" Difficulty deepens primarily through *demand* (compositions, objectives, hazards, fewer recovery opportunities), not bigger numbers; enemies threaten through **behavior**, not stats. Matchup texture is **soft — never hard gates** (forced by DEC-021: a finite build must be viable everywhere without being optimal everywhere). **Readability is the governor on growth**: complexity may grow only as fast as it remains readable — if an interaction needs a wiki, it has outrun readability. Every new enemy/question must introduce a genuinely new problem; every new build option must solve problems *differently*, not just bigger.

**Important boundaries:** Specific enemy behaviors, modifier catalogs, and difficulty curves remain **[UNRESOLVED]**.

### DEC-025 — Combat Feel
Combat's baseline mood is **competence** (players feel capable; threat challenges confidence, it does not replace it; death reads as "we pushed too far," never "the game cheated"). The rhythm is **flow → threat → recovery**, tilting toward threat with depth. An expedition/Operation should generate **attrition** — a felt sense of increasing exposure the deeper a run goes (mechanism open). Build legibility is **kinesthetic**: players feel their build every few seconds, and changing a build changes how combat *feels*, not just the numbers.

**Important boundaries:** The attrition mechanism (health/resources/hazards/other) and all tuning remain **[UNRESOLVED]**.

### DEC-026 — Content Model: Authored Questions, Generated Answers
Refines DEC-011. The world presents **authored, readable categories** (which give the player agency to choose and let veterans imagine their build against a known kind of challenge) wrapped around **generated, hidden instances** (which preserve curiosity and wonder): *reveal the category, hide the instance.* Long-term novelty is sustained by an **expanding, seasonally-refreshed vocabulary of problem-primitives that combine**, not by hand-authoring every encounter. The object of player curiosity is expected to migrate over a character's life from the world ("what's out there?") to the build×world interaction ("what happens if I bring *this* build out there?").

**Important boundaries:** Authored-vs-generated boundaries, generation guardrails (honoring the category, respecting soft-counters), and tooling remain **[UNRESOLVED]** (see DEC-011).

### DEC-027 — Onboarding: Directional Possibility
The bridge emotion for a new player's first hours is **possibility** (supported by competence, driven by curiosity, seeded by ownership). It is **directional** possibility ("a direction you're excited to become"), not unlimited possibility — honest about the finite budget (DEC-021). A new player's sense of ownership comes primarily from the **first moment a choice is validated in gameplay**, not from the selection screen. Onboarding's purpose is to convince the player that the journey of building their character is worth taking — not to teach the entire game.

**Important boundaries:** Concrete first-session flow, tutorialization, and pacing remain **[UNRESOLVED]**.

### DEC-028 — Character Permanence Across Seasons; Season Reset Under Revision
**Characters never disappear.** Character attachment is a core pillar of Point Clear. Whatever a Season resets, it never *deletes* a player's character. **This places DEC-016's clause "Only a new Season resets character progression" — and the identical statements in [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) (points 22, 25) and [VISION.md](VISION.md) — UNDER REVISION.** The exact seasonal model (what resets vs. carries, seasonal vs. permanent character, economy reset) is intentionally **[UNRESOLVED]** and deferred to a dedicated Seasons workshop; only "characters never disappear" is locked now. Per methodology, this problem is not to be solved before it is ready to be solved.

**Important boundaries:** No seasonal reset/carry model is approved by this decision. Do not implement or assume one.

### DEC-029 — Emotion-First Design Methodology Adopted
Point Clear's design process is emotion-first: **Emotion → Philosophy → Mechanic (smallest that produces it) → Validation (pre-registered, falsifiable) → Constraints → Kill/Defer → Identity Filter** ("does this deepen Point Clear's soul?"). It is a loop, not a waterfall. The full method and its guardrails live in [DESIGN_DNA.md](DESIGN_DNA.md) § Design Methodology.

**Important boundaries:** This is a process decision; it does not itself specify any game system.

### DEC-030 — Build Divergence Has Two Timescales; Sprint 2.10 Is a Behavioral Divergence Check
**[APPROVED — CURRENT EXPERIMENTAL INSTRUMENT, REVISABLE — not immutable doctrine]** (2026-07-12)

**The model.** Build divergence is not one thing. It has **two kinds on two timescales**, and *both* world pressure and scarcity contribute to *both* — so the tempting split "world creates divergence, the finite budget creates commitment weight" is **rejected as inaccurate** (DEC-021's own text gives the finite budget an anti-generalist / divergence-preservation role). What is clean is the **timescale** separation, not a per-system responsibility split:
- **Behavioral divergence (near-term):** *how* players engage the world's questions while progressing. Primarily **seduced by world pressure** (DEC-023/DEC-024), riding on in-moment friction (cooldowns, attention).
- **Structural / terminal divergence (long-term):** whether *finished* builds avoid collapsing to one generalist. This is where the **finite budget (DEC-021)** does its work.

**DEC-021 scope clarification (by reference; DEC-021's text is not changed):** the finite budget owns **commitment weight** and the **terminal/structural** convergence-prevention role. It is **not** the near-term behavioral-divergence mechanism and is **not** a prerequisite for the behavioral experiment.

**Sprint 2.10 is redefined** from a terminal build-convergence check to a **Behavioral Divergence Check**: it measures *how players engage the world's questions*, not their unlocked or optimized loadouts. It requires ≥2 genuinely different world-questions, so **Sprint 2.11 now precedes it** (see [ROADMAP.md](ROADMAP.md)).

**The instrument (current, revisable — the falsifiable core):**
- **Probe A — within-subject (primary):** same player, two contrasting builds; does their engagement of the same enemies change? This isolates the build's effect from the player's personality and is the causal spine (it is DEC-025's "changing a build changes how combat feels" made testable).
- **Probe B — between-subject (corroboration only):** several players, same start; record first tool reached for per enemy, hold-vs-reach, first investments, one-sentence self-described playstyle.
- **Decision rule (pre-registered):** **PASS** (build-sensitive engagement + durable cross-situational spread) → proceed. **FAIL** (behavior flat across builds, or convergence on one approach as pure stimulus-response) → expand the interaction space among existing layers, re-run. **INCONCLUSIVE** (preconditions unmet: <2 real questions, a dominant approach, sample too small/short) → not yet runnable. **Trust a PASS more than a FAIL** — FAIL is provisional (identity may not have formed yet).
- **Knowingly-accepted biases** (accepted because this is a design instrument for a solo developer, not an academic study): personality leaks into Probe B; small sample / no statistics; non-blind coding; loyalty-vs-not-yet-learned is ambiguous early; self-report is confabulation-prone; identity may form slower than the test or along unwatched axes. Mitigated — not eliminated — by the within-subject spine, the pre-registered rule, behavior-over-self-report, and the trust-PASS-over-FAIL asymmetry.

**Relationship to CORE_PHILOSOPHY (scope note, no doc rewrite):** point 7 ("two players… end up with different builds") is the **terminal** claim, validated only by the later structural experiment; the behavioral check validates **DEC-023/DEC-024**, not point 7. A green behavioral result must not be reported as terminal validation.

**Important boundaries:** This freezes the *current working instrument*, expected to evolve. It **reopens only if playtesting shows the instrument itself is misleading** — not for further abstract refinement. It does **not** schedule or design the terminal/structural experiment, does **not** implement the finite budget, and does **not** edit CORE_PHILOSOPHY or DEC-021 text (their scope is clarified here by reference). The terminal experiment remains a deferred, much-later milestone (requires DEC-021 implemented + a substantially larger build space).

### DEC-031 — Front-End Vertical Slice Architecture (Single-Scene Layered Canvas)
**[APPROVED FACT]** (2026-07-13, PC-015 — playtest-approved)

The first complete player journey (Main Menu → Character Creation → Starting Direction → World Map → Operation → Results → World Map → Main Menu) is implemented as a **greybox front-end that wraps the existing gameplay loop**, under these durable architectural decisions:

- **Single scene, layered Canvas.** All front-end screens are uGUI Canvas layers inside the existing `PrototypeScene` (a multi-scene design was evaluated and rejected). The scene footprint is a single `FrontEnd` GameObject; the UI is built in code (`UIFactory`/`FrontEndUI`). Rationale: the World-Map↔Operation loop then reuses the **existing non-reloading Operation lifecycle** (`StartOperation`/`ReturnToReady` + events), so Level / XP / Skill Points / Banked currency / secured Weapon Module are preserved with **no changes to progression or combat code** (consistent with DEC-016).
- **Scene reload = the only New-Character reset boundary.** Starting a new character reloads the active scene once, producing a fresh Level-1 progression state, rather than adding manual reset APIs across the progression systems.
- **`SessionContext` is a single static seam** for temporary, in-session state (character name, preset, confirmed node, navigation) — deliberately **not** a save system, dependency-injection container, service locator, or account model. It is designed not to block future up-to-four-player support (DEC-002).
- **`CombatBridge` is the single seam** between front-end and gameplay: it touches combat only through existing public surfaces (never duplicating `OperationController` logic), gates player input + combat HUDs, applies the character (preset color + name), and begins/returns the Operation.
- **Entry is via the New Character flow.** Pressing Play starts a fresh temporary character; there is no Continue/persistence yet.

**Important boundaries:** This is a **greybox** slice — not final UI/art, settings, localization, or platform integration, and **not** save/load, accounts, character slots, run history, or a real map/skill-tree. Persistence and **basic-weapon-only start** (Active Skills earned later rather than available from Level 1) are recorded as **deferred follow-ups**, not decided here. This decision governs front-end architecture only; it changes no gameplay or design rule.

### DEC-032 — Death XP Penalty: Flat % of the Current Level Bar, Never De-Levels
**[APPROVED FACT]** (2026-07-13, PC-016 — Game-Director directed; playtest-approved 2026-07-13)

Resolves the number left open by DEC-019 ("exact retained-Experience amount on mission failure"). On Operation **failure by player death**, the character loses only **progress into the current level**, by a **configurable flat fraction of that level's XP bar** (default **20%** — `DeathXPPenalty.penaltyFraction`); the amount is capped at the current progress so the total can **never drop below the current level's floor**. **Levels, total earned Skill Points, Skill Tree allocations, and Banked rewards are never lost.** Example at 0.2: Level 10 at 40% → Level 10 at 20%.

This **supersedes the prior "XP fully retained on failure"** behaviour that was live since Sprint 2.7 (a plain reading of DEC-019's placeholder). It remains consistent with [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) points 21–24: characters persist, no mission removes a level, and failure never erases *all* long-term progression — a real but bounded cost. The design is drawn from the extraction/ARPG genre's convention of losing sub-level progress on death; it is Point Clear's own rule, not a copy of any specific game's numbers.

**Architecture:** implemented as a run-scoped rule (`DeathXPPenalty`) that subscribes to the existing `OperationController.OperationFailed` event and never modifies the Operation lifecycle — the same secure/lose pattern as `CurrencyWallet` and `WeaponModule`. `PlayerXP` gains a level-agnostic `RemoveXP`; the "never de-level" cap lives in `DeathXPPenalty`, which reads `PlayerLevel`'s current-level boundaries. Single-player scope: it penalises the local player only; a future party-wipe rule changes only this component.

**Important boundaries:** The **fraction is prototype tuning, not a balance decision** — the exact value is expected to move with playtesting. This decides the *failure* penalty only; it does not change XP gain, the XP curve, or any reward-securing rule. Death **outside** an Operation (the prototype respawn loop) is intentionally **not** penalised. This is off the pre-existing roadmap sequence (inserted at Game-Director direction) and is **playtest-approved (2026-07-13)**. This resolves a number, not the whole persistence model: no save system is implied (DEC-016/020 boundary stands).

### DEC-033 — Player-Driven Character-Start Skill Allocation (Data-Driven Starting Skills)
**[APPROVED FACT]** (2026-07-13, PC-016 — Game-Director directed; playtest-approved 2026-07-13)

A new character no longer begins with its Active skills pre-selected. Instead, before the first run the **Starting Direction screen becomes the initial Skill Point allocation step**: the player spends their starting Skill Points (DEC-020 / 2 by default) on **starting skills**, then explicitly confirms to proceed. This realises [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) build-identity intent (the build is *chosen*, a "starting vector, not a class" — DEC-021) and closes the starting-skill item PC-015 explicitly deferred.

Rules (all Game-Director-decided this task):
- **Data-driven starting set.** Which skills are offered at start is a per-asset flag, `SkillDefinition.AvailableAtCharacterStart` — **not hardcoded**. Add/remove starting skills by toggling the flag, no code change. Current starting set: Fracture Bolt + Detonation Field (both `StartingLevel` moved 1 → 0); the Volatile Fracture passive is **not** a starting skill.
- **Active skills are gated on rank ≥ 1.** Each Active skill only activates once allocated — an unallocated skill genuinely cannot be used (previously it fired at rank-0 tuning, the gap PC-015 noted). So a fresh character is **basic-weapon-only** until they invest.
- **Spending is optional; confirmation is explicit.** The player may begin with points unspent (a valid weapon-only start) but must press Confirm — the first run cannot begin until that confirmation (`SessionContext.InitialAllocationConfirmed`). This is the *character-start* step only; auto-opening the tree on every in-run level-up is a separate future flow.
- **Non-start skills stay reachable through normal progression** (this task **does not** introduce a hard unlock lock — that would regress the already-approved Volatile Fracture passive). A future "hard unlock through progression nodes" mechanism is left as a seam, not built.

**Architecture:** reuses the existing `SkillProgression` allocation core through the `CombatBridge` seam only — **no second skill-tree system** (constraint honoured). The front-end never touches the skill system directly. Future save/load is seam-ready: a loaded character sets `InitialAllocationConfirmed` (skipping the forced step) — same idea as `SkillPoints.LoadSavedBalance` (DEC-016/020 boundary stands; no save system implemented).

**Important boundaries:** Greybox UI (reuses the front-end's button pattern; not final skill-tree UI). The starting-skill *set*, point count, and node/tree shape remain prototype choices, not balance/DEC-021-budget decisions. **Playtest-approved (2026-07-13).**

**Clarification (2026-07-13, folded in by DEC-034 sync):** the Skill Tree is part of the character's **long-term identity**, not a per-run Loadout choice. The intended flow is Character Creation → **Initial Skill Tree Allocation** → Operation Loop, and a player may inspect/allocate earned Skill Points later during gameplay through the Skill Tree (e.g. via a dedicated input such as Tab). The exact in-game skill-tree UI behaviour is not locked. This supersedes any prior framing of a pre-run "Starting Skill" that is simply handed to the player from a Loadout.

### DEC-034 — Current Prototype Proving Scope (The Arena Loop)
**[APPROVED FACT]** (2026-07-13, Game-Director directed)

The current prototype is intentionally a **compressed version of the full game**, built to prove one thing: **is the Arena gameplay loop fun enough that players immediately want to play another Run?** Everything else waits on that answer.

**Prototype terminology (current scope only — these do not replace the long-term Operation/Zone/Map terms):**
- **Arena** — the physical combat space. **The Arena is NOT infinite** — it is one bounded space.
- **Run** — one complete visit to the Arena (enter → fight → extract → results).
- **Cycle** — five Runs.
- **Difficulty Tier** — the progression level reached after completing a Boss Cycle.

**What is infinite is the sequence of Runs and Difficulty Tiers, not the Arena.** The proving-target loop:

**Character Creation → Initial Skill Tree Allocation → Enter Arena → Fight → Extraction → Results → Re-enter Arena → Repeat**, with every 5th Run a Boss Run and the Difficulty Tier rising each completed Cycle (DEC-035, DEC-036).

Until this loop is proven fun, the prototype **deliberately does not build**: multiple Operations, **multiple Arenas**, connected Zones, semi-procedural generation, dynamic objective variety, the Lobby/Party hubs, world/campaign progression, or **story**. The current Arena has a simple objective (currently a kill target) that ends at the Extraction Point.

**This does NOT delete or downgrade the long-term vision.** The full multi-Operation, multi-Zone, dynamic-objective, Lobby/Party, boss-per-Operation, world-progression, seasonal game (DEC-009 through DEC-013, DEC-022 through DEC-026, and [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)) **remains the approved long-term target**; those decisions stand and are simply **out of current prototype scope**. The long-term terms **Map, Zone, and Operation are unchanged** and continue to describe the final game; "Arena" is a prototype-scope term only. Canonical docs now distinguish **Long-Term Game Vision** from **Current Prototype / Proving Scope** rather than presenting the full vision as the immediate definition.

**Important boundaries:** This is a *scope-sequencing* decision, not a change to any long-term rule. What "proven fun" means is the Game Director's call (playtest-driven). No new gameplay systems are approved by this entry.

### DEC-035 — Five-Run Boss Cadence (Prototype Loop)
**[APPROVED FACT]** (2026-07-13, Game-Director directed)

Within the current prototype loop (DEC-034), a **boss recurs on a five-run cadence**:
- **Runs 1–4:** normal escalating runs, each ending in a successful **Extraction** that secures the run's rewards.
- **Run 5 — Boss Run:** the boss is the run's **main objective**. Flow: enter the Boss Run → reach/fight the boss → defeat it → **boss reward drops** → the Extraction Point becomes available → **extract successfully**.
- Defeating the boss **does not replace Extraction** — the player must still extract to secure the run. The permanent principle **"the boss is not the finish line — Extraction is"** ([DESIGN_DNA.md](DESIGN_DNA.md) § Extraction) therefore remains true.
- After a successful boss extraction, the **difficulty tier increases** and the five-run cycle begins again.

**Important boundaries:** The **boss reward system is future work — documented as *intended*, not designed or implemented here.** The number "5", the boss's exact encounter design, and its rewards are prototype targets subject to playtesting, not locked values. Relationship to the long-term vision: the full game's "one Mini Boss → Boss per Operation, before Extraction" (DEC-009, [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)) is a *different, later* structure; this cadence is the prototype's way of proving boss pacing inside the compressed loop and does not overwrite it.

### DEC-036 — Gentle Onboarding & Cross-Cycle Difficulty Escalation
**[APPROVED FACT]** (2026-07-13, Game-Director directed)

The prototype loop's difficulty is governed by these **permanent design rules** (not locked numbers):
- The **first runs must be gentle and easy to understand** (learn controls, small objective, simple enemies).
- **Early runs introduce mechanics gradually** — one new idea at a time.
- **Enemy density, behavioural variety, and objective pressure increase across the cycle.**
- **The fifth run is the boss checkpoint** (DEC-035).
- **Each completed boss cycle increases difficulty** (a new, harder tier).

Illustrative curve (an **example** of the desired shape — **NOT locked tuning**): Run 1 — simple enemies, small kill target, learn controls; Run 2 — more enemies; Run 3 — a new enemy behaviour; Run 4 — higher intensity; Run 5 — boss. This complements the existing **within-run** escalation (the kill-driven phase ramp in `EnemySpawner`) with a **cross-run** escalation axis, and it operationalises the onboarding principle in DEC-027 / [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Onboarding (ownership is still *validated in gameplay* during the first runs — the initial allocation screen only sets the choice up).

**Important boundaries:** **Exact enemy counts, kill thresholds, phase timings, and scaling values remain configurable and subject to playtesting** — example figures discussed in design (e.g. 10 / 25 / 30 / 50 / 100) are illustrations of a curve *shape*, never permanent values. This entry owns the difficulty-shape rule only; specific tier-scaling formulas are unresolved.

### DEC-037 — Canonical Gameplay Direction Frozen
**[APPROVED FACT]** (2026-07-13, Game-Director directed)

The gameplay direction established by the 2026-07-13 Gameplay Vision Sync is **frozen as the project's canonical gameplay direction.** Specifically, the following now stand as settled canon and are not to be re-opened casually:
- The **two-layer model** — Long-Term Game Vision vs Current Prototype / Proving Scope (DEC-034).
- The **prototype Arena loop, five-Run boss cadence, and gentle-onboarding / cross-Cycle escalation** (DEC-034, DEC-035, DEC-036) and the terminology **Arena / Run / Cycle / Difficulty Tier**.
- The **canonical gameplay loop** as documented in [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md), and the permanent rules in [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md), [GAME_PILLARS.md](GAME_PILLARS.md), and [DESIGN_DNA.md](DESIGN_DNA.md).

**Evaluation rule (binding):** future gameplay proposals must be **evaluated against this vision, not used to redefine it.** A new proposal is assessed by how well it serves the frozen direction (does it help prove the Arena loop is fun? does it create/improve/reward/expand build diversity? does it respect the two-layer scope?) — it is **not** an occasion to restate or replace the direction. Any change to the frozen direction itself is a deliberate act that requires explicit Game-Director approval and a **new DEC entry that supersedes the relevant one(s)** — it must never happen silently or as a side effect of a feature proposal. This mirrors how [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) and [GAME_PILLARS.md](GAME_PILLARS.md) are already treated.

**Important boundaries:** "Frozen" means *stable and canonical*, not *forbidden to ever revisit* — playtest evidence remains the legitimate trigger for a future, explicit, GD-approved revision. Freezing the *direction* does not lock *tuning* (enemy counts, thresholds, tier scaling, exact boss/reward design remain open and playtest-driven per DEC-035/036). This entry adds no gameplay system; it is a governance decision about how the direction is used.

---

## Unresolved Decisions

**[UNRESOLVED]** — none of the items below have been decided. Do not implement or assume a default for any of these without explicit Game Director approval.

**Scope note (DEC-034):** several items below concern **long-term-vision systems that are intentionally out of the current prototype's proving scope** — notably multiple Operations, Zones and Zone transitions, procedural generation, dynamic-objective variety, and the Lobby/Party hubs. These remain genuinely unresolved *as future design*, but they are **deferred, not actively open questions for the current prototype** (which uses a single repeating **Arena** — DEC-034). Do not treat them as near-term work.

- Networking solution
- Hosting model
- Backend service
- Leaderboard verification model
- Exact number of simultaneous enemies
- Exact progression structure — high-level model now resolved (persistent character, XP/Loot separation, mission risk, Level-Up grants persistent build potential only — DEC-016 through DEC-020); exact skill trees, Skill Point acquisition/spend rules, XP curve, and loot tables remain open
- Whether Mutations, Relics, or Temporary Operation Effects persist between missions once found (DEC-013, DEC-020) — individual layer persistence rules remain unresolved
- Exact season rules — cadence resolved (DEC-004/005); the reset/carry model is **under revision** and deferred to a future Seasons workshop; only "characters never disappear" is locked (DEC-028). Exactly what (if anything) carries over between Seasons remains open
- Exact primary leaderboard metric
- Monetization model
- Initial release platform and store
- Exact number and selection of Operations; Operation length/pacing targets (DEC-009)
- Zone count and size targets; Zone transition mechanics (DEC-010)
- Procedural-generation techniques and tooling (DEC-011)
- Objective type catalog; objective selection and frequency rules (DEC-012)
- Build layer exact rules, slot counts, acquisition methods, balance and rarity systems (DEC-013)
- Whether "Mission" replaces, splits from, or is another name for "Operation" (raised by recent design discussion; not resolved by DEC-014 through DEC-019)
- Loot tables, drop rates, rarity tiers, and itemization (DEC-018)
- ~~Exact retained-Experience amount on mission failure~~ — **resolved by DEC-032** (flat % of the current level bar, default 20%, never de-levels; the *value* remains prototype tuning). The exact definition of "mission failure" beyond player death (DEC-019) is still open
- Character persistence save/technical implementation (DEC-016, DEC-020) — no save system exists yet; persistence is the approved design model only, not an implemented feature
- **Reward Philosophy [FUTURE DESIGN — backlog, 2026-07-13].** The project still needs to define its Reward Philosophy, answering the primary question: **"What is the primary reason the player wants to immediately begin another Run?"** This future design work must define the long-term relationship between **Loot, Character Progression, Build Growth, Boss Rewards, and Endgame Motivation**. It underpins the Golden Rule ([Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)) and the boss-reward system left open by DEC-035. No implementation or further documentation is requested yet — this is a recorded backlog item only.

---

## Adding a New Decision

Use [Templates/DECISION_TEMPLATE.md](Templates/DECISION_TEMPLATE.md). Assign the next sequential `DEC-XXX` ID, and move the corresponding item out of "Unresolved" if it resolves one.

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [VISION.md](VISION.md)
- [ROADMAP.md](ROADMAP.md)
- [CHANGELOG.md](CHANGELOG.md)
- [GLOSSARY.md](GLOSSARY.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
