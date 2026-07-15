# Point Clear — Asset Validation (Real Candidate Products)

> ## ⚠️ STATUS: SUPERSEDED FOR PURCHASING — retained as a methodology & historical candidate record
> **For current purchasing evidence, use [`POINT_CLEAR_LIVE_ASSET_SHORTLIST.md`](POINT_CLEAR_LIVE_ASSET_SHORTLIST.md)** (live-verified 2026-07-15). The products, prices, dates, licences, render-pipeline support, and Unity compatibility in *this* document were **NOT live-verified against official store pages** — they are from prior knowledge (early-2026), are **indicative only**, and are **superseded for any purchasing decision** (for example, this document references *Character Creator 4*; the current Reallusion generation is **CC5** — see the Live Shortlist). **This is not a shopping list.** It is retained as a **methodology and historical candidate record** — how candidates are evaluated and what was considered — not as current purchasing evidence.
>
> **Milestone:** Research/validation. **Documentation only.** Nothing purchased, nothing committed, Unity untouched, Phase 0 not started.
> **Companion to:** `POINT_CLEAR_ASSET_DISCOVERY.md` (sourcing strategy) · governed by *Art Bible v1.0* (frozen).
>
> ### ⚠️ Read this first — data-accuracy disclaimer
> Product names and publishers below are **real** and current to my knowledge (**early-2026**). But **price, "last update" date, and verified Unity-6/URP status change frequently** and vary by sale. Every such field is written as **`~approximate` / "verify on store page."** Do **not** treat any price or date here as authoritative — confirming them on each product's live store page **is** the "technical-compatibility + licence verification" step required by the frozen purchasing policy. This is guidance, not legal advice.
>
> ### The single most important finding
> You cannot "buy everything." Assets only read as one game if they belong to **one internally-cohesive stack**. There are **two viable stacks** for Point Clear, and **they must not be mixed**:
> - **Stack A — Stylized/Cohesive (Synty-led):** unbeatable cohesion + mobile performance + cost, but **flat/cartoon-leaning** — a *risk against the Bible's "stylized, not cartoon, grounded materials."*
> - **Stack B — Grounded/Semi-real (Leartes + CC4-led):** closest to the New World-grounded target, but **heavier, PC-leaning, harder to keep cohesive solo.**
>
> The Bible points at **Stack B**, but Stack B carries the mobile/cohesion risk. **This is resolved by the paid single-arena test on The Shattered Coast, not on paper.** Everything below is organized so either stack can be chosen cleanly.

---

## Legend
- **Price:** `~USD`, indicative, frequent sales — **verify**.
- **U6 / URP / Mobile:** Yes / Likely / No / N-A — **verify current version on store page**.
- **Licence:** *ASE* = Unity Asset Store EULA (per-seat, royalty-free, no raw redistribution) · *CC0* = public domain (zero risk) · *Vendor* = product-specific EULA.
- **Maintained:** whether the product is actively updated (verify exact date).

---

## 1. Environment

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Stack | Fit |
|---|---|---|---|---|---|---|---|---|
| **POLYGON Fantasy Kingdom / Dungeons / Nature / Knights (Synty Studios)** | ~$30–90 ea (bundles cheaper) | ASE | Yes | Yes (URP mats provided) | **Excellent** | Active | **A** | Cohesive, cheap, mobile — but flat |
| **Environment / Medieval / Dungeon / Graveyard / Swamp Megapacks (Leartes Studios)** | ~$30–150+ ea | ASE | Likely (verify) | URP variant (verify) | Medium (PC-leaning) | Active | **B** | Grounded, New World-adjacent |
| **KitBash3D fantasy/medieval kits (via Fab)** | ~$50–200+ / Cargo sub | Vendor | via import | via import | Low (heavy) | Active | **B (landmarks)** | Film-grade hero pieces |
| **Quixel Megascans (via Fab)** | Free w/ account (verify) | Vendor | via Bridge | Yes | Medium (needs care) | Active | **B (materials)** | Grounded materials/rocks/ruins |
| **KayKit — Medieval/Dungeon (Kay Lousberg)** | **Free (CC0)** + paid bundles | CC0 | Yes | Yes | **Excellent** | Active | **A** | Zero-risk stylized; ideal greybox |
| **Fantasy nature/dungeon (Quaternius)** | **Free (CC0)** | CC0 | Yes | Yes | Excellent | Active | **A** | Zero-risk greybox volume |
| **GAIA Pro + GeNa Pro (Procedural Worlds)** | ~$100+ / ~$60+ | ASE | Yes | Yes | Medium (tune) | Active | tool | Terrain + scatter for bounded arenas |

