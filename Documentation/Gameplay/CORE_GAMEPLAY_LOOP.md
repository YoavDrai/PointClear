# Point Clear — Core Gameplay Loop

**[APPROVED FACT]** — Version 1.3 (2026-07-13: added the Long-Term-Vision vs Current-Prototype distinction and the compressed proving loop — DEC-034/035/036)

**[FROZEN CANONICAL — DEC-037]** This document is the project's canonical gameplay direction. Future gameplay proposals are **evaluated against** this vision, not used to redefine it; any change to the direction itself requires explicit Game-Director approval and a superseding DEC. Tuning values remain open and playtest-driven.

This document defines the complete gameplay loop of Point Clear: the player's journey from entering the game until starting the next run. This is a design blueprint, not a technical document — every gameplay system must support this loop. This document owns the Operation's place and sequence in a run; canonical term definitions live in [GLOSSARY.md](../../GLOSSARY.md). The permanent rules this loop must obey — persistent characters, automatic experience, loot as reward, mission risk — are defined in [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md); this document does not restate them, only sequences them. See also: [DESIGN_DNA.md](../../DESIGN_DNA.md), [GAME_PILLARS.md](../../GAME_PILLARS.md), [VISION.md](../../VISION.md), [ROADMAP.md](../../ROADMAP.md), [DECISIONS.md](../../DECISIONS.md).

**Terminology note:** this document uses "Operation" per [DEC-009](../../DECISIONS.md), the currently approved formal term. Recent design discussion has used "mission" for the same or a closely related concept informally. Whether "Mission" replaces, splits from, or is simply another name for "Operation" is **[UNRESOLVED]** and is deliberately not decided by this pass — see [DECISIONS.md](../../DECISIONS.md) Unresolved Decisions.

## Purpose

This document describes the player's journey from entering the game until starting the next run. It is the design blueprint for the entire game. Every gameplay system must support this gameplay loop.

## The Golden Rule — Run Success Test

Every run must answer one question: "Do I want to start another run immediately?" If the answer is no, the gameplay loop failed.

## Two Layers: Long-Term Vision vs Current Prototype Scope

**[APPROVED FACT] — DEC-034.** This document describes two things that must not be conflated:

- **Long-Term Game Vision** — the full game: a Lobby/Party hub, multiple selectable Operations, each a multi-Zone, semi-procedural, dynamic-objective journey with Elites, a Mini Boss and a Boss before Extraction, plus world/season progression. This is the approved destination and is **kept intact** below.
- **Current Prototype / Proving Scope** — a deliberately **compressed** version built to prove the core loop is *fun* before the world is expanded (DEC-034). It runs in a single, repeating **Arena** (see terminology below). Everything in the Long-Term Vision that is not part of the prototype (Zones, multiple Operations, multiple Arenas, dynamic objectives, Lobby/Party, world progression, story, campaign) is **deferred, not cancelled**.

**Prototype terminology (current-scope only — these do not replace the long-term Map/Zone/Operation terms):** **Arena** = the physical combat space (bounded, **not infinite**); **Run** = one complete visit to the Arena; **Cycle** = five Runs; **Difficulty Tier** = the progression level after completing a Boss Cycle. What is infinite is the **sequence of Runs and Difficulty Tiers**, not the Arena.

Sections below marked **[LONG-TERM VISION]** describe the destination; sections marked **[CURRENT PROTOTYPE]** describe what the prototype is actually proving now.

## High-Level Loop — [LONG-TERM VISION]

Main Menu → Lobby → Party → Loadout → Select Operation → Deploy → Explore Zones → Fight → Complete Objectives → Gain Experience → Find Loot & Temporary Effects → Grow Stronger → Face Increasing Danger → Mini Boss → Continue Building → Boss → Extraction → Results → Permanent Progression → Lobby → Repeat.

## Current Prototype Loop — [CURRENT PROTOTYPE]

**[APPROVED FACT] — DEC-034, DEC-035, DEC-036.** The prototype proves this compressed loop first:

**Character Creation → Initial Skill Tree Allocation → Enter Arena → Fight → Extraction → Results → Re-enter Arena → Repeat.**

- **Character Creation** (cosmetic only) happens once, up front, before any Run.
- **Initial Skill Tree Allocation** — the new character's first pass at its **Skill Tree** (see § Loadout / Skill Tree, below): 2 starting Skill Points, spent by the player (or explicitly left unspent) before the first Run. Active skills are **not** auto-granted.
- **Enter Arena → Fight → Extraction → Results** — the player enters the **Arena**, fights an escalating encounter, completes the current objective (currently a kill target), reaches the **Extraction Point** to secure the Run's rewards, and sees Results. Every normal Run ends in Extraction.
- **Re-enter Arena → Repeat** — the same Arena is re-entered for the next Run, harder.

