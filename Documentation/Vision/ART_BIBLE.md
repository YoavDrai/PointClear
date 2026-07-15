# Point Clear — Art Bible v1.0 — Officially Frozen

> **Status:** **Officially Frozen (v1.0)** — all 7 Game-Director decisions applied.
>
> **Canonical source of truth:** *this Markdown document in the repository.* It is the authoritative version of the Art Bible. If it ever diverges from any other copy, this file wins.
>
> **Visual companion / reference (non-canonical):** the rendered, styled Art Bible Artifact —
> <https://claude.ai/code/artifact/99180619-c8d5-473b-ae71-ac4c8d068178>
> The Artifact is a presentation view only; it does not override this document.
>
> **Scope:** Milestone 2 pre-production · **Engine:** Unity 6 · URP · PC + Mobile · **View:** Top-down twin-stick.
>
> *Exported verbatim from the frozen v1.0 Artifact on 2026-07-15; full approved content preserved. Documentation only — no Unity, assets, purchases, or Phase-0 production are implied or begun by this file.*

---

# The Visual Foundation

*Point Clear · Art Bible v1.0 · official-freeze candidate*

> A dark-fantasy world painted in weathered stone and patina — so that everything trying to kill you burns bright against it, read cleanly from straight overhead. This is the constitution every future arena, class, and skill is built to obey.

- v1.0 — official-freeze candidate · all 7 GD decisions applied
- **Scope** Milestone 2 pre-production
- **Engine** Unity 6 · URP · PC + Mobile
- **View** Top-down twin-stick

> **The global visual identity — one line for the whole game**
>
> “Top-down stylized-natural **dark fantasy**, combining *Walkers Attack*-level combat readability with *New World*-inspired materials, atmosphere, equipment, and environmental richness.”
>
> *This defines the **entire game**, not any one arena. The coastal island is only the first validation arena (§1); the same identity must carry castles, forests, mines, temples, graveyards, and swamps without redesign.*

> **What "New World" means here — and what it does not**
>
> New World is a reference for **material richness, grounded fantasy atmosphere, equipment design, and lighting** only. It is **not** a reference for MMO scope, open-world structure, or any required level of fidelity. Point Clear is a bounded-arena game (DEC-034); the aesthetic is borrowed, the scope is not.

> **Visual Reference Matrix — three reference groups**
>
> Three references triangulate the game. **Group A — Walkers Attack** (attached frames): how it *plays and reads* — camera, crowd scale, map composition. **Group B — New World**: how it *looks* — materials, atmosphere, lighting. **Group C — the greybox**: what must *not change* — the validated gameplay & readability baseline.

#### Group A — Attached gameplay frames — *Walkers Attack* *(external gameplay & readability reference)*

- **Borrow:** The high **top-down camera** height, angle & gameplay framing; **small-but-readable** character scale; **extremely dense enemy crowds**; **instant player location** inside a horde; **simple enemy silhouettes** at crowd scale; **special enemies staying readable** within the mass; **structured maps** (rooms, paths, walls, entrances, props, landmarks); detail concentrated at **boundaries & landmarks**; a **calm, readable combat floor**; soft lighting & shadows that give depth without hiding gameplay.
- **Reject:** The **modern setting**; **guns / firearms**; office, city, or contemporary **architecture**; and any **exact** characters, enemies, layouts, UI, colours, or visual identity. Principles only — never the look.
- **Transform:** Translate the readability, camera, crowd-scale, and environment-composition principles into an **original grounded dark-fantasy world**: the lone protagonist → a dark-fantasy adventurer; the walker mass → the blight faction; the mansion/museum interior → castle / temple / crypt arenas; the guns → the weapon & skill kit.
- **Governs:** Laws iii & vi · §1 Environment & Arena System · §2 Player scale & locator · §3 crowd-scale silhouettes · §6 Horde Rendering · camera/framing spec

#### Group B — New World *(external art & material reference)*

- **Borrow:** **Material richness** (aged metal, stone, cloth, wood); grounded dark-fantasy **atmosphere**; **equipment / armour** design language; **environmental richness**; naturalistic **lighting** quality.
- **Reject:** **MMO scope**; open-world structure; exact designs; realism-fidelity as a **requirement**; the third-person camera.
- **Transform:** Realistic materials → **stylized-natural tuned to arena readability** (the saturation budget); dynamic lighting → a **fixed top-down key** + baked mood; equipment richness → **class-flexible gear legible from directly above**.
- **Governs:** Global identity · §1 world materials/atmosphere/lighting · §2 equipment · §7 UI material world · §9 environment-pack style

#### Group C — Current Point Clear greybox *(internal baseline — not a visual target)*

- **Borrow:** **Preserve exactly:** the already-proven gameplay readability; the **frozen enemy roles & identity colours** (grey / violet / orange / gold); the **semantic VFX meanings** (cyan hit / white death / red hurt); the **player-vs-enemy distinction**; the **telegraph requirements**; the **arena proportions** & gameplay behaviour.
- **Reject:** Its look, as a target: capsule primitives, flat grey ground, debug HUD, yellow VFX overload. **The greybox is a validated baseline, not a visual goal.**
- **Transform:** Dress every validated system in final dark-fantasy art **without changing what it communicates** — capsules → faction forms carrying the same hues; the proven reads become art-quality while every semantic meaning is preserved.
- **Governs:** All readability laws · §3 frozen roles/colours · §4 semantic VFX · §5 player-vs-enemy & telegraphs · §6 readability under load · §1 arena proportions

## Six laws before a single asset

The approved gameplay is frozen; art serves it, never the reverse. If a later choice fights one of these, the choice is wrong — not the law.

- **i. Readability is law** — Combat legibility outranks realism, always. A beautiful frame that hides a Charger's telegraph has failed.
- **ii. The saturation budget** — The *world* is desaturated dark-fantasy. The *gameplay layer* — enemies, VFX, pickups, the player locator — owns the saturation.
- **iii. Read from above** — Top-down means silhouette, footprint, and colour do the work — not facial detail. Every model is designed for its shadow.
- **iv. The semantic colour lock** — Cyan = your hit. White = death. Red = you are hurt. Gold = value. Warm = enemy telegraph. No effect reuses a meaning.
- **v. One language, many arenas** — The identity is global; the arena is a skin. Every rule scales to any bounded fantasy arena without redesign.
- **vi. The player is always found** — A permanent, **non-semantic** locator keeps the player readable in any horde. No enemy may ever reuse it (§2).