**Selected — Stack B path (recommended, matches Bible):** **Leartes environment megapacks** as the grounded base + **Quixel Megascans** (Fab) for rocks/ruins/materials, unified by our shared shader + grade.
- **Why over alternatives:** Synty (Stack A) is more cohesive and mobile-friendly but reads *cartoon*, which the Bible forbids; Leartes is the closest cohesive grounded ecosystem for the seven biomes. KitBash is grounded but too heavy for a whole mobile game (reserve for *landmarks* only).
- **Pros:** grounded New World materials; broad biome coverage. **Cons:** heavier (mobile budgets must be watched); cohesion needs the shared shader; verify URP + Unity 6 variants per pack.
- **Greybox first (both stacks):** **KayKit + Quaternius (CC0)** cost nothing and carry Phase 0/1 until the look is proven.

---

## 2. Characters (player base)

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Fit |
|---|---|---|---|---|---|---|---|
| **Character Creator 4 + AccuRIG (Reallusion)** | ~$199 (Pipeline higher); content extra | Vendor (chars usable commercially — verify content packs) | via Auto Setup | Auto Setup for URP | Medium (LOD/optimize) | Active | **Stack B** — grounded modular base, one rig |
| **POLYGON Modular Fantasy Hero (Synty)** | ~$60–90 | ASE | Yes | Yes | Excellent | Active | **Stack A** — cohesive w/ Synty env |
| **UMA 2 (Unity, community)** | **Free** | ASE/open | Yes (verify) | Yes | Medium | Community | Runtime modular/dye framework (needs art) |
| **Infinity PBR / Dungeon Mason modular characters** | ~$40–120 | ASE | Likely (verify) | verify | Medium | Varies | Alt grounded characters |

**Selected:** **Character Creator 4** (Stack B) — the class-neutral base + modular sockets + dye, one shared rig. If Stack A is chosen instead, switch to **Synty Modular Fantasy Hero** for cohesion with Synty environments.
- **Why over alternatives:** the player must carry one reusable rig through every future class (decision 5). CC4 delivers that grounded and modular faster than hand-modelling; Synty's hero only fits if the whole game goes Stack A. UMA is the free fallback for runtime modularity if CC4's pipeline proves heavy.
- **Pros:** true modularity, dye, one rig, grounded. **Cons:** learning curve; verify content-pack redistribution licence; must be pulled toward "stylized" by the shared shader (avoid photoreal skin).

---

## 3. Enemies ("The Hollowed" faction)

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Fit |
|---|---|---|---|---|---|---|---|
| **Undead / Fantasy monster packs (PROTOFACTOR)** | ~$30–90 ea | ASE | Likely (verify) | verify | Medium | Active | Grounded creatures — reference/base |
| **POLYGON Dungeons/undead enemies (Synty)** | in POLYGON packs | ASE | Yes | Yes | Excellent | Active | Stack A cohesive enemies |
| **Dungeon Mason / Infinity PBR creatures** | ~$40–120 | ASE | verify | verify | Medium | Varies | Alt grounded creatures |
| **AI-generated + cleanup (Meshy/Tripo/TRELLIS)** | subscription/free-local | Vendor (paid-tier commercial — verify) | N-A (produces FBX) | N-A | depends on cleanup | N-A | **Recommended** for the four-role variants |

**Selected — hybrid, AI-seeded (per Discovery §2):** build the **shared blight base + skeleton on the Thrall** (end-to-end template), then produce **Stalker/Brute/Corruptor as AI-generated-then-cleaned variants** on that one skeleton, tinted by the shader. Use **PROTOFACTOR** (Stack B) or **Synty** (Stack A) as a *reference/base* only if a single publisher happens to nail all four role silhouettes cohesively.
- **Why over alternatives:** the four roles need specific role-telegraphing silhouettes on **one shared rig** — no pack ships exactly that. AI erases the volume problem; the shared skeleton keeps animation/VFX unified.
- **Pros:** cohesion via one rig + shader. **Cons:** AI style-drift risk (mitigate with one locked concept sheet + the Acceptance Gate); cleanup time.

---

