import { readFile } from "node:fs/promises";
import { unified } from "unified";
import remarkParse from "remark-parse";
import remarkGfm from "remark-gfm";
import { toString as mdastToString } from "mdast-util-to-string";
import type { Root, Content, List, ListItem, Table } from "mdast";

const processor = unified().use(remarkParse).use(remarkGfm);

export async function loadMarkdownTree(absPath: string): Promise<Root> {
  const raw = await readFile(absPath, "utf8");
  return processor.parse(raw) as Root;
}

export function nodeText(node: Content | Root): string {
  return mdastToString(node).trim();
}

/** True if a paragraph node's entire content is a single bold ("strong") run. */
export function strongOnlyText(node: Content): string | null {
  if (node.type !== "paragraph") return null;
  const children = (node as any).children as Content[];
  if (children.length !== 1 || children[0].type !== "strong") return null;
  return nodeText(children[0]).trim();
}

/** Splits a "Label: rest of the sentence" list into labeled fields. */
export function labeledListFields(list: List): { label: string; text: string }[] {
  const out: { label: string; text: string }[] = [];
  for (const item of list.children as ListItem[]) {
    const full = nodeText(item);
    const m = full.match(/^([^:]{1,60}):\s*(.*)$/s);
    if (m) {
      out.push({ label: m[1].trim(), text: m[2].trim() });
    } else {
      out.push({ label: "Note", text: full });
    }
  }
  return out;
}

/** Plain checkbox items from a GFM task list. */
export function checklistItems(list: List): { text: string; checked: boolean }[] {
  return (list.children as ListItem[])
    .filter((li) => typeof li.checked === "boolean")
    .map((li) => ({ text: nodeText(li), checked: !!li.checked }));
}

export function tableRows(table: Table): string[][] {
  return table.children.map((row) => row.children.map((cell) => nodeText(cell as any)));
}

export function isHeading(node: Content, depth?: number): node is Content & { depth: number } {
  return node.type === "heading" && (depth === undefined || (node as any).depth === depth);
}
