# Point Clear — Vision Workshop Record — 2026-07-12

## What this document is

A faithful record of a vision/design workshop between Yoav (Game Director) and Claude (acting as design-partner / pressure-tester). It consolidates the decisions **locked**, **deferred**, and left **open** during that session, plus the places where these decisions **contradict current canonical documentation** and will need reconciliation.

**Status of this document — read before using it:**
- This is a **workshop record**, not canonical truth. It has **not** been folded into [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md), [VISION.md](../../VISION.md), or [DECISIONS.md](../../DECISIONS.md) yet.
- Per the team roles ([PROJECT_BIBLE.md § Team Roles](../../PROJECT_BIBLE.md)), **documentation planning** — deciding how/whether these locks amend the canonical docs and become `DEC-###` entries — is Yoav + ChatGPT's call, to be done deliberately in a fresh session. This record exists to make that pass easy and lossless.
- Nothing in the roadmap, tasks, or code was changed as part of this session.

**Status vocabulary used below:** **LOCKED** (Game-Director-decided this session), **DEFERRED** (own workshop scheduled; a constraint may be locked), **OPEN** (not yet worked), **⚠ CONTRADICTS CANONICAL** (conflicts with an existing approved doc; must be reconciled).

---

## 1. The spine

**Crystallized thesis (Yoav's words):** *"Point Clear is a game about solving an ever-growing space of interesting problems through the unique identity of the build you've created. Everything else exists to support that."*

**Emotional hierarchy (the tiebreaker when two desired feelings conflict):**

> **Attachment** (terminal goal) ← **Identity** (what you become) ← **Solving problems through your build** (the activity that forges identity) ← **Curiosity** (the ignition that starts every chain).

- The crystallized thesis describes the **activity** layer; the **terminal** value is still **attachment** ("this is MY character"), which is what drove the "characters never disappear" constraint. Do not let the mechanical framing demote the emotional core.
- **Curiosity chain (Yoav):** Curiosity → Exploration → Decisions → Consequences → Attachment → Memories. Curiosity is the **ignition**, not the long-term **fuel** (mastery, attachment, and social bonds sustain at hour 300).

---

## 2. LOCKED — Design Methodology (7 steps)

Every future design discussion follows this sequence:

1. **Emotion** — what should the player *feel*? (Not what happens, not what mechanic.)
2. **Philosophy** — why does that emotion matter; which higher philosophy does it serve; resolve conflicts with other emotions *before* mechanics.
3. **Mechanic** — the *smallest* mechanic that consistently produces the emotion (not the most impressive).
4. **Validation** — a pre-registered, *falsifiable* playtest signal that players genuinely felt it.
5. **Constraints** — verify it respects everything locked (finite investment, soft counters, readability, build identity, curiosity, co-op, engineering).
6. **Kill / Defer** — explicit permission to shelve a real, good emotion when no affordable + legible mechanic exists yet.
7. **Identity Filter** — *"Does this make Point Clear more like Point Clear?"* A good, fun, well-designed system can still not belong here.

**Guardrails (locked with the methodology):**
- **It's a loop, not a waterfall.** Validation can *invalidate*: a failed signal means either the mechanic is wrong (→ back to step 3) or the *emotion* was wrong (→ back to step 1).
- **Validation must be pre-registered and falsifiable** — define what *failure* looks like *before* building, or validation becomes self-congratulation. (Cluster A — an interaction that was correct but invisible — is the cautionary tale.)
- **The Identity Filter tests the *soul*, not surface resemblance.** "More like Point Clear" = deepens the locked philosophy/hierarchy, *not* resembles today's build. It is a **veto against contradiction**, not a **gate for conformity** — things that *expand* the identity must pass, or it strangles the curiosity/seasonal-growth engine. Invoking it **requires naming the specific locked principle** it serves or violates; "doesn't feel like Point Clear" alone is taste wearing the uniform of principle.
- **Boundary:** the methodology is a *quality filter*, not a *sequencer*. It makes each system good; it does not decide what to build next or whether an emotion is worth its cost. Roadmap/prioritization needs a separate impact×cost tool.

---

## 3. LOCKED — Roles

- **Yoav** owns the vision, the emotions, and the decisions — the game's **soul**.
- **Claude** is a design partner whose primary job is to **pressure-test**: challenge assumptions, find contradictions, protect the philosophy, stress-test long-term consequences. Claude may *propose* mechanics in service of testing/reconciling (a counter-proposal is a form of pressure-test) but never authors the soul.
- **Standing mandate (Yoav):** he wants resistance, not agreement. *"If we ever reach a point where you mostly agree with me, deliberately start looking harder for what could be wrong."* Honesty over being right. If several turns pass with only affirmation, that is a smell to be called out.

---

## 4. LOCKED — Design decisions

### 4.1 Build Identity & Freedom
- **L1 — Finite tree budget.** The progression tree hands out a *finite* budget; a character can never fill most of it by playing long enough. Fantasy: almost any destination is reachable, but only a small slice is ownable at once. Every direction chosen means giving up others — **opportunity cost, not a class wall.**
- Starting archetype = where the journey *begins*, not where it must end. Becoming something else (e.g. Barbarian → mage) means *redirecting* finite investment; you cannot own two complete identities at once. Different characters stay meaningfully different even at max progression.
- **L2 — Respec exists but is costly reinvention**, not a free loadout switch: gradual where practical, costly enough to prevent meta-switching, accessible enough to avoid permanent traps, more forgiving during onboarding. Cost curve TBD; principle locked: *"protects players from regret without removing the consequences of commitment."* (Given L1, respec is the *mechanism* of transformation, and its cost is the key identity-economy knob.)
- **L3 — Two clocks.** Fast/experimentation layer = equipment, weapon-granted skills, active-skill loadouts, other easily-swappable components. Slow/identity layer = finite tree investment. **Experimentation teaches the player what is worth committing to** (the fast layer de-risks the slow one).

### 4.2 Expedition / Risk Loop
- **L4 — The expedition is the unit of risk** (a chain of missions/nodes from the Base of Operations), not the single mission. Loot/rewards are **unsecured** until banked (return to base, or a deliberate extraction opportunity). Deeper = ↑ difficulty, ↑ rewards, ↑ unsecured value, ↑ failure cost. Central group decision: *"Do we secure what we have, or risk one more?"*
- Failure loses *unsecured* expedition loot; **previously banked equipment stays safe**; a reduced amount of character XP may be retained (tuning TBD).
- The tension is **escalating greed-vs-safety**, not fear of losing established gear (this deliberately diverges from Tarkov's punishment model).
- Checkpoints, partial extraction, party-disagreement, and individual-death rules are **OPEN** — do not assume them.

### 4.3 Combat Feel (experience-level; mechanisms deliberately open)
- **L5 — Attrition, as a *feeling*.** An expedition should feel increasingly dangerous the longer it continues — "you increasingly have something to lose." Mechanism (health/healing/resources/durability/corruption/escalation/hazards/other) is **OPEN**. Rationale: without attrition, the L4 greed-vs-safety loop is toothless.
- **L6 — Dynamic density / rhythm:** Flow → Threat → Recovery → Flow. Ordinary enemies in satisfying numbers (express your build, feel powerful); an elite or dangerous combination shifts the player from expression to attention. Deeper expeditions tilt the rhythm toward threat.
- **L7 — Baseline mood = competence.** Players generally feel capable; threat *challenges* confidence, it does not replace it. Death should read as *"we pushed too far / we weren't prepared,"* never *"the game got unfair."*
- **L8 — Build legibility is mandatory and kinesthetic.** You feel your build every few seconds; changing your build feels different *in combat*, not on a spreadsheet. If players can't recognize their own build through gameplay, Point Clear has failed as a build game.
- **L9 — Wonder:** combat should continuously *create* questions ("what if I combine this with my build?"), not only answer them.
- **Content constraint implied by L8+L9:** every build needs a distinct *kinesthetic signature*, and combat must surface emergent interactions legibly. This is the hardest, never-finished ongoing content demand — a permanent tax, not a one-time feature.

### 4.4 Problem-Space Philosophy
- **L10 — The world asks the questions.** Enemies are one voice; mission, environment, route, expedition depth, modifiers, boss (and later weather) all ask *"how does your build solve me?"* No single system carries build diversity alone.
- **L11 — Deeper = harder through *demand*, not primarily HP/damage:** dangerous compositions, complex objectives, hazards, expedition modifiers, elite combos, boss pressure, fewer recovery opportunities, more decisions under pressure. Numbers scale to *support*, not *define*.
- **L12 — Enemies threaten through *behavior*, not stats.** Same behavior + 5× HP is not interesting; behavior creates decisions.
- **L13 — Readability governs growth (both directions).** Instantly read *"what is this enemy trying to do?"*; enemies visibly react to the build (CC visibly controls, poison visibly spreads, chains visibly chain). *"If you need a wiki, we failed."* **Complexity may grow without bound only as fast as readability keeps pace** — the wiki-moment is the failure line, and this is also most of the onboarding answer.
- **L14 — Co-op scales through *problems*, not HP.** More players → more/simultaneous problems and combinations, never "the same enemy with 4× health."
- **L15 — Justify-your-existence.** Every new enemy must introduce a *new problem* that didn't exist before; every new build option must solve problems *differently*, not just produce bigger numbers. Growth = more interesting questions, not more content volume.
- **Soft counters, never hard gates** (forced by L1): the world creates *matchup texture* (your build handles some problems gracefully, others awkwardly-but-possibly) but never *"impossible without capability X."* Hard counters + a finite budget = an unwinnable or identity-destroying game. The finite build survives a combinatorial problem-space only because **non-build tools** (positioning, consumables, teammates, routing, banking) cover its gaps — so problem-difficulty and player escape-hatches must scale *together*.
- **Qualitative vs quantitative:** "adds a new question, not bigger numbers" governs the *content catalog* (breadth grows qualitatively). A *character's* investment in its chosen lane may still grow *quantitatively* — mastery of your lane should make you measurably better at it, or commitment feels unrewarding. Don't apply "no bigger numbers" to all progression.

### 4.5 Content-Generation Direction
- **Content model = "authored questions, generated answers."** Authored, readable **categories** (the agency layer + the veteran's build-curiosity) wrap generated, hidden **instances** (the curiosity + wonder layer). *"Reveal the category, hide the instance."*
- **Map-reveal emotion hierarchy:** PRIMARY = **Curiosity** (active — a verb that pulls you forward), SECONDARY = **Agency** (enough info to own the decision), SUPPORTING = **Wonder** (never fully known). Combination = anticipation via *"a decision that genuinely belongs to them."*
- **The object of curiosity must migrate** from *world* ("what's out there?", hour 1) to *build × world* ("what happens if I bring THIS build out there?", hour 300), or veterans go dead at the doorway. The map must communicate the *kind of questions* an expedition asks, in terms that connect to build capabilities.
- **Why this model:** curiosity is the fastest-decaying fuel; neither pure-authored (a finite battery) nor pure-procedural (a learnable generator) sustains it. The sustainable form is **combinatorial generation from an expanding, seasonally-refreshed primitive vocabulary** — which is *also* the same mechanic that keeps the balance/QA surface affordable for a small team (the emotion and the economics want the same machine).

---

## 5. DEFERRED (own workshops; constraint locked)

- **Seasons — DEFERRED.** Locked constraint: **characters never disappear.** A season should feel like a fresh expedition / new possibilities, *not* "you lost everything." What resets vs. persists (economy? ladder? world? a seasonal character?) is unresolved and needs its own workshop. ⚠ **CONTRADICTS CANONICAL** — see §7.
- **Trading — DEFERRED.** Locked philosophy: trading should *enrich the world* (create stories — *"this isn't for my build, but my friend would love it"*), and must **never replace/bypass progression**. Form (party-only gifting / auction house / marketplace / other) is deliberately undecided. Note the anti-MMO boundary biases toward friends/party-scoped trading.

---

## 6. OPEN — for future workshops

- **Multiplayer mechanics:** the trinity trap (synergy without forced roles), solo/co-op balance parity, world-map ownership when differently-progressed friends party, and co-op loss rules (who loses a gifted item on death). Locked co-op *philosophy* already exists (L14 + "multiply possibilities, not damage"); the *mechanics* are open.
- **Onboarding / complexity:** a game defined by an ever-growing interacting problem-space is the hardest kind to learn; L13 (readability governor) is the primary mitigant but the onboarding experience itself is undesigned.
- **Production realities of the content model** (the three costs consciously parked): (1) the pre-registered **validation signal** for map-reveal curiosity; (2) the **bounded generator** — instances must honor the authored category's promise *and* respect soft-counters, requiring curation guardrails; (3) the **content-treadmill commitment** — sustained curiosity now depends on regularly shipping new primitives, a standing live-ops obligation for a small team.
- **Technical magnitude:** the vision describes a live, server-authoritative online game with a persistent economy, trading, seasons, and anti-cheat. The current project is a single-player prototype and Phase 1 (networked prototypes) has not started. This gap is a different *kind* of project, and it is the largest unbuilt foundation under the whole vision. Foundation systems (netcode, persistence, integrity) are **engineering-first**, not emotion-first.

---

## 7. ⚠ Contradictions with current canonical docs (reconciliation to-do)

- **Seasons vs. permanence.** [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) points 22 & 25 state *"only a new Season resets character progression"* / seasons "begin with a fresh character progression." This session locked **characters never disappear** and an intent that seasons are *not* a "you lost everything" wipe. These cannot both stand as written — the seasons workshop must produce a model (e.g. seasonal characters migrating to a permanent legacy realm; a permanent account spine under a seasonal expedition layer; economy-only reset) and the canonical text must be amended accordingly. Do **not** paste the new constraint alongside the old wording.
- **General:** several locks above sharpen or extend CORE_PHILOSOPHY (finite budget → point 10; soft-counters; readability-as-growth-governor; the emotion-first methodology itself). Most are *deepenings* rather than contradictions, but each should be consciously placed during the canonical consolidation pass rather than assumed.

---

## 8. Reference — success tests stated this session

- **Master player test:** the game should naturally, repeatedly provoke *"What if I tried something completely different?"* over hundreds of hours.
- **Build-game test:** a player should be able to recognize their own build purely through how combat *feels*.
- **Identity test (Yoav's success criterion):** players describe Point Clear as *"reminds me of those games in some ways… but it feels like Point Clear."*
- **Anti-goals (design boundaries):** not an MMO; not a fixed-class RPG; not a highest-damage-number game; not a one-correct-build game; not a copy of PoE / Diablo / Tarkov (borrow *feelings*, not systems).
