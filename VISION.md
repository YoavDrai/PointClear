# Point Clear — Vision

Status legend: **[APPROVED FACT]**, **[ASSUMPTION]**, **[PROPOSAL]**, **[UNRESOLVED]**. See also: [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md), [DESIGN_DNA.md](DESIGN_DNA.md), [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [GAME_PILLARS.md](GAME_PILLARS.md), [ROADMAP.md](ROADMAP.md), [DECISIONS.md](DECISIONS.md).

## Summary

**[APPROVED FACT]**

Point Clear is an original, persistent, seasonal cooperative Action ARPG designed for solo play and online co-op for up to four players. The player controls a character manually from an isometric camera perspective. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Identity for the full statement of what Point Clear is and is not.

## Intended Features

**[APPROVED FACT]** — the following are the currently intended features of Point Clear. None of these are implemented yet; this is a description of intent, not a status report.

- Fast real-time combat
- Manual character movement
- Active combat abilities
- Large groups of enemies
- Deep build creation through a layered build system (weapon, active skills, passives, mutations, relics, team synergies, temporary Operation effects)
- Persistent character progression carried across missions — level, experience, equipment, weapons, and skills all persist. **[Seasonal reset UNDER REVISION — see [DECISIONS.md](DECISIONS.md) DEC-028]**: what a Season resets vs. carries is deferred to a future Seasons workshop; the locked constraint is that **a character never disappears** (see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Persistence Philosophy)
- Build identity from a **finite** progression budget — a character can travel toward almost any direction but owns only a small part of the space at once (DEC-021)
- Solo and co-op both first-class, with *experience parity* — co-op multiplies possibilities, not damage, and never gates content behind grouping (DEC-022, DEC-023)
- Mission-scoped risk on top of that persistence — loot and gold earned during a mission are not secured until it is completed successfully (see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Mission Risk Philosophy)
- Skill and passive progression systems
- Weapons, items, abilities, and build combinations
- Operations composed of multiple connected, semi-procedural Zones
- Dynamic mandatory and optional objectives per Operation
- Maps and repeatable runs
- Solo, duo, trio, and four-player co-op
- Seasonal competition
- Seasons planned approximately every four months
- Friends and global leaderboards
- Separate leaderboard categories for solo, duo, trio, and four-player parties
- Run records that preserve the party composition, build, weapons, items, skills, and relevant performance statistics

See [DECISIONS.md](DECISIONS.md) DEC-009 through DEC-013 for the approved boundaries of Operation, Zone, objective, and build-layer structure, and [GLOSSARY.md](GLOSSARY.md) for term definitions.

## Current Prototype Scope

**[APPROVED FACT] — DEC-034.** The features above are the **Long-Term Game Vision** and remain the destination. The **current prototype is a deliberately compressed version** built to prove one thing: **is the Arena gameplay loop fun enough that players immediately want to play another Run?** Its proving target is a single, repeating **Arena** (a bounded combat space — the Arena itself is *not* infinite; the **sequence of Runs and Difficulty Tiers** is):

**Character Creation → Initial Skill Tree Allocation → Enter Arena → Fight → Extraction → Results → Re-enter Arena → Repeat**, with **every 5th Run a Boss Run** (DEC-035) and **gentle onboarding that escalates across each Cycle** (DEC-036).

Deliberately deferred until the loop is proven fun: multiple Operations, **multiple Arenas**, connected Zones, semi-procedural generation, dynamic-objective variety, the Lobby/Party hubs, world/campaign progression, and **story**. None of these are cancelled — they are the long-term target, sequenced after the loop is validated. The long-term **Map/Zone/Operation** terms are unchanged; **Arena/Run/Cycle/Difficulty Tier** are prototype-scope terms. See [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) for the Long-Term-Vision vs Current-Prototype split.

## Possible Leaderboard Statistics

**[PROPOSAL]** — these are candidate leaderboard statistics, **not final and not permanently approved game rules**:

- Highest difficulty or tier completed
- Fastest completion
- Longest survival
- Highest DPS

The exact primary leaderboard metric, and the full set of leaderboard categories, remain unresolved — see [DECISIONS.md](DECISIONS.md).

## Identity Rule

**[APPROVED FACT]**

Point Clear must be described by its own systems and identity.

- Do not use other commercial games as implementation specifications.
- Do not directly copy another game's content, terminology, assets, progression structure, or user interface.

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [DESIGN_DNA.md](DESIGN_DNA.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [GAME_PILLARS.md](GAME_PILLARS.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
- [GLOSSARY.md](GLOSSARY.md)
- [ROADMAP.md](ROADMAP.md)
- [DECISIONS.md](DECISIONS.md)
