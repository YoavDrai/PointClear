# Point Clear — Live Asset Shortlist (The Shattered Coast benchmark)

> **Purpose:** resolve the one decision blocking the art pipeline — **which visual stack can deliver Point Clear's approved (Art Bible v1.0) identity** — by live-verifying only the assets needed to build **one representative "The Shattered Coast" benchmark scene**.
> **Verification date:** **2026-07-15** · sources = official Unity Asset Store / vendor product pages (URLs recorded per candidate).
> **Status:** research/validation only. **Nothing purchased. Nothing committed. Unity untouched. Phase 0 not started. No assets generated.**

### Verification method & honesty rules
- Every candidate was checked against its **live official product page** on 2026-07-15.
- Fields the page did **not** display are marked **`not confirmed`** — I do **not** infer compatibility from appearance or prior knowledge.
- **Tooling limitation (material):** the page-fetch reliably returned **price, version, update date, original Unity version, licence, file size**, but frequently did **not** expose the JS-rendered "technical details" tab (render pipeline, mobile, prefab counts, LODs, rig). Those are therefore recorded **`not confirmed on page — verify in the Asset Store "Technical details" tab / in-editor before purchase.`**
- A few fields are **`search-derived`** (from official-adjacent sources, not the product page itself) and flagged as such.
- Prices are the **displayed price on the date above** and change with sales.

### The blocking decision
| | **Stack A — Cohesive stylized** | **Stack B — Grounded stylized-natural / semi-real** |
|---|---|---|
| Character | Synty POLYGON ecosystem | Reallusion Character Creator 5 |
| Environment | Synty POLYGON packs | DEXSOFT / Leartes grounded packs |
| Art Bible fit | **Conflicts** — Synty's own keywords are "Cartoon" | **Matches** — grounded, New World-adjacent |
| Cost / cohesion / mobile | Cheaper via subscription · very cohesive · mobile-proven | Cheaper via perpetual · cohesion via our shader · mobile risk |

---

## 1. Stack A — Cohesive stylized · complete test-scene package

### 1.1 Environment base
**POLYGON — Fantasy Kingdom Pack — Art by Synty (Synty Studios)**
- **URL:** https://assetstore.unity.com/packages/3d/environments/fantasy/polygon-fantasy-kingdom-pack-art-by-synty-164532
- **Price:** **$349.99 USD** (verified 2026-07-15) · **Version:** 1.12.4 · **Updated:** 2026-05-27 · **File:** 265.6 MB
- **Licence:** Restricted Single Entity — Extension Asset (Unity Asset Store EULA). Commercial use OK for a solo/small business. **Verify** Extension-Asset terms.
- **Original Unity version:** 2022.3.56. **Unity 6:** *strong evidence* — Synty published an official "Fantasy Kingdom Demo in Unity 6" (syntystore.com/blogs/blog/fantasy-kingdom-demo-in-unity-6; unity.com/demos/fantasy-kingdom). **Confirm the installed package's Unity-6 support in-page.**
- **Render pipeline / URP / mobile / prefab count / LODs:** `not confirmed on page` — verify in Technical Details tab. (Synty historically ships URP + Built-in materials — verify.)
- **Content (search-derived):** ~2,100 prefabs, modular castle/house w/ interiors, 22 characters, weapons, props.
- **Fit:** cohesive, mobile-proven — **but flat/cartoon**, against the Bible's "stylized, not cartoon."

*Alternate (zero-cost greybox):* **KayKit — Dungeon Remastered / Medieval Hexagon (Kay Lousberg, itch.io)** — https://kaylousberg.itch.io/kaykit-dungeon-remastered — **CC0** (free, commercial, no attribution; don't resell unmodified) — verified 2026-07-15. Ideal free greybox for either stack.

### 1.2 Modular player base
**POLYGON — Modular Fantasy Hero Characters Pack — Art by Synty (Synty Studios)**
- **URL:** https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/polygon-modular-fantasy-hero-characters-pack-art-by-synty-143468
- **Price:** **$149.99 USD** · **Version:** 1.4.0 · **Updated:** 2026-05-27 · **File:** 156.8 MB
- **Licence:** Restricted Single Entity — Extension Asset.
- **Original Unity:** 2022.3.56. **Unity 6 / URP / rig type / mobile:** `not confirmed on page` — verify.
- **Content (search-derived):** ~720 modular pieces, custom recolour shader, demo scene + random character generator, ~120 premade characters; "URP + Built-in, Unity 2022.3+" per publisher text (**not confirmed on the page fetch — verify**).
- **Fit:** true modular sockets + recolour (dye) — matches decision 5 mechanically; **flat/cartoon** look against the Bible.

