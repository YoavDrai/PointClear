import type { Content, Table } from "mdast";
import { isHeading, loadMarkdownTree, nodeText, tableRows } from "./markdown.js";
import type { KnowledgeDomainRow, KnowledgeMapData } from "../model/types.js";

export async function parseKnowledgeMap(absPath: string): Promise<KnowledgeMapData> {
  const tree = await loadMarkdownTree(absPath);
  const children = tree.children as Content[];

  const idx = children.findIndex((n) => isHeading(n, 2) && nodeText(n) === "Knowledge Topic Registry");
  const rows: KnowledgeDomainRow[] = [];

  if (idx !== -1) {
    for (let j = idx + 1; j < children.length; j++) {
      const sib = children[j];
      if (sib.type === "heading") break;
      if (sib.type === "table") {
        const raw = tableRows(sib as Table);
        for (const r of raw.slice(1)) {
          // skip the header row
          if (r.length < 4) continue;
          rows.push({ domain: r[0], topic: r[1], owner: r[2], status: r[3] });
        }
        break;
      }
    }
  }

  return { rows };
}