**Five-Run boss cadence (DEC-035):** Runs 1–4 are normal escalating Runs each ending in Extraction. **Every 5th Run is a Boss Run**, whose flow is:

**Enter Arena → Reach Boss → Defeat Boss → Boss Reward → Extraction → Difficulty Tier +1 → New Cycle begins.**

The boss is the Boss Run's main objective, but **defeating the boss is never enough by itself — Extraction always remains the true finish line.** After a successful Boss Run extraction the **Difficulty Tier increases** and a new Cycle begins. (The boss *reward* system is intended future work — not designed here; see the Reward Philosophy backlog item in [DECISIONS.md](../../DECISIONS.md).)

**Gentle onboarding & cross-cycle escalation (DEC-036):** early Runs are deliberately gentle and teach one idea at a time; enemy density, behavioural variety, and objective pressure rise across the Cycle; each completed Boss Cycle raises the Difficulty Tier. Example *shape* only (**never locked numbers** — figures like 10 / 25 / 30 / 50 / 100 are illustrations): Run 1 simple enemies / small target / learn controls → Run 2 more enemies → Run 3 a new enemy behaviour → Run 4 higher intensity → Run 5 boss. This is a **cross-Run** escalation axis layered on top of the existing **within-Run** phase ramp (see § Difficulty Curve).

The proving goal (DEC-034): confirm this loop answers one question — **"is the Arena gameplay loop fun enough that players immediately want to play another Run?"** (the Golden Rule) — before investing in Zones, multiple Operations/Arenas, world progression, story, or campaign.

## Lobby — [LONG-TERM VISION]

The Lobby is the player's home. Here players can invite friends, join a party, inspect builds, change cosmetics, select or create a character, configure loadout, view leaderboards, and review previous runs. Character creation is cosmetic only — there are no predefined gameplay classes to choose between (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Character Philosophy). The Lobby should feel calm — no combat happens here.

**[CURRENT PROTOTYPE]:** the Lobby/Party hub is **deferred** (DEC-034). The prototype front-end is a compressed flow — Main Menu → Character Creation → Initial Skill Tree Allocation → **Enter Arena** (via a single-node greybox screen) → Results — with no party, cosmetics store, leaderboards, or run history yet. Character creation still happens up front and stays cosmetic-only.

## Party

Players may enter an Operation alone or with friends. Supported: Solo, Duo, Trio, Squad (4). See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Multiplayer Philosophy for why a fixed team composition is never required.

## Loadout / Skill Tree

**Revised (DEC-033, DEC-034) — this section previously described a pre-run "Starting Skill" simply handed to the player from a Loadout; that framing is superseded.**

The **Skill Tree is part of the character's long-term identity, not a temporary per-run Loadout choice.** The intended flow is:

**Character Creation → Initial Skill Tree Allocation → Operation Loop.**

- A new character starts with **2 Skill Points** (DEC-020, DEC-033).
- **Before the first run**, the player is presented with the Skill Tree (currently the initial-allocation screen) and **decides how to spend those points — or may explicitly continue without spending them** (a basic-weapon-only start is valid).
- **Active skills are not automatically unlocked** — a skill only activates once the player has allocated into it. Which skills are offered at the start is data-driven (see [DECISIONS.md](../../DECISIONS.md) DEC-033).
- Earned Skill Points can be **inspected and allocated later during gameplay** through the Skill Tree (e.g. via a dedicated input such as Tab). The exact in-game skill-tree UI behaviour is **not yet locked**.

The Primary Weapon is the character's baseline; further build layers (Passives, and — in the Long-Term Vision — Mutations, Relics, Team Synergies, Temporary Operation Effects) are discovered/earned rather than chosen from a pre-run loadout.

## Operation Start

Every run begins with deployment into an Operation — a complete mission session from leaving the Lobby to returning to it, not merely a map (DEC-009; see [GLOSSARY.md](../../GLOSSARY.md)). The player starts relatively weak. This is intentional — growth is part of the experience.

## Zones — [LONG-TERM VISION]

An Operation is composed of multiple large, connected Zones rather than one linear space (DEC-010). Each Zone may offer multiple routes, encounters, objectives, secrets, and Elite encounters before transitioning to the next. The overall structure, major landmarks, and authored identity of an Operation stay recognizable across runs; individual runs vary within that structure — routes, events, enemy composition, and rewards can differ (semi-procedural — DEC-011). Exact procedural-generation techniques are **[UNRESOLVED]**.

