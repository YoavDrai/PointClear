## Task ID
PC-005

## Title
Documentation Cleanup — Align Class-Oriented Templates with "The Build Is the Class"

## Status
TODO

## Priority
Medium

## Owner
(unassigned)

## Reviewer
Yoav (Game Director)

## Dependencies
- None — pure documentation cleanup, no code or design-system dependency

## Related Documents
- [DECISIONS.md](../../DECISIONS.md) DEC-017 (No Fixed Classes)
- [CORE_PHILOSOPHY.md](../../CORE_PHILOSOPHY.md) § Character Philosophy (points 16–17)
- [Templates/CLASS_TEMPLATE.md](../../Templates/CLASS_TEMPLATE.md)
- [Templates/SKILL_TEMPLATE.md](../../Templates/SKILL_TEMPLATE.md)
- [Templates/WEAPON_TEMPLATE.md](../../Templates/WEAPON_TEMPLATE.md)

## Objective

Resolve a documentation contradiction discovered 2026-07-11 while auditing consistency for the new Build Identity / Discovery / Interaction Space / Healthy Meta principles: `Templates/CLASS_TEMPLATE.md` still describes "documenting a playable character class... once class design begins," and both `SKILL_TEMPLATE.md` and `WEAPON_TEMPLATE.md` carry an "Associated Class(es)" field pointing at it. This directly contradicts DEC-017 ("Point Clear has no predefined gameplay classes — no Warrior/Mage/Rogue-style archetypes") and CORE_PHILOSOPHY point 17 ("Gameplay identity is created entirely through build... the build is the class"). These templates predate the ARPG/no-classes pivot and were never reconciled with it.

## Background

Flagged as a separate, pre-existing issue during the CORE_PHILOSOPHY architecture review rather than folded into that pass — it's unrelated in origin (predates today's session) and bundling it in would have muddied a focused change set. `Documentation/Classes/` currently contains only a `.gitkeep` — no class content exists yet, so this is purely a template/documentation fix with no content migration required.

## Requirements

- Decide (Game Director call, not to be assumed): retire `CLASS_TEMPLATE.md` entirely, or redesign it into something consistent with "the build is the class" — e.g., a template for a cosmetic character-creation option, if that's ever built.
- Remove or repurpose the "Associated Class(es)" field in `SKILL_TEMPLATE.md` and `WEAPON_TEMPLATE.md` to match whatever the above decision produces.
- Check whether `POINT_CLEAR_KNOWLEDGE_MAP.md`'s Content Library (KD-12) domain references classes anywhere and needs a matching update.

## Acceptance Criteria

- [ ] No template implies fixed, predefined gameplay classes exist or are planned
- [ ] `SKILL_TEMPLATE.md` and `WEAPON_TEMPLATE.md` no longer contradict DEC-017
- [ ] Resolution is recorded in `CHANGELOG.md`

## Out of Scope

- Any actual character-creation or cosmetic-customization system design — this task is documentation-only
- The Build Identity / Discovery / Interaction Space / Healthy Meta pass (separate, already-completed work)

## Risks

- None identified — pure documentation, no gameplay code involved

## Implementation Report

(Not started.)

## Review Notes

(Not started.)

## Game Director Approval

- [ ] Approved by Yoav
- Date:
- Notes:

## Definition of Done Checklist

- [ ] Acceptance criteria pass
- [ ] Unity has no new compiler errors
- [ ] Required testing was completed
- [ ] Yoav approved the result
- [ ] Technical review was completed
- [ ] Relevant documentation was updated
