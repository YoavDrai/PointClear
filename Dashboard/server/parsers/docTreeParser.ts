import { readdir } from "node:fs/promises";
import { join, relative, basename } from "node:path";
import type { DocEntry, KnowledgeMapData } from "../model/types.js";

async function walk(dir: string): Promise<string[]> {
  let entries;
  try {
    entries = await readdir(dir, { withFileTypes: true });
  } catch {
    return [];
  }
  const out: string[] = [];
  for (const e of entries) {
    const full = join(dir, e.name);
    if (e.isDirectory()) {
      out.push(...(await walk(full)));
    } else if (e.isFile() && e.name.endsWith(".md")) {
      out.push(full);
    }
  }
  return out;
}

function ownershipFor(filename: string, km: KnowledgeMapData): string | null {
  const hit = km.rows.find((r) => r.owner.includes(filename));
  return hit ? hit.status : null;
}

export async function parseDocTree(repoRoot: string, km: KnowledgeMapData): Promise<DocEntry[]> {
  const docs: DocEntry[] = [];

  const rootEntries = await readdir(repoRoot, { withFileTypes: true });
  for (const e of rootEntries) {
    if (e.isFile() && e.name.endsWith(".md")) {
      docs.push({
        path: e.name,
        title: e.name,
        group: "Root",
        ownership: ownershipFor(e.name, km),
      });
    }
  }

  const docFolder = join(repoRoot, "Documentation");
  const docFiles = await walk(docFolder);
  for (const full of docFiles) {
    const rel = relative(repoRoot, full);
    const group = "Documentation/" + rel.split("/").slice(1, -1).join("/");
    docs.push({
      path: rel,
      title: basename(full),
      group: group === "Documentation/" ? "Documentation" : group,
      ownership: ownershipFor(basename(full), km),
    });
  }

  return docs;
}
