# Point Clear — Core Gameplay Loop

**[APPROVED FACT]** — Version 1.1

This document defines the complete gameplay loop of Point Clear: the player's journey from entering the game until starting the next run. This is a design blueprint, not a technical document — every gameplay system must support this loop. This document owns the Operation's place and sequence in a run; canonical term definitions live in [GLOSSARY.md](../../GLOSSARY.md). The permanent rules this loop must obey — persistent characters, automatic experience, loot as reward, mission risk — are defined in [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md); this document does not restate them, only sequences them. See also: [DESIGN_DNA.md](../../DESIGN_DNA.md), [GAME_PILLARS.md](../../GAME_PILLARS.md), [VISION.md](../../VISION.md), [ROADMAP.md](../../ROADMAP.md), [DECISIONS.md](../../DECISIONS.md).

**Terminology note:** this document uses "Operation" per [DEC-009](../../DECISIONS.md), the currently approved formal term. Recent design discussion has used "mission" for the same or a closely related concept informally. Whether "Mission" replaces, splits from, or is simply another name for "Operation" is **[UNRESOLVED]** and is deliberately not decided by this pass — see [DECISIONS.md](../../DECISIONS.md) Unresolved Decisions.

## Purpose

This document describes the player's journey from entering the game until starting the next run. It is the design blueprint for the entire game. Every gameplay system must support this gameplay loop.

## The Golden Rule — Run Success Test

Every run must answer one question: "Do I want to start another run immediately?" If the answer is no, the gameplay loop failed.

## High-Level Loop

Main Menu → Lobby → Party → Loadout → Select Operation → Deploy → Explore Zones → Fight → Complete Objectives → Gain Experience → Choose Upgrades → Become Stronger → Face Increasing Danger → Mini Boss → Continue Building → Boss → Extraction → Results → Permanent Progression → Lobby → Repeat.

## Lobby

The Lobby is the player's home. Here players can invite friends, join a party, inspect builds, change cosmetics, select or create a character, configure loadout, view leaderboards, and review previous runs. Character creation is cosmetic only — there are no predefined gameplay classes to choose between (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Character Philosophy). The Lobby should feel calm — no combat happens here.

## Party

Players may enter an Operation alone or with friends. Supported: Solo, Duo, Trio, Squad (4). See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Multiplayer Philosophy for why a fixed team composition is never required.

## Loadout

Before deployment, players prepare. Initial MVP includes: Primary Weapon, Starting Skill, and an optional Passive Choice. Everything else is discovered during the run.

## Operation Start

Every run begins with deployment into an Operation — a complete mission session from leaving the Lobby to returning to it, not merely a map (DEC-009; see [GLOSSARY.md](../../GLOSSARY.md)). The player starts relatively weak. This is intentional — growth is part of the experience.

## Zones

An Operation is composed of multiple large, connected Zones rather than one linear space (DEC-010). Each Zone may offer multiple routes, encounters, objectives, secrets, and Elite encounters before transitioning to the next. The overall structure, major landmarks, and authored identity of an Operation stay recognizable across runs; individual runs vary within that structure — routes, events, enemy composition, and rewards can differ (semi-procedural — DEC-011). Exact procedural-generation techniques are **[UNRESOLVED]**.

## Exploration

Players move through the Operation's Zones. Goals include finding combat, discovering rewards, helping teammates, completing objectives, and preparing for larger encounters. Exploration should never feel empty — movement should naturally lead players into interesting situations.

## Objectives

Each Operation selects from multiple objective types and combinations for that run rather than using a fixed set (dynamic objectives — DEC-012). Objectives may be mandatory or optional, and may include risk-versus-reward, Zone-specific, or cooperative objectives. Completing mandatory objectives is required to unlock progression toward the final Boss. Objectives should make co-op meaningful, not just function as combat markers. Exact objective types, selection rules, and frequency are **[UNRESOLVED]**.

## Combat

Combat is continuous. See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Combat Is King for how combat must feel. Every fight should improve the player's build in some way.

## Experience

Enemies grant Experience immediately on death — Experience is never a physical pickup (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Progression Philosophy). Experience fills the Level Bar. Leveling up immediately pauses the action for Upgrade Selection. Players should always be excited when leveling. Levels earned during a mission are not lost if the mission is abandoned partway — see Permanent Progression, below.

## Upgrade Selection

Players choose one option from multiple upgrade choices (e.g., choose 1 of 3). Every choice should significantly influence the future build. Avoid small statistical upgrades whenever possible — every selection should create anticipation.

## Build Growth

The build evolves constantly through combinations of the layered build system — Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, and Temporary Operation Effects (DEC-013; see [GLOSSARY.md](../../GLOSSARY.md) and [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](../Progression/BUILD_SYSTEM_OVERVIEW.md)). Exact layer rules, slot counts, and acquisition methods are **[UNRESOLVED]**. Build creation is the heart of Point Clear — every layer above exists to serve build diversity and player agency (see [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Build Philosophy). A run should feel like a journey: Weak → Interesting → Powerful → Ridiculous, layered on top of whatever the character has already permanently earned — see Permanent Progression, below, for how much of that growth carries forward. Players should feel noticeably stronger every few minutes.

## Difficulty Curve

As player power increases, enemy pressure increases. The game constantly attempts to keep tension high. Players should never feel completely safe.

## Elite Enemies

Elite enemies introduce spikes in difficulty. Their purpose is to challenge the build, reward adaptation, and teach mechanics — not simply to have more health.

## Mini Boss

A Mini Boss tests the current build. Players should discover weaknesses before the final encounter.

## Final Boss

The Boss is the final gameplay test. It should challenge movement, cooperation, positioning, build quality, and decision making. The Boss should never simply be a damage sponge.

## Extraction

Defeating the Boss does not complete the run — Extraction begins. Players must survive until evacuation. See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Extraction for why this phase matters.

## Results

The Results screen summarizes the journey: Operation, Time, Difficulty, Kills, Deaths, Revives, Damage, Final Build, Build Timeline, Rewards, Leaderboard Position. Players should immediately understand what happened during the run — including which rewards were actually secured (see Permanent Progression, below) versus lost to mission failure.

## Permanent Progression

**Revised under [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Persistence Philosophy and § Mission Risk Philosophy — this section previously stated the opposite of the approved model and has been rewritten, not merely amended.**

The character is the persistent unit of progression, not the account in isolation. Level, Experience, equipped Weapons, Skills, and owned Equipment all carry forward from mission to mission — a player does not start over at zero each time they deploy.

What a mission actually secures, at Results:
- **Loot, Gold, and other in-mission rewards** are not permanently owned until the mission is completed successfully. Mission failure loses them.
- **Experience** is retained at least in part regardless of outcome — failure should still leave the character measurably stronger than before the attempt, even if the mission itself was lost.
- **Account-level rewards** (cosmetics, achievements, and similar account-wide unlocks not tied to a specific character's build) persist independently of any single mission's outcome, the same as before.

Only a new Season resets character-level progression (level, experience, equipment, skills). No individual mission does.

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