## 01 · Environment & Arena System

*A global environment language that any bounded fantasy arena inherits — then the coastal island as the first validation arena.*

### 1a · The global environment language (applies to every arena)

These rules are arena-independent. A castle courtyard, a flooded mine, a temple ruin, a graveyard, or a swamp all obey them — only the dressing changes.

| Rule | Applies to every arena |
| --- | --- |
| **Bounded & framed** | One crafted, bounded space ringed by natural or built barriers that read as walls from above without invisible-wall artifice. |
| **Mid-value play floor** | The combat floor stays mid-value, low-chroma, low-frequency — the canvas the saturated gameplay layer stands on. Never busy, never high-contrast. |
| **Detail at the perimeter** | Tall props, foliage, water, and hazards live at the edge, framing the fight. The play space stays clear. |
| **Fixed flattering key** | One directional key at a top-down-flattering angle; soft shadows for depth, never hard enough to read as objects. Baked where possible for mobile. |
| **Atmosphere via fog + post** | Thin distance/height fog desaturates and pushes the perimeter back; a shared post/grade sets each arena's mood without touching the readability of the floor. |
| **Dark-fantasy mood band** | Arenas range from bright-grim (coast at grim dusk) to deep-dark (mine, crypt) — but the saturation budget and mid-value floor hold at every mood. |

### 1b · The arena system — how the language scales

Each future arena is a **reskin of the same readability model**: swap the ground/perimeter/palette-grade/hazard, keep the rules. This table is the template.

| Arena | Ground & perimeter | Mood grade | Signature hazard-frame |
| --- | --- | --- | --- |
| **The Shattered Coast** Arena 01 · validation | Weathered stone paths, courtyards & broken gates; broad calm floors; cliffs + sea + dark vegetation ring | Grim coastal dusk | Low ruins, shoreline & drop-offs |
| **Castle / Keep** | Flagstone floor; walls, battlements, gates | Overcast stone-grey | Courtyard edges, portcullis |
| **Forest** | Loam/root floor; dense treeline ring | Deep green half-light | Thickets, fallen trunks |
| **Mine** | Rock/rail floor; hewn walls, timber | Dark, lantern-lit | Shafts, flooded lows |
| **Temple** | Tiled/eroded stone; colonnades, idols | Dim sacral gold | Broken floor, pits |
| **Graveyard** | Turf/gravel; iron fence, tombs, mist | Cold blue-grey night | Open graves, crypt mouths |
| **Swamp** | Boardwalk/mud islets; hanging canopy | Sickly green murk | Water channels, bog |

### 1c · Arena 01 — "The Shattered Coast" (approved · first Milestone 2 validation arena)

**The Shattered Coast** is a **ruined coastal fortress courtyard on a bounded temperate island** — where the language is **proven**, not where it is defined. It combines: **broad, calm combat floors**; **weathered stone paths and courtyards**; **broken gates and low ruins**; environmental detail concentrated at the **edges and landmarks**; **cliffs, sea, and dark vegetation** as natural boundaries; and **structured spaces and entrances** rather than an empty circular arena. It validates the whole pipeline against one arena before the system scales to the list above.

> **Separation of concerns**
>
> Nothing in Arena 01 (a specific palette, a specific prop) may become a global rule. The **rules live in §1a/1b; the coast is one instance.** If a decision only makes sense for the beach, it belongs to Arena 01, not the Bible.

## 02 · Player — one base, every class

*A single readable top-down base silhouette that expresses warrior, ranger, mage, battlemage, necromancer, and beyond. Direction only.*

The player is not a settler — that was too narrow. The direction is a **grounded dark-fantasy adventurer base** built to carry **any future class** through gear, dye, and skill families, while keeping **one constant silhouette read** from above. Class is expressed on top of the base; the base itself never stops reading as "the player."

**One base, many classes** — *warrior · ranger · mage · battlemage · necromancer · …*

- **Constant:** One rig, one proportion, one top-down footprint — always "the player."
- **Expressed by:** Gear silhouette accents, dye, weapon, and the skill-family VFX the class casts (§5) — not a different body.
- **Rule:** Class flavour may add to the silhouette; it may never dissolve the base read or the locator.

**Armour & material** — *New World equipment richness*

- **Style:** Layered leather / cloth / metal, dark-fantasy weathering; class-appropriate (plate-lean warrior, robe-lean mage) on the same base.
- **Budget:** Detail on top-facing surfaces (shoulders, head, back) — the only ones the camera sees.

**Colour & customization** — *a reserved dye band*

- **Base:** Desaturated naturals so the player never out-shouts an enemy.
- **Accent:** Customizable dye from a **reserved palette disjoint from every enemy identity hue**.
- **Future:** Modular gear sockets + dye slots on one shared rig — customization can never break silhouette or locator.

> **Law vi · the permanent player locator — approved stack (v1.0)**
>
> The player must be **instantly findable inside a dense horde** through a **unique, non-semantic locator** that **no enemy may ever reuse**, surviving every class, dye, gear change, and performance tier. The **approved stack** is: (1) a **subtle, permanent cool rim-light** keyed to the player only; (2) a **very thin ground ring** that becomes slightly more visible *only under high crowd density*; (3) a **physical facing cue** through asymmetric top-facing gear — shoulder shape, cape, hood, or weapon placement; and (4) **no permanent overhead icon**. The result must read as **grounded and diegetic** — never a mobile-game selection marker. The locator's colour sits **outside all semantic and enemy bands** so it can never be mistaken for a hit, a danger, or a creature.

> **Approved Milestone 2 player scope (v1.0)**
>
> Milestone 2 produces **one class-neutral, grounded dark-fantasy adventurer base — only.** The first art slice must prove: the **shared rig**, the **top-down silhouette**, the **permanent locator**, **modular equipment sockets**, **dye / accent customization**, and **core movement, aim, and attack animations**. **No finished classes are built in Milestone 2** — warrior, ranger, mage, battlemage, and necromancer remain future expressions of this same base.