## 4. Weapons

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Stack |
|---|---|---|---|---|---|---|---|
| **POLYGON weapon sets (Synty)** | in/with POLYGON | ASE | Yes | Yes | Excellent | Active | A |
| **KayKit weapons (Kay Lousberg)** | **Free (CC0)** / bundles | CC0 | Yes | Yes | Excellent | Active | A |
| **Fantasy/RPG weapon packs (Dungeon Mason, PolygonMaker, etc.)** | ~$15–60 | ASE | verify | verify | Medium | Varies | B-ish |
| **Megascans/AI hero weapons** | free/sub | Vendor | via import | verify | verify | Active | B |

**Selected:** the weapon set from **whichever stack is chosen** (Synty weapons for Stack A; a grounded RPG-weapons pack or CC4-compatible weapons for Stack B), so weights/skinning and style match the character. **KayKit (CC0)** covers greybox weapons at zero risk.
- **Why over alternatives:** weapons must socket onto the shared rig and match character style; cross-stack weapons break cohesion. **Pros:** cheap, plentiful. **Cons:** verify rig/attachment compatibility.

---

## 5. Armor (modular)

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Stack |
|---|---|---|---|---|---|---|---|
| **CC4 modular outfits / Fantasy clothing + SkinGen (Reallusion)** | ~$0–100+ per pack | Vendor | via Auto Setup | Yes | Medium | Active | **B** |
| **POLYGON Modular Fantasy Hero armor modules (Synty)** | ~$60–90 | ASE | Yes | Yes | Excellent | Active | **A** |
| **Modular armor packs (Infinity PBR, etc.)** | ~$40–120 | ASE | verify | verify | Medium | Varies | B |

**Selected:** armor on the **same base as the player** — **CC4 modular outfits** (Stack B) or **Synty modular armor** (Stack A). Non-negotiable: armor must socket onto the one shared rig and support **dye** (Bible §2).
- **Why over alternatives:** modularity + dye + one rig is the whole point; standalone armor packs that don't fit the base rig create rework. **Pros:** true modular sockets. **Cons:** CC4 content licensing to verify; keep top-down silhouette clean.

---

## 6. Props

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Stack |
|---|---|---|---|---|---|---|---|
| **Same environment ecosystem (Synty **or** Leartes)** | included | ASE | per pack | per pack | per stack | Active | A/B |
| **Poly Haven (props, materials, HDRIs)** | **Free (CC0)** | CC0 | via import | Yes | Medium | Active | B |
| **KayKit / Kenney props** | **Free (CC0)** | CC0 | Yes | Yes | Excellent | Active | A |
| **Megascans props/debris (Fab)** | Free w/ acct (verify) | Vendor | via Bridge | Yes | Medium | Active | B |

**Selected:** props from the **chosen environment ecosystem** for cohesion + **CC0 (Poly Haven / KayKit)** for fills and greybox. **Gameplay-critical props** (currency, module, extraction point, boundary landmarks) stay **studio-controlled** for instant readability (Bible §4).
- **Why over alternatives:** buying props from a *different* publisher than the environment is the #1 cohesion killer. **Pros:** free CC0 volume. **Cons:** gameplay props must not be off-the-shelf.

---

## 7. Vegetation

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Notes |
|---|---|---|---|---|---|---|---|
| **The Vegetation Engine (BOXOPHOBIC)** | ~$40–50 | ASE | Yes | Yes (URP/HDRP/BiRP) | Yes (scalable) | Active | **Unifier + wind/interaction shader** — makes mixed foliage cohesive |
| **POLYGON Nature (Synty)** | ~$30–60 | ASE | Yes | Yes | Excellent | Active | Stack A foliage |
| **Nature Manufacture (Meadow / Dynamic Nature)** | ~$50–200 | ASE | verify | URP variants | Medium | Active | Stack B grounded foliage |
| **Quixel foliage (Fab)** | Free w/ acct | Vendor | via Bridge | Yes | Medium | Active | Stack B grounded |

**Selected:** **The Vegetation Engine (Boxophobic)** as the **unifier** (wind, interaction, one vegetation shader — huge cohesion + perf win across sources) + foliage from the chosen stack (Synty Nature for A; Nature Manufacture/Quixel for B). Keep foliage **at the perimeter**, low-contrast (Bible readability rule).
- **Why over alternatives:** The Vegetation Engine solves cohesion + wind + LOD across *any* foliage source — exactly the "unify centrally" doctrine. **Pros:** one shader for all plants, mobile-scalable. **Cons:** verify current URP/Unity-6 status; still need foliage art to feed it.

