import { describe, it, expect, afterEach } from "vitest";
import { parseTaskFile } from "../parsers/taskParser.js";
import { withFixtureFile, cleanupFixture } from "./testUtil.js";

const FIXTURE = `## Task ID
PC-999

## Title
Fixture Task For Parser Testing

## Status
REVIEW

## Priority
High

## Owner
Claude

## Acceptance Criteria
- [x] First criterion done
- [x] Second criterion done
- [ ] Third criterion not done

## Definition of Done Checklist
- [x] Acceptance criteria pass
- [ ] Yoav approved the result

## Game Director Approval
- [ ] Approved by Yoav
- Date:
- Notes:
`;

let fixturePath: string;

afterEach(async () => {
  if (fixturePath) await cleanupFixture(fixturePath);
});

describe("parseTaskFile", () => {
  it("extracts fields and checklists, and takes column from the caller, not the file", async () => {
    fixturePath = await withFixtureFile("PC-999_fixture.md", FIXTURE);
    const task = await parseTaskFile(fixturePath, "REVIEW");

    expect(task.id).toBe("PC-999");
    expect(task.title).toBe("Fixture Task For Parser Testing");
    expect(task.statusField).toBe("REVIEW");
    expect(task.column).toBe("REVIEW");
    expect(task.priority).toBe("High");

    expect(task.acceptanceCriteria).toHaveLength(3);
    expect(task.acceptanceCriteria.filter((i) => i.checked)).toHaveLength(2);

    expect(task.approved).toBe(false);
  });

  it("falls back to the filename for the task ID if the field is missing", async () => {
    fixturePath = await withFixtureFile("PC-888_no-id-field.md", "## Title\nNo ID field here\n");
    const task = await parseTaskFile(fixturePath, "TODO");
    expect(task.id).toBe("PC-888");
  });
});
