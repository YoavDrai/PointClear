import type { Content, List, Root } from "mdast";
import { isHeading, labeledListFields, loadMarkdownTree, nodeText, strongOnlyText } from "./markdown.js";
import type { Cluster, DecisionGate, Phase, Roadmap, Sprint } from "../model/types.js";

const SPRINT_RE = /^Sprint (\d+\.\d+)\s*—\s*(.+)$/;
const GATE_RE = /^Cluster ([A-Z])\s+Decision Gate$/i;
const CLUSTER_HEADING_RE = /^Cluster ([A-Z])\s*—\s*(.+)$/;
const PHASE_HEADING_RE = /^Phase (\S+)\s*—\s*(.+)$/;

export async function parseRoadmap(absPath: string): Promise<Roadmap> {
  const tree = await loadMarkdownTree(absPath);
  const children = tree.children as Content[];

  const phases: Phase[] = [];
  const clusters: Cluster[] = [];
  const sprints: Sprint[] = [];

  let currentPhase: Phase | null = null;
  let currentCluster: Cluster | null = null;
  // Only the goal list immediately under a Phase (before any Cluster
  // sub-structure starts) belongs to that phase — Phase 2 nests Clusters
  // and Sprints, each with their own bullet lists, which must not be
  // absorbed into Phase.goals.
  let phaseHasEnteredClusters = false;

  for (let i = 0; i < children.length; i++) {
    const node = children[i];

    if (isHeading(node, 2)) {
      const text = nodeText(node);
      const m = text.match(PHASE_HEADING_RE);
      if (m) {
        currentPhase = { id: m[1], title: m[2], status: "", goals: [] };
        phases.push(currentPhase);
        phaseHasEnteredClusters = false;
        continue;
      }
    }

    if (isHeading(node, 3)) {
      const text = nodeText(node);
      const m = text.match(CLUSTER_HEADING_RE);
      if (m) {
        phaseHasEnteredClusters = true;
        currentCluster = { id: m[1], title: m[2], purpose: "", sprintIds: [], gate: null };
        clusters.push(currentCluster);
        // next paragraph, if it starts with "Exists to answer:", is the purpose
        const next = children[i + 1];
        if (next && next.type === "paragraph") {
          const t = nodeText(next);
          if (/^Exists to answer:/i.test(t)) {
            currentCluster.purpose = t.replace(/^Exists to answer:\s*/i, "");
          }
        }
        continue;
      }
    }

    if (currentPhase && node.type === "paragraph") {
      const t = nodeText(node);
      const statusMatch = t.match(/^Status:\s*(.+)$/);
      if (statusMatch && !currentPhase.status) {
        currentPhase.status = statusMatch[1];
      }
    }

    if (currentPhase && node.type === "list" && !phaseHasEnteredClusters) {
      // Phase-level goal bullets only — Phase 0/1's flat lists, or Phase 2's
      // now-superseded initial-target list if present. Never a Cluster's or
      // Sprint's own field list (guarded by phaseHasEnteredClusters).
      for (const li of (node as List).children) {
        currentPhase.goals.push(nodeText(li));
      }
    }

    const strong = strongOnlyText(node);
    if (strong) {
      const sprintMatch = strong.match(SPRINT_RE);
      if (sprintMatch && currentCluster) {
        const list = children[i + 1];
        const fields = list && list.type === "list" ? labeledListFields(list as List) : [];
        const sprint: Sprint = {
          id: sprintMatch[1],
          title: sprintMatch[2],
          cluster: currentCluster.id,
          fields,
        };
        sprints.push(sprint);
        currentCluster.sprintIds.push(sprint.id);
        continue;
      }

      const gateMatch = strong.match(GATE_RE);
      if (gateMatch && currentCluster) {
        const list = children[i + 1];
        const fields = list && list.type === "list" ? labeledListFields(list as List) : [];
        const byLabel = (label: string) => fields.find((f) => f.label.toLowerCase() === label.toLowerCase())?.text ?? "";
        const gate: DecisionGate = {
          cluster: gateMatch[1],
          question: byLabel("Question"),
          decision: byLabel("Decision"),
          extra: fields.find((f) => /^Relationship/i.test(f.label))?.text,
        };
        currentCluster.gate = gate;
        continue;
      }
    }
  }

  return { phases, clusters, sprints };
}
