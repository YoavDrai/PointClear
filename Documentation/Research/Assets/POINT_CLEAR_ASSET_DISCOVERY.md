# Point Clear — Asset Discovery & Sourcing Guide

> **Status:** Research & planning milestone. **Documentation only.** No assets created, no purchases made, no Unity/Blender/Meshy work, no Phase 0.
> **Governing spec:** *Point Clear Art Bible v1.0* (frozen). Every sourcing decision here must satisfy the Bible's readability laws, the saturation budget, the semantic-colour lock, the top-down design rules, PC + mobile targets, and the **Asset Acceptance Gate**.
> **Purchasing policy (frozen, Art Bible §9):** one cohesive environment-pack family · one modular character base · one animation-library source · one licensed display serif + one humanist sans. **Every purchase requires: researched shortlist → licence verification → technical-compatibility check → Game-Director approval.**
>
> ⚠️ **Prices and licence terms below are indicative (early-2026 knowledge) and MUST be re-verified at purchase time.** This document is guidance, not legal advice. It identifies candidates, workflows, and considerations — it does not lock final vendors, prices, or licences.

---

## 0. How to read this document

The central problem for a **solo developer** targeting a **stylized-natural dark-fantasy** look (New World-inspired materials, *not* flat cartoon) across **seven biomes** on **PC + mobile** is **cohesion**, not availability. Assets are cheap and plentiful; making a mixed bag of them read as *one game* is the hard part.

**The unifier is already in the Art Bible:** the shared **URP shader library + one lighting/fog/post grade**. That lets us source the *best-fit* asset per category from *different* origins (packs, AI, custom) and unify them in-engine through one material/lighting language. This doctrine — **source diversely, unify centrally** — underpins every recommendation here.

**Style-fit caution used throughout:** the Bible says *stylized, **not cartoon**, grounded materials*. That rules out pure flat-shaded low-poly as the hero look, and rules out full photoreal (mobile perf + cohesion cost). The target is **semi-realistic stylized PBR** ("hand-painted-leaning but grounded"). Each recommendation is scored against that target.

---

## 1. Character pipeline (the player)

**Requirement (Art Bible §2 / decision 5):** ONE class-neutral base; a **shared rig**; top-down silhouette; a permanent locator; **modular equipment sockets**; **dye/accent** customization; core move/aim/attack anims. Reusable for all future classes.

### Options

| Solution | What it is | Fit | Cost (indicative) | Licensing notes |
|---|---|---|---|---|
| **Reallusion Character Creator 4 (+ AccuRIG, iClone)** | Parametric human base + modular clothing/armour ecosystem + auto-rig + retarget | **Strong** — built for modular gear + a single reusable base; grounded realistic-stylized | CC4 mid-hundreds one-time; content packs extra; AccuRIG free | Per-seat; created characters usable commercially; **verify** content-pack redistribution terms |
| **Unity UMA (Multipurpose Avatar)** | Free open modular avatar framework (runtime dye, slot-based gear) | **Good** for modular sockets/dye at runtime; needs art to feed it; older tooling | Free (asset) | Open; art you add carries its own licence |
| **Synty Modular Fantasy Hero / POLYGON characters** | Cohesive modular fantasy character kit | **Partial** — very cohesive & cheap, but flat/cartoon-leaning vs "grounded" | Low tens per pack | Unity Asset Store EULA (see §11) |
| **Ready Player Me** | Cross-game avatar SDK | **Weak** — stylized-generic, not dark-fantasy-grounded | Free tier / commercial tiers | SDK terms; **verify** commercial use |
| **Meshy / Tripo / Rodin (AI) + cleanup** | Generate a base or gear pieces from image/text | **Support role** — concepting + variant gear, not the hero rig | Subscription (see §9) | Commercial on paid tiers — **verify per tool** |
| **Mixamo** | Free auto-rig + animation library (Adobe) | **Strong** for rig + base locomotion (retarget target) | Free | Free commercial use; **verify** current Adobe terms |

### Recommendation
**Character Creator 4 as the base + custom modular gear (sockets/dye) + Mixamo/AccuRIG rig + retargeted library animation**, with **AI (Meshy/Tripo) used for gear-piece concepting and variants only**. Rationale: the player is on screen constantly and must carry a *single reusable rig* through every future class — that demands a controlled, riggable, modular base, which CC4 gives faster than hand-modelling and more grounded than Synty. UMA is the fallback if runtime modularity/dye proves fiddly.