### 1.3 First enemy-family base
**POLYGON — Dungeon Realms Pack — Art by Synty (Synty Studios)** *(contains undead — fits "The Hollowed")*
- **URL:** https://assetstore.unity.com/packages/3d/environments/dungeons/polygon-dungeon-realms-pack-art-by-synty-189093
- **Price:** **$199.99 USD** · **Version:** 1.4.4 · **Updated:** 2026-05-28 · **File:** 177.9 MB
- **Licence:** Restricted Single Entity — Extension Asset. **Original Unity:** 2022.3.56+.
- **Render pipeline / mobile / rig / prefab count:** `not confirmed on page` — verify.
- **Content (search-derived):** ~1,118 prefabs incl. Demon Skeletons + Undead Knight.
- **Fit:** cohesive with the Synty stack; supplies undead silhouettes for the four roles.

*Stack-agnostic alternative:* **AI-seeded on a shared rig** (Meshy/Tripo/TRELLIS → cleanup → gate) — no product page to verify; commercial terms per tool/tier.

### 1.4 Core locomotion/attack animation *(stack-agnostic — see §3.4)*

### 1.5 Vegetation/terrain support *(stack-agnostic — see §3.5)*

---

## 2. Stack B — Grounded stylized-natural / semi-real · complete test-scene package

### 2.1 Environment base
**Medieval Castle (URP) (DEXSOFT)** — *closest Unity-6-verified grounded fortress/courtyard*
- **URL:** https://assetstore.unity.com/packages/3d/environments/historic/medieval-castle-urp-308772
- **Price:** **$39.99 USD** · **Version:** 1.1 · **Updated:** 2025-11-24 · **File:** 5.8 GB
- **Licence:** Single Entity (Unity Asset Store EULA).
- **Supported Unity:** **6000.1.1f1 and 2023.2.18f1 — Unity 6 CONFIRMED on page.**
- **Render pipeline:** **URP only — CONFIRMED** ("not compatible" with Built-in/HDRP).
- **Mobile / prefab count / LODs / texture sizes / demo scene:** `not confirmed on page` — verify. **5.8 GB file is a mobile red flag — profile before commit.**
- **Fit:** grounded medieval stone/castle — strong for a *ruined coastal fortress courtyard*; add coast/cliffs separately.

*Alternate grounded env:* **Medieval Castle / Village Environment (Leartes Studios)** — https://assetstore.unity.com/publishers/44386 (publisher) — **~€119.60** (**search-derived**, page not fetched); "HDRP + URP + Built-in, ~300 static meshes, PBR" (**search-derived — verify on page**). Broader biome coverage across Leartes' catalogue.

### 2.2 Modular player base
**Character Creator 5 — Perpetual (Reallusion)**
- **URL:** https://www.reallusion.com/plan-and-pricing/individual/perpetual/character-creator-5-3033
- **Price:** **$299 USD** perpetual (**CC5 Deluxe $499**) · **Released:** 2025-08 (CC4 is superseded — corrected from prior docs).
- **Licence:** **royalty-free perpetual commercial export** for games/apps/AR-VR (Reallusion Content License) — strong. **Verify** licence on any add-on content packs.
- **Unity export:** **free Auto Setup plugin** — one-click export that "translates shaders and skeletons" for Unity (per Reallusion). **Confirm current Auto Setup Unity-6/URP support in-page.**
- **Mobile:** `not confirmed` — CC output is high-fidelity; **must be LOD/texture-optimized for mobile.**
- **Fit:** grounded, modular, dye-able, one reusable rig — matches decision 5 and the Bible's grounded target.

### 2.3 First enemy-family base
**UNDEAD (PROTOFACTOR, Inc)** *(verify exact SKU/URL)*
- **Evidence:** protofactor.biz + Asset Store listing (search-derived). **Price ~$34.99–$38.49** (inconsistent across sources — **verify**) · **Version 2.1** · **Updated 2020-10-07** · Unity **2018.4.2+** · generic rig · ~319 MB.
- **⚠️ Age flag:** last updated **2020** — **Unity 6 / URP compatibility `not confirmed`; test import before purchase.**
- **Licence:** Asset Store EULA (verify).
- **Fit:** grounded undead creatures; older, may need reshading/retarget.

*Stack-agnostic alternative:* **AI-seeded on a shared rig** (same as §1.3) — recommended primary for the Hollowed per the Discovery doc; product packs are a fallback/reference.

### 2.4 Core locomotion/attack animation *(stack-agnostic — see §3.4)*

### 2.5 Vegetation/terrain support *(stack-agnostic — see §3.5)*

---

## 3. Stack-agnostic components (identical for both packages)

