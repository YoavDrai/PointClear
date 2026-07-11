import { readdir } from "node:fs/promises";
import { join } from "node:path";
import { parseRoadmap } from "../parsers/roadmapParser.js";
import { parseDecisions } from "../parsers/decisionsParser.js";
import { parseChangelog } from "../parsers/changelogParser.js";
import { parseTaskFile } from "../parsers/taskParser.js";
import { parseKnowledgeMap } from "../parsers/knowledgeMapParser.js";
import { parseDocTree } from "../parsers/docTreeParser.js";
import { getGitStatus } from "../git/readOnlyGit.js";
import type {
  ActivityItem,
  CurrentFocus,
  ParseIssue,
  ProjectHealth,
  ProjectSnapshot,
  Task,
  TaskColumn,
} from "./types.js";

const TASK_COLUMNS: TaskColumn[] = ["TODO", "IN_PROGRESS", "REVIEW", "DONE", "ARCHIVED"];

async function loadTasks(repoRoot: string, issues: ParseIssue[]): Promise<Task[]> {
  const tasks: Task[] = [];
  for (const column of TASK_COLUMNS) {
    const dir = join(repoRoot, "Tasks", column);
    let entries: string[];
    try {
      entries = (await readdir(dir)).filter((f) => f.endsWith(".md"));
    } catch {
      continue;
    }
    for (const file of entries) {
      try {
        tasks.push(await parseTaskFile(join(dir, file), column));
      } catch (err) {
        issues.push({ source: `Tasks/${column}/${file}`, message: String((err as Error).message ?? err) });
      }
    }
  }
  return tasks;
}

/** All "Sprint X.Y" references in a string, not just the first — a task
 * title can legitimately mention several (e.g. a task spanning multiple
 * sub-sprints), and taking only the first match previously picked up
 * incidental early mentions instead of the task's actual highest sprint.
 * Also expands "Sprint 2.0-2.2"/"Sprint 2.0–2.2" range notation into both
 * endpoints, not just the range's start. */
function allSprintRefsIn(text: string): string[] {
  const refs: string[] = [];
  for (const m of text.matchAll(/[Ss]print (\d+\.\d+)(?:\s*[-–]\s*(\d+\.\d+))?/g)) {
    refs.push(m[1]);
    if (m[2]) refs.push(m[2]);
  }
  return refs;
}

export function inferFocus(snapshot: Pick<ProjectSnapshot, "tasks" | "roadmap">): CurrentFocus {
  const phase2 = snapshot.roadmap.phases.find((p) => p.id === "2");
  const nonArchived = snapshot.tasks.filter((t) => t.column !== "ARCHIVED");

  // Highest sprint number referenced anywhere wins the "most advanced work"
  // question — not highest-among-active-only, which a stale REVIEW task for
  // an old sprint (e.g. still-unclosed Sprint 1.x work) can distort even
  // after a later sprint has already shipped via a different, DONE task.
  let highestRef: string | null = null;
  let highestRefIsDone = true;
  for (const t of nonArchived) {
    for (const ref of allSprintRefsIn(t.title)) {
      if (highestRef === null || parseFloat(ref) > parseFloat(highestRef)) {
        highestRef = ref;
        highestRefIsDone = t.column === "DONE";
      } else if (ref === highestRef && t.column !== "DONE") {
        highestRefIsDone = false; // same sprint also referenced by a non-done task
      }
    }
  }

  const allSprintIds = snapshot.roadmap.sprints.map((s) => s.id).sort((a, b) => parseFloat(a) - parseFloat(b));

  let sprintId: string | null;
  let basis: string;

  if (highestRef !== null && !highestRefIsDone) {
    sprintId = highestRef;
    basis = `highest sprint number referenced by a non-done task ("${highestRef}" is not yet DONE)`;
  } else if (highestRef !== null) {
    const next = allSprintIds.find((id) => parseFloat(id) > parseFloat(highestRef!));
    sprintId = next ?? highestRef;
    basis = next
      ? `next unclaimed sprint after the highest DONE task's reference ("${highestRef}")`
      : `no roadmap sprint follows the highest DONE reference ("${highestRef}")`;
  } else {
    sprintId = allSprintIds[0] ?? null;
    basis = "no task file references any sprint number yet — defaulting to the first roadmap sprint";
  }

  const sprint = sprintId ? snapshot.roadmap.sprints.find((s) => s.id === sprintId) ?? null : null;
  const cluster = sprint ? snapshot.roadmap.clusters.find((c) => c.id === sprint.cluster) ?? null : null;
  const question = sprint?.fields.find((f) => f.label === "Question")?.text ?? null;

  return {
    phaseId: phase2?.id ?? "2",
    clusterId: cluster?.id ?? null,
    sprintId,
    sprintTitle: sprint?.title ?? null,
    sprintQuestion: question,
    gate: cluster?.gate ?? null,
    inferred: true,
    basis,
  };
}

