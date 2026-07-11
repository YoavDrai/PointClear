import type { ProjectSnapshot } from "../../server/model/types";

export interface SearchResult {
  kind: "Sprint" | "Task" | "Decision" | "Changelog";
  id: string;
  title: string;
  detail: string;
}

export function searchSnapshot(snapshot: ProjectSnapshot | null, query: string): SearchResult[] {
  if (!snapshot || !query.trim()) return [];
  const q = query.trim().toLowerCase();
  const results: SearchResult[] = [];

  for (const s of snapshot.roadmap.sprints) {
    const haystack = [s.id, s.title, ...s.fields.map((f) => f.text)].join(" ").toLowerCase();
    if (haystack.includes(q)) {
      results.push({ kind: "Sprint", id: s.id, title: `Sprint ${s.id} — ${s.title}`, detail: s.fields[0]?.text ?? "" });
    }
  }

  for (const t of snapshot.tasks) {
    if (`${t.id} ${t.title}`.toLowerCase().includes(q)) {
      results.push({ kind: "Task", id: t.id, title: `${t.id} — ${t.title}`, detail: t.column });
    }
  }

  for (const d of snapshot.decisions) {
    if (`${d.id} ${d.title} ${d.body}`.toLowerCase().includes(q)) {
      results.push({ kind: "Decision", id: d.id, title: `${d.id} — ${d.title}`, detail: d.body.slice(0, 90) });
    }
  }

  for (const c of snapshot.changelog) {
    if (`${c.summary} ${c.bullets.join(" ")}`.toLowerCase().includes(q)) {
      results.push({ kind: "Changelog", id: c.date, title: c.summary, detail: c.date });
    }
  }

  return results.slice(0, 30);
}