## 03 · Enemy family — one faction, frozen roles

*The four enemies keep their exact mechanics and colours, and now belong to one coherent dark-fantasy creature family.*

The greybox colour-coding is a shipped readability system; it stays. What v0.2 adds is **in-world cohesion**: the four roles read as members of **one faction**, corrupted by the same source, sharing a material language and skeleton — so the world feels authored, not assorted. **No gameplay changes**; Walker, Surrounder, Charger, and Empowerer are mechanically frozen.

> **Approved first faction (v1.0) — "The Hollowed"**
>
> An island-**blight** faction: humanoids and beasts consumed by the same corruption. The four Milestone-2 members are **Hollowed Thrall** (Walker), **Hollowed Stalker** (Surrounder), **Hollowed Brute** (Charger), and **Hollowed Corruptor** (Empowerer). Each is a different **expression of one blight** — shared cracked-flesh / growth material with a sickly emissive in the role's identity hue, shared skeleton, shared death-dissolve; the corruption *is* why each glows its colour, grounding the semantic system in the fiction. **The names remain editable later without changing the frozen gameplay roles, visual laws, or production pipeline.**

**Walker · "Thrall" #8B9188** — *move & shoot — the baseline threat*

- **Fiction:** Shambling blighted masses — the corruption's foot soldiers.
- **Silhouette:** Simple, upright, aggressive; deliberately **quiet** so specials pop.
- **Colour:** Cool neutral grey-green blight — clearly hostile, clearly not a special.

**Surrounder · "Stalker" #9940D9** — *keep moving — don't get flanked*

- **Fiction:** Feral, corruption-quickened hunters that circle the prey.
- **Silhouette:** Lean, elongated, lithe — visibly lighter than the Thrall; a circling gait.
- **Colour:** Cool violet blight — the "comes from the side" hue.

**Charger · "Brute" #FF8C1A** — *read the telegraph — dodge the dash*

- **Fiction:** Overgrown, corruption-armoured juggernauts.
- **Silhouette:** Bulky, front-heavy; big footprint. Telegraph = a warm wind-up glow + clear dash lane.
- **Colour:** Warm orange→red on the charge — heat = impact = danger.

**Empowerer · "Corruptor" #FFCC1A** — *identify & kill the priority target*

- **Fiction:** A ritualist that spreads the blight-frenzy to nearby kin.
- **Silhouette:** Regal/ritual read; hangs at the backline. Visible gold **aura + tethers** to buffed allies.
- **Colour:** Gold — value, authority, priority.

> **The family rule**
>
> Warm = things that **hit you**; cool = things that **reposition around you**; neutral = baseline. A new enemy inherits this logic from its **lesson first**, then gets a model.

> **Frozen for Milestone 2 · extensible after (v0.2.1)**
>
> **Milestone 2:** Walker, Surrounder, Charger, and Empowerer stay mechanically frozen, and the first faction ("The Hollowed") **dresses those four roles**.
> **Beyond Milestone 2:** future factions are **not limited to cosmetic reskins**. They may reuse the proven roles where useful, but future **approved gameplay design** may also introduce **new behaviours, elites, ranged units, summoners, area-denial units, and bosses**. Every such addition **inherits the Bible's laws** — the silhouette discipline, the identity-colour logic, the motion-and-telegraph grammar, and readability-first — so a brand-new enemy still reads instantly, even though it is not one of the original four. This Bible defines the *visual language* those future roles will speak; it does not design or approve the gameplay, which remains the Game Director's call.

## 04 · Combat VFX — the semantic layer

*Small, stylized, restrained — every colour means exactly one thing. Direction only.*

- **Your hit landed** `#4FE6FF · cyan` — Contact spark + brief flash. Nothing else.
- **Death** `#FFFFFF + identity` — Quick dissolve/burst in white + the enemy's hue.
- **You are hurt** `#FF3B30 · red` — Player flash + screen-edge vignette. Self only.
- **Enemy telegraph** `warm orange→red` — Wind-ups, charge lanes, danger floors.
- **Value / reward** `#FFD524 · gold` — Currency, module, secured gains.
- **Player locator** `reserved · non-semantic` — Rim/ring/facing. No enemy may use it.
The core VFX events are unchanged from v0.1: player **tracer/muzzle/trail** in a reserved cool-cored player-fire hue (the QA found yellow collided with gold/warm); the shipped **cyan hit** + **white-plus-identity death dissolve**; the **red player-hurt** flash + vignette. Skill effects are now governed in full by §5.

> **The VFX constitution**
>
> No new effect borrows a semantic colour for a new meaning; no effect out-shouts an enemy telegraph; juice stays under the ceiling — **short, small, no shake or hitstop unless a playtest proves it's needed.**

## 05 · Skill Visual Language

*A grammar for every skill the game will ever ship — so a player instantly tells their own power from incoming death. Visual language only; no gameplay is designed here.*

#### The first divide · player skill vs enemy attack

The most important read in the game: **"is this mine or is this about to kill me?"** The answer is carried by **hue band + origin**, before shape.

|  | Player skills | Enemy attacks |
| --- | --- | --- |
| **Hue band** | Reserved cool/school hues (§ schools below) | Warm telegraph band (orange→red) + the enemy's identity hue |
| **Origin** | Emanate from the player / the player's aim | Emanate from the enemy / land as a floor decal |
| **Danger floor** | Player areas never use the red danger-decal language | All incoming ground danger uses one consistent warm decal that **fills as it arms** |

> **Ownership separation stack — enforced (v0.2.1)**
>
> Enemy danger **always** uses the standardized warm red/orange **arming ground-decal** — a flat floor zone that fills as it arms. Player fire magic may use fire colours, but it must **never resemble that decal system**. Ownership is guaranteed across **six channels at once — hue plus origin alone is not sufficient**, and it must hold inside a dense horde at every performance tier:

| Channel | Player skill — incl. Fire | Enemy danger — the arming decal |
| --- | --- | --- |
| **Core colour** | The reserved school core — a fire skill keeps a distinct heart (brighter / whiter / more golden), never the enemy's danger red. | The reserved danger red/orange. |
| **Outline / edge** | Crisp, bright rim; a defined, self-owned edge. | Soft filling edge; no rim. |
| **Motion** | Rises, emanates, kinetic — travels outward from the caster. | Fills / pulses flat on the ground as it arms. |
| **Origin** | The player / the player's aim. | The enemy / the floor beneath the threat. |
| **Footprint** | Volumetric and player-owned; **never** a flat arming floor-zone. | A flat floor-zone — a reserved footprint. |
| **Layering** | The player-effect layer, above the floor. | The dedicated danger-floor layer. |

> **Reserved-system rule**
>
> The **arming ground-decal footprint and its danger layer are reserved to enemy danger.** No player effect — whatever its hue — may adopt that footprint or that layer. So even a red player flame reads as *yours*, because it never becomes a filling floor-zone in the danger layer. Ownership readability is preserved by the full stack, not by hue and origin alone.

#### The skill grammar · five readable verbs

Every skill — present or future — is built from a fixed visual grammar, so a new skill is legible on sight:

- **Cast** — a readable origin beat at the player/hand (wind-up length = power).
- **Direction** — where it goes: *point*, *line/beam*, *cone*, *lobbed*, or *self/aura*. Each direction has one standard treatment reused across all skills.
- **Impact** — the contact beat, in the skill's school hue (never cyan — cyan is the basic weapon's "you hit").
- **Duration** — lingering fields have a **clearly bounded footprint** with a visible lifetime (edge fade as it expires); the player must always know exactly where a field is and when it ends.
- **Danger area** — anything that will hurt the player (enemy or a self-damage-by-design skill) uses the warm arming-decal; anything safe never does.

#### Visual families · magic schools

Future magic is organized into **schools**, each a reserved **hue + motion + material** family that no enemy band and no other school reuses. This is how a battlemage's fire never reads as a Brute's charge, and a necromancer's blight never reads as a Thrall.

| School (approved roster · v1.0) | Hue & motion signature (provisional — not frozen) | Material feel |
| --- | --- | --- |
| **Fracture / Force** — produced in the first slice | Cyan-white, sharp, angular, kinetic snaps | Crystalline shards / shockwave |
| **Fire** | Warm amber-to-white, rising, volatile — *separated from enemy danger by the full ownership stack; fire colours, never the arming-decal footprint/layer* | Ember / heat-haze |
| **Frost** | Pale blue, slow, crystallizing | Ice / brittle |
| **Storm** | Electric white-violet, staccato, arcing | Arc / spark |
| **Shadow / Necromancy** | Sickly green-violet, creeping, low — *kept distinct from Stalker/Corruptor identity by the ownership stack + player origin* | Rot / shadow / soul-wisp |
| **Holy** | Radiant gold-white, clean — *must be co-tested against the gold "value/reward" semantic and the Corruptor's gold; separation via the ownership stack, not hue* | Radiance / consecration |

> **School roster approved (v1.0) — hues deliberately not frozen**
>
> The six-school framework above is **approved as the visual-language roster**. **Only Fracture / Force is produced during the first art slice;** the other five are **language definitions only — they must not trigger gameplay or production scope.** Exact **hue values are NOT frozen**: each school's reserved band is locked only after the schools are **tested together in a representative gameplay scene**, against enemies, rewards, telegraphs, and the environment — so real risks (Fire vs enemy-warm, Holy vs the gold-reward semantic, Shadow vs the blight) are proven separable *in situ*. Separation is always enforced by the **full ownership stack** — core colour, outline, motion, origin, footprint, layering — never hue alone.

#### Skill-evolution rules

As a skill ranks up, its visual **grows, never changes species**: the silhouette of the effect stays recognizable (a Fracture Bolt at max rank is still obviously a Fracture Bolt) while intensity, count, and reach scale. Evolution reads as *"more of the same,"* and **headroom is reserved** so a max-rank cast never blows the coverage budget below.

#### VFX density & screen-coverage budget

| Budget | Rule |
| --- | --- |
| **Single-effect cap** | No single skill effect may obscure more than a defined share of the screen or the play space around the player. The player's own feet stay visible. |
| **Aggregate cap** | Total simultaneous player-VFX coverage is capped; when many effects stack, secondary decoration culls first so the **combat floor never disappears**. |
| **Enemy-telegraph priority** | Player VFX may never render over an active enemy telegraph in a way that hides it — telegraphs draw on top / punch through. |

#### Horde-safe & mobile degradation

Under load or on mobile, skills degrade **from the outside in**: particle counts drop, then secondary flourishes, then trails — but the **core readable silhouette and the semantic read survive last**. A degraded Detonation Field is smaller and simpler; it is never invisible or mistakable for something else. (Tiers defined in §6.)

#### The first family, defined · Fracture / Force

Today's three skills **establish the first school** and seed the grammar every later skill reuses:

| Skill | Grammar verb | Establishes |
| --- | --- | --- |
| **Fracture Bolt** | Directed *projectile* + shard impact | The school's cyan-white crystalline hue + the "projectile" template all future bolts inherit. |
| **Detonation Field** | Lobbed *mark* + bounded *AoE* duration | The "bounded field with a visible lifetime" template all future zones inherit. |
| **Volatile Fracture** | *Synergy* amplifier (passive) | How a passive **reads on the effects it modifies** (an added shard/charge layer) rather than as its own effect — the synergy template. |

## 06 · Horde Rendering & Visual Scalability

*On a top-down arena with a crowd on screen, performance *is* readability. This is the budget that keeps both alive on PC and mobile.*

| System | Policy |
| --- | --- |
| **Shared materials** | One enemy master material, tinted per-instance (as greybox already does) so the crowd batches. Identity colour rides a property block, not a new material. |
| **Skeleton reuse** | One shared enemy skeleton across the faction → shared/instanced animation and one animation pipeline for the whole family. |
| **Animation LOD** | Distant / off-screen / low-priority enemies update animation at reduced rate; full rate near the player and for specials. |
| **Shadow policy** | Player + special enemies cast the readable shadows; trash uses a cheap blob or drops shadow under load. Shadows never accumulate into visual noise. |
| **VFX reduction under load** | Hit/death VFX simplify as density rises — the **semantic read (cyan hit, white death) survives; decoration is culled** first. |
| **Corpse cleanup** | The death beat dissolves and despawns quickly — no persistent corpses to accumulate cost or clutter the floor. |
| **GPU instancing** | Instance the trash mesh/material (Thralls) where the platform supports it; the crowd is the cheapest thing on screen. |
| **Special-enemy priority** | Specials (Brute, Corruptor, Stalker) **always keep full fidelity, shadow, and telegraph VFX**; only trash degrades — so the priority target and the telegraph never lose legibility. |
| **PC / mobile visual tiers** | Two tiers share **one identical readability layer** (silhouette, identity colour, telegraph, locator, semantic VFX); only decoration scales — mobile caps particles, shadow count, and crowd fidelity. The game never becomes *less legible* on mobile, only less decorated. |