### 3.4 Core locomotion/attack animation
**Movement Animset Pro (Kubold)** — https://assetstore.unity.com/packages/3d/animations/movement-animset-pro-14047
- **Price:** **$65.00 USD** (**search-derived** — page not fetched; verify) · 180+ mocap locomotion/climb/jump/punch · Unity 2020+ (Asset Store minimum) · humanoid mocap for **retarget**. Companion sets exist (Fighting/Sword&Shield/Bow Animset Pro).
- **Licence:** Asset Store EULA. **Unity 6 / rig details:** `not confirmed` — verify.

**Mixamo (Adobe)** — https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html
- **Price:** **Free.** **Licence (verified 2026-07-15):** commercial use permitted, no attribution required, **cannot redistribute animations as standalone assets** (must be in a project). Terms tied to Adobe CC and historically **ambiguous** — flag for legal comfort on a shipped title.

*Both are rig-agnostic (retarget to the chosen character rig).*

### 3.5 Vegetation / terrain support
**The Visual Engine (BOXOPHOBIC)** — https://assetstore.unity.com/packages/tools/utilities/the-visual-engine-286827
- **Price:** **$90 USD** · **Version:** 21.8.0 · **Updated:** 2026-07-04 · **File:** 390.2 MB
- **Supported Unity:** **2022.3.0 and later.** **Render pipeline:** **Built-in + URP + HDRP — CONFIRMED on page.**
- **Licence:** Extension Asset (Asset Store EULA). **Mobile:** `not confirmed` — verify.
- **⚠️ Correction:** this **replaces "The Vegetation Engine," which is now DEPRECATED/unavailable for new purchase** (verified 2026-07-15). The prior docs' recommendation was stale — corrected here.
- **Foliage art still required** (Synty Nature for Stack A; Quixel/CC0/Nature-Manufacture for Stack B) — The Visual Engine is the **unifier/shader**, not the plants.

---

## 4. Total current cost per package (verified prices; verify all before purchase)

| Component | **Stack A (perpetual)** | **Stack A (Synty subscription)** | **Stack B** |
|---|---|---|---|
| Environment | Fantasy Kingdom **$349.99** | Synty sub **~$30/mo** (whole library) | DEXSOFT Castle **$39.99** (or Leartes ~$130) |
| Enemy source | Dungeon Realms **$199.99** | *(in Synty sub)* | PROTOFACTOR **~$35** (or AI-seeded) |
| Player base | Modular Hero **$149.99** | *(in Synty sub)* | Reallusion CC5 **$299** |
| Animation | Kubold **$65** + Mixamo free | Kubold **$65** + Mixamo free | Kubold **$65** + Mixamo free |
| Vegetation tool | Visual Engine **$90** | Visual Engine **$90** | Visual Engine **$90** |
| **Benchmark total** | **≈ $854.97** | **≈ $185 (month 1)** | **≈ $528.97** (DEXSOFT env) |

**Non-obvious finding:** with individual perpetual packs, **Stack A ($855) is the *most expensive*, and Stack B ($529) is cheaper** — because DEXSOFT's env is $39.99 and CC5 is $299, versus Synty's three packs totalling ~$700. Stack A only becomes cheapest via the **$30/mo subscription** (which does not grant perpetual ownership — a licensing trade-off).

---

## 5. Technical & licensing risks

| Risk | Stack A | Stack B |
|---|---|---|
| **Unverifiable page fields** | Render pipeline / mobile / rig **not confirmed on page** for all three Synty packs | Env URP+U6 **confirmed**; CC5 export confirmed; **PROTOFACTOR (2020) U6 not confirmed** |
| **Licensing** | Restricted Single Entity / Extension Asset (fine solo); **subscription ≠ ownership** if that route is taken | CC5 **royalty-free perpetual** (strong); DEXSOFT/Leartes Single Entity (fine); Mixamo redistribution + Adobe-terms ambiguity |
| **Deprecation** | — | Vegetation Engine deprecated → use **The Visual Engine** (handled) |
| **File weight** | Light (Synty low-poly) | **DEXSOFT env 5.8 GB** — build size + streaming concern |
| **Cohesion** | **Native** (one publisher) | **Assembled** — env + CC5 + enemy from 3 sources; relies on our shared shader to cohere |

---

## 6. Expected cleanup & integration work

**Stack A:** import + place (fast). **But to satisfy the Bible's "not cartoon,"** the flat Synty look must be pulled toward grounded via our shared shader/grade — that reshading **fights the pack's identity** (real effort + risk). Retarget Mixamo/Kubold to the Synty rig. *Low effort if we accept a stylized look; higher effort to make it grounded.*

**Stack B:** DEXSOFT env is URP/Unity-6-ready (least import work); **CC5 → Auto Setup → then LOD/texture-optimize for mobile** (the main labour); enemy = AI-seeded on shared rig (or retarget/reshade PROTOFACTOR, verifying Unity-6 import); unify env+player+enemy via the shared shader + grade; foliage via The Visual Engine. *Moderate effort concentrated on mobile optimization + cross-source unification.*

