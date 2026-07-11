// Shared shape of everything the dashboard displays. This is a pure,
// derived view of the repository at one instant — nothing here is ever
// the only place a fact lives. Rebuilt from disk on every relevant file
// change, never hand-edited, never persisted as authoritative state.

export type ParseStatus = "ok" | "failed";

export interface ParseIssue {
  source: string;
  message: string;
}

export interface GitStatus {
  branch: string;
  headSha: string;
  headSubject: string;
  ahead: number;
  behind: number;
  modified: string[];
  untracked: string[];
  isClean: boolean;
}

export interface SprintField {
  label: string;
  text: string;
}

export interface Sprint {
  id: string; // "2.3"
  title: string;
  cluster: string; // "A" | "B" | "C"
  fields: SprintField[]; // Question, Dependencies, Why before next, Playable loop at end, Out of scope, Decision...
}

export interface DecisionGate {
  cluster: string;
  question: string;
  decision: string;
  extra?: string;
}

export interface Cluster {
  id: string; // "A"
  title: string;
  purpose: string;
  sprintIds: string[];
  gate: DecisionGate | null;
}

export interface Phase {
  id: string; // "0" | "1" | "2"
  title: string;
  status: string;
  goals: string[];
}

export interface Roadmap {
  phases: Phase[];
  clusters: Cluster[];
  sprints: Sprint[];
}

export interface Decision {
  id: string; // "DEC-020"
  title: string;
  body: string;
  boundaries: string | null;
}

export type TaskColumn = "TODO" | "IN_PROGRESS" | "REVIEW" | "DONE" | "ARCHIVED";

export interface TaskAcceptanceItem {
  text: string;
  checked: boolean;
}

export interface Task {
  id: string; // "PC-006"
  title: string;
  column: TaskColumn; // derived from folder — authoritative
  statusField: string | null; // the file's own "## Status" text — supplementary, may disagree with column
  priority: string | null;
  owner: string | null;
  filePath: string;
  acceptanceCriteria: TaskAcceptanceItem[];
  definitionOfDone: TaskAcceptanceItem[];
  approved: boolean;
}

export interface ChangelogEntry {
  date: string; // "2026-07-11"
  summary: string;
  bullets: string[];
}

export interface KnowledgeDomainRow {
  domain: string;
  topic: string;
  owner: string;
  status: string;
}

export interface KnowledgeMapData {
  rows: KnowledgeDomainRow[];
}

export interface DocEntry {
  path: string;
  title: string;
  group: string;
  ownership: string | null;
}

export interface ProjectHealth {
  parseFailures: number;
  tasksInReview: number;
  tasksBlocked: number;
  unresolvedDecisions: number;
  documentationCoveragePercent: number;
  documentationTotal: number;
  documentationWritten: number;
}

export interface ActivityItem {
  date: string;
  kind: "changelog" | "decision";
  text: string;
  refId?: string;
}

export interface CurrentFocus {
  phaseId: string;
  clusterId: string | null;
  sprintId: string | null;
  sprintTitle: string | null;
  sprintQuestion: string | null;
  gate: DecisionGate | null;
  inferred: boolean;
  basis: string;
}

export interface ProjectSnapshot {
  generatedAt: string;
  projectName: string;
  git: GitStatus;
  roadmap: Roadmap;
  decisions: Decision[];
  unresolvedDecisions: string[];
  tasks: Task[];
  changelog: ChangelogEntry[];
  knowledgeMap: KnowledgeMapData;
  docs: DocEntry[];
  health: ProjectHealth;
  activity: ActivityItem[];
  focus: CurrentFocus;
  issues: ParseIssue[];
}