> **The scalability law**
>
> Degradation is always **decoration-first, readability-last**. On the weakest supported device at peak horde, a player must still instantly read: where they are, which enemy is which, what's about to hit them, and whether their shot landed.

> **Approved performance targets (v1.0) — outcomes, not numbers**
>
> Frozen **outcome targets**: **PC — stable 60 FPS at 1080p** on the eventual minimum-spec target; **Mobile — stable 30 FPS** on the eventual minimum supported device; and **gameplay readability identical across tiers while only decoration scales**. The Bible deliberately **does not invent** final polygon, texture, draw-call, particle, or crowd-count limits. Those concrete budgets are **measured in a Phase-0 representative benchmark scene** — final camera, Arena 01 materials & lighting, the player, the Thrall crowd, representative VFX, profiled on PC and mobile — then recorded in a **separate Technical Art Budget document referenced from this Bible**, not hard-coded here.

## 07 · UI style — the expedition journal

*One material world across every screen: aged parchment, weathered bronze, engraved type.*

| Surface | Direction |
| --- | --- |
| **Main Menu** | Atmospheric dark-fantasy vista + the emblem/wordmark; the visual and melodic identity in one frame. |
| **Character Creation** | The journal's "who are you" page — name + preset/dye + (future) class, previewed live on the base character. |
| **Skill Allocation** | An engraved constellation of the skill tree; unmistakable "spend a point" + locked/unlocked read; school hues (§5) preview the family. |
| **HUD** | Minimal, peripheral: HP, cooldowns, objective (kills/quota), extraction state, currency & module. Semantic colours only — always meaning something. |
| **Results screen** | The "expedition log": secured vs lost, level & points, module — a journal page that drives the replay pull. |
| **Fonts** | An inscriptional display serif (codex) + a clean humanist sans (HUD/body). Two families, no more. |
| **Icons** | One cohesive engraved-line set — single weight, single style, on a grid, legible tiny. |
| **Colours** | Neutrals = parchment + stone + bronze; accents borrow semantic gameplay colours only for state. |

## 08 · Audio direction — sound as telegraph

*A live-sounding, mature dark-fantasy score where the most important cues are also the most legible.*

| Layer | Direction |
| --- | --- |
| **Music style** | Orchestral-hybrid, New World-adjacent, dark-fantasy — live strings/woodwinds, restrained percussion; adaptive explore → combat → peak. |
| **Ambient** | Per-arena bed (surf, wind, dripping mine, crypt hush). Desaturated like the visuals — foundation, not distraction. |
| **Combat music** | Layered intensity tracking the Run's pressure → lull → build; a reserved peak/boss layer. |
| **Menu music** | Quiet, atmospheric theme — the melodic signature. |
| **Skill sounds** | Per-school signatures; cut through a horde without masking enemy cues. |
| **Enemy sounds** | Audio **reinforces role and telegraph**: Brute wind-up growl (audio telegraph), Stalker light/quick, Corruptor a locatable "buff hum", Thrall generic. Sound is a readability tool. |
| **Player sounds** | Weapon punchy/consistent; surface-aware footsteps; a clear "you're hit" cue paired with the red vignette. |
| **UI sounds** | Cohesive parchment/metal foley, one set across every screen. |

## 09 · Asset strategy — a solo pipeline

*Built for one developer working with Claude, AI tools (Meshy), commercial packs, and selective outsourcing — not a permanent internal art team.*

> **The acceptance gate — replaces "AI concepts, never ships"**
>
> Any asset — **purchased, AI/Meshy-generated, or hand-made** — may ship only if it passes the same gate: **visual cohesion · technical (topology, UVs, scale) · performance (PC + mobile budgets) · licensing (clear, redistributable rights) · rigging (shared-rig compatible) · material (fits the shader library + palette) · readability (serves the semantic system)**. **Origin alone neither approves nor rejects** — an AI mesh that passes ships; a purchased mesh that fails does not.

> **Solo doctrine — the order of preference**
>
> 1 · one cohesive **purchased** environment base. 2 · a **purchased or generated modular character base** where viable. 3 · **shared rigs + retargeted animation** everywhere. 4 · **controlled AI/Meshy variants after cleanup** for volume (enemy variants, props, icon drafts). 5 · **fully custom / outsourced** only where gameplay identity truly requires it. Claude assists throughout (shaders, tooling, review, docs).

> **Purchasing policy — frozen (v1.0); specific purchases pending research**
>
> No specific assets are selected or purchased at freeze. The frozen **policy** is: **one cohesive environment-pack family** wherever possible; **one modular character-base solution**; **one compatible animation-library source**; **one licensed display serif and one licensed humanist sans**. Every purchase requires a **researched shortlist → license verification → technical-compatibility check → Game-Director approval** before acquisition.

