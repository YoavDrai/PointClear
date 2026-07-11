import { describe, it, expect } from "vitest";
import { inferFocus } from "../model/buildSnapshot.js";
import type { Roadmap, Task } from "../model/types.js";

function task(id: string, title: string, column: Task["column"]): Task {
  return {
    id,
    title,
    column,
    statusField: null,
    priority: null,
    owner: null,
    filePath: "",
    acceptanceCriteria: [],
    definitionOfDone: [],
    approved: false,
  };
}

const roadmap: Roadmap = {
  phases: [],
  clusters: [{ id: "A", title: "First Real Build", purpose: "", sprintIds: ["2.3"], gate: null }],
  sprints: [
    { id: "1.3", title: "Old", cluster: "A", fields: [] },
    { id: "2.2", title: "Older still", cluster: "A", fields: [] },
    { id: "2.3", title: "Active Skill System Validation", cluster: "A", fields: [{ label: "Question", text: "?" }] },
    { id: "2.4", title: "Next", cluster: "A", fields: [] },
  ],
};

describe("inferFocus", () => {
  it("regression: a stale REVIEW task naming an early sub-sprint must not win over a later DONE task", () => {
    const tasks: Task[] = [
      task(
        "PC-003",
        "Sprint 1 — First Playable Combat Prototype (includes Sprint 1.1 — Playability and Combat Feedback Fix, Sprint 1.2 — Combat Space & Enemy Behaviour)",
        "REVIEW",
      ),
      task("PC-004", "Sprint 2.0-2.2 Progression Foundation", "DONE"),
    ];

    const focus = inferFocus({ tasks, roadmap });

    // PC-004 (DONE) references the higher sprint (2.2); since it's done,
    // focus should advance to the next roadmap sprint after 2.2 — not fall
    // back to 1.1 just because PC-003 is still sitting in REVIEW.
    expect(focus.sprintId).toBe("2.3");
    expect(focus.sprintTitle).toBe("Active Skill System Validation");
  });

  it("prefers a non-done task's sprint reference when it is the highest", () => {
    const tasks: Task[] = [
      task("PC-004", "Sprint 2.0-2.2 Progression Foundation", "DONE"),
      task("PC-007", "Sprint 2.3 Active Skill System Validation", "IN_PROGRESS"),
    ];
    const focus = inferFocus({ tasks, roadmap });
    expect(focus.sprintId).toBe("2.3");
    expect(focus.basis).toContain("not yet DONE");
  });

  it("falls back to the first roadmap sprint when nothing references a sprint number", () => {
    const tasks: Task[] = [task("PC-999", "Unrelated tooling task", "TODO")];
    const focus = inferFocus({ tasks, roadmap });
    expect(focus.sprintId).toBe("1.3");
  });
});