---

## 8. VFX

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Notes |
|---|---|---|---|---|---|---|---|
| **Custom — Unity VFX Graph / Shuriken** | free (engine) | — | Yes | Yes | Yes (LODs) | — | **Primary** — required by the semantic lock |
| **Hovl Studio (Magic / Explosions / Trails / Epic Toon FX)** | ~$25–60 ea | ASE | Yes | Yes (URP versions) | Medium | Active | Best **library/reference** for magic/impact |
| **KriptoFX — Realistic Effects Pack 4 / War FX** | ~$60–80 / ~free | ASE | Yes | Yes | Medium | Active | Grounded impacts/explosions |
| **Jean Moreno — Cartoon FX Remaster / War FX** | ~$40 / ~$20 | ASE | Yes | Yes | Excellent | Active | Stylized/cheap; selective |

**Selected:** **custom VFX Graph (primary)** + **one Hovl Studio pack as a technique/motion library** (retinted to reserved school hues, trimmed to the coverage budget, never shipped as-is).
- **Why over alternatives:** the semantic-colour lock, reserved school hues, coverage budget, and horde-degradation LODs (Bible §4/§5/§6) can't be met by an off-the-shelf pack; but a library accelerates learning + motion. **Pros:** full control + a reference library. **Cons:** custom VFX is skill/time (Claude-assisted).

---

## 9. UI

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Notes |
|---|---|---|---|---|---|---|---|
| **GUI PRO — Fantasy RPG (Layer Lab)** | ~$40–60 | ASE | Yes (textures/UGUI) | N-A (UI) | Yes | Active | Best fantasy UI kit — **scaffold/reference** |
| **Fantasy Wooden GUI (various)** | ~$0–30 | ASE | Yes | N-A | Yes | Varies | Cheap scaffold |
| **Custom UI Toolkit/uGUI + AI-drafted icons** | free + AI sub | — | Yes | N-A | Yes | — | **Recommended** for the journal identity |

**Selected:** **custom UI + custom icon set** (the "expedition journal" identity, Bible §7), with **Layer Lab GUI PRO Fantasy** bought **only as a layout scaffold/reference**, and AI (Scenario/Leonardo/Recraft) for icon drafts → one unification pass.
- **Why over alternatives:** a bought UI skin instantly breaks the bespoke identity; UI is small surface / high identity value. **Pros:** cohesion, uniqueness. **Cons:** more hands-on (Claude-assisted layout/logic).

---

## 10. Audio

| Product (Publisher) | ~Price | Licence | Use | Maintained | Notes |
|---|---|---|---|---|---|
| **Sonniss — GDC Game Audio Bundle** | **Free** | Royalty-free (keep PDF) | SFX | Annual | Huge pro SFX base, clearly licensed |
| **RPG Essentials Sound Effects (Cafofo)** | ~$15–30 | ASE | SFX | Active | Cohesive game SFX set |
| **Michael Ghelfi Studios — fantasy music & ambience** | varies | Vendor/ASE (verify game-use) | Music/ambience | Active | Excellent dark-fantasy score/ambience |
| **Epidemic Sound / Artlist (subscription)** | ~$10–25/mo | Subscription sync | Music/ambience | Active | **Verify perpetual game-distribution terms** |
| **BOOM Library** | ~$$$ | Vendor | SFX/ambience | Active | Premium shaping libraries |

**Selected:** **Sonniss (free) + Cafofo RPG SFX** for sound design; **Michael Ghelfi or a commissioned adaptive score** for music/ambience; deliberately-designed enemy/skill **audio telegraphs** (Bible §8). Verify subscription music's **shipped-game** licence (known gotcha).
- **Why over alternatives:** Sonniss/Cafofo are cheap and clearly licensed; a cohesive adaptive dark-fantasy score wants Ghelfi or a composer, not random tracks. **Pros:** cheap, on-genre. **Cons:** subscription-music perpetuity must be verified.

---

## 11. Animation