**Pros:** grounded look, true modularity, one rig, fast. **Cons:** CC4 has a learning curve; content-pack licensing needs verification; keeping it stylized (not photoreal) requires the shared shader to pull it toward the Bible's look.

---

## 2. Enemy pipeline ("The Hollowed" faction)

**Requirement (Art Bible §3):** four members (Thrall / Stalker / Brute / Corruptor) on **one shared skeleton**, identity-coloured via the tint shader, role-telegraphing silhouettes, shared hit/death beats. Extensible to future factions later.

### Workflows compared

| Workflow | How | Cohesion | Speed | Risk |
|---|---|---|---|---|
| **AI-generated + cleanup (Meshy/Tripo/Rodin)** | Generate four role-silhouette variants from one concept, retopo/clean, bind to shared rig | Medium (needs a firm concept + cleanup discipline) | **Fast** | Topology/UV debt if not gated; style drift between generations |
| **Asset-pack creatures (e.g., PROTOFACTOR, Dungeon Mason/Infinity PBR, Synty)** | Buy a cohesive creature set, reskin/tint to the four roles | High **if single publisher** | Fast | May not hit the exact four role silhouettes; rig may differ from player |
| **Hybrid (recommended)** | One **shared blight base** (bought or AI or hand-blocked) → generate/sculpt four role variants → one shared skeleton → shared anim/VFX | **Highest** — one material language, one rig | Medium | Requires the base + rig locked first (that's Phase 2's job) |
| **Fully custom** | Hand-model each | Highest | Slow | Not solo-realistic for a whole family |

### Shared-rig strategy
Lock **one humanoid-ish skeleton** for the faction first (Thrall = the template, Art Bible Phase 2). All four bind to it → shared locomotion, shared hit-reaction/death-dissolve, animation instancing (§6 horde). Future factions reuse the same skeleton where their body plan allows; genuinely different body plans get a second shared rig — never one-off rigs.

### Recommendation
**Hybrid, seeded by AI.** Establish the shared "blight" material + skeleton on the **Thrall** (end-to-end template), then produce the other three as **AI-generated-then-cleaned variants** bound to that skeleton, each tinted to its identity hue by the shader. Buy a creature pack **only** if a single publisher happens to nail all four silhouettes cohesively (rare). **Cost:** mostly AI subscription + time. **Biggest risk:** style drift across AI generations → mitigate by generating from one locked concept sheet and running every result through the Acceptance Gate.

---

## 3. Environment pipeline

**Requirement:** cohesive coverage of **forest · ruined castle · coastal fortress · temple · mine · graveyard · swamp**, all obeying the arena-system rules (mid-value calm floor, detail at edges/landmarks, bounded). Style: **semi-realistic stylized dark fantasy**. First arena = **The Shattered Coast**.

### Single-publisher ecosystems (prefer these)

| Publisher / ecosystem | Style | Biome coverage | Fit vs "New World grounded" | Notes |
|---|---|---|---|---|
| **Synty — POLYGON** (Fantasy Kingdom, Dungeons, Nature, Dark Fantasy, Knights) | Flat-shaded stylized low-poly | **Excellent, cohesive, cheap** | **Too cartoon-leaning** for the Bible, but the *most cohesive & mobile-friendly* option; could work for a more stylized interpretation | Huge single-publisher ecosystem; unbeatable for cohesion + mobile |
| **Leartes Studios** (Environment megapacks — medieval, dungeons, temples, swamps, graveyards) | Realistic-stylized PBR | Very broad | **Closest to New World grounded** | Heavier (PC-leaning); cohesion good within Leartes but assembly needed; watch mobile budgets |
| **KitBash3D** (via Fab/Cargo — fantasy/medieval sets) | High-end realistic | Strong per-set | Grounded but **film-grade / heavy** | Premium; great for landmarks/hero pieces, overkill for whole mobile game |
| **Quixel Megascans (via Fab)** | Photoreal scans (materials, rocks, foliage, debris) | Materials/props, not authored scenes | Grounded raw material | Free with an Epic/Fab account (**verify current terms**); needs stylizing via shader; heavy without care |
| **Quaternius / Kenney / KayKit (CC0)** | Stylized low-poly, CC0 | Broad-ish | Stylized, cartoon-leaning | **CC0 = zero licence risk**; great for greybox/prototyping and mobile |
| **Manufactura K4 / Sabresphere / Gamertose** | Stylized-PBR medieval/fantasy | Medium | Good mid-ground | Smaller ecosystems; mix-and-match risk |
| **Terrain/biome tooling — GAIA + GeNa (Procedural Worlds)** | Terrain/scattering | All biomes (bases) | N/A (a tool, not art) | Speeds bounded-arena terrain + placement |