| Category | Primary path (solo) | Why |
| --- | --- | --- |
| **Environment** | One cohesive pack AI/kitbash landmarks | Largest surface, lowest gameplay stakes — buy consistency + speed + mobile budgets. Generated/kitbashed unique landmarks pass through the gate. |
| **Player** | Modular base gen variants selective outsource | A purchased/generated modular character base gives a riggable body fast; class gear via modular sockets. Outsource only a hero-polish pass if the gate demands it. Shared rig. |
| **Enemies** | Meshy variants + cleanup shared rig | Generate role-silhouette variants of one faction base, clean up, gate, and bind to the **shared skeleton**. Custom/outsource only a hero silhouette that generation can't hit. |
| **Props** | Pack gen gameplay props | Set-dressing from the pack; gameplay props (currency, module, extraction) generated or made, then gated for instant readability. |
| **UI & Icons** | AI drafts unified pass | AI-drafted icons/frames, then a single style-unification pass + component kit so it reads bespoke. Claude-assisted layout. |
| **VFX** | Custom (VFX Graph) | Solo-doable and Claude-assisted; must stay studio-controlled for the semantic lock, budgets, and juice ceiling. No bought VFX packs. |
| **Materials & Shaders** | Custom URP + Claude | Small shared Shader Graph library (lit / tint / telegraph / dissolve / ground / water); Claude assists authoring. The look and dual-platform perf depend on owning these. |
| **Animations** | Library / mocap signature only | Retarget library/mocap locomotion to shared rigs; hand-key or outsource only the gameplay-critical telegraphs (Brute wind-up). |
| **Audio** | Libraries + tools composer/outsource score | Curated SFX libraries + AI music tools (gated on licensing) for scratch; a composer or outsourced pass for the signature adaptive score. Audio telegraphs deliberately designed. |

## 10 · Asset list — the production checklist

*Everything the first-arena vertical slice needs before Milestone 2 art is called complete. Grouped by discipline.*

#### Foundation lock first

- Master palette + semantic-colour lock
- Reserved player-dye band (disjoint)
- Reserved player-skill spectrum (schools)
- Player-locator spec (law vi)
- Style-target "one screen" mood frame
- Top-down camera + zoom spec
- PC + mobile budgets & visual tiers
- Asset acceptance-gate checklist

#### Environment buy + integrate

- Global environment-language rules doc
- Arena-system template (reskin recipe)
- Arena 01 — coastal base pack
- Ground material set (mid-value rule)
- Perimeter cliffs / treeline / sea
- Lighting + fog + skybox + post rig
- Arena 01 greybox at final proportions

#### Player modular base

- Modular base mesh + shared rig
- Top-down silhouette + facing pass
- Permanent locator (rim/ring/gear)
- Core locomotion + aim/shoot anims
- Dye material + modular sockets
- Class-agnostic base + 2 presets

#### Enemies faction, shared rig

- Shared faction skeleton + tint material
- Thrall (Walker) — pipeline template
- Stalker (Surrounder) — lean flanker
- Brute (Charger) — heavy + telegraph pose
- Corruptor (Empowerer) — aura + tethers
- Per-enemy telegraph animations

#### Props mixed

- Currency pickup (gold, instant-read)
- Weapon module pickup
- Extraction point + open/closed states
- Boundary landmarks / set-dressing

#### VFX & Skills custom

- Player tracer + muzzle + trail
- Cyan hit spark · death dissolve
- Brute telegraph glow + dash lane
- Corruptor aura field + tethers
- Fracture school: Bolt / Field / Volatile
- Danger-decal (arming) template
- Coverage-budget + degradation LODs

#### UI & Icons AI + unify

- Component kit (frames/buttons/panels)
- Main Menu · Character Creation
- Skill Allocation chart
- In-arena HUD (peripheral)
- Results / expedition log
- Icon set — skills, currency, module, stats

#### Fonts license

- Display serif (inscriptional)
- Humanist sans (HUD/body)
- In-engine SDF atlases

#### Materials & Shaders custom

- Stylized lit master (mobile-safe)
- Enemy tint / identity shader
- Telegraph emissive · death dissolve
- Ground blend · water
- Player-locator rim/ring shader

#### Animation & Audio library + signature

- Retargeted locomotion (shared rigs)
- Signature telegraph anims
- Menu theme + arena ambient bed
- Adaptive combat layers (+ peak)
- Weapon/hit/hurt/death + enemy telegraphs
- Fracture-school SFX · UI foley

## 11 · Production roadmap — order to minimize rework

*Lock the language, prove one full arena slice, then scale. Nothing final is built on an unlocked foundation.*

### Foundation — lock the language *(Do first · begins only on explicit GD go)*

Build the master palette + semantic lock + **reserved Fracture/Force** band + **player-locator spec** (the approved rim + density-ring + physical-facing stack), the shared shader library, the lighting/fog/post rig, the acceptance gate, and the **Shattered Coast (Arena 01)** greybox at **final proportions**.

Then run the **representative benchmark scene** — final camera, Arena 01 materials & lighting, the player, the Thrall crowd, representative VFX, profiled on **PC and mobile** — and record the measured poly/texture/draw/particle/crowd budgets in a **separate Technical Art Budget document** the Bible references. Kick off **purchasing research** (shortlists + licence + compatibility) for GD sign-off — research, not buying.

*Why first: palette, shaders, camera, proportions, the locator, and the measured budgets are what force total rework if changed late.*

### Arena 01 base pass *(After foundation)*

Integrate the cohesive nature pack into the coastal arena; tune ground to the readability rule. Get the world **correctly desaturated** before any saturated art sits on it.

*Depends on: shaders + palette. Blocks: enemy/VFX colour tuning.*

### One enemy end-to-end — the Thrall *(The pipeline template)*

Take the Thrall fully through generate → cleanup → gate → shared rig → anim → tint shader → hit/death VFX → audio, proving the solo enemy pipeline and its readability against the finished arena. The other three reuse it.

*Depends on: arena + shaders + shared rig. Blocks: the rest of the faction.*

### Faction + Player *(Reuse the proven pipeline)*

Stalker, Brute, Corruptor on the shared rig. Then the Player modular base + rig + core anims + locator + dye — built **after** enemy colours are final so the reserved player band locks against them.

*Depends on: Thrall template + final enemy hues. Prevents: player/enemy colour collision.*

### VFX + Skill family *(Tuned in context)*

Player fire, hit, death, telegraphs, aura/tethers, and the Fracture school (Bolt/Field/Volatile) with the grammar, danger-decals, coverage budget, and degradation LODs — all judged against the finished arena, enemies, and horde.

*Depends on: arena + enemies + player + horde tiers existing.*

### UI · icons · fonts *(Semi-parallel from Phase 0)*

Screen-space work depends only on the locked palette + type, so it runs alongside the 3D track.

*Depends on: palette + type lock. Low 3D dependency.*

### Audio *(Against near-final visuals)*

