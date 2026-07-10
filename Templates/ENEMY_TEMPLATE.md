# Enemy Template

Use for documenting a standard enemy type under [Documentation/Enemies/](../Documentation/Enemies/) once enemy design begins. No enemies have been designed yet — this template is a placeholder structure only. For boss-tier enemies, use [BOSS_TEMPLATE.md](BOSS_TEMPLATE.md) instead.

---

## Enemy Name


## Status
[PROPOSAL] | [APPROVED FACT] | [ASSUMPTION] | [UNRESOLVED]

## Summary

(One or two sentences: what the enemy is and its role in encounters.)

## Behavior Description

(Design-level description of movement, attack patterns, and AI behavior.)

## Threat Role

(How this enemy contributes to "large groups of enemies" encounters — e.g. swarm, ranged pressure, control, etc. Do not assume specific roles beyond what's been approved.)

## Performance Considerations

(Notes relevant to Pillar-driven performance-aware design for large enemy counts — see [PROJECT_BIBLE.md § Engineering Principles](../PROJECT_BIBLE.md#5-engineering-principles).)

## Networking Considerations

(Server-authoritative behavior expectations, once a networking solution is chosen — see [DECISIONS.md](../DECISIONS.md).)

## Data / Configuration

(Which stats/behaviors should be data-driven rather than hardcoded.)

## Open Questions

(Anything unresolved that blocks full specification.)

## Related Documents

- [GAME_PILLARS.md](../GAME_PILLARS.md)
- [DECISIONS.md](../DECISIONS.md)
