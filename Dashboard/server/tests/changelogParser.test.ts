import { describe, it, expect, afterEach } from "vitest";
import { parseChangelog } from "../parsers/changelogParser.js";
import { withFixtureFile, cleanupFixture } from "./testUtil.js";

const FIXTURE = `# Fixture Changelog

## 2026-07-11 — Restructured Phase 2 into three research clusters

- Rewrote ROADMAP.md
- Added Decision Gates

## 2026-07-10 — Established Point Clear project foundation

- Created repository documentation scaffolding
`;

let fixturePath: string;

afterEach(async () => {
  if (fixturePath) await cleanupFixture(fixturePath);
});

describe("parseChangelog", () => {
  it("extracts dated entries newest-first regardless of file order", async () => {
    fixturePath = await withFixtureFile("CHANGELOG.md", FIXTURE);
    const entries = await parseChangelog(fixturePath);

    expect(entries).toHaveLength(2);
    expect(entries[0].date).toBe("2026-07-11");
    expect(entries[0].summary).toContain("three research clusters");
    expect(entries[0].bullets).toContain("Added Decision Gates");
    expect(entries[1].date).toBe("2026-07-10");
  });
});
