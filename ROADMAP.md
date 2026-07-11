# Point Clear — Roadmap

Status legend: **[APPROVED FACT]**, **[ASSUMPTION]**, **[PROPOSAL]**, **[UNRESOLVED]**. See also: [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md), [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [VISION.md](VISION.md), [DECISIONS.md](DECISIONS.md).

**Note — two separate numbering systems, do not conflate them:** the informal "Sprint 1", "Sprint 2.x" numbering used in day-to-day development (tracked in `Tasks/` and `CHANGELOG.md`) and this document's own Phase 0/1/2 numbering are unrelated schemes that happen to both use numbers. **"Sprint 2.x" is not "Phase 2"** — there is no rule that Sprint N maps to Phase N, and the two are not expected to reconcile into a single sequence. Each Phase's status below is assessed independently, against that Phase's own listed goals, regardless of which Sprint number is current. As of 2026-07-11: informal Sprint work has reached "Sprint 2.x" while, on their own separate criteria, Phase 0 is still in progress and Phase 1 has not started — this is expected under this model, not an inconsistency to resolve.

This roadmap describes phase intent, not detailed production promises. Later phases are tentative and will be broken down further as earlier phases complete.

---

## Phase 0 — Foundation and Pre-Production

**Status: In Progress** — **not completed**.

**[APPROVED FACT]** Goals:
- Repository and documentation foundation
- Game vision refinement
- Core gameplay loop definition — see [Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md](Documentation/Gameplay/CORE_GAMEPLAY_LOOP.md)
- Multiplayer requirements definition — unblocked now that Operation structure is defined (DEC-009–DEC-012); not yet written
- Networking technology evaluation
- Technical risk prototypes
- Initial scope definition — unblocked now that Operation structure is defined (DEC-009–DEC-012); not yet written

---

## Phase 1 — Technical Prototypes

**Status: Not Started** — none of this phase's own listed goals have been attempted. The single-player combat/progression prototyping tracked informally as "Sprint 1.x"/"Sprint 2.x" does not count toward this phase; none of it is networked or multiplayer. Flagged as a sequencing note, not a status change: single-player Vertical-Slice-shaped content (see Phase 2, below) is being built ahead of the multiplayer technical prototypes this phase covers.

**[PROPOSAL]** Potential prototypes:
- Two connected players
- Networked movement
- Server-authoritative enemy
- Networked damage
- Basic ability execution
- Enemy-count performance testing

---

## Phase 2 — Vertical Slice

**Status: In Progress** — assessed against this phase's own goals below, independent of "Sprint 2.x" numbering (see the Terminology Note at the top of this document). Not complete.

**[PROPOSAL]** Initial target:
- 1 small map — not done (current greybox arena is a combat-prototype test space, not a designed map)
- 1 playable character (cosmetic-only creation — no fixed classes, per [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Character Philosophy) — in progress (one placeholder playable character exists; cosmetic-only creation not built)
- Small ability set — in progress (one weapon exists; a small set of active skills is the next planned increment)
- Small enemy set — in progress (one enemy type exists)
- 1 boss — not done
- 1–2 player online co-op proof — not done (no networking solution chosen; see Unresolved Decisions in [DECISIONS.md](DECISIONS.md))
- Basic persistent character progression across missions (level, experience, equipment — see [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md) § Persistence Philosophy), not run-reset progression — in progress (persistent Level/Experience/Skill Points implemented as a design model per DEC-020; no equipment yet; no save system yet)
- Basic results screen — not done
- Prototype leaderboard submission — not done

---

## Later Phases (Tentative)

**[PROPOSAL]** — listed for awareness only, not scoped:

- Pre-Alpha
- Alpha
- Beta
- Season 0
- Season 1

---

## Related Documents

- [CORE_PHILOSOPHY.md](CORE_PHILOSOPHY.md)
- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [VISION.md](VISION.md)
- [DECISIONS.md](DECISIONS.md)
- [CHANGELOG.md](CHANGELOG.md)
