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

---

## Unresolved Decisions

**[UNRESOLVED]** — none of the items below have been decided. Do not implement or assume a default for any of these without explicit Game Director approval.

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
- Exact retained-Experience amount on mission failure, and the exact definition of "mission failure" (DEC-019)
- Character persistence save/technical implementation (DEC-016, DEC-020) — no save system exists yet; persistence is the approved design model only, not an implemented feature

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