| Product (Publisher) | ~Price | Licence | U6 | URP | Mobile | Maintained | Notes |
|---|---|---|---|---|---|---|---|
| **Mixamo (Adobe)** | **Free** | Free commercial (verify) | via FBX | N-A | Yes | Active | Auto-rig + base locomotion (retarget source) |
| **Movement / Sword & Shield / Bow Animset Pro (Kubold)** | ~$40 ea | ASE | Yes (Mecanim) | N-A | Yes | Active | **Gold-standard game-ready combat animsets** |
| **ActorCore mocap library (Reallusion)** | per-pack | Vendor | via export | N-A | Yes | Active | High-quality mocap, CC-compatible |
| **RPG Character Mecanim packs (various)** | ~$20–40 | ASE | verify | N-A | Yes | Varies | Cheaper alt animsets |
| **Rokoko / Cascadeur (AI-assisted)** | sub / free-indie | Vendor | via FBX | N-A | Yes | Active | Signature telegraph anims |

**Selected:** **Mixamo (free) + Kubold animsets** retargeted to the shared rig for locomotion/combat + **custom/Cascadeur** for signature telegraphs (Brute wind-up).
- **Why over alternatives:** Kubold is the proven, cohesive, game-ready standard; Mixamo is free and fills the base; only the gameplay-critical telegraphs need custom. **Pros:** fast, high-quality, cheap. **Cons:** retarget cleanup; verify rig compatibility with the chosen character base.

---

## 12. Ecosystem Compatibility Matrix

**Rule: pick ONE stack and stay in it. Cross-stack combinations (grey cells) break cohesion.**

| Component | **Stack A — Stylized/Cohesive** | **Stack B — Grounded/Semi-real (Bible-preferred)** | Cross-stack? |
|---|---|---|---|
| **Environment** | Synty POLYGON | Leartes + Megascans | ❌ never mix Synty + Leartes |
| **Props** | Synty / KayKit (CC0) | Leartes / Poly Haven / Megascans (CC0/Fab) | ❌ |
| **Vegetation** | Synty Nature **+ The Vegetation Engine** | Nature Manufacture/Quixel **+ The Vegetation Engine** | ✅ *Vegetation Engine works in both* |
| **Player** | Synty Modular Fantasy Hero | **Character Creator 4** | ❌ don't put CC4 realism in a Synty world |
| **Armor/Weapons** | Synty modular + Synty weapons | CC4 outfits + grounded weapon pack | ❌ |
| **Enemies** | Synty enemies / AI-on-Synty-base | **AI-seeded on shared rig** / PROTOFACTOR | ⚠️ AI works in both if styled to match |
| **Animation** | Mixamo + Kubold | Mixamo + Kubold + ActorCore | ✅ *rig-agnostic; works in both* |
| **VFX** | Custom VFX Graph + Hovl | Custom VFX Graph + Hovl/KriptoFX | ✅ *custom = stack-agnostic* |
| **UI** | Custom (Layer Lab scaffold) | Custom (Layer Lab scaffold) | ✅ *custom = stack-agnostic* |
| **Audio** | Sonniss + Cafofo + Ghelfi | same | ✅ *audio has no stack* |
| **Mobile fit** | **Excellent** | **Medium** (watch budgets) | — |
| **Bible fit ("stylized, not cartoon, grounded")** | **Risk: reads cartoon** | **Closest to target** | — |

**Stack-agnostic buys (safe regardless of the fork):** The Vegetation Engine · Mixamo + Kubold animation · custom VFX/UI · Sonniss/Cafofo/Ghelfi audio · CC0 greybox (KayKit/Quaternius/Poly Haven). **Buy these early; defer the stack-locked environment/character purchase until the single-arena test.**

---

## 13. Production Cost Analysis — time saved per purchase

> Indicative dev-time **saved vs building the equivalent custom**, for a solo dev. Hours are order-of-magnitude planning figures, not quotes.