**[CURRENT PROTOTYPE]:** Zones and semi-procedural generation are **deferred** (DEC-034). The prototype uses a **single Arena** (see § Two Layers for the Arena/Run/Cycle/Tier terms); proving the loop's fun does not depend on Zones and must not wait on them.

## Exploration

Players move through the Operation's Zones. Goals include finding combat, discovering rewards, helping teammates, completing objectives, and preparing for larger encounters. Exploration should never feel empty — movement should naturally lead players into interesting situations.

## Objectives — [LONG-TERM VISION]

Each Operation selects from multiple objective types and combinations for that run rather than using a fixed set (dynamic objectives — DEC-012). Objectives may be mandatory or optional, and may include risk-versus-reward, Zone-specific, or cooperative objectives. Completing mandatory objectives is required to unlock progression toward the final Boss. Objectives should make co-op meaningful, not just function as combat markers. Exact objective types, selection rules, and frequency are **[UNRESOLVED]**.

**[CURRENT PROTOTYPE]:** dynamic-objective variety is **deferred** (DEC-034). A prototype run currently uses a **single simple objective** (a kill target) that, once met, opens the Extraction Point. Objective pressure still rises across the cycle (DEC-036), but the *catalog* of objective types is future work.

## Combat

Combat is continuous. See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Combat Is King for how combat must feel. Every fight should improve the player's build in some way.

## Experience

**Revised under [DECISIONS.md](../../DECISIONS.md) DEC-020 — this section previously described leveling as pausing the run for an in-mission upgrade choice; that description is superseded, not merely amended.**

Enemies grant Experience immediately on death — Experience is never a physical pickup (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Progression Philosophy). Experience fills the character's persistent Level Bar — the same Level and total Experience the character carries into every future mission within the current Season (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Persistence Philosophy). Leveling up unlocks persistent build potential — a Skill Point banked on the character for spending on permanent Skills/Passives — it does not pause the mission for an in-mission choice, and it grants no power by itself (see DEC-020). Players should always be excited when leveling. Levels and Skill Points earned during a mission are never lost, regardless of mission outcome — see Permanent Progression, below.

## In-Run Power Growth

**Revised under DEC-020 — replaces this section's former title, "Upgrade Selection," and its framing of upgrade choices as tied to leveling.**

A run's Weak → Ridiculous power curve (see Build Growth, below) comes from what is found and used during the Operation itself — Loot, Relics and Mutations discovered but not yet secured, and Temporary Operation Effects scoped to the current run (DEC-013) — not from Experience or Level, which are permanent character progression (see § Experience, above). Whether any in-run finds present the player with a choice (e.g., "1 of 3"), and exactly how they are acquired, is **[UNRESOLVED]** pending the Loot/Equipment system design. Whatever form in-run choices take, the same principle applies: avoid small statistical upgrades whenever possible — every choice should create anticipation, not just a bigger number.

## Build Growth

The build evolves constantly through combinations of the layered build system — Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, and Temporary Operation Effects (DEC-013; see [GLOSSARY.md](../../GLOSSARY.md) and [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../Progression/BUILD_SYSTEM_OVERVIEW.md)). Exact layer rules, slot counts, and acquisition methods are **[UNRESOLVED]**. Build creation is the heart of Point Clear — every layer above exists to serve build diversity and player agency (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Build Philosophy). A run should feel like a journey: Weak → Interesting → Powerful → Ridiculous, driven by in-run finds (see In-Run Power Growth, above) layered on top of whatever the character has already permanently earned — see Permanent Progression, below, for how much of that growth carries forward. Players should feel noticeably stronger every few minutes.

## Difficulty Curve

Difficulty escalates on **two axes**:

- **Within a run** — as player power increases, enemy pressure increases; the game keeps tension high and players never feel completely safe. (Realised today by the kill-driven phase ramp in `EnemySpawner`.)
- **Across runs — [CURRENT PROTOTYPE], DEC-036** — the prototype loop also escalates *between* runs: early runs are gentle and introduce mechanics gradually, and each completed five-run boss cycle raises the difficulty tier. Exact counts, thresholds, and scaling are configurable and playtest-driven, never locked.

## Elite Enemies

Elite enemies introduce spikes in difficulty. Their purpose is to challenge the build, reward adaptation, and teach mechanics — not simply to have more health.

## Mini Boss — [LONG-TERM VISION]

A Mini Boss tests the current build. Players should discover weaknesses before the final encounter.

**[CURRENT PROTOTYPE]:** the Mini Boss tier is **deferred** (DEC-034). The prototype expresses boss pacing through the **five-run cadence** (DEC-035) rather than a per-Operation Mini Boss → Boss sequence.

## Final Boss — [LONG-TERM VISION]

The Boss is the final gameplay test. It should challenge movement, cooperation, positioning, build quality, and decision making. The Boss should never simply be a damage sponge.