---

## 7. Mobile-performance risk

- **Stack A: LOW.** Synty low-poly is mobile-proven; the main watch-item is draw calls at horde scale (batching/instancing).
- **Stack B: MODERATE–HIGH.** A **5.8 GB grounded PBR environment + CC5-fidelity characters** must be proven to hold **30 FPS on the minimum device** (Art Bible target). **This is precisely the question the paid benchmark exists to answer** — texture budgets, LODs, and the mobile visual tier will decide it.

---

## 8. Art Bible fit

- **Stack A: LOW.** Synty's own store keywords include **"Cartoon."** The Bible explicitly requires **"stylized, *not cartoon*, grounded New World materials."** Cohesion cannot fix an identity conflict; adopting Stack A as-is would effectively **revisit a frozen decision** (a GD call, not a sourcing one).
- **Stack B: HIGH.** Grounded semi-real (DEXSOFT stone/castle, CC5 grounded characters) is the closest available match to the frozen identity — its risk is *performance/cohesion*, not *identity*.

---

## 9. Weighted comparison score

| Criterion (weight) | Stack A (1–5) | Stack B (1–5) |
|---|---|---|
| **Art Bible fit — grounded, not cartoon (25%)** | 2 | 5 |
| **Cohesion (15%)** | 5 | 3 |
| **Unity 6 / URP confirmed (15%)** | 3 | 4 |
| **Mobile suitability (15%)** | 5 | 2 |
| **Cost (10%)** | 3 | 3 |
| **Licensing clarity (10%)** | 4 | 4 |
| **Integration ease (10%)** | 3 | 3 |
| **Weighted total** | **≈ 69 / 100** | **≈ 72 / 100** |

The score is **close** — and honestly so. Stack A wins **cohesion + mobile**; Stack B wins the **highest-weighted criterion, Art Bible fit**, which is the frozen identity and therefore the tie-breaker.

---

## 10. Recommendation — which stack enters the paid visual benchmark

**Recommend: Stack B (Grounded semi-real) enters the paid visual benchmark for The Shattered Coast.**

**Why:**
1. **It is the only stack that can satisfy the frozen Art Bible identity** ("stylized, not cartoon, grounded"). Stack A conflicts with that identity at the keyword level; choosing it would mean *revisiting a frozen GD decision*, which is out of scope for an asset-sourcing test.
2. **Its risk is measurable, and the benchmark exists to measure it** — the open question for Stack B is **mobile performance** (5.8 GB env, CC5 fidelity vs the 30 FPS target). That is exactly what a Shattered-Coast benchmark, profiled on PC + mobile, is designed to answer.
3. **It is also cheaper on perpetual licences** (~$529 vs ~$855) and carries the strongest licence (CC5 royalty-free perpetual).

**Benchmark package to buy *only on GD approval* (≈ $529, verify all prices/licences/compat first):** DEXSOFT *Medieval Castle (URP)* ($39.99) · Reallusion *Character Creator 5* ($299) · *Kubold Movement Animset Pro* ($65) + Mixamo (free) · *The Visual Engine* ($90) · enemy via **AI-seeded on a shared rig** (or PROTOFACTOR ~$35 after Unity-6 test). Greybox everything first with **KayKit CC0 (free)**.

**Explicit fallback (kept, not chosen):** if the benchmark proves Stack B **cannot** hold 30 FPS on the minimum device even after optimization, the decision escalates to a **GD conversation about the identity** — either a more-stylized direction (Stack A territory, cheap via Synty subscription) or a reduced mobile spec. **That is a deliberate identity decision, never a silent drift to cartoon.**

---

### Pre-purchase checklist (required by the frozen purchasing policy — still all gated on explicit GD approval)
- [ ] Verify each candidate's **render-pipeline + Unity-6 + mobile** in the Asset Store "Technical details" tab / in-editor (several are `not confirmed` here).
- [ ] Confirm **PROTOFACTOR UNDEAD** Unity-6 import (2020 asset) or commit to AI-seeded enemies.
- [ ] Confirm **CC5 Auto Setup** current Unity-6/URP support + any content-pack licences.
- [ ] Re-confirm **all displayed prices** (sales fluctuate) and the **Synty subscription-vs-perpetual** trade-off.
- [ ] Confirm **Mixamo** terms are acceptable for a shipped commercial title (or use Kubold/ActorCore only).
- [ ] Record every licence + invoice in the project licence register.

*Document status: live-verified 2026-07-15, research only. Nothing purchased, generated, committed, or integrated. Compatibility fields marked `not confirmed` were not confirmable from the official page and must be verified before any purchase.*
