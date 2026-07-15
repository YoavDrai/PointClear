# Point Clear — Visual Benchmark Plan

> **Status:** DRAFT — planning milestone only. Pending Game-Director review.
>
> **Type:** Production blueprint (non-canonical until approved). Derived from and governed by **[Art Bible v1.0 — frozen](ART_BIBLE.md)** (§1, §2, §3, §4, §6, §11, and GD decisions 4 & 7).
>
> **This is a plan, not an execution.** Nothing in this document builds anything yet. **No Unity changes, no asset purchases, no AI generation, and no Phase-0 production have begun or are authorized by this file.** Phase 0 begins only on an explicit Game-Director "go" (Art Bible §11 / §13).
>
> **On approval, this document becomes the blueprint for the first visual implementation of Point Clear** — the single scene that either proves or disproves the whole visual direction, *and validates the one provisional stack already chosen for it*, before any broader production investment or stack-wide purchasing commitment. **The benchmark does not select between Stack A and Stack B** — that provisional selection happens upstream (see the Entry Gate).
>
> **Placement note:** filed in `Documentation/Vision/` as the operational companion to the Art Bible. Move to repo root or a future `Documentation/Production/` on request.

---

## The one question this benchmark exists to answer

> **"If we build this single scene successfully, are we confident that the entire game's visual direction is correct?"**

Everything below is scoped to answer exactly that — and nothing more. If an element does not help answer it, the element is Optional or excluded. The benchmark validates the **visual language**, not gameplay. Combat exists here only as the minimum needed to make the semantic layer (hits, deaths, hurt, telegraphs) observable.

This is deliberately the **smallest** scene that still exercises every load-bearing assumption in the Art Bible at once: one arena, one enemy type, one player, a real horde, the semantic VFX, on both target platforms.

---

## Entry Gate — prerequisites before any benchmark execution

This benchmark sits at **step 4 of a 5-step sequence.** It does **not** choose the production stack; it **validates a single provisional stack that has already been selected and purchased in limited scope.** The full sequence — do not reorder:

1. **Select** one provisional stack for the paid benchmark, using the live asset shortlist ([`Documentation/Research/Assets/POINT_CLEAR_LIVE_ASSET_SHORTLIST.md`](../Research/Assets/POINT_CLEAR_LIVE_ASSET_SHORTLIST.md); current recommendation there: **Stack B — grounded semi-real**, *pending explicit GD approval*).
2. **Verify** every outstanding technical, licensing, pipeline, and compatibility (`not confirmed`) field for that provisional package.
3. **Approve** the exact, limited benchmark shopping list, with explicit GD purchase authorization.
4. **Build** the benchmark scene per this plan. ← *this document.*
5. **Validate, reject, or revise** the provisional stack from the results — *before* any broader purchasing or production commitment.