### The core decision (flag for GD + a moodboard test)
There is a genuine style fork:
- **Path A — Cohesive stylized (Synty/CC0-led):** best cohesion, cheapest, mobile-safe, but risks reading "cartoon," which the Bible forbids.
- **Path B — Grounded semi-real (Leartes-led + Megascans materials):** closest to New World, but heavier, harder to keep cohesive solo, mobile-risky.
- **Path C (recommended) — Grounded base, unified by our shader + grade:** pick the *most grounded cohesive base we can afford* (Leartes-leaning), keep the **arena floors calm and low-detail** (Bible rule) so material cost concentrates at edges/landmarks, and rely on the **shared lit shader + post grade** to pull everything toward one "stylized-natural" look. Use **CC0 (Quaternius/KayKit)** for the *greybox* pass so Phase 0/1 costs nothing until the look is proven.

**Resolve this with a paid-for-one-arena test on The Shattered Coast before committing to a whole-game ecosystem** (matches vertical-slice-first).

---

## 4. Props

rocks · vegetation · barrels · crates · ruins · bridges · statues · furniture · torches · gates · environmental decoration.

| Source | Best for | Style-fit | Licence |
|---|---|---|---|
| **Same environment ecosystem as §3** | Everything (cohesion first) | Matches whatever §3 path we pick | Per ecosystem |
| **Quixel Megascans (Fab)** | Rocks, debris, ruins, foliage, natural detail | Grounded (stylize via shader) | Free w/ account — **verify** |
| **CC0 (Quaternius/Kenney/KayKit/PolyHaven)** | Barrels, crates, generic props, greybox | Stylized; PolyHaven = grounded PBR + HDRIs/materials | **CC0 — no risk** |
| **AI (Meshy/Tripo)** | One-off hero props (gates, statues, the extraction landmark) | Needs cleanup + gate | Commercial paid tiers — **verify** |

**Rule:** *gameplay-critical* props (currency pickup, weapon module, extraction point, boundary landmarks) stay **studio-controlled** for instant readability; ambient set-dressing comes from the cohesive ecosystem. **Torches/gates/statues** are prime candidates for **shared shader** treatment (emissive torch = a controlled light source, not a random pack material).

---

## 5. Equipment (modular)

armor · helmets · cloaks · boots · gloves · shields · swords · axes · bows · staffs · accessories.

| Source | Best for | Fit | Licence |
|---|---|---|---|
| **Reallusion CC4 modular outfits / SkinGen** | Full modular armour/cloth on the player base (§1) | **Strong** (same base = one rig) | Verify content-pack terms |
| **Synty Modular Fantasy Hero + weapon packs** | Cohesive modular gear + weapon sets | Cohesive but flat | Asset Store EULA |
| **Dungeon Mason / Infinity PBR, PolygonMaker, RPG weapon packs** | Weapons (swords/axes/bows/staffs), shields | Varies — pick a single publisher per weapon family | Asset Store EULA |
| **AI (Meshy/Tripo) + cleanup** | Gear variants, accessory volume | Support role (concept + variants) | Paid-tier commercial — verify |

**Doctrine:** equipment must **socket onto the one shared rig** (§1) and stay **legible from top-down**. Prefer gear from the **same base ecosystem as the player** so weights/skinning are compatible. Weapons are the easiest to source per-family from packs; armour is best kept on the CC4/base pipeline for modularity + dye.

---

## 6. VFX

magic · explosions · trails · hit effects · death effects · environment effects.

