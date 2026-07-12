# Point Clear — Design Architecture Review — 2026-07-12

## What this document is

A faithful record of an **architecture review** between Yoav (Game Director) and Claude (acting as pressure-tester). It grew out of Sprint 2.11 enemy design and turned into a question about the *shape* of Point Clear's design philosophy: is the game governed by a single design law, or by something else?

**Status of this document — read before using it:**
- This is an **architecture review, NOT canon** — with one exception now promoted (see next bullet). Its central conclusion has **not** been folded into [PROJECT_BIBLE.md](../../PROJECT_BIBLE.md), [DESIGN_DNA.md](../../DESIGN_DNA.md), [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md), or [GAME_PILLARS.md](../../GAME_PILLARS.md), and those documents were **left unchanged**.
- **Promoted 2026-07-12:** a later sub-thread of this review (the *build-divergence measurement* model) **was** promoted into canon as **[DECISIONS.md](../../DECISIONS.md) DEC-030** (two timescales of divergence; Sprint 2.10 reframed as a Behavioral Divergence Check; instrument marked *current & revisable*), with the corresponding [ROADMAP.md](../../ROADMAP.md) updates. That is the *only* piece promoted.
- The **broader conclusion** below (Point Clear is guided by several *scoped design pillars in tension* rather than one governing law) remains **UNDER REVIEW** and non-canonical. Promoting it is a deliberate future decision (Yoav + ChatGPT, per [PROJECT_BIBLE.md § Team Roles]), not something this record performs.
- No roadmap, task, or code change resulted from this session. Sprint 2.11 (PC-014) design was paused mid-exploration to run this review; the Cluster B gate remains intentionally open and unaffected.

---

## How we got here (the ladder)

The review began with a concrete question — what *kind* of question should the second enemy (Sprint 2.11) ask — and progressively abstracted, one reframe at a time:

1. **Usage lens** → "the enemy should revive the least-used mechanic (Volatile Fracture)." *Rejected:* optimizes usage frequency, not understanding.
2. **Comprehension lens** → "the enemy should reveal the *purpose* of a mechanic the player never discovers." Better. Pointed at Volatile Fracture + Fracture Bolt's spread identity.
3. **Mental-model lens** → "the enemy should change how the player *thinks*, not teach a button." The chaser installs a **fortress mindset** (hold the center, let the world flow inward); the second enemy reveals its boundary.
4. **Teaching-philosophy lens** → the fortress mindset isn't *wrong*, only *incomplete*. The enemy should **reveal a boundary, not shatter a belief** — target emotion "I didn't know the game could ask this," never "I was wrong."
5. **Consequence lens** → the boundary should be **incompleteness, not lethality**: the enemy denies *closure* (it won't die to fortress play) rather than threatening *survival*. This reads identically across skill levels and preserves the lesson.

At step 5 the insight looked bigger than one enemy, and the review pivoted to: *is "incompleteness before punishment" a design law for the whole game?*

---

## What we tried to canonize, and why each attempt broke

Each candidate "law" was stress-tested by attacking it, not defending it. Every one broke, and the way it broke is the useful part.

- **"Incompleteness before punishment (everywhere)."** *Broke:* the secure-or-lose layer (unsecured currency and the unbanked Weapon Module are *lost* on death) is punishment on purpose — it's the entire fear-of-loss thread. The rule contradicted the game's own central open experiment.
- **"Permanent systems teach through incompleteness; volatile systems create stakes through loss."** *Broke:* the Skirmisher itself (a *volatile* system that teaches through *incompleteness*); the finite build budget (a *permanent* system whose weight comes from opportunity cost + costly respec, i.e. stakes); leaderboards; the deferred economy; and it would have silently pre-decided the intentionally-open seasons question (DEC-028).
- **"Teaching → incompleteness; stakes → bounded loss; the self is never at stake, only provisional ownership."** *Partially survived, then broke on scope:*
  - *Stakes can come from incompleteness* (endgame "push to your ceiling"; opportunity cost) — breaks the 1:1 mapping.
  - *Teaching can come from loss* (the death-retry boss) — the rule only holds where a non-lossy exit (extraction/disengage) always exists.
  - *The self/ownership line blurs* — in an ARPG, identity is stored in possessions, so losing a signature item feels like losing self.
  - *Silent on whole categories* — value exchange (economy/vendors/crafting/trading), social/relational stakes (co-op/guilds/leaderboards), impermanence-by-design, urgency.
- **"Authored consequence / agency."** More unifying, but *so abstract it stopped guiding decisions* — the diagnostic signature of over-compression: a principle that explains everything predicts nothing.

---

## The architectural conclusion (under review)

**Point Clear is not reducible to a single governing law.** The repeated pattern — each "deeper" law more abstract and less actionable than the last — is evidence that we were compressing a plural thing into one, not that we hadn't yet found the right one.

The structural reason a single law can't hold: **Point Clear's identity lives in the *tension between* principles, and a single law has no internal tension.** The clearest case is already in the build — "never invalidate the self" and "create real fear-of-loss" pull against each other, and **secure-or-lose is that tension made mechanical** (the self is safe; the provisional is at risk). Compress those two into one rule and you delete the seam that makes the loop interesting.

But "several independent principles" is not the end state either — a flat list becomes a grab-bag where any decision cites whichever principle is convenient. The shape that fits is:

> **A small set of Design Pillars, each with an explicit scope, held in deliberate tension.**

Two disciplines make it a system rather than a pile:
1. **Scope each pillar.** The whole earlier mess came from taking a *pedagogy* principle ("teach through incompleteness") and misapplying it to risk, economy, and social systems. A principle without a stated domain *will* be misapplied.
2. **Name the tensions; don't resolve them away.** The tensions are load-bearing. *How* Point Clear resolves each one is more of its identity than any single pillar is.

### Candidate pillars surfaced (PROVISIONAL — not canon, not exhaustive)

These are the axes the review surfaced. They answer genuinely different questions (identity, content, build-structure, pedagogy, risk), which is why they don't reduce to one root:

- **Self-permanence** — the player's identity is never invalidated. *(Consistent with DEC-016, DEC-028. Open sub-question: does "self" include identity-bearing possessions? Promoting this pillar would also foreclose permadeath/hardcore — a deliberate cost to weigh.)*
- **The world asks the questions** — content threatens through behavior; the build answers. *(DEC-024.)*
- **Expanding the interaction space** — build identity comes from combination, not stat inflation. *(CORE_PHILOSOPHY.)*
- **Teach through incompleteness** — pedagogy reveals boundaries; first contact denies closure, not survival. *(Newly surfaced; the Skirmisher's design principle.)*
- **Stakes through authored, bounded loss** — tension is the downside of a bet the player chose to make. *(DEC-019, secure-or-lose.)*
- **Scarcity creates identity** — a finite budget makes choices mean something. *(DEC-021.)*

### The tensions worth naming (the actual map)

- **Self-permanence ⟷ Stakes.** Resolved today by the self/provisional split (secure-or-lose).
- **Expanding the interaction space ⟷ Finite budget.** Combine richly, but you can't have everything; scarcity forces the choices that create identity.
- **Teach gently ⟷ Real teeth.** First contact with a new question is soft (incompleteness); the same question earns teeth once understood.

---

## Practical spillover for Sprint 2.11 (also not canon)

Independently of the philosophy question, the review produced a concrete, testable design posture for the second enemy: it should **reveal a boundary through incompleteness, not lethality** — low on survival threat, deliberate about denying *closure* (e.g. an enemy that keeps its distance and will not resolve to "let it come to you" play). This makes an under-understood part of the toolkit reveal itself (reach / ranged marking) and — critically for why 2.11 was chosen over 2.10 — creates the second "question" the build-convergence check needs to be interpretable, while staying soft enough not to collapse build diversity into a single mandated answer.

This posture is **a design lead, not a spec and not canon.** The full PC-014 specification is deliberately unwritten until the enemy candidate is chosen.

---

## Explicit non-actions

- **No canonical philosophy document was changed** (PROJECT_BIBLE, DESIGN_DNA, CORE_PHILOSOPHY, GAME_PILLARS, DECISIONS all unchanged).
- **No `DEC-###` was added.** The pillars-and-tensions model is not a decision yet.
- **No task, roadmap, or code change.** Sprint 2.11 remains in design; the Cluster B gate remains open.
- Promotion of any of this into canon is a **deliberate, separate decision** for the Game Director and documentation process.
