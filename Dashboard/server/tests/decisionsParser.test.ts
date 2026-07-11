import { describe, it, expect, afterEach } from "vitest";
import { parseDecisions } from "../parsers/decisionsParser.js";
import { withFixtureFile, cleanupFixture } from "./testUtil.js";

const FIXTURE = `# Fixture Decisions

## Accepted Decisions

### DEC-001 — Multiplayer-First
Point Clear is multiplayer-first.

### DEC-020 — Level-Up Grants Persistent Build Potential Only
Character Level and Experience are persistent within a Season.

**Important boundaries:** This does not specify exact Skill Point mechanics.

---

## Unresolved Decisions

**[UNRESOLVED]** — none of the items below have been decided.

- Networking solution
- Hosting model
`;

let fixturePath: string;

afterEach(async () => {
  if (fixturePath) await cleanupFixture(fixturePath);
});

describe("parseDecisions", () => {
  it("extracts decisions with boundaries and the unresolved list", async () => {
    fixturePath = await withFixtureFile("DECISIONS.md", FIXTURE);
    const { decisions, unresolved } = await parseDecisions(fixturePath);

    expect(decisions).toHaveLength(2);
    expect(decisions[0].id).toBe("DEC-001");
    expect(decisions[1].id).toBe("DEC-020");
    expect(decisions[1].title).toBe("Level-Up Grants Persistent Build Potential Only");
    expect(decisions[1].body).toContain("persistent within a Season");
    expect(decisions[1].boundaries).toContain("Skill Point mechanics");
    expect(decisions[0].boundaries).toBeNull();

    expect(unresolved).toEqual(["Networking solution", "Hosting model"]);
  });
});
