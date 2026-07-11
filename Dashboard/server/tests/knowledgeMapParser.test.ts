import { describe, it, expect, afterEach } from "vitest";
import { parseKnowledgeMap } from "../parsers/knowledgeMapParser.js";
import { withFixtureFile, cleanupFixture } from "./testUtil.js";

const FIXTURE = `# Fixture Knowledge Map

## Knowledge Topic Registry

| Domain | Topic | Canonical Owner (current or intended) | Status |
|---|---|---|---|
| KD-01 | Permanent Design Principles | \`CORE_PHILOSOPHY.md\` | SOURCE OF TRUTH |
| KD-06 | Build Philosophy | \`CORE_PHILOSOPHY.md\` | SOURCE OF TRUTH |

## Derived / Generated Documents

- Not part of the registry table.
`;

let fixturePath: string;

afterEach(async () => {
  if (fixturePath) await cleanupFixture(fixturePath);
});

describe("parseKnowledgeMap", () => {
  it("extracts registry rows and skips the header row", async () => {
    fixturePath = await withFixtureFile("POINT_CLEAR_KNOWLEDGE_MAP.md", FIXTURE);
    const { rows } = await parseKnowledgeMap(fixturePath);

    expect(rows).toHaveLength(2);
    expect(rows[0].domain).toBe("KD-01");
    expect(rows[0].owner).toContain("CORE_PHILOSOPHY.md");
    expect(rows[1].topic).toBe("Build Philosophy");
  });

  it("returns an empty row list when the registry heading is absent", async () => {
    fixturePath = await withFixtureFile("EMPTY.md", "# No registry here\n");
    const { rows } = await parseKnowledgeMap(fixturePath);
    expect(rows).toHaveLength(0);
  });
});
