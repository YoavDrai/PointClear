# Point Clear — Core Philosophy

**[APPROVED FACT]** — the highest-level design document in the project.

*"Point Clear is designed to be played for years, not completed once."*

This document defines the permanent design principles of Point Clear — the rules every future system, feature, and document must follow. It does not describe implementation. It does not describe individual systems. Where a rule here needs elaboration of *why* it matters or *how it should feel*, that lives in [DESIGN_DNA.md](DESIGN_DNA.md); where it needs a dated record of approval, that lives in [DECISIONS.md](DECISIONS.md). This document states the rule itself, once, permanently.

## Identity

1. Point Clear is a persistent, seasonal, cooperative Action ARPG — not a Survivors-like, not a traditional roguelite, and not a clone of any existing ARPG. It takes inspiration from the endgame philosophy of games like Diablo and Path of Exile, but must be described by its own systems and its own identity.
2. Combat is dense and constant. Builds are deep. Loot is meaningful. Missions carry real risk.

## Long-Term Vision — A Living Game

3. The game should never feel finished. It is built to keep expanding for years — through new Seasons, new mechanics, and new build possibilities — not to reach a final, complete state.
4. The purpose of a Season is not to reset progression — that reset is a side effect. The purpose of a Season is to continuously introduce new ways to play.

## Build Philosophy — The Core Thesis

5. Point Clear is fundamentally a game about creating your own build. Every other system exists to support that goal:
   - Combat exists to test builds.
   - Loot exists to improve builds.
   - Experience exists to unlock builds.
   - Skills exist to create builds.
   - Equipment exists to define builds.
   - Missions exist to challenge builds.
   - Seasons exist to introduce new build possibilities.
6. The player fantasy is not "I killed a lot of enemies." The player fantasy is "I created a build that feels uniquely mine."
7. Player agency is a core value. Two players with the same amount of playtime should naturally end up with different builds, different equipment, different strategies, and different stories — divergence between players is a designed outcome, not an accident.
8. Every major system should either create, improve, reward, or expand build diversity. A system that does none of these should be questioned before it is built.
9. The objective is not perfect balance. The objective is to support many viable and enjoyable builds.
10. Meaningful choices create meaningful builds. A build should not be able to become everything — choosing one direction should mean giving up another. Players should regularly face decisions that open one path while closing another.
11. Permanent or difficult-to-reverse choices are valuable when they create identity, but they must never punish experimentation unfairly.
12. Interesting interactions are worth more than larger numbers. A build choice should change how the game is played, not just its stat sheet.
13. Build diversity should expand horizontally, not vertically: new content earns its place by expanding the interaction space — creating new combinations with what already exists — not by replacing it or simply outscaling it in isolation. A new Skill, Item, Passive, Mutation, Relic, or mechanic should be judged first by how many new interactions it creates with existing content, not only by how good it is on its own. This sharpens point 8's general evaluation question along one specific axis, and is distinct from point 12: point 12 is about what a player should value when choosing an upgrade; this is about what a designer should optimize for when creating one. (Internally nicknamed "Horizontal Growth.")
14. There should never be only one intended way to play.
15. Every Season should expand build diversity — new weapons, skills, mechanics, and items that create new viable ways to play — not simply larger numbers.

The following sharpen the rules above into permanent principles (approval and boundaries in [DECISIONS.md](DECISIONS.md)):

15a. **Identity comes from finite, scarce investment** (DEC-021). Long-term progression is a finite budget, not a fillable checklist: a build can travel toward almost any direction but can only own a small part of the space at once. This is the mechanism behind point 10 — it is what keeps builds from converging on one generalist.
15b. **The world asks the questions** (DEC-024). Combat, missions, environments, routes, and depth all pose *"how does your build solve me?"* Enemies threaten through *behavior*, not stat inflation; difficulty deepens through *demand*, not bigger numbers. Matchup texture is **soft — never a hard gate** (a finite build must be viable everywhere without being optimal everywhere).
15c. **Readability governs growth** (DEC-024). Complexity may grow only as fast as it remains readable; the battlefield itself must explain its interactions. If an interaction needs a wiki, it has outrun readability.
15d. **Content grows by expanding questions, not numbers** (DEC-026). New content earns its place by adding new interactions — authored, readable *categories* of challenge wrapped around generated, hidden *instances* — so the space of interesting questions keeps expanding without replacing what exists.

### Player Agency & Divergent Journeys

- Players should constantly feel they are making meaningful choices, not following a predefined path.
- A player should feel *"this is my build,"* never *"this is the build."*
- This is the deeper reason there are no predefined classes, why identity is driven by equipment and skill choices, and why seasonal experimentation, loot priorities, and progression paths are allowed to differ from player to player — these systems all exist in service of this principle, not by coincidence.
- The test: two players who started the same Season on the same day should be able to compare characters later and discover genuinely different journeys, shaped by the choices each of them made.
- **Solo and co-op are both first-class** (DEC-022). Builds are self-sufficient — players *want* each other because together they create experiences that cannot exist alone (*want, not need*), never because survival requires a role. There is no co-op-exclusive content; parity is *experience* parity — the efficiency gap between modes must stay small enough that players choose a mode for its experience, not because the game pressures them into the efficient one.
- **Diversity comes from identity, and the world validates it rather than dictating it** (DEC-023). Players identify with their build first; the world's job is to keep giving many different, beloved identities opportunities to feel special — not to force a composition. Diversity is seduced, never enforced; a party of four identical builds always remains viable.