| Source | Best for | Fit vs Art Bible §4/§5 | Licence |
|---|---|---|---|
| **Unity VFX Graph / Shuriken (custom)** | The semantic-locked core (cyan hit, white+identity death, telegraphs, player fire, Fracture school) | **Primary** — the semantic lock + juice ceiling demand studio control | Engine-native |
| **Hovl Studio** (Magic, Explosions, Trails, Beams) | Reference + library for magic/impact motion | Popular, cohesive; must be **retinted to reserved school hues** + trimmed under the coverage budget | Asset Store EULA |
| **KriptoFX (Realistic Effects Pack / War FX)** | Grounded impacts/explosions | Grounded; retint/trim | Asset Store EULA |
| **Jean Moreno — Cartoon FX / JMO** | Stylized bursts | May be too cartoon; selective | Asset Store EULA |
| **Gabriel Aguiar / Mirza Beig** | Tutorials + some packs for stylized-real magic | Great learning + build-your-own | Mixed |

**Recommendation:** **custom VFX Graph is primary** (the Bible's semantic-colour lock, reserved school hues, coverage budget, and horde-degradation LODs cannot be met by an off-the-shelf pack). Buy **one** VFX pack (Hovl or KriptoFX) as a **motion/technique library and reference**, never shipped as-is. Environment effects (fog, embers, dust, water) → shader + VFX Graph, tuned to *desaturate*, not distract.

---

## 7. UI

**Requirement (Art Bible §7):** one bespoke "expedition journal" identity — aged parchment + weathered bronze + engraved type; minimal peripheral HUD; semantic-only accent colours.

| Source | Verdict |
|---|---|
| **Fantasy UI packs** (Layer Lab GUI PRO, Fantasy Wooden GUI, RPG/MMO UI kits, Synty UI) | **Scaffold only.** Good for rapid layout and as a component reference, but a bought skin breaks the bespoke journal identity. |
| **Custom (recommended)** — component kit in Unity UI Toolkit/uGUI, **AI-drafted icons + one style-unification pass**, licensed fonts | **Preferred.** UI is small surface, high identity value; custom keeps cohesion. Claude assists layout/logic. |

**Recommendation:** **custom UI + custom icon set** (one engraved-line style), AI (Scenario/Leonardo/Recraft) for icon *drafts* then a single unification pass, a licensed display serif + humanist sans (§11). Buy a UI kit only as a layout scaffold if it accelerates the component system.

---

## 8. Audio

music · ambience · SFX · voice.

| Category | Best sources | Notes / licence |
|---|---|---|
| **Music (adaptive orchestral dark-fantasy)** | A **composer** (best cohesion) OR a cohesive library — **Epidemic Sound / Artlist** (subscription sync licence), fantasy score packs on Asset Store | Subscription libraries: licence lasts while subscribed — **verify** perpetuity terms for a shipped game. AI music (Suno/Udio) = **licence risk, verify commercial + ownership** |
| **Ambience** | Per-arena beds — Asset Store ambience packs, **BOOM Library / Soundly**, field-recording libs | Verify per-title use rights |
| **SFX** | **Sonniss "GDC Game Audio Bundle" (free, royalty-free)**, BOOM Library, Soundly, Asset Store SFX | Sonniss free bundles are excellent + clearly licensed |
| **Voice** | Minimal for now; **ElevenLabs** (AI voice) for stingers/UI if needed | AI voice licensing + likeness terms — **verify carefully** |

**Recommendation:** **Sonniss + one shaping library for SFX**, an **Epidemic/Artlist subscription (or a commissioned score)** for adaptive music, and **deliberately-designed enemy/skill audio telegraphs** (Art Bible §8) built from those libraries. Defer voice.

---

## 9. AI pipeline — tool-by-tool

> Every AI output ships **only** if it passes the Acceptance Gate (Art Bible §9). AI origin neither approves nor rejects; the gate does. **Commercial-licence and ownership terms for AI tools change frequently and vary by tier and jurisdiction — verify at adoption.**

### 3D generation (image/text → mesh)

| Tool | Strengths | Weaknesses | Ideal use | Commercial licence | Pipeline fit |
|---|---|---|---|---|---|
| **Meshy** | Fast text/image→3D, PBR maps, auto-rig (beta), remesh | Topology needs cleanup; detail varies | Enemy/gear/prop **variants & blockouts** | Commercial on **paid** tiers (verify tier) | Concept→AI stage; cleanup mandatory |
| **Tripo (Tripo3D)** | Very fast image→3D, decent topology, good silhouettes | Fine detail limited | Rapid silhouette exploration, props | Paid-tier commercial (verify) | Same slot as Meshy; A/B them |
| **Rodin (Hyper3D)** | High detail/quality, good for hero-ish meshes | Slower/pricier; still needs retopo | Higher-fidelity single pieces | Verify tier | Hero prop/creature blockouts |
| **TRELLIS (Microsoft, open-source)** | Runs **locally**, controllable, no per-gen cost, permissive licence | Setup/GPU needed; rawer output | Cost-free volume generation for a solo dev | Open (verify current licence) | Local variant factory |
| **Kaedim** | Image→game-ready with human-in-loop | Pricier, subscription | When cleaner topology matters | Verify | Higher-quality AI slot |
| **Sloyd / CSM / Luma Genie** | Parametric / alt generators | Varies | Backups / niche props | Verify | Optional |

### 2D concept / texture / icon generation

| Tool | Strengths | Weaknesses | Ideal use | Licence |
|---|---|---|---|---|
| **Midjourney** | Best-in-class concept art & moodboards | Not for direct asset extraction; style consistency needs effort | **Moodboards, concept sheets, look-dev** | Commercial on paid plans — **verify** |
| **Scenario** | **Trainable custom style models**, game-asset focus (props, icons, textures, skyboxes) | Learning curve to train a model | **Cohesive, on-style 2D** (icons, textures, concept) once a Point-Clear model is trained | Commercial tiers — verify |
| **Leonardo.ai** | Game-focused, textures/icons/concept, control tools | Style drift without care | Icons, textures, concept | Commercial tiers — verify |
| **Flux (Black Forest Labs)** | High-quality images; open weights (schnell = Apache; dev = non-commercial; Pro = API) | Licence varies by variant | Concept/textures (use commercially-licensed variant) | **Variant-dependent — verify** |
| **ChatGPT Image (gpt-image-1 / DALL·E)** | Fast concept/icon drafts, iteration with Claude/GPT | Not production-final | Rapid drafts, moodboard fills | Per OpenAI commercial terms — verify |
| **Stable Diffusion (local + ControlNet)** | Free, controllable, tileable textures | Setup/skill | Seamless textures, controlled concept | Model-licence dependent |
| **Recraft / Krea / Ideogram** | Vector/icon/text-in-image | Niche | Icon/vector drafts | Verify |

### Rigging & animation (AI-assisted)
| Tool | Use | Licence |
|---|---|---|
| **Mixamo** | Free auto-rig + locomotion library (retarget source) | Free (verify Adobe terms) |
| **Reallusion AccuRIG** | Free auto-rig for CC/others | Free |
| **Rokoko / DeepMotion / Move.ai** | AI/video mocap for signature anims | Subscription — verify |
| **Cascadeur** | AI-assisted keyframe (physics) for telegraphs | Free/indie tiers — verify |

**Where AI fits (summary):** **Midjourney/Scenario** own *concept + moodboard + on-style 2D*; **Meshy/Tripo/TRELLIS** own *3D volume/variants + blockouts*; **Mixamo/AccuRIG/Rokoko** own *rig + base/signature animation*; **Scenario/Leonardo/Recraft** own *icon/texture drafts*. **Nothing AI is final until cleaned + gated.**

---

## 10. Asset Store / marketplace research — cohesive ecosystems

**Prefer a single-publisher ecosystem per discipline.** Do not assemble random disconnected packs.

| Marketplace | Role for Point Clear |
|---|---|
| **Unity Asset Store** | Primary for Unity-native packs (Synty, Leartes, VFX, tools). One-time purchases, engine-ready. |
| **Fab (Epic)** | Cross-engine marketplace; **hosts Quixel Megascans** (free w/ account — verify) + KitBash3D Cargo + more. Grounded materials/props. |
| **CC0 sources (Kenney, Quaternius, KayKit, Poly Haven)** | **Zero-licence-risk** greybox + mobile-friendly stylized props + grounded PBR materials/HDRIs. Ideal for Phase 0/1. |
| **Reallusion Content Store** | Player base + modular gear ecosystem (CC4). |
| **Sketchfab** | One-off models — **licence per-model (CC/commercial), verify each**. |

**Primary-ecosystem candidates (pick one for environment/props, then unify via shader):**
- **Grounded route:** *Leartes* environment megapacks (+ Megascans materials via Fab).
- **Cohesion/mobile route:** *Synty POLYGON* (accept a more stylized look) or CC0 *KayKit/Quaternius*.
- **Character route (separate):** *Reallusion CC4* ecosystem.
- **VFX route:** *custom VFX Graph* + one *Hovl/KriptoFX* library.

**Decide the environment ecosystem via the paid single-arena (Shattered Coast) test — not on paper.**

---

## 11. Licensing — what matters

> Summaries for planning; **not legal advice**. Verify current EULAs at purchase (policy requirement).

- **Unity Asset Store EULA:** royalty-free, **per-seat**, use in unlimited *your* projects; **cannot redistribute the raw asset** as an asset; "Single Entity" vs "Multi Entity" licence matters for teams (solo = single is fine). Keep invoices.
- **CC0 (Kenney/Quaternius/KayKit/Poly Haven):** public domain — **no attribution required, zero risk**. Best for greybox and anything you'll heavily modify.
- **Creative Commons (CC-BY etc., common on Sketchfab):** attribution / share-alike obligations — **read each**; CC-BY-NC = **not usable commercially**.
- **Quixel Megascans / Fab:** currently free with an Epic account for use in your projects — **verify current Fab standard licence and any engine restrictions**.
- **AI-generated assets:** two distinct issues — (1) **commercial-use rights** (tool-and-tier dependent; some free tiers forbid commercial use); (2) **copyright ownership/indemnity** (AI output may have limited/uncertain copyright protection in some jurisdictions; some vendors offer indemnification on paid tiers). **Verify per tool + tier; keep records of tool, tier, and date.**
- **Meshy exports:** commercial use generally on **paid** tiers; free tier may restrict — **verify the exact tier's licence before shipping any Meshy-derived mesh.**
- **Audio subscriptions (Epidemic/Artlist):** sync licence typically valid **while subscribed** — confirm the terms for a **shipped, permanently-distributed game** (this is a known gotcha). Sonniss bundles are royalty-free — keep the licence PDF.
- **Fonts:** desktop vs **app/embedding** licences differ — buy a licence that explicitly covers **embedding in a game/app** (SDF atlases count as embedding).
- **AI music/voice (Suno/Udio/ElevenLabs):** commercial + ownership terms are evolving and **higher-risk** — verify carefully or prefer commissioned/library audio.

**Record-keeping rule:** every acquired asset → store invoice + licence text + (for AI) tool/tier/date, in a licence register referenced by the project.

---

## 12. Production pipeline — concept → Unity

Every asset flows through this, and **must pass the Acceptance Gate before Unity integration**:

```
IDEA          role/need defined by the Art Bible (a lesson, a landmark, a gear slot)
  ↓
CONCEPT       Midjourney / Scenario / Claude — moodboard + concept sheet on-style
  ↓
REFERENCE     pin against Reference Groups A/B/C (Walkers Attack read · New World material · greybox baseline)
  ↓
AI or ASSET   Meshy/Tripo/TRELLIS (generate) · OR a cohesive pack · OR CC0 (greybox)
  ↓
CLEANUP       retopo, UVs, scale, LODs, texel density; strip to budget  ── ▶ ACCEPTANCE GATE
  ↓                                                                         (visual · technical · perf ·
RIG           bind to the shared rig (player / faction) — Mixamo/AccuRIG    licence · rig · material · readability)
  ↓
ANIMATION     retargeted library locomotion + custom signature telegraphs
  ↓
UNITY         apply the shared shader library + tint + locator; wire prefabs; semantic VFX
  ↓
OPTIMIZATION  LODs, GPU instancing, animation LOD, shadow policy, mobile tier (Art Bible §6)
  ↓
QA            top-down readability check at horde scale, PC + mobile profiling vs the frozen 60/30 targets
```

**Gate placement is deliberate:** nothing enters Unity until it passes visual cohesion, technical health, performance budget, licensing, rig compatibility, material fit, and readability. AI, pack, and custom assets all pass through the *same* gate.

---

## 13. Budget (indicative ranges — verify)

> Ballpark **software/asset** spend for the Milestone-2 vertical slice + language foundation (excludes hardware, and excludes ongoing subscriptions beyond the build). **Estimates only.**

| Tier | ~Range | What it buys |
|---|---|---|
| **Minimal** | **~$0–400** | CC0 environment/props (KayKit/Quaternius) + Mixamo (free) + UMA/free base + free AI tiers (TRELLIS local, ChatGPT/Flux-schnell) + Sonniss free SFX + one licensed font. Ships, but style-fit is a stretch and more manual work. |
| **Recommended** | **~$800–2,500** | One cohesive environment ecosystem (Leartes/Synty tier) + Reallusion CC4 + a weapon/gear pack + 1–2 AI subscriptions (Meshy + Scenario/Midjourney) + one VFX library + a licensed serif & sans + an audio subscription **or** a small commissioned track. Best quality-for-solo balance. |
| **High-quality** | **~$3,500–8,000+** | Premium grounded environment ecosystem + Megascans/KitBash landmarks + full CC4 pipeline + premium AI tiers + premium VFX + **commissioned adaptive score** + **selective outsourcing** of 1–2 hero pieces (player/boss). Highest fidelity, still solo-managed. |

Prices shift; treat these as planning envelopes, not quotes. The **paid single-arena test** should be costed first (a few hundred dollars) before any whole-game ecosystem commitment.

---

## 14. Final recommendation — the workflow for Point Clear

**The highest visual quality realistically achievable by a solo dev with Claude + AI + commercial assets is a "grounded cohesive base, unified centrally, with AI as the volume multiplier and custom where identity lives."** Concretely:

1. **Foundation is in-engine, not bought.** The shared **URP shader library + one lighting/fog/post grade + the semantic-colour lock** is what makes mixed sources read as one game. Build it first (Art Bible Phase 0). *This is the single highest-leverage decision.*
2. **Environment:** run a **paid single-arena test on The Shattered Coast** comparing a **grounded ecosystem (Leartes-leaning + Megascans materials)** against a **cohesive stylized one (Synty/KayKit)**, judged on the *finished* look through our shader. Commit to **one** ecosystem only after that test. Greybox everything with **CC0** until then (zero spend, zero risk).
3. **Player:** **Reallusion CC4** base + modular gear + dye, one shared rig, Mixamo/AccuRIG + retargeted animation. Class-neutral only (decision 5).
4. **Enemies:** **hybrid, AI-seeded** — Thrall end-to-end as the template, the other three as **AI-generated-then-cleaned** variants on the shared skeleton, tinted by the shader.
5. **AI as the multiplier, never the shipper:** Midjourney/Scenario for concept + on-style 2D; Meshy/Tripo/TRELLIS for 3D volume/variants; everything cleaned and **gated**. Train a **Scenario style model** once the look is locked, to keep 2D (icons/textures/concept) cohesive.
6. **Custom where identity lives:** VFX (semantic lock), UI + icons (journal identity), gameplay-critical props, and telegraph animations stay studio-made (Claude-assisted).
7. **Audio:** Sonniss + one shaping library for SFX; a commissioned or subscription adaptive score; deliberately-designed telegraph audio.
8. **Prove one slice, then scale.** One arena (Shattered Coast), the full Hollowed faction, the player, core VFX/UI/audio — approved — **before** buying into more biomes, factions, schools, or classes.

**Why this wins for Point Clear specifically:** it spends money on the few things that carry the game (a grounded cohesive base + a controllable player pipeline + the unifying shader), uses AI to erase the volume problem that usually sinks solo scope, and keeps every identity-critical, readability-critical surface in studio hands — exactly the split the frozen Art Bible already mandates.

---

### Next steps (all gated on explicit Game-Director approval; still no purchases, no Unity, no Phase 0)
1. GD reviews this document and picks the **environment style fork** direction to test (or approves the paid single-arena test to decide it).
2. Build **researched shortlists** (2–3 candidates) for each frozen purchasing category (env ecosystem, character base, animation source, fonts) with current prices + licence checks, per the purchasing policy.
3. On GD go, Phase 0 begins with the **shader/lighting/palette/locator foundation + the benchmark scene + the single-arena environment test** — the point at which the first (small, test-scoped) spend would be proposed for approval.

*Document status: research/planning only. No assets created, nothing purchased, Unity untouched, Phase 0 not started.*
