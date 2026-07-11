import { basename } from "node:path";
import type { Content, List } from "mdast";
import { checklistItems, isHeading, loadMarkdownTree, nodeText } from "./markdown.js";
import type { Task, TaskColumn } from "../model/types.js";

function fieldParagraph(children: Content[], headingText: string): string | null {
  const idx = children.findIndex((n) => isHeading(n, 2) && nodeText(n) === headingText);
  if (idx === -1) return null;
  for (let j = idx + 1; j < children.length; j++) {
    const sib = children[j];
    if (sib.type === "heading") return null;
    if (sib.type === "paragraph") {
      const t = nodeText(sib);
      return t.length ? t : null;
    }
  }
  return null;
}

function fieldChecklist(children: Content[], headingText: string) {
  const idx = children.findIndex((n) => isHeading(n, 2) && nodeText(n) === headingText);
  if (idx === -1) return [];
  for (let j = idx + 1; j < children.length; j++) {
    const sib = children[j];
    if (sib.type === "heading") return [];
    if (sib.type === "list") return checklistItems(sib as List);
  }
  return [];
}

export async function parseTaskFile(absPath: string, column: TaskColumn): Promise<Task> {
  const tree = await loadMarkdownTree(absPath);
  const children = tree.children as Content[];

  const idFromField = fieldParagraph(children, "Task ID");
  const idFromFilename = basename(absPath).match(/^(PC-\d+)/)?.[1] ?? "PC-???";
  const id = idFromField ?? idFromFilename;

  const title = fieldParagraph(children, "Title") ?? basename(absPath);
  const statusField = fieldParagraph(children, "Status");
  const priority = fieldParagraph(children, "Priority");
  const owner = fieldParagraph(children, "Owner");

  const acceptanceCriteria = fieldChecklist(children, "Acceptance Criteria");
  const definitionOfDone = fieldChecklist(children, "Definition of Done Checklist");
  const approvalChecklist = fieldChecklist(children, "Game Director Approval");
  const approved = approvalChecklist.some((i) => /Approved by Yoav/i.test(i.text) && i.checked);

  return {
    id,
    title,
    column,
    statusField,
    priority,
    owner,
    filePath: absPath,
    acceptanceCriteria,
    definitionOfDone,
    approved,
  };
}