### Onboarding — Directional Possibility

The first hours must bridge a beginner (who has no attachment, mastery, or build-curiosity yet) to the long arc that makes Point Clear special. The bridge emotion is **possibility** — but *directional* possibility ("a direction you're excited to become"), honest about the finite budget, never "you can be anything." Ownership is planted by the first moment a choice is *validated in gameplay*, not by a selection screen. Onboarding's job is to convince the player the journey of building their character is worth taking — not to teach the whole game. (Approval and boundaries: [DECISIONS.md](DECISIONS.md) DEC-027.)

### The Player Mindset

Point Clear should constantly provoke a specific kind of curiosity:
- "What if I combine these two mechanics?"
- "What if I build around this weapon?"
- "What if this item changes my entire build?"
- "Can I make this unusual idea actually work?"

This curiosity — not grinding, not routine — is what should bring players back Season after Season.

### Player Expression & The Meta

- A dominant meta will emerge naturally over time. This is expected, not a failure.
- The goal is not to eliminate the meta. New viable alternatives continue to appear as a direct consequence of expanding the interaction space (point 13) — Season after Season — not as a separately-pursued goal in itself.

### How These Ideas Connect

These principles form a chain, not a list of unrelated rules. Build identity (points 5–6) — *"I created a build that feels uniquely mine"* — is the terminal player fantasy this document exists to serve. It is produced by discovery (see The Player Mindset, above): players earn ownership over a build by finding it themselves, not by being handed one. Discovery requires something to find, which is why the interaction space must keep expanding (point 13) — and why new content must add to that space rather than replace what's already in it. A healthy, ever-shifting meta (above) is the observable result when this is working: many viable builds, no single forced solution, Season after Season.

## Character Philosophy

16. There are no predefined gameplay classes. Character creation is cosmetic only — appearance, not gameplay.
17. Gameplay identity is created entirely through build: equipment, skills, passives, and upgrades. The build is the class.

## Progression Philosophy

18. Experience is automatic progression, and it exists to unlock build potential — levels and skill points, not stat inflation on its own. Enemy deaths grant experience immediately; experience is never represented as a physical pickup.
19. Loot is reward, and it exists to improve and diversify builds. Physical, RNG-based drops — gold, currency, equipment, crafting materials, rare and legendary items — exist to be found and fought for.
20. Experience and Loot are permanently separate systems and must never be merged into one mechanic.

## Persistence Philosophy

21. Characters persist. Level, experience, equipment, weapons, and skills carry forward from mission to mission — so a build can be developed, tested, and refined over time, not rebuilt from scratch every session.
22. **[UNDER REVISION — see [DECISIONS.md](DECISIONS.md) DEC-028]** ~~Only a new Season resets character progression.~~ No individual mission resets a character. What a Season resets vs. carries is deferred to a future Seasons workshop. What is locked: **a character never disappears** — whatever a Season does, it never deletes a player's character.
22a. **Character attachment is a core pillar.** Players are meant to become attached to a character they develop over hundreds of hours; the game must never betray that attachment. (This is the terminal value the build-identity chain serves — see *How These Ideas Connect*.)

## Mission Risk Philosophy

23. Missions exist to challenge builds. They create meaningful risk: rewards earned during a mission are not permanently owned until the mission is completed successfully.
24. Failure has a real cost: loot, gold, and other in-mission rewards are lost. Failure must not erase all long-term progression — some earned experience is retained regardless of outcome.

## Seasons

25. Seasons last approximately four months. **[UNDER REVISION — see [DECISIONS.md](DECISIONS.md) DEC-028]** ~~Each Season begins with a fresh character progression and economy.~~ What a Season resets vs. carries is intentionally deferred to a future Seasons workshop and must not be assumed; the one locked constraint is that a Season never deletes a character (point 22). See Long-Term Vision, above, for why Seasons exist beyond the reset itself.

## How These Principles Are Used

- Every future system design must be checked against this document before implementation begins. The first question for any proposal should be: **does this create, improve, reward, or expand build diversity?**
- A proposal that conflicts with a principle here must have that conflict raised explicitly to the Game Director — never resolved silently.
- These principles are expected to be stable for the life of the project. Changing one is a decision in its own right and must be recorded in [DECISIONS.md](DECISIONS.md).

## Related Documents
- [DESIGN_DNA.md](DESIGN_DNA.md)
- [GAME_PILLARS.md](GAME_PILLARS.md)
- [VISION.md](VISION.md)
- [DECISIONS.md](DECISIONS.md)
- [GLOSSARY.md](GLOSSARY.md)
