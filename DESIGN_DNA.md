# Point Clear — Game Design DNA

**[APPROVED FACT]**

"Games are remembered by the stories players tell, not by the features they contain."

This document is the philosophical foundation behind [GAME_PILLARS.md](GAME_PILLARS.md) and [VISION.md](VISION.md). Where those documents state *what* Point Clear is, this document states *why* — the design instincts every feature, system, and proposal should be checked against. [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) is the project's highest-level, permanent design document and sits above this one: it states the permanent rules; this document elaborates on why they matter and how they should feel. See also: [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [ROADMAP.md](ROADMAP.md), [DECISIONS.md](DECISIONS.md).

## Purpose

Point Clear exists to create unforgettable cooperative combat experiences. The game is designed to generate stories naturally through gameplay.

Players should remember the impossible boss they defeated, the teammate they revived at the last second, the insane build they accidentally created, the extraction they barely survived — not menus, systems, or statistics.

## Mission

Build the most satisfying cooperative Action ARPG where:

- Combat feels incredible
- Builds constantly surprise players
- Teamwork creates unforgettable moments
- Every run feels different
- Players always want "one more run"

## Vision

Point Clear is not built around content. Point Clear is built around experiences.

Every feature exists to strengthen one or more of the following experiences:

- Becoming stronger
- Discovering new combinations
- Saving teammates
- Surviving impossible situations
- Building something unique
- Beating personal and global records

## The Five Pillars

These are the design pillars behind the DNA. For the binding, checklist-style pillars used to evaluate proposals, see [GAME_PILLARS.md](GAME_PILLARS.md).

### 1. Combat Is King

Everything begins with combat. If combat is not satisfying, nothing else matters.

Combat must always feel: responsive, fast, dangerous, fair, rewarding. Every weapon must feel powerful. Every enemy must demand attention.

The baseline mood is **competence** — players should feel capable; threat *challenges* that confidence, it does not replace it, and death should read as *"we pushed too far,"* never *"the game cheated."* The rhythm is **flow → threat → recovery**, tilting toward threat the deeper a run goes. Build legibility is **kinesthetic**: a player should feel their build every few seconds, and changing a build should change how combat *feels*, not just the numbers. Enemies threaten through **behavior**, not stat inflation. (Permanent rules and boundaries: [DECISIONS.md](DECISIONS.md) DEC-024, DEC-025.)

### 2. Builds Create Stories

The purpose of progression is not bigger numbers — it is changing gameplay. Players should constantly discover interactions they have never seen before. The ideal player reaction is: "Wait... what just happened?"

### 3. Cooperation Creates Memories

The best moments happen between players: saving someone, protecting someone, risking everything for a revive, escaping together. Point Clear is designed around shared victories.

### 4. Every Run Must Feel Different

No two successful runs should feel identical. Randomness exists to encourage adaptation. Player decisions matter more than luck. Build identity should emerge naturally during each operation.

### 5. One More Run

The game succeeds when players voluntarily start another run — not because of rewards, daily missions, or fear of missing out, but because they genuinely want another experience.

## The Player Journey

The emotional journey of a single run should look like this:

"I feel weak." → "I found something interesting." → "My build is starting to work." → "I feel unstoppable." → "I might actually die." → "We can beat this." → "That was incredible." → "Let's do another run."

## Build Philosophy