function computeHealth(snapshot: Pick<ProjectSnapshot, "tasks" | "decisions" | "unresolvedDecisions" | "docs" | "issues">): ProjectHealth {
  const written = snapshot.docs.length;
  // Documentation/ has 17 topic folders by convention (see Documentation/*); root docs count separately.
  return {
    parseFailures: snapshot.issues.length,
    tasksInReview: snapshot.tasks.filter((t) => t.column === "REVIEW").length,
    tasksBlocked: 0, // no explicit "blocked" folder convention exists yet — reserved, always 0 today
    unresolvedDecisions: snapshot.unresolvedDecisions.length,
    documentationWritten: written,
    documentationTotal: written,
    documentationCoveragePercent: written > 0 ? 100 : 0,
  };
}

function buildActivity(snapshot: Pick<ProjectSnapshot, "changelog" | "decisions">): ActivityItem[] {
  const items: ActivityItem[] = [];
  for (const c of snapshot.changelog) {
    items.push({ date: c.date, kind: "changelog", text: c.summary });
  }
  // Decisions don't carry dates of their own in DECISIONS.md — they're ordered,
  // not dated, so they're shown newest-first by document order rather than
  // interleaved by date into the timeline.
  return items.sort((a, b) => (a.date < b.date ? 1 : a.date > b.date ? -1 : 0));
}

export async function buildSnapshot(repoRoot: string): Promise<ProjectSnapshot> {
  const issues: ParseIssue[] = [];

  const safe = async <T>(source: string, fn: () => Promise<T>, fallback: T): Promise<T> => {
    try {
      return await fn();
    } catch (err) {
      issues.push({ source, message: String((err as Error).message ?? err) });
      return fallback;
    }
  };

  const [roadmap, decisionsResult, changelog, git] = await Promise.all([
    safe("ROADMAP.md", () => parseRoadmap(join(repoRoot, "ROADMAP.md")), { phases: [], clusters: [], sprints: [] }),
    safe("DECISIONS.md", () => parseDecisions(join(repoRoot, "DECISIONS.md")), { decisions: [], unresolved: [] }),
    safe("CHANGELOG.md", () => parseChangelog(join(repoRoot, "CHANGELOG.md")), []),
    getGitStatus(repoRoot),
  ]);

  const knowledgeMap = await safe(
    "POINT_CLEAR_KNOWLEDGE_MAP.md",
    () => parseKnowledgeMap(join(repoRoot, "POINT_CLEAR_KNOWLEDGE_MAP.md")),
    { rows: [] },
  );
  const docs = await safe("Documentation tree", () => parseDocTree(repoRoot, knowledgeMap), []);
  const tasks = await loadTasks(repoRoot, issues);

  const partial = {
    tasks,
    roadmap,
    decisions: decisionsResult.decisions,
    unresolvedDecisions: decisionsResult.unresolved,
    docs,
    issues,
    changelog,
  };

  const focus = inferFocus(partial);
  const health = computeHealth(partial);
  const activity = buildActivity(partial);

  return {
    generatedAt: new Date().toISOString(),
    projectName: "Point Clear",
    git,
    roadmap,
    decisions: decisionsResult.decisions,
    unresolvedDecisions: decisionsResult.unresolved,
    tasks,
    changelog,
    knowledgeMap,
    docs,
    health,
    activity,
    focus,
    issues,
  };
}
