# Point Clear — Decisions

This document is the persistent record of accepted decisions and open questions. When a decision changes, update its entry rather than deleting history where practical, and log the change in [CHANGELOG.md](CHANGELOG.md). See also: [PROJECT_BIBLE.md](PROJECT_BIBLE.md), [ROADMAP.md](ROADMAP.md).

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

---

## Adding a New Decision

Use [Templates/DECISION_TEMPLATE.md](Templates/DECISION_TEMPLATE.md). Assign the next sequential `DEC-XXX` ID, and move the corresponding item out of "Unresolved" if it resolves one.

## Related Documents

- [PROJECT_BIBLE.md](PROJECT_BIBLE.md)
- [VISION.md](VISION.md)
- [ROADMAP.md](ROADMAP.md)
- [CHANGELOG.md](CHANGELOG.md)