The rule itself — build diversity, player agency, and interactions over numbers — is defined in [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Build Philosophy. This section exists only to illustrate what that rule looks like in practice:

Bad upgrade: `+10% Damage`

Good upgrade: Bullets ricochet. Poison spreads. Enemies explode. Lightning chains. Fire melts armor. Ice shatters frozen enemies.

The reason: excitement should come from discovering how mechanics chain into each other, not from unlocking another button. The best moments are propagation chains — one effect setting up the next: Mark → Fracture → Explosion. Poison → Spread → Corpse Explosion. Freeze → Shatter. Fire → Ignite → Detonation. A new skill, item, or passive is doing its job when it makes an existing chain reach further, not just when it's strong in isolation. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Build Philosophy (Expanding the Interaction Space) for the permanent rule this expresses.

## Multiplayer Philosophy

Solo must always be enjoyable. Co-op must always be better.

Players should naturally help each other. The game should reward teamwork without forcing specific class compositions — no mandatory tank, no mandatory healer, no mandatory support. Freedom creates creativity.

## Difficulty

Difficulty should reward mastery, not patience.

Players should lose because they made mistakes, they took risks, or they failed to adapt — never because the game cheated.

## Death

Death is not punishment. Death is feedback.

A failed mission should create motivation, not frustration. The exact mechanical cost of failure — what is lost, what is kept — is defined in [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Mission Risk Philosophy, not here. This section owns only the emotional framing: failure should teach, not punish.

## Extraction

The boss is not the finish line. Extraction is.

The final moments of every run should create maximum tension. Victory is earned only after the team escapes.

## Leaderboards

Leaderboards are motivation, not the objective. Players compete because they are proud of what they achieved. The leaderboard is a celebration of mastery, not a measure of time invested.

## Seasons

A season should feel like a fresh adventure, not simply a reset. Every season should introduce new discoveries, new build opportunities, new strategies, and new reasons to experiment. See [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Long-Term Vision for why this matters at the highest level, and § Seasons for the approved cadence and reset boundary.

## What We Will Never Build

Point Clear will never include systems simply because other games have them. Every feature must justify its existence.

We reject:

- Meaningless grinding
- Pay-to-win
- Fake choices
- Mandatory meta builds
- Complexity without purpose
- Systems that distract from the core experience

Mandatory meta builds specifically are what a continuously expanding interaction space (see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Build Philosophy) is designed to prevent structurally — not simply a bad outcome to avoid by intention alone.

## Design Principles

Whenever a new feature is discussed, ask:

- Does it improve combat? If not, why are we building it?
- Does it create memorable moments? If not, why are we building it?
- Does it encourage experimentation? If not, why are we building it?
- Does it improve co-op? If not, why are we building it?
- Would players tell their friends about it? If not, why are we building it?

## Design Methodology

Point Clear is designed **emotion-first**: mechanics are downstream of feelings. Every design discussion follows seven steps (a loop, not a waterfall):

1. **Emotion** — what should the player *feel*? (Not what happens, not what mechanic.)
2. **Philosophy** — why does that emotion matter; which higher emotion does it serve; resolve conflicts with other emotions *before* mechanics.
3. **Mechanic** — the *smallest* mechanic that consistently produces the emotion, not the most impressive.
4. **Validation** — a *pre-registered, falsifiable* playtest signal that players genuinely felt it. (A failed signal can send you back to the mechanic — or back to the *emotion*.)
5. **Constraints** — verify it respects everything locked (finite budget, soft counters, readability, build identity, curiosity, co-op, engineering).
6. **Kill / Defer** — permission to shelve a real, good emotion when no affordable, legible mechanic exists yet. Do not solve a problem before it is ready to be solved.
7. **Identity Filter** — *does this deepen Point Clear's soul* (its locked philosophy), not merely resemble its surface? Invoking it requires naming the specific principle it serves or violates.

The **emotional hierarchy** breaks ties: **Attachment** (terminal) ← **Identity** ← **solving problems through your build** (the activity) ← **Curiosity** (the ignition). Curiosity starts every chain; attachment is what it is all for.

Two standing cautions: an *intended* emotion is only real once a real player *felt* it (validate, don't assert); and the emotional layer is designed emotion-first while foundational plumbing (netcode, persistence, integrity) is engineering-first — do not emotion-wash the foundations. (Adoption recorded as [DECISIONS.md](DECISIONS.md) DEC-029.)

## The MVP Rule

The MVP exists to answer one question: is Point Clear fun? Nothing else matters until this question is answered.

## The Golden Rule

Before implementing any feature, ask one question: will this make players excited to start another run?

If the answer is not a clear "yes," the feature does not belong in Point Clear.

## Studio Motto

Combat First. Builds Second. Everything Else Exists to Support Them.

## Final Statement

Point Clear is not trying to become the biggest Action ARPG. Point Clear is trying to become the one players recommend to their friends.

Because every run creates a story. And every story creates the desire for one more run.

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [GAME_PILLARS.md](GAME_PILLARS.md)
- [VISION.md](VISION.md)
- [ROADMAP.md](ROADMAP.md)
- [DECISIONS.md](DECISIONS.md)
