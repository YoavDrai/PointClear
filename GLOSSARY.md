# Point Clear — Glossary

**Status: LIVING** — this document grows every time a new Point-Clear-specific term is coined. It owns concise, canonical definitions only. It does not own philosophy (see [DESIGN_DNA.md](DESIGN_DNA.md)), sequencing (see [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)), or exact mechanics (see the relevant Systems document once it exists). If a definition below needs more than a sentence or two to state, that is a signal the term needs its own design document — this file should stay a dictionary, not grow into a spec.

See also: [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md), [DECISIONS.md](DECISIONS.md), [DESIGN_DNA.md](DESIGN_DNA.md), [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md).

## Terms

**Boss** — The final gameplay test of an Operation, arriving before Extraction. Challenges movement, cooperation, positioning, build quality, and decision-making. See [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Final Boss.

**Build** — The combination of a player's Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, and Temporary Operation Effects that defines how they play during a run. See [DESIGN_DNA.md](DESIGN_DNA.md) § Build Philosophy for the guiding principle and DEC-013 in [DECISIONS.md](DECISIONS.md) for the approved layer list.

**Build Layer** — One of the seven approved categories a Build is composed from: Weapon, Active Skills, Passives, Mutations, Relics, Team Synergies, Temporary Operation Effects. Exact rules, slot counts, acquisition methods, and balance per layer are **[UNRESOLVED]**. See DEC-013 in [DECISIONS.md](DECISIONS.md) and [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md).

**Deployment** — The moment a run begins: the point at which a party leaves the Lobby and enters an Operation. See [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Operation Start.

**Dynamic Objective** — The system by which an Operation selects from multiple Objective types and combinations for a given run, rather than using a fixed objective set. Types may be mandatory, optional, risk-versus-reward, Zone-specific, cooperative, or required to unlock progression toward the final Boss. Exact objective types, selection rules, and frequency are **[UNRESOLVED]**. See DEC-012 in [DECISIONS.md](DECISIONS.md).

**Elite** — An enemy tier that introduces a spike in difficulty within a Zone. Its purpose is to challenge the current build, reward adaptation, and teach mechanics — not simply to have more health. See [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Elite Enemies.

**Extraction** — The phase after the final Boss is defeated, during which the party must survive until evacuation to complete the Operation successfully. See [DESIGN_DNA.md](DESIGN_DNA.md) § Extraction for why it matters and [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Extraction for its place in the sequence.

**Loadout** — The player's preparation before deployment, drawn from their persistent character (see Persistent Character, below) — not a fresh, temporary choice each time. Current MVP scope: Primary Weapon, Starting Skill, and an optional Passive Choice. See [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Loadout.

**Loot** — The physical, RNG-based reward found during missions: gold, currency, equipment, crafting materials, rare and legendary items. Deliberately separate from Experience — see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Progression Philosophy. Not permanently owned until the mission that produced it is completed successfully — see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Mission Risk Philosophy.

**Mini Boss** — An enemy encounter that tests the party's current build before the Operation's final encounter. See [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) § Mini Boss.

**Mutation** — One of the seven approved Build Layers. Exact mechanics are **[UNRESOLVED]**. See DEC-013 in [DECISIONS.md](DECISIONS.md).

**Objective** — A goal a party can pursue within an Operation. May be mandatory or optional. See Dynamic Objective and Optional Objective, below.

**Operation** — A complete mission session: the full playable structure from leaving the Lobby to returning to it, including deployment, multiple connected Zones, combat and exploration, objectives, build development, escalating danger, a final Boss phase, Extraction, and Results. An Operation is not merely a map. See DEC-009 in [DECISIONS.md](DECISIONS.md) for the approved definition and [CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md) for its place in the run sequence.
**Note:** recent design discussion has used "mission" informally for this same or a closely related concept. Whether "Mission" replaces, splits from, or is simply another name for "Operation" is **[UNRESOLVED]** — see [DECISIONS.md](DECISIONS.md) Unresolved Decisions. This entry is not changed until that is explicitly resolved.

**Optional Objective** — An Objective a party may choose to pursue or skip, as distinct from a mandatory Objective required to progress toward the final Boss. Exact optional-objective types and rewards are **[UNRESOLVED]**. See DEC-012 in [DECISIONS.md](DECISIONS.md).

**Persistent Character** — The player's character as the unit of long-term progression: level, Experience, equipped Weapons, Skills, and owned Equipment all carry forward from mission to mission rather than resetting each attempt. Only a new Season resets it. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Persistence Philosophy.

**Relic** — One of the seven approved Build Layers. Exact mechanics are **[UNRESOLVED]**. See DEC-013 in [DECISIONS.md](DECISIONS.md).

**Run** — One complete attempt at an Operation, from Deployment to either Results (success or failure) or abandonment. Used interchangeably with "Operation" when referring to a single playthrough rather than the mission structure itself. A Run does not reset the Persistent Character attempting it — see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Persistence Philosophy.

**Season** — A content and competitive period lasting approximately four months (DEC-005), at the end of which character progression resets — the only thing that resets it (DEC-004; see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Long-Term Vision and § Seasons). Each Season is expected to introduce new build possibilities (new mechanics, items, or systems), not simply a fresh economy. Exactly what carries over between Seasons, if anything, is **[UNRESOLVED]**.

**Team Synergy** — One of the seven approved Build Layers; represents interactions between party members' builds rather than a single player's choices alone. Exact mechanics are **[UNRESOLVED]**. See DEC-013 in [DECISIONS.md](DECISIONS.md).

**Temporary Operation Effect** — One of the seven approved Build Layers; a build modifier that applies only for the current Operation rather than persisting afterward. Exact mechanics are **[UNRESOLVED]**. See DEC-013 in [DECISIONS.md](DECISIONS.md).

**Zone** — One of several large, connected areas that make up an Operation. Not a narrow linear corridor; may contain multiple routes, combat encounters, objectives, events, optional risks and rewards, secrets, Elite encounters, rewards, and transitions to later Zones. See DEC-010 in [DECISIONS.md](DECISIONS.md).

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [DECISIONS.md](DECISIONS.md)
- [DESIGN_DNA.md](DESIGN_DNA.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
- [Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md](Documentation/Progression/BUILD_SYSTEM_OVERVIEW.md)
- [POINT_CLEAR_KNOWLEDGE_MAP.md](POINT_CLEAR_KNOWLEDGE_MAP.md)