**[CURRENT PROTOTYPE] — DEC-035:** the boss is a **recurring Boss Run every 5th run**, not a one-per-Operation finale. On the Boss Run the boss is the **main objective**: reach/fight → defeat → **boss reward drops** → the Extraction Point opens → the player must still **extract successfully**. Defeating the boss does not end the run. Boss encounter design and the **boss reward system are intended future work**, documented here as intent only.

## Extraction

Defeating the Boss does not complete the run — Extraction does. Players must survive/reach evacuation to secure the run. The permanent principle **"the boss is not the finish line — Extraction is"** ([DESIGN_DNA.md](../../DESIGN_DNA.md) § Extraction) holds in both layers.

**[CURRENT PROTOTYPE] — DEC-035:** **every normal run ends in Extraction** — fight, meet the objective, reach the Extraction Point, and secure the run's rewards. Extraction is not gated behind a boss; on a Boss Run (every 5th), the boss is defeated first and *then* Extraction opens. Reaching Extraction is what turns this run's unsecured rewards into permanent ones (see § Permanent Progression). Failing to extract loses this run's unsecured rewards (mission risk).

## Results

The Results screen summarizes the journey: Operation, Time, Difficulty, Kills, Deaths, Revives, Damage, Final Build, Build Timeline, Rewards, Leaderboard Position. Players should immediately understand what happened during the run — including which rewards were actually secured (see Permanent Progression, below) versus lost to mission failure.

## Permanent Progression

**Revised under [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Persistence Philosophy and § Mission Risk Philosophy — this section previously stated the opposite of the approved model and has been rewritten, not merely amended.**

The character is the persistent unit of progression, not the account in isolation. Level, Experience, equipped Weapons, Skills, and owned Equipment all carry forward from mission to mission — a player does not start over at zero each time they deploy.

**Permanent progression and run progression are two intentionally separate systems** (DEC-018, DEC-020, DEC-032, DEC-033):
- **Permanent progression** belongs to the character and is never lost to a run: **Character Level, total Experience, Skill Points, and Skill Tree allocations.** XP is *permanent* progression — earned immediately on kill, it unlocks build potential (Skill Points), never stat inflation on its own.
- **Run progression** is everything gained *during* a run and **at risk until Extraction**: loot, currency, and any not-yet-secured build finds. Extraction secures it; failure loses it.

What a run actually secures, at Results:
- **Loot, currency, and other in-run rewards** are not permanently owned until the run is extracted successfully. Failure loses them.
- **Experience is retained** regardless of outcome — a failed run still leaves the character measurably stronger. The exact failure cost is **DEC-032**: on death the character loses only a **configurable fraction of progress within the current level, floored so a Level is never lost** (default 20%; example Level 10 at 40% → Level 10 at 20%). **Levels, Skill Points, and allocations are never lost.**
- **Account-level rewards** (cosmetics, achievements, and similar account-wide unlocks not tied to a specific character's build) persist independently of any single run's outcome.

No individual run resets a character. The seasonal reset boundary is **[UNDER REVISION — DEC-028]**; what is locked is that a character never disappears.

## Emotional Curve

See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Player Journey for the emotional arc every run is designed to produce. The phases described above — from Operation Start through Results and the return to Lobby — are what generate that arc, in sequence.

## Failure

Failure is expected. Failure teaches. Failure should motivate another attempt. The exact cost of failure is defined in [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Mission Risk Philosophy and summarized in Permanent Progression, above — it must never be so heavy that players stop playing, and must never erase all long-term progression.

## Success

A successful run creates memories — not because of rewards, but because of what happened. Players should leave every run with at least one story worth telling.

## Design Rules

Every gameplay system must improve at least one of: Combat, Builds, Cooperation, Replayability, Memorable Moments. If it improves none of them, it does not belong in Point Clear.

## Questions Every Feature Must Answer

- Does this make combat better?
- Does this create interesting choices?
- Does this improve teamwork?
- Does this increase replayability?
- Does this create memorable moments?
- Does this make players want another run?

If the answer is "No," reconsider the feature.

## Final Statement

Point Clear is not about finishing Operations. Point Clear is about creating stories together. Every successful run should end with one sentence: "Let's do another one."

## Related Documents

- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md)
- [DESIGN_DNA.md](../../DESIGN_DNA.md)
- [GAME_PILLARS.md](../../GAME_PILLARS.md)
- [VISION.md](../../VISION.md)
- [GLOSSARY.md](../../GLOSSARY.md)
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../Progression/BUILD_SYSTEM_OVERVIEW.md)
- [ROADMAP.md](../../ROADMAP.md)
- [DECISIONS.md](../../DECISIONS.md)
