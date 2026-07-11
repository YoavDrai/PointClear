import { describe, it, expect, afterEach } from "vitest";
import { parseRoadmap } from "../parsers/roadmapParser.js";
import { withFixtureFile, cleanupFixture } from "./testUtil.js";

const FIXTURE = `# Fixture Roadmap

## Phase 2 — Vertical Slice

**Status: In Progress** — assessed independently.

### Cluster A — First Real Build

Exists to answer: can Point Clear create meaningful Build Identity at all?

**Sprint 2.3 — Active Skill System Validation**
- Question: Can Point Clear support mechanically diverse Active Skills?
- Dependencies: PlayerXP, PlayerLevel.
- Why before 2.4: Skill Points need real skills to allocate into.
- Playable loop at end: two skills exist and feel distinct.
- Out of scope: Skill Points, selection screen.

**Sprint 2.4 — Persistent Skill Points & Allocation**
- Question: Does leveling grant a real build choice?
- Dependencies: Sprint 2.3.

**Cluster A Decision Gate**
- Question: Did we prove that Point Clear can create meaningful Build Identity?
- Decision: If yes, proceed to Cluster B. If no, return to Sprint 2.3 or 2.4.
`;

let fixturePath: string;

afterEach(async () => {
  if (fixturePath) await cleanupFixture(fixturePath);
});

describe("parseRoadmap", () => {
  it("extracts phases, clusters, sprints, and gates", async () => {
    fixturePath = await withFixtureFile("ROADMAP.md", FIXTURE);
    const roadmap = await parseRoadmap(fixturePath);

    expect(roadmap.phases).toHaveLength(1);
    expect(roadmap.phases[0].id).toBe("2");
    expect(roadmap.phases[0].status).toContain("In Progress");

    expect(roadmap.clusters).toHaveLength(1);
    expect(roadmap.clusters[0].id).toBe("A");
    expect(roadmap.clusters[0].purpose).toContain("Build Identity");

    expect(roadmap.sprints).toHaveLength(2);
    expect(roadmap.sprints[0].id).toBe("2.3");
    expect(roadmap.sprints[0].cluster).toBe("A");
    const question = roadmap.sprints[0].fields.find((f) => f.label === "Question");
    expect(question?.text).toContain("mechanically diverse Active Skills");

    expect(roadmap.clusters[0].gate).not.toBeNull();
    expect(roadmap.clusters[0].gate?.question).toContain("meaningful Build Identity");
    expect(roadmap.clusters[0].gate?.decision).toContain("proceed to Cluster B");
  });

  it("does not let a Cluster's or Sprint's bullet lists leak into Phase.goals", async () => {
    fixturePath = await withFixtureFile("ROADMAP.md", FIXTURE);
    const roadmap = await parseRoadmap(fixturePath);
    // The fixture's Phase 2 has no flat goal list of its own before Cluster A
    // starts, so goals must stay empty — not absorb the sprint/gate fields.
    expect(roadmap.phases[0].goals).toHaveLength(0);
  });

  it("returns empty structures for a file with no matching sections", async () => {
    fixturePath = await withFixtureFile("EMPTY.md", "# Nothing here\n\nJust prose.\n");
    const roadmap = await parseRoadmap(fixturePath);
    expect(roadmap.phases).toHaveLength(0);
    expect(roadmap.clusters).toHaveLength(0);
    expect(roadmap.sprints).toHaveLength(0);
  });
});