Score, SFX, and enemy/skill audio telegraphs, mixed so telegraph timing matches the visual tells.

*Depends on: telegraph anims + VFX timing settled.*

### Deliberately waits — after the slice is approved *(Do not build yet)*

Additional **arenas** (castle/forest/mine/temple/graveyard/swamp), additional **factions**, boss visuals, further **magic schools** & classes, deep customization. The Bible defines the language for all of them so they conform later without redesign.

> **Rework-minimization doctrine**
>
> (1) Lock palette + shaders + camera + proportions + locator before any final art. (2) Prove **one full arena vertical slice** — Arena 01, one faction member fully, the player, core VFX/skills/UI/audio — and approve it before scaling to the arena/faction/school lists. (3) Keep the greybox playable throughout; swap art in per-category so gameplay never breaks.

## 12 · Risks — and how each is avoided

*The threats that actually apply to a solo, AI-assisted, dark-fantasy arena game.*

#### Readability risks

- **High · Naturalism / darkness camouflages enemies**
  - Dark-fantasy mood + New World materials can swallow a Thrall or hide a telegraph — worse in dark arenas (mine, crypt).
  - **Avoid:** the saturation budget + mid-value floor at every mood; foliage/props off the play space; enemies own the saturation; dark arenas lift enemy emissive, never the floor.
- **High · Skill VFX overload masks telegraphs**
  - Many schools + evolving skills + a horde = the exact overload that nearly broke greybox.
  - **Avoid:** the semantic lock + reserved school spectrum + the §5 coverage budget + telegraph-priority + degradation-from-outside-in.
- **High · Losing the player in the crowd**
  - Dense hordes + a class-flexible body risk the player vanishing.
  - **Avoid:** law vi — a permanent, non-semantic, enemy-forbidden locator that survives every class, tier, and horde.

#### Visual risks

- **High · Incohesion from mixed sources / AI variance**
  - Packs + AI + outsourced parts can read as a bundle, and AI output drifts in style.
  - **Avoid:** the acceptance gate's **cohesion criterion**; one palette + shader library + post rig unifying everything; one cohesive environment base; a single UI/icon unification pass.
- **Med · Realism / darkness fights the arena**
  - Chasing New World fidelity or moody darkness can erode legibility.
  - **Avoid:** the saturation-budget reconciliation; readability is law when they conflict; fidelity is a reference, not a requirement (§ identity).

#### Production risks

- **Med · Solo pipeline stalls · AI cleanup debt · mobile perf**
  - Bespoke where a pack would do; shipping ungated AI meshes; ignoring mobile budgets.
  - **Avoid:** the solo doctrine (buy the base, generate + gate variants, shared rigs); the acceptance gate catches AI debt; budgets + tiers set in Phase 0; prove **one enemy end-to-end** before scaling.

#### Scope risks

- **High · Replacing the whole greybox before the look is proven**
  - Momentum pushes toward "art everything now"; the goal is a foundation.
  - **Avoid:** vertical-slice-first — one arena, one faction member fully, player, core VFX/skills/UI/audio — approved before scaling. Greybox stays playable.
- **Med · MMO / open-world creep from the reference**
  - New World is an MMO; borrowing its look can drag in its scope.
  - **Avoid:** the § identity clarification — New World is materials/atmosphere/equipment/lighting only, never scope/structure/fidelity. Hold the bounded-arena scope (DEC-034).

## 13 · Freeze summary, decisions ledger & changelog

*The v1.0 changes, the full version history, the all-resolved decisions ledger, the freeze summary, and what production may begin on your go.*

### What changed in v1.0 — official-freeze candidate

All seven Game-Director decisions were applied and baked into their sections; the open-decisions list became the approved ledger below. Specifically: the **Hollowed** faction & four member names (§3); the approved **locator stack** (Law vi / §2); the six-school **roster** with only Fracture produced and hues deferred (§5); **The Shattered Coast** as Arena 01 (§1); the **one class-neutral base** M2 scope (§2); the frozen **purchasing policy** (§9); and the **performance targets** with numeric budgets deferred to a Phase-0 benchmark + separate budget doc (§6, §11). The three-group Reference Matrix, all readability laws, and the frozen gameplay are unchanged.

### What changed in v0.2.2 — final-freeze candidate

| Change | Detail |
| --- | --- |
| **Three-group Reference Matrix** | Rebuilt the matrix around **three** references: **A — *Walkers Attack*** (the now-attached frames) as the external **gameplay/readability** reference (camera, crowd scale, structured maps, player-in-horde, calm floor, soft light); **B — New World** as the external **art/material** reference (unchanged); **C — the greybox** as the internal **validated baseline to preserve** (roles, colours, semantic VFX, telegraphs, proportions), explicitly *not a visual target*. Each group lists borrow / reject / transform / **governs**. |
| **Open decision #8 closed** | The "screenshots missing" note is removed and #8 is marked RESOLVED — the frames are incorporated as Group A. |

### What changed in v0.2.1 — final-freeze candidate

| # | Correction | Change made |
| --- | --- | --- |
| **1** | Strengthen player-fire vs enemy-danger separation | Added the **Ownership Separation Stack** (§5): enemy danger always uses the standardized warm **arming ground-decal**; player pyromancy may use fire colours but is separated across **six channels** — core colour, outline, motion, origin, footprint, layering — with the arming-decal footprint + danger layer **reserved to enemies**. Hue + origin alone is explicitly declared insufficient; must hold in a dense horde. |
| **2** | Future factions are not cosmetic-only | Rewrote the §3 faction rule: the four roles stay frozen for Milestone 2 and the first faction dresses them, but future **approved gameplay design** may add new behaviours, elites, ranged, summoners, area-denial, and bosses — each **inheriting the Bible's silhouette / colour / motion / telegraph / readability laws** rather than being a reskin. |
| **3** | Add the Visual Reference Matrix | Added a **borrow · reject · transform** matrix for **Group A** (current Point Clear greybox build) and **Group B** (New World), in the identity section. Flagged that screenshots were not attached and Group A is grounded in the observed build. |

### What changed from v0.1 → v0.2

