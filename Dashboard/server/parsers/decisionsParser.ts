import type { Content, List } from "mdast";
import { isHeading, loadMarkdownTree, nodeText } from "./markdown.js";
import type { Decision } from "../model/types.js";

const DEC_RE = /^(DEC-\d+)\s*—\s*(.+)$/;

export async function parseDecisions(absPath: string): Promise<{ decisions: Decision[]; unresolved: string[] }> {
  const tree = await loadMarkdownTree(absPath);
  const children = tree.children as Content[];

  const decisions: Decision[] = [];
  let unresolved: string[] = [];

  for (let i = 0; i < children.length; i++) {
    const node = children[i];

    if (isHeading(node, 3)) {
      const text = nodeText(node);
      const m = text.match(DEC_RE);
      if (m) {
        const bodyParts: string[] = [];
        let boundaries: string | null = null;
        for (let j = i + 1; j < children.length; j++) {
          const sib = children[j];
          if (sib.type === "heading" || sib.type === "thematicBreak") break;
          if (sib.type === "paragraph") {
            const t = nodeText(sib);
            if (/^Important boundaries:/i.test(t)) {
              boundaries = t.replace(/^Important boundaries:\s*/i, "");
            } else {
              bodyParts.push(t);
            }
          }
        }
        decisions.push({ id: m[1], title: m[2], body: bodyParts.join(" "), boundaries });
        continue;
      }
    }

    if (isHeading(node, 2) && nodeText(node) === "Unresolved Decisions") {
      for (let j = i + 1; j < children.length; j++) {
        const sib = children[j];
        if (sib.type === "heading" && (sib as any).depth <= 2) break;
        if (sib.type === "list") {
          unresolved = (sib as List).children.map((li) => nodeText(li));
          break;
        }
      }
    }
  }

  return { decisions, unresolved };
}
