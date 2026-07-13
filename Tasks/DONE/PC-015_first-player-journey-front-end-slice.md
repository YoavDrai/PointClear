## Task ID
PC-015

## Title
First Player Journey — Front-End Vertical Slice (greybox)

## Status
DONE (2026-07-13) — Implemented in five verified stages and **approved by the Game
Director** after the first complete hands-on playtest of the whole vertical slice
(2026-07-13). Documentation synchronized (ROADMAP, CHANGELOG, DECISIONS DEC-031,
README). Committed as the milestone commit `feat: add first-player-journey front-end
vertical slice` on `feature/sprint-1-first-playable`; **pushed by Yoav via GitHub
Desktop**. Technical-review box left unchecked (no Technical Director present); DONE on
the Game Director's explicit authority, flagged (consistent with PC-007..013).

## Priority
High

## Owner
Claude (implementation) — design/UX decisions owned by Yoav (Game Director)

## Reviewer
Yoav (Game Director)

## Milestone (not a Cluster sprint)
This is a **front-end vertical slice** that wraps the existing Operation loop. It
is NOT part of the Phase 2 Cluster A/B/C research-sprint sequence and does not
touch that sequence's gates. It does not reopen game philosophy; vision and
canonical docs are treated as stable.

## Objective
Build the smallest COMPLETE player journey so the game feels like a real game
from launch, without entering combat directly:

Main Menu → New Character → Starting Direction → World Map → (existing) Operation
→ Results → Return to World Map (re-enterable) → path back toward Main Menu.

Greybox only: functional flow + readable placeholder UI. NOT final art, settings
infrastructure, localization, platform integration, or any persistence to disk.

## Architecture Decision (approved)
- **Single scene.** All front-end screens are **uGUI Canvas layers authored
  inside the existing `PrototypeScene`** (the combat `OperationHud` already lives
  there — front-end UI coexisting with combat is the established pattern). No new
  authored scenes; the rejected `FrontEnd`/`WorldMapShell` additive-scene design
  bought only "leave PrototypeScene.unity untouched" (a VC/workflow benefit, not a
  technical one) at the cost of additive load-ordering + cross-scene wiring.
- **`SessionContext`** — a plain `static` holder (name, presetIndex,
  confirmedNode, flowState). Survives scene reloads within a Play session; reset
  on domain reload via `[RuntimeInitializeOnLoadMethod]`. No DI, no service
  locator, no save abstraction, no account/profile model, no multiplayer registry.
- **A flow controller** (front-end state machine) owns which Canvas layer is
  shown, gates player input outside combat, guards against duplicate scene loads /
  double-confirm, and fails safe to Main Menu on missing refs / invalid state.
- **Reload boundary:** the World-Map↔Operation re-entry loop reuses the EXISTING
  non-reloading Operation lifecycle (`OperationController.StartOperation()` + its
  `OperationStarted/Succeeded/Failed` events) — nothing unloads, so Level / XP /
  Skill Points / Banked currency / equipped Module are preserved untouched
  (DEC-016). A `LoadScene("PrototypeScene", Single)` self-reload is used **only**
  as the "New Character" progression-reset boundary (UC18) — the idiomatic reset,
  avoiding hand-written resets across the six progression components.

## Player flow & required screens
1. **Main Menu** — Play (functional), Options (labeled placeholder w/ Back),
   Credits (simple screen w/ Back). Every secondary screen returns to Main Menu.
2. **Play → New Character** — no Continue/saved-character flow this milestone.
3. **Character Creation** — name field (empty/whitespace blocks Confirm with
   feedback), two presets (A=green / B=pink recolor of the current player capsule),
   live preview of name+preset, Confirm creates the temp in-session character,
   Back preserves temp selections where practical.
4. **Starting Direction** — simplified skill-tree stand-in: ONE available starting
   node; select/confirm; copy communicates "this is where you begin / more paths
   later / a starting vector, not a class prison." Back → Character Creation with
   choices intact.
5. **World Map** — ONE available, clearly clickable node; selecting shows
   Operation name, objective, basic danger indicator, available status; Confirm →
   Enter Operation. Structure must allow adding nodes later without implementing
   the final procedural map.
6. **Operation** — reuse existing combat/Operation systems unchanged; carry the
   character name + preset into the session; existing progression stays intact.
7. **Results → World Map** — reuse the existing results summary; the next
   navigation returns to the World Map (not a full restart); the same temp
   character + session progress remain; the Operation is re-enterable; a clear
   path leads back toward Main Menu.

## Starting skills (decision)
Basic-weapon-only is **explicitly deferred to a separate task.** Active skills
currently activate on input UNGATED by level (level only scales magnitude; level 0
still fires at index-0 tuning). Making a character start weapon-only requires (a)
adding a level>0 activation gate to each Active skill and (b) a new "unlock"
mechanism — behavioral changes to `Skills/` + input/progression. This milestone
carries the player in with CURRENT skill availability unchanged (do not silently
change skill availability).

