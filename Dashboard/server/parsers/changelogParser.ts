import type { Content, List } from "mdast";
import { isHeading, loadMarkdownTree, nodeText } from "./markdown.js";
import type { ChangelogEntry } from "../model/types.js";

const DATE_RE = /^(\d{4}-\d{2}-\d{2})\s*—\s*(.+)$/;

export async function parseChangelog(absPath: string): Promise<ChangelogEntry[]> {
  const tree = await loadMarkdownTree(absPath);
  const children = tree.children as Content[];
  const entries: ChangelogEntry[] = [];

  for (let i = 0; i < children.length; i++) {
    const node = children[i];
    if (!isHeading(node, 2)) continue;
    const text = nodeText(node);
    const m = text.match(DATE_RE);
    if (!m) continue;

    const bullets: string[] = [];
    for (let j = i + 1; j < children.length; j++) {
      const sib = children[j];
      if (sib.type === "heading") break;
      if (sib.type === "list") {
        bullets.push(...(sib as List).children.map((li) => nodeText(li)));
      }
    }
    entries.push({ date: m[1], summary: m[2], bullets });
  }

  // newest first (file is already written newest-first by convention, but don't assume it)
  entries.sort((a, b) => (a.date < b.date ? 1 : a.date > b.date ? -1 : 0));
  return entries;
}