**Benchmark execution (the Phase-0 build) may begin only after ALL of the following are true:**
- [ ] **Provisional stack approved** by the Game Director — *one* stack, not a comparison of both.
- [ ] **Live verification complete** for every remaining `not confirmed` field in the chosen package (render pipeline / Unity 6 / URP / mobile / rig / LODs / licences / current prices — per the shortlist's pre-purchase checklist).
- [ ] **Exact benchmark shopping list approved** — the specific, limited asset set only; nothing broader.
- [ ] **Explicit purchase authorization** granted by the Game Director.
- [ ] **Target test devices confirmed**, or clearly documented **temporary proxies** recorded (§10 — the devices remain unresolved placeholders until then; this plan does not invent them).

Until every box is checked, this remains a plan: no purchases, no Unity, no Phase 0.

> **One stack, not two.** The benchmark buys and builds **one** provisional stack. It never assumes both stacks are purchased or implemented for a side-by-side comparison. If the benchmark fails, the outcome is a Game-Director identity/stack conversation (live shortlist §10 fallback) that *revises or rejects the provisional stack* — never an automatic second full build.

---

## 1 · Benchmark objective

### What we are trying to prove
That the frozen Art Bible identity — *"top-down stylized-natural dark fantasy, combining Walkers-Attack-level combat readability with New-World-inspired materials, atmosphere, equipment, and environmental richness"* — can be realized as an actual rendered scene, **using the one provisional stack chosen for the benchmark** (selected upstream per the live shortlist — see the Entry Gate), that stays **readable under a dense horde on both PC and mobile**, without compromising the frozen gameplay reads.

Concretely, the benchmark proves three things about the **already-selected provisional stack**:
- whether it can **achieve the frozen Art Bible identity** in a real scene;
- whether it can **meet the readability and performance requirements** at real crowd density on both platforms;
- whether it **deserves broader production investment** — or must be revised or rejected.

The benchmark is a **go/no-go gate for the entire visual pipeline** and for the provisional stack. It does **not** pick the stack. A pass means the language *and* the provisional stack are proven, and production may scale to more arenas/enemies/skills on the same rules. A fail means we **revise or reject the provisional stack — and, if the fault is the language itself, redesign the language** — *before* any broader purchasing or production commitment (see §13). Catching that here, in one scene with one limited purchase, is the whole point of doing this first.

### Art Bible assumptions under test
Each is a specific, falsifiable claim from frozen v1.0:

| # | Art Bible assumption | Source | How this benchmark tests it |
|---|---|---|---|
| A1 | A desaturated **mid-value combat floor** can host a saturated gameplay layer without either fighting the other | Law i–ii, §1a | Build the floor to the rule; observe whether enemies/VFX pop and the floor recedes |
| A2 | **Read-from-above** silhouettes + footprints + identity colour carry recognition (not facial detail) | Law iii, §1a | Judge Thrall + player recognition purely from the top-down camera |
| A3 | The **semantic colour lock** (cyan/white/red/gold/warm) stays unambiguous in motion, in a crowd | Law iv, §4 | Trigger all five meanings simultaneously in a horde and check for collisions |
| A4 | The **permanent, non-semantic player locator** keeps the player findable inside a dense horde | Law vi, §2 | Bury the player in the stress crowd and check instant findability |
| A5 | **Naturalism/dark-fantasy mood does not camouflage** enemies or hide danger | §1a, §12 Risk | Grade to grim dusk; confirm a grey Thrall never disappears into stone |
| A6 | The **horde-rendering model** (shared material + shared skeleton + tint + LOD) holds the frame targets | §6 | Profile at target and stress crowds on PC + mobile |
| A7 | **Degradation is decoration-first, readability-last** — mobile is less decorated, never less legible | §6 scalability law | Compare PC vs mobile tier; confirm the readable layer is identical |
| A8 | **60 FPS @1080p (PC min-spec) / 30 FPS (mobile min device)** are achievable at real crowd density | GD decision 7, §6 | Measure; this run also produces the first concrete budgets |
| A9 | The identity reads as **grounded dark fantasy**, not cartoon and not photoreal-MMO | Global identity, §12 | GD subjective judgment against the reference matrix |

If A1–A9 all hold in one scene, the visual direction is proven. If any load-bearing one fails, we stop (see §13).

---

## 2 · Scene composition

**The Shattered Coast (Arena 01)** — a *ruined coastal fortress courtyard on a bounded temperate island*, built at **final gameplay proportions** (Art Bible §1c, §11 Foundation). Not a dressing exercise: the layout is the real arena the game will ship, greyboxed-then-dressed, so the readability test is honest.

The scene is one bounded, framed space. Detail concentrates at the **perimeter and landmarks**; the **play space stays clear** (§1a "detail at the perimeter").

| Element | Description | Art Bible rule it serves |
|---|---|---|
| **Combat floor** | Broad, calm, mid-value, low-chroma, low-frequency weathered-stone courtyard + paths. The canvas the saturated layer stands on. Never busy, never high-contrast. This is the single most important surface to get right. | Law ii, §1a "mid-value play floor" |
| **Ruined fortress** | Broken gates, low ruined walls, courtyard structure, clear entrances — *structured space, not an empty circle.* Walls read as boundaries from directly above without invisible-wall artifice. | §1c, §1a "bounded & framed" |
| **Cliffs** | Natural vertical barrier ringing part of the arena; frames the fight and reads as a wall from above. | §1c, §1a perimeter |
| **Sea** | Bounded-island water beyond the cliffs/shoreline; a readable sea plane with a shared water material. Atmosphere and boundary, not a play surface. | §1c |
| **Vegetation** | Dark coastal vegetation at the edges (treeline/scrub) as a natural boundary and frame. Off the play space. | §1c, §1a |
| **Props** | Boundary landmarks + minimal set-dressing at the perimeter for orientation. Plus the gameplay props needed for the semantic test: a **gold currency pickup** (value read) and an **extraction/landmark** for spatial reference. | §1a, §7, §10 props |
| **Lighting** | One fixed directional key at a top-down-flattering angle; soft shadows for depth (never hard enough to read as objects); baked where possible for the mobile tier. | §1a "fixed flattering key" |
| **Fog** | Thin distance/height fog that desaturates and pushes the perimeter back, without touching floor readability. | §1a "atmosphere via fog + post" |
| **Sky** | A grim-coastal-dusk sky/skybox setting the mood band; contributes ambient, does not compete with the floor. | §1a "dark-fantasy mood band" |
| **Camera framing** | The final top-down twin-stick framing (see §9). Everything above is composed *for this camera only.* | Global (top-down), §2/§6 |

**Mood target:** grim coastal dusk (Art Bible §1b, Arena 01 row). Bright-grim end of the dark-fantasy mood band — chosen deliberately because dusk is a *harder* readability test than full daylight but easier than a deep-dark arena; if the language holds at dusk, the mid-value-floor rule is proven for the whole band.

---

## 3 · Required assets

Fidelity tiers for the benchmark **build** (when Phase 0 is approved). "Placeholder" means a representative stand-in is sufficient to answer the one question — final hero fidelity is **not** required to validate the language. Nothing here is purchased or generated by this plan.

### Required (the benchmark cannot answer its question without these)
- Shattered Coast arena at **final proportions**: combat floor, courtyard, broken gates, low ruins, entrances.
- **Ground material** tuned to the mid-value / low-chroma / low-frequency rule (the make-or-break surface).
- **Perimeter framing**: cliffs, a readable sea plane, dark vegetation ring — enough to prove "detail at the edge, clear centre."
- **Lighting + fog + sky + post/color-grade rig** (the shared mood grade).
- **Player**: one class-neutral base mesh on the shared rig, with the full **locator stack** (§4-player), a dye material, and core movement/aim/attack animations.
- **Thrall (Walker)**: mesh on the shared faction skeleton, the shared tint/blight material, locomotion + attack + hit-reaction + death-dissolve.
- **Temporary VFX** (§7): cyan hit spark, white+identity death dissolve, red player-hurt flash + vignette, player projectile/tracer, one warm arming danger-decal, gold pickup glow.
- **Top-down camera rig** at the final framing (§9).
- **Profiling setup**: PC + mobile builds instrumented for the §10 metrics.

### Optional (raise fidelity or confidence, but not needed to answer the question)
- Extra set-dressing / vegetation variety beyond boundary framing.
- Advanced water (foam, motion, caustics) beyond a readable sea plane.
- Ambient audio bed (surf/wind) — this benchmark is **visual**; audio is a later slice (§8 of the Art Bible).
- A second mood variant (e.g. darker dusk) to pre-test the mood band.
- Extraction-point open/closed state visuals.
- A hero-polish pass on the player or Thrall.

### Placeholder (explicit stand-ins — deliberately not final)
- Player and Thrall may be **representative mid-fidelity or dressed-greybox** meshes; final production character quality is out of scope. The test is silhouette/footprint/colour/material *language*, not final sculpt.
- Sea = simple plane + shared water shader.
- Props = primitive-derived or simple stand-ins carrying the correct material + semantic colour.
- The benchmark's actual meshes/materials come from the **approved provisional-stack benchmark package** (purchased only after the Entry Gate is cleared) plus a free CC0 greybox pass; any purchased, generated, or stand-in asset still passes the Art Bible §9 acceptance gate before it counts toward the result. This plan authorizes none of that spend — the Entry Gate does.

---

## 4 · Player

One **class-neutral, grounded dark-fantasy adventurer base** — the entire Milestone-2 player scope (Art Bible §2, GD decision 5). No finished classes.

| Aspect | Specification | Source |
|---|---|---|
| **Equipment** | Layered leather/cloth/metal, dark-fantasy weathering, class-neutral. Detail only on **top-facing surfaces** (shoulders, head, back) — the only surfaces the camera sees. Modular sockets present but the benchmark ships one loadout. | §2 armour & material |
| **Colours** | Desaturated naturals so the player never out-shouts an enemy; one **customizable dye accent** from a reserved palette **disjoint from every enemy identity hue** and from the semantic bands. | Law ii, §2 colour |
| **Silhouette** | One constant top-down footprint that always reads as "the player." Asymmetric top-facing gear gives a built-in facing read. | Law iii, §2 |
| **Locator** (the load-bearing test — Law vi) | The full **approved v1.0 stack**: (1) subtle permanent **cool rim-light** keyed to the player only; (2) a **very thin ground ring** that becomes slightly more visible *only under high crowd density*; (3) a **physical facing cue** via asymmetric top-facing gear; (4) **no overhead icon**. Locator colour sits **outside all semantic and enemy bands.** Must read grounded/diegetic — never a mobile-game selection marker. | §2 Law vi |
| **Animations** | Core only: locomotion (idle/move), aim, attack/fire, hit-reaction. Just enough to observe the player moving and fighting inside the horde. | §2, §10 |

**What the player element proves:** A4 (findability), A2 (silhouette read), and part of A3 (that the player's own fire/hit reads as *yours*, never as enemy danger).

---

## 5 · Enemy — Thrall only

The benchmark uses **exactly one enemy type: the Hollowed Thrall (Walker)** — Art Bible §3. It is the correct choice because the Thrall *is the pipeline template* (§11) and the horde: proving it proves the shared-material/shared-skeleton/tint/instancing model the other three enemies reuse. Specials (Stalker/Brute/Corruptor) are **out of scope** for this benchmark.

| Aspect | Specification | Source |
|---|---|---|
| **Model** | Simple, upright, aggressive humanoid silhouette. Deliberately **quiet/plain** so future specials pop against it. Designed for its top-down shadow, not its face. | §3 Thrall, Law iii |
| **Material** | Shared **cracked-flesh / blight** master material, tinted per-instance via a property block (not a new material) to the Thrall identity hue **`#8B9188`** (cool neutral grey-green), with a subtle sickly emissive so corruption *is* why it reads its colour. | §3 faction, §6 shared materials |
| **Animation** | Shared faction skeleton; shambling locomotion + a readable **attack/shoot tell** + hit-reaction + shared **death-dissolve**. Animation-LOD-ready (distant instances update at reduced rate). | §3, §6 skeleton reuse / animation LOD |
| **Telegraph** | The Thrall is the *baseline "move & shoot"* threat, so its telegraph is minimal by design: a clear pre-attack tell before it fires. Full special-enemy telegraphs (e.g. the Brute wind-up) are **deferred**. To still validate the **reserved warm danger-decal language**, the benchmark includes **one placeholder arming ground-decal** (§7) proving warm-on-mid-value reads — even though the Thrall itself is ranged. | §4, §5 ownership stack |
| **Readability** | Must read at three ranges at once inside a crowd: **hostile** (not neutral), **not-a-special** (baseline grey, saturation-budget-correct), and **individually trackable** enough that hits/deaths land legibly. Grey Thrall must never camouflage into grey stone (A5). | §3, §12 Risk |

**What the enemy element proves:** A2, A5, A6, and the crowd half of A3.

---

## 6 · Crowd

Crowd counts are among the budgets this benchmark exists to **measure** (Art Bible §6/§11 defer exact numbers). The values below are **proposed test setpoints** to profile against — the *confirmed* numbers are an output, recorded in the future **Technical Art Budget** document, not frozen here.

| Tier | Proposed count | Purpose |
|---|---|---|
| **Minimum crowd** | ~30 Thralls | The floor at which "player inside a crowd" is a genuine readability test. Below this, findability and semantic collisions can't be judged. Establishes the baseline reads are correct at all. |
| **Target crowd** | ~120 Thralls | The representative peak of intended normal play density (Walkers-Attack-scale horde). **This is the tier the frame-rate targets must hold at** (§10). If the language and performance both hold here, the design intent is validated. |
| **Stress-test crowd** | ~350–500 Thralls | Deliberately beyond intended play to **discover the ceiling** and to prove **decoration-first degradation** (A7): as density climbs, decoration must cull while the semantic read (silhouette, identity colour, cyan hit, white death, locator) survives last. Finds where — and how gracefully — the model breaks. |

All three tiers run the **same** player, arena, and VFX; only Thrall count changes. Each tier is captured on both platforms (§10, §11).

---

## 7 · Temporary VFX

Only enough VFX to make the semantic layer observable. All are **temporary/representative** — final VFX authoring (VFX Graph, coverage budget, degradation LODs) is a later slice (Art Bible §5). Every effect obeys the **semantic colour lock** exactly; none may borrow another's meaning.

| VFX | Colour | Validates | Rule |
|---|---|---|---|
| **Player projectile / tracer** | Reserved cool-cored player-fire hue (**not** gold/warm — the QA collision noted in §4) | That the player's own fire reads as *yours* and emanates from the player/aim | §4, §5 ownership stack |
| **Enemy hit ("you hit it")** | Cyan **`#4FE6FF`** | Hit feedback landing legibly in a crowd; semantic cyan never confused with anything | Law iv, §4 |
| **Death** | White **`#FFFFFF`** + the enemy's identity hue | Death read + quick dissolve/despawn (no corpse accumulation) | §4, §6 corpse cleanup |
| **Player hurt ("you are hurt")** | Red **`#FF3B30`** — player flash + screen-edge vignette, self only | The most important defensive read survives a busy screen | Law iv, §4 |
| **Warm arming danger-decal** (placeholder) | Warm orange→red arming ground-decal that **fills as it arms** | The **reserved enemy-danger language** reads on the mid-value floor; proves warm-telegraph legibility even before specials exist | §4, §5 reserved-system rule |
| **Value / reward** | Gold **`#FFD524`** pickup glow | The value read is unambiguous and distinct from the Corruptor-gold/Holy questions (co-test deferred) | Law iv, §4 |

**Explicitly out of scope for this benchmark:** skill-school VFX (Fracture/Force etc.), special-enemy auras/tethers, shake/hitstop (kept off unless a later playtest proves needed — §4 VFX constitution).

**What VFX proves:** A3 (semantic lock, no collisions), the hit/hurt/death feedback reads, and the warm-danger language (partial telegraph validation).

---

## 8 · Lighting & post processing

One fixed grade for the whole arena; the mobile tier bakes what it can (Art Bible §1a). These are the settings the benchmark **tunes and then freezes on success** (§14).

| Parameter | Specification | Source |
|---|---|---|
| **Sun direction** | One directional key at a **top-down-flattering angle** — high enough that shadows give depth but never long/hard enough to read as objects on the floor. Fixed for the arena; a real setpoint to be dialled in and recorded. | §1a fixed key, Law iii |
| **Fog** | Thin distance/height fog: **desaturates and recedes the perimeter**, must **not** wash the mid-value floor or reduce enemy/VFX contrast in the play space. Tuned so the edge softens while the centre stays crisp. | §1a atmosphere |
| **Ambient** | Low, cool dusk ambient consistent with the sky; lifts shadow detail just enough for material read without flattening the value structure that carries readability. | §1a mood band |
| **Colour grading** | Grim-coastal-dusk grade applied via one shared post stack: sets mood, **holds the saturation budget** (world desaturated, gameplay layer saturated), and must not crush the semantic hues (cyan/red/gold must survive the grade intact). | Law ii, §1a |

**Hard constraint:** no lighting/fog/grade choice may reduce the legibility of an enemy, a telegraph, a hit, or the locator. Readability is law; the grade serves it (Law i).

---

## 9 · Camera

The benchmark establishes and freezes the **final top-down twin-stick camera** (Art Bible global identity, §11 "the camera that forces rework if changed late").

| Parameter | Specification | Notes |
|---|---|---|
| **Height** | High top-down, Walkers-Attack-scale — high enough to hold the **target crowd + arena landmarks** in frame with the player centred, low enough that the player and individual Thralls stay **small-but-readable.** A real setpoint to dial and record. | §1 crowd scale, Group A |
| **Angle** | Near-top-down with a slight tilt if it improves silhouette/material read without breaking the "read from above" discipline or introducing occlusion of the floor by tall perimeter geo. Exact angle is a benchmark output. | Law iii |
| **Zoom** | One default gameplay zoom for the benchmark; note the min/max range the framing tolerates before readability degrades (informs any future zoom feature). | §10 asset list "camera + zoom spec" |
| **Framing** | Player-centred; combat floor dominant; perimeter detail frames but never crowds the play space. The composition the whole scene is built *for.* | §1a, §2 |

Every screenshot (§11) is taken through this exact rig — the camera is part of what is being validated, not just a viewing convenience.

---

## 10 · Performance validation

This benchmark **produces the first concrete performance budgets.** The Art Bible deliberately did **not** invent poly/texture/draw/particle/crowd numbers (GD decision 7); they are measured here and recorded in a **separate Technical Art Budget document referenced from the Art Bible** — not hard-coded in the Bible or in this plan.

**Frozen outcome targets (Art Bible GD decision 7):**
- **PC:** stable **60 FPS @ 1080p** on the eventual **minimum-spec** target.
- **Mobile:** stable **30 FPS** on the eventual **minimum supported device.**
- **Both:** gameplay readability **identical across tiers**; only decoration scales.

Both targets must hold at the **target crowd (~120)**, not just an empty scene. The stress crowd is for ceiling discovery, not a pass condition.

> **Reference-device note:** the Art Bible defers exact min-spec/min-device. For the benchmark, the GD should confirm a **PC min-spec proxy** and a **mobile min-device proxy** to profile against (proposed as placeholders, to be set at review). Results are only meaningful against a named device.

### PC — metrics to measure (at min, target, stress crowds; 1080p)
- Average FPS + **1% low** FPS (target: 60 avg at target crowd)
- CPU main-thread ms and GPU ms per frame (identify the bound)
- Draw calls / SetPass calls / batches (and instancing effectiveness on the Thrall crowd)
- Triangles / vertices on screen
- Texture + VRAM memory footprint
- Active particle count and shadow-caster count
- **Headroom:** the crowd count at which FPS drops below 60 (how much margin over target)

### Mobile — metrics to measure (at min, target, stress crowds; min-device proxy)
- Average FPS + **1% low** (target: 30 sustained at target crowd)
- CPU ms / GPU ms per frame (mobile is usually GPU/bandwidth-bound)
- **Overdraw** (critical on mobile — fog, transparent VFX, water are prime suspects)
- Draw calls / batches; verify shared-material batching + GPU instancing actually engages
- Texture memory footprint vs device budget
- **Sustained thermal test:** hold target crowd for ~10 minutes; measure FPS decay and thermal throttling (a 30 FPS spike that collapses to 18 after 5 minutes is a **fail**)
- Battery/thermal qualitative note

### Both platforms — readability-under-load checks
- Overdraw visualization at stress crowd
- Confirm **decoration-first degradation** actually triggers (VFX/shadow/animation-LOD culling) before the semantic layer is touched
- Greyscale/value capture at target crowd (readability must survive loss of hue — see §11)

---

## 11 · Screenshot checklist

The exact captures for Game-Director review. All taken through the final §9 camera rig, at the grim-dusk grade, unless noted. Each maps to one or more assumptions (§1).

| # | Screenshot | Proves |
|---|---|---|
| 1 | **Empty arena, wide** — floor + perimeter, no actors | A1 mid-value floor; A5 mood; composition |
| 2 | **Arena + player alone** — no enemies | A2 silhouette; locator reads even with no crowd; scale vs arena |
| 3 | **Player in minimum crowd (~30)** | A4 findability floor; A2 Thrall read |
| 4 | **Player in target crowd (~120)** | A4 at design density; A6 the intended-density picture |
| 5 | **Player in stress crowd (~350–500)** | A4 worst case; A7 decoration-first degradation |
| 6 | **Combat — cyan hits landing** in a crowd | A3 hit semantic; feedback legibility under load |
| 7 | **Death moment** — white+identity dissolves | A3 death semantic; corpse cleanup read |
| 8 | **Player-hurt** — red flash + edge vignette | A3 the defensive read on a busy screen |
| 9 | **Warm arming danger-decal** on the floor | A3/telegraph — warm-on-mid-value legibility |
| 10 | **Gold pickup** in frame with combat | A3 value read vs the rest of the palette |
| 11 | **Perimeter framing** — cliffs / sea / ruins / vegetation | §2 "detail at edge, clear centre"; boundary read |
| 12 | **Lighting/fog mood** — the grim-dusk signature frame | A5; the "one screen" style-target |
| 13 | **PC tier vs mobile tier, same moment, side by side** | A7 identical readability layer, less decoration |
| 14 | **Greyscale of #4 (target crowd)** — hue removed | A2/A3 value structure alone carries the read (no reliance on colour) |

Optional supporting captures: overdraw visualization (stress crowd), a short capture/GIF of the player moving through the target crowd (findability in motion), a zoom min/max pair.

---

## 12 · Success criteria — what makes the benchmark PASS

The benchmark **passes** only if **all** of the following hold. A single failed load-bearing criterion means not-yet-proven (see §13).

**Readability (must all pass):**
- The **combat floor reads mid-value and recedes**; the saturated gameplay layer sits cleanly on top (A1).
- The **player is found instantly** at minimum, target, **and** stress crowds — the locator survives the horde (A4).
- The **Thrall reads as hostile, baseline (not special), and trackable** at crowd scale; it never camouflages into the environment at dusk (A2, A5).
- The **five semantic meanings never collide**: cyan hit, white death, red hurt, gold value, warm danger are each unambiguous, in motion, in a crowd (A3).
- **Hit, death, and hurt feedback stay legible** on the busiest screen; the warm danger-decal reads on the floor.
- The **greyscale capture (#14) still reads** — value structure carries recognition without hue.

**Performance (must all pass):**
- **PC holds 60 FPS @1080p at the target crowd** on the min-spec proxy, with non-trivial headroom (A8).
- **Mobile holds a sustained 30 FPS at the target crowd** on the min-device proxy through the 10-minute thermal test (A8).
- **Degradation is verifiably decoration-first**: under stress, decoration/shadows/LOD cull while the semantic + locator layer survives last (A7).
- The **mobile and PC readable layers are identical** — mobile is less decorated, never less legible (A7).

**Identity (GD judgment):**
- The scene reads as **grounded dark fantasy** — Walkers-Attack readability + New-World material/atmosphere — and **not** as cartoon and **not** as photoreal-MMO (A9), judged against the three-group Reference Matrix.
- The **Game Director signs off** that this single scene makes them confident the whole game's visual direction is correct — the literal one-question test.

---

## 13 · Failure criteria — when we stop and revise before broader buying

If any of these occur, **halt**: do **not** expand purchasing beyond the limited benchmark package, do **not** commit the provisional stack to broader production, and **revise or reject the provisional stack — and, where the fault is the language itself, the visual language (or the Art Bible assumption at fault)** first. Catching this here — cheaply, in one scene with one limited purchase — is the entire reason the benchmark exists.

- **The floor can't be made mid-value / recessive** with the chosen approach, or the world **camouflages** enemies or hides the danger-decal at dusk (A1/A5 fail).
- **The player gets lost** in the target or stress crowd — the locator stack is insufficient in a real horde (A4 fail). *This is a redesign-the-locator trigger, and a Law-vi review.*
- **Semantic colours collide or read ambiguously** in motion (e.g. player fire reads as enemy danger, or death reads as a hit) (A3 fail). *Triggers a §5 ownership-stack review.*
- **Frame targets cannot be met at the target crowd** on the reference devices even after reasonable optimization — especially **mobile thermal collapse** over the sustained test (A8 fail). *This is the core mobile-risk question the provisional stack was chosen to answer (live shortlist §7/§10); a fail here rejects or revises the provisional stack and escalates to a GD identity/stack conversation — never a silent drift.*
- **Degradation breaks readability** — decoration-first is not achievable; the semantic layer degrades before decoration (A7 fail).
- **The identity reads wrong** — the GD judges it cartoon, generic, or photoreal-MMO rather than grounded dark fantasy (A9 fail). *Triggers an identity/reference-matrix revisit before spend.*

A failure is a **cheap, early, correct outcome**, not a setback — it means the benchmark did its job before money was committed.

---

## 14 · Lessons expected — what becomes permanently frozen on success

On a PASS, the following graduate from "proposed" to **frozen production standards** — precisely the choices the Art Bible §11 says force total rework if changed late. Several feed the new **Technical Art Budget** document.

**Freeze into the pipeline (design/authoring standards):**
- The **master palette + semantic-colour lock** final values (cyan/white/red/gold/warm + player-fire hue), validated in situ.
- The **mid-value combat-floor recipe** — the material/value/frequency settings that make the floor recede.
- The **player-locator implementation** — the exact rim + density-ring + facing-cue stack that survived the horde (Law vi, made concrete).
- The **lighting + fog + ambient + grade rig** setpoints for the grim-dusk mood (and the tuning method for other moods).
- The **top-down camera** height / angle / default zoom / framing.
- The **shared-material + shared-skeleton + tint + GPU-instancing** enemy pipeline as the proven template all future enemies reuse.
- The **PC/mobile tier + decoration-first degradation policy** — what culls, in what order.

**Freeze into the Technical Art Budget document (measured numbers):**
- Concrete **poly / vertex, texture / VRAM, draw-call, particle, shadow-caster, and crowd-count budgets** measured at target crowd on both reference devices.
- The confirmed **target and stress crowd counts** and the PC headroom figure.
- The confirmed **min-spec PC and min-device mobile** reference devices.

**Unblocks (separate, later, GD-gated decisions — not part of this benchmark):**
- A **verdict on the provisional stack** — validate it for broader production, or reject/revise it — now judged against *measured evidence* instead of estimates. (The benchmark never selected the stack; it tested the one already chosen.)
- **Broader purchasing** beyond the limited benchmark package — the full Art Bible §9 flow (shortlist → licence → compatibility → GD approval) — may proceed only if the provisional stack passed.
- Scaling the proven language to the **next arena, the rest of the Hollowed faction, the player classes, and the skill schools**, per the vertical-slice-first roadmap (§11).

---

> **Scope reaffirmation:** this document plans a benchmark; it builds nothing. No Unity, no purchases, no AI generation, no Phase-0 production are authorized or begun by it. Execution starts only on an explicit Game-Director "go," at which point this plan becomes the blueprint for Point Clear's first visual implementation.