## Temporary session state (only what the slice needs)
Character name; selected preset; confirmed starting node; current navigation
state; references to the existing session progression where necessary. No disk
saving. Stopping Play Mode may reset the temp character. Do NOT build save/load,
account profiles, cloud save, character slots, delete-character, Continue,
seasonal storage, or lobby state. Avoid designs that block future ≤4-player
support (hence SessionContext is a deliberate single-seam holder, not a hostile
singleton assumption).

## Use cases the implementation must satisfy
1 first launch→Main Menu · 2 Menu→Play · 3 missing name blocks confirm w/
feedback · 4 preset selection visibly updates · 5 Back from Char Creation→Main
Menu · 6 Confirm character→Starting Direction · 7 Back from Starting
Direction→Char Creation w/o losing temp choices · 8 Confirm direction→World Map ·
9 select node→Operation info · 10 Confirm→Operation · 11 Success→Results→World
Map · 12 Failure→Results→World Map · 13 re-enter Operation same session · 14
World Map→toward Main Menu · 15 no duplicate scene loads / double-confirm · 16
missing refs/invalid state fail safe to a known screen · 17 return from combat
does NOT reset Level/XP/Skill Points/Banked/secured equipment · 18 New Character
resets ONLY the old temp character's data.

## UI direction
uGUI (Canvas), NOT OnGUI, for all new front-end screens. Simple, readable,
consistent, clearly-a-prototype panels/buttons/labels with visible selection
states. Placeholder shapes/colors/text only. No downloaded assets, no third-party
UI packages without approval.

## Implementation stages (compile + test after each)
- **S0** Environment verify (Unity reachable, uGUI/TMP available, inspect
  PrototypeScene) + `SessionContext` static + flow-controller skeleton.
- **S1** Main Menu + Options placeholder + Credits + reliable back-to-menu.
- **S2** Character Creation (name validation, 2 presets, live preview, Back
  preserves choices, Confirm→SessionContext).
- **S3** Starting Direction (single node, confirm/back w/ preserved choices).
- **S4** World Map (single node, operation info, Enter Operation) + apply
  name/preset to player + input gating + drive existing OperationController.
- **S5** Results→Return to World Map (observe Operation events), re-entry, return
  toward Main Menu, New-Character self-reload reset, guards (UC15/16/17/18).
- **S6** Full-journey verification pass + exact hands-on test steps for Yoav.

## Files Allowed to Edit
- **New:** `Assets/Systems/FrontEnd/*.cs` (SessionContext, flow controller, per-
  screen presenters, player preset applier). New meta files as generated.
- **Modify (additive, self-contained):** `Assets/Scenes/Prototype/PrototypeScene.unity`
  (add a self-contained FrontEnd Canvas root + wiring; touch NOTHING PC-014
  touches). Possibly `Assets/Scenes/Bootstrap/Bootstrap.unity` only if flow entry
  requires it.
- **Docs:** `CHANGELOG.md`, `ROADMAP.md` (milestone note only), `Tasks/**/PC-015_*.md`.

## Files Forbidden to Edit
- All combat/progression/skill/operation SYSTEMS code: `OperationController.cs`,
  `ExtractionPoint.cs`, `OperationState.cs`, `CurrencyWallet.cs`, `CurrencyPickup.cs`,
  `EnemyCurrencyDrop.cs`, `WeaponModule*.cs`, `Health.cs`, all `Skills/`,
  `PlayerController.cs`, `PlayerRespawn.cs`, `PlayerStats.cs`, `PlayerXP.cs`,
  `PlayerLevel.cs`, `SkillPoints.cs`, `SkillProgression.cs`, `HitscanWeapon.cs`.
  (Front-end drives these ONLY through existing public surfaces.)
- All PC-014 in-review work: `EnemySpawner.cs`, `StalkerAI.cs`, `Stalker.prefab`,
  and the undocumented `Charger/Empowerer/Surrounder` AI + prefabs. Do not stage,
  commit, overwrite, or refactor any of it.
- All canonical philosophy docs; the Cluster gate sections of ROADMAP/CHANGELOG.

## Out of scope (do NOT build)
Final menu art/animations, settings infrastructure, localization, platform
integration; body/face/hair/gender customization or cosmetic inventory;
full/scrolling skill tree, pathfinding, class system; procedural map, multiple
routes/tiers, modifiers, fog of war, seasonal maps; run history, statistics,
campaign progression, node unlocking, multiple Operations, cross-restart
persistence; any save/load/account/cloud/slots/Continue; multiplayer lobby.
No generic menu framework, DI, service locator, or save abstraction.

## Review Notes
Machine-checkable criteria (navigation, validation, no-duplicate-load, progression
preserved on re-entry, correct reset on New Character, zero console errors) will be
Play-Mode verified by Claude. Felt/subjective criteria (does it feel like a real
game's front end; is it readable/pleasant-enough) are reserved for Yoav's hands-on
review. Stop in REVIEW; provide exact hands-on test steps covering the full
journey.