| Recommended purchase | ~Cost | ~Time saved | Notes |
|---|---|---|---|
| **Environment ecosystem** (Leartes **or** Synty) | ~$100–400 | **~8–16 weeks** | 7 biomes of modelling/texturing is the single biggest time sink; a pack collapses it to assembly + shader-unify |
| **Character Creator 4** (player base) | ~$199+ | **~3–6 weeks** | A riggable, modular, dye-able grounded base + auto-rig you'd otherwise model/rig by hand |
| **The Vegetation Engine** | ~$40–50 | **~2–4 weeks** | Wind/interaction/LOD shader + cross-source cohesion you would not want to author |
| **Kubold animsets** | ~$40–120 | **~3–5 weeks** | Pro game-ready combat locomotion vs hand-keying |
| **Mixamo** | Free | **~1–2 weeks** | Auto-rig + base locomotion |
| **Hovl VFX library** | ~$25–60 | **~1–2 weeks** | Technique/motion reference that accelerates custom VFX |
| **Layer Lab GUI scaffold** | ~$40–60 | **~3–5 days** | Layout/reference for the custom kit |
| **Sonniss (SFX)** | Free | **~1–2 weeks** | A pro SFX base vs sourcing/recording |
| **CC0 greybox (KayKit/Quaternius/Poly Haven)** | Free | **~2–4 weeks** | Free Phase-0/1 blockout so no spend precedes the proof |
| **AI subscriptions (Meshy/Tripo + Scenario/MJ)** | ~$20–60/mo | **~4–8 weeks** across enemies/props/icons | Erases the *volume* problem; cleanup time is the cost |

**Headline:** the environment ecosystem + character base + The Vegetation Engine + Kubold + Mixamo together plausibly save **~5–8 months** of solo production versus custom — the core argument for buying at all. The spend is small relative to the time; the risk is **cohesion**, which the stack discipline + shared shader control.

---

## 14. Final recommended shopping list

> **Ordered by when to buy. Nothing here is a purchase authorization** — each still needs a live-store price/licence/compat check and Game-Director approval (frozen policy).

### Buy-now (stack-agnostic, low risk) — enables Phase 0/greybox
| Priority | Item | ~Cost | Why now |
|---|---|---|---|
| 1 | **CC0 greybox: KayKit + Quaternius + Poly Haven** | **$0** | Phase 0/1 with zero spend/risk |
| 2 | **Mixamo** | **$0** | Rig + base locomotion immediately |
| 3 | **Sonniss GDC bundle** | **$0** | SFX base |
| 4 | **The Vegetation Engine (Boxophobic)** | ~$40–50 | Works in either stack; a cohesion/perf force-multiplier |
| 5 | **AI subs to trial:** Meshy **or** Tripo (3D) + Midjourney **or** Scenario (2D) | ~$20–60/mo | Volume multiplier; trial before committing |

### Buy-after-the-single-arena-test (stack-locked — pick A or B first)
| Item (Stack B, Bible-preferred) | ~Cost | Item (Stack A alt) | ~Cost |
|---|---|---|---|
| **Leartes environment megapack(s)** | ~$100–400 | **Synty POLYGON Fantasy Kingdom/Dungeons/Nature** | ~$90–250 |
| **Quixel Megascans (Fab)** materials/props | free (verify) | KayKit paid bundles | ~$0–60 |
| **Character Creator 4** (+ a fantasy outfit pack) | ~$199+ | **Synty Modular Fantasy Hero** | ~$60–90 |
| **Kubold animsets** (Movement + Sword&Shield) | ~$80 | Kubold animsets | ~$80 |
| **Hovl Studio** VFX library | ~$25–60 | Hovl Studio VFX library | ~$25–60 |
| **Layer Lab GUI PRO Fantasy** (scaffold) | ~$40–60 | Layer Lab GUI PRO Fantasy | ~$40–60 |
| **Cafofo RPG SFX** + music (Ghelfi/commissioned) | ~$15–300+ | same | ~$15–300+ |

### Approximate totals (verify all)
- **Minimal (mostly free + Stack A CC0/Synty-lite):** **~$150–500**
- **Recommended (Stack B, Bible-preferred):** **~$700–1,800** one-time + ~$40–120/mo AI/audio during production
- **High-quality (Stack B + Megascans landmarks + commissioned score + selective outsourcing):** **~$3,000–7,000+**

### The one recommendation
**Buy the buy-now list today (mostly free), run the paid single-arena test on The Shattered Coast to decide Stack A vs Stack B, then commit to exactly one stack's environment + character purchase.** This spends almost nothing before the look is proven, keeps every stack-agnostic force-multiplier (Vegetation Engine, Kubold, custom VFX/UI, Sonniss) safe to buy early, and prevents the single biggest failure mode — an incohesive mix of stylized and grounded assets.

---

*Document status: research/validation only. Nothing purchased, nothing committed, Unity untouched, Phase 0 not started. All prices, dates, and compatibility claims are indicative and require live verification per the frozen purchasing policy.*
