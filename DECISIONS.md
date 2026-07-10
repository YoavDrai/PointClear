# Point Clear — Decisions

This document is the persistent record of accepted decisions and open questions. When a decision changes, update its entry rather than deleting history where practical, and log the change in [CHANGELOG.md](CHANGELOG.md). See also: [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [ROADMAP.md](ROADMAP.md), [GLOSSARY.md](GLOSSARY.md).

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

---

## Unresolved Decisions

**[UNRESOLVED]** — none of the items below have been decided. Do not implement or assume a default for any of these without explicit Game Director approval.

- Networking solution
- Hosting model
- Backend service
- Leaderboard verification model
- Exact number of simultaneous enemies
- Exact progression structure
- Exact season rules
- Exact primary leaderboard metric
- Monetization model
- Initial release platform and store
- Exact number and selection of Operations; Operation length/pacing targets (DEC-009)
- Zone count and size targets; Zone transition mechanics (DEC-010)
- Procedural-generation techniques and tooling (DEC-011)
- Objective type catalog; objective selection and frequency rules (DEC-012)
- Build layer exact rules, slot counts, acquisition methods, balance and rarity systems (DEC-013)

---

## Adding a New Decision

Use [Templates/DECISION_TEMPLATE.md](Templates/DECISION_TEMPLATE.md). Assign the next sequential `DEC-XXX` ID, and move the corresponding item out of "Unresolved" if it resolves one.

## Related Documents

- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [VISION.md](VISION.md)
- [ROADMAP.md](ROADMAP.md)
- [CHANGELOG.md](CHANGELOG.md)
- [GLOSSARY.md](GLOSSARY.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
