# Point Clear — Build System Overview

**[APPROVED FACT]** — high-level structure only (DEC-013).

This document owns the approved high-level structure of the layered build system: which layers exist, and the principle governing how they combine. It does not own exact rules, slot counts, acquisition methods, or balance — those remain **[UNRESOLVED]** pending a future Systems-layer document (`Documentation/Systems/UPGRADE_BUILD_SYSTEM.md`), which stays blocked until `Documentation/Progression/PROGRESSION_OVERVIEW.md` (not yet written) also exists. Build diversity and player agency, which this system exists to serve, are defined in [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Build Philosophy. See also: [DESIGN_DNA.md](../../DESIGN_DNA.md), [GLOSSARY.md](../../GLOSSARY.md), [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](../Gameplay/CORE_GAMEPLAY_LOOP.md), [DECISIONS.md](../../DECISIONS.md).

**Note on persistence:** under [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Persistence Philosophy and § Mission Risk Philosophy, some layers (e.g. equipped Weapon, Passives) are expected to persist with the character once acquired, while others (e.g. Temporary Operation Effects, and any Relics/Mutations found but not yet secured) are expected to remain mission-scoped until a mission is completed successfully. Which layers fall into which category is **[UNRESOLVED]** — a future decision, not assumed by this document. [DECISIONS.md](../../DECISIONS.md) DEC-020 resolves the adjacent question of Level/Experience/Skill Points specifically (persistent, not mission-scoped) but explicitly does not decide this per-layer question — whether Mutations, Relics, or Temporary Operation Effects persist between missions once found remains open.

**Prototype status (2026-07-13):** the two-system split is now concretely demonstrated in the prototype. **Permanent progression** (Character Level, Experience, Skill Points, Skill Tree allocations) is confirmed separate and character-owned (DEC-020, DEC-032, DEC-033). **Run progression** uses a **secure-on-Extraction / lose-on-failure** model, demonstrated for **currency** (PC-011) and the **Weapon layer** (the Detonator Module — PC-013, DEC-019): unsecured while carried, banked on a successful Extraction, lost on death. The per-layer question for **Mutations, Relics, and Temporary Operation Effects specifically remains [UNRESOLVED]** — those layers are not yet implemented and their persistence rule is still a future decision.

## Summary

**[APPROVED FACT]** — DEC-013. Point Clear uses a layered build system: a player's build is created through combinations of several system layers rather than being defined by one item or one skill. A build is defined by interactions between layers. Build decisions should change gameplay, not only increase numerical values. See [DESIGN_DNA.md](../../DESIGN_DNA.md) § Build Philosophy for the guiding principle this operationalizes.

## Build Layers

**[APPROVED FACT]** — the following seven layers are the current approved high-level structure. Their order below is not a ranking.

1. Weapon
2. Active Skills
3. Passives
4. Mutations
5. Relics
6. Team Synergies
7. Temporary Operation Effects

## What Is Not Yet Approved

**[UNRESOLVED]**:

- Exact rules for how each layer functions
- Number of slots per layer
- Acquisition methods (how a player gains access to a layer's contents)
- Balance and rarity systems
- Specific weapons, skills, mutations, relics, or synergies — none exist as approved content. Any examples discussed in prior conversation are illustrations only, not approved content.

## Related Documents

- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md)
- [DESIGN_DNA.md](../../DESIGN_DNA.md)
- [GLOSSARY.md](../../GLOSSARY.md)
- [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](../Gameplay/CORE_GAMEPLAY_LOOP.md)
- [DECISIONS.md](../../DECISIONS.md)
- [POINT_CLEAR_KNOWLEDGE_MAP.md](../../POINT_CLEAR_KNOWLEDGE_MAP.md)