| # | Correction | Change made |
| --- | --- | --- |
| **1** | Separate global identity from the first arena | Added the global identity statement + New World clarification up top; rebuilt §1 into a global environment language (1a), an arena-scaling system with a 7-arena template (1b), and Arena 01 coastal as the first *validation* arena only (1c). |
| **2** | Strengthen dark-fantasy identity | Reframed the whole identity as **dark fantasy**; §2 player is now a class-flexible base (warrior/ranger/mage/battlemage/necromancer…) on one silhouette; §3 enemies gain one coherent faction ("The Hollowed" blight, proposed) with roles frozen. |
| **3** | Add Skill Visual Language | New §5: player-vs-enemy read, the five-verb skill grammar, magic-school families, evolution rules, density/coverage budgets, horde/mobile degradation, and the Fracture school defined from today's three skills. |
| **4** | Quality-gate AI, don't ban it | Replaced "AI concepts, never ships" with the **acceptance gate** (§9) — AI/Meshy ships if it passes the same criteria as purchased/custom; origin alone neither approves nor rejects. |
| **5** | Realistic solo strategy | Rebuilt §9 around a solo dev + Claude + AI/Meshy + packs + selective outsourcing; a 5-step solo doctrine; no internal Blender team assumed; every category's primary path re-tagged. |
| **6** | Permanent player locator | Added as **Law vi** and detailed in §2: a non-semantic locator (rim + ground ring + head/shoulder + facing) no enemy may reuse. |
| **7** | Horde rendering & scalability | New §6: shared materials, skeleton reuse, animation LOD, shadow policy, VFX reduction, corpse cleanup, GPU instancing, special-enemy priority, PC/mobile tiers. |
| **8** | Clarify the New World reference | Explicit callout up top + a scope-creep risk: materials/atmosphere/equipment/lighting only — not MMO scope, open-world structure, or required fidelity. |

**Retained from v0.1 unchanged:** the readability laws, the saturation budget, the semantic-colour system, the top-down design rules, the asset-production order, and the vertical-slice-first roadmap — per your approval.

### Decisions ledger — v1.0 (all resolved)

| # | Decision | Status | Resolution |
| --- | --- | --- | --- |
| **1** | First enemy faction | Approved | **The Hollowed** — Thrall / Stalker / Brute / Corruptor. Names editable later; roles, laws, pipeline frozen. |
| **2** | Permanent player locator | Approved | Cool rim-light + a very thin ground ring (stronger only under high density) + a physical facing cue; **no overhead icon**; grounded & diegetic. |
| **3** | Magic-school roster | Approved | Fracture/Force · Fire · Frost · Storm · Shadow/Necromancy · Holy. Only Fracture produced now; others language-only; **hues co-tested later, not frozen**. |
| **4** | First arena | Approved | **The Shattered Coast** — ruined coastal fortress courtyard; validation arena, not the game's whole identity. |
| **5** | Milestone 2 player scope | Approved | **One class-neutral base only**; proves rig, silhouette, locator, sockets, dye, core anims. No finished classes in M2. |
| **6** | Fonts, packs, purchases | Policy-approved | Purchasing **policy** frozen; specific selections/purchases **pending research** (shortlist → licence → compatibility → GD approval). |
| **7** | Performance & budgets | Target-approved | Targets frozen (PC 60 FPS @ 1080p min-spec · Mobile 30 FPS min device · readability constant, decoration scales). **Numeric budgets pending Phase-0 benchmark** → separate Technical Art Budget doc. |
| **8** | Reference-matrix frames | Resolved | The *Walkers Attack* frames are Reference Group A; three-group matrix in place. |

### Final freeze summary

> **Point Clear Art Bible v1.0 — official-freeze candidate**
>
> The **global dark-fantasy identity**, the **six readability laws**, the **saturation budget**, the **semantic-colour lock**, the **three-group Reference Matrix**, the **Hollowed** faction and its four members, the **player locator**, the **six-school roster** (Fracture produced first), **The Shattered Coast** as Arena 01, the **one class-neutral player base**, the **purchasing policy**, and the **60/30 FPS performance targets** are all set. The *only* deferred items are **concrete budgets** (Phase-0 benchmark), **specific purchases** (research + GD approval), and the **future schools' exact hues** (co-test) — each gated, not open-ended. Frozen gameplay, pillars, and enemy roles are unchanged; the art serves them.

### What production may begin — only after explicit Game-Director approval

On freeze *and* an explicit "go", Roadmap Phase 0 may start. Nothing below begins until then; still no Unity changes, assets, or purchases.

#### Phase 0 — foundation (gated on GD go)

- Lock the master palette + semantic-colour lock + the reserved Fracture/Force band.
- Author the **player-locator spec** (rim + density-ring + physical facing) and the acceptance-gate checklist.
- Build the shared URP shader library (lit / tint / telegraph / dissolve / ground / water / locator).
- Build the lighting / fog / post rig and the final top-down camera framing.
- Build **The Shattered Coast** greybox at final proportions.
- Run the **representative benchmark scene** (camera + Arena 01 mats/light + player + Thrall crowd + VFX, PC & mobile) → record concrete budgets in the separate **Technical Art Budget** document.
- Begin **purchasing research** — shortlists, licence verification, compatibility checks — for GD sign-off (research, not buying).

#### Then, on the vertical-slice-first doctrine (§11)

- Phase 1 — Arena 01 base pass (desaturated world first).
- Phase 2 — the **Hollowed Thrall** end-to-end as the pipeline template.
- Phase 3 — the rest of the faction + the class-neutral player base.
- Phase 4 — combat VFX + the Fracture/Force skill family.
- Phase 5 — UI · icons · fonts (semi-parallel).
- Phase 6 — audio, against near-final visuals.
- Approve the single-arena slice before scaling to more arenas, factions, schools, or classes.

---

***Point Clear — Visual Foundation / Art Bible v1.0 · official-freeze candidate.** Documentation-only. No project files, Unity scenes, or assets were created or modified; nothing was purchased and no production (including Phase 0) has begun. All seven Game-Director decisions are applied. On your explicit freeze + go this becomes the official visual direction and Phase 0 may begin — deferred budgets, purchases, and future-school hues are gated, not open. On freeze this becomes the official visual direction and production begins at Roadmap Phase 0. Frozen gameplay, pillars, and enemy identities are unchanged — the art serves them.*
