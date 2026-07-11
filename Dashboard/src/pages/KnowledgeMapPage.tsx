import { useMemo, useState } from "react";
import type { ProjectSnapshot } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

export default function KnowledgeMapPage({ snapshot }: { snapshot: ProjectSnapshot }) {
  const [view, setView] = useState<"graph" | "tree">("graph");
  const rows = snapshot.knowledgeMap.rows;

  const domains = useMemo(() => {
    const map = new Map<string, typeof rows>();
    for (const r of rows) {
      if (!map.has(r.domain)) map.set(r.domain, []);
      map.get(r.domain)!.push(r);
    }
    return [...map.entries()].sort(([a], [b]) => a.localeCompare(b));
  }, [rows]);

  return (
    <section>
      <h1 className="page-title">Knowledge Map</h1>
      <p className="page-sub">
        Rendered directly from POINT_CLEAR_KNOWLEDGE_MAP.md's own Knowledge Topic Registry — not hand-drawn. If that
        document changes, this changes.
      </p>

      <IssueBanner issues={snapshot.issues} />

      {rows.length === 0 ? (
        <p className="page-sub">No registry rows parsed — check the table under "Knowledge Topic Registry".</p>
      ) : (
        <>
          <div className="kmap-toggle">
            <button className={view === "graph" ? "active" : ""} onClick={() => setView("graph")}>
              Graph
            </button>
            <button className={view === "tree" ? "active" : ""} onClick={() => setView("tree")}>
              Tree
            </button>
          </div>

          {view === "graph" ? (
            <div className="kmap-graph">
              <div
                style={{
                  display: "grid",
                  gridTemplateColumns: "repeat(auto-fill, minmax(200px, 1fr))",
                  gap: 14,
                }}
              >
                {domains.map(([domain, topics]) => (
                  <div key={domain} className="card">
                    <div style={{ fontFamily: "var(--mono)", fontWeight: 700, color: "var(--accent-text)", fontSize: 13 }}>
                      {domain}
                    </div>
                    <div style={{ fontSize: 11, color: "var(--text-faint)", marginBottom: 8 }}>
                      {topics.length} topic{topics.length !== 1 ? "s" : ""}
                    </div>
                    {topics.slice(0, 3).map((t, i) => (
                      <div className="kmap-node" key={i} style={{ display: "block", margin: "4px 0" }}>
                        {t.topic.length > 40 ? t.topic.slice(0, 40) + "…" : t.topic}
                      </div>
                    ))}
                    {topics.length > 3 && (
                      <div style={{ fontSize: 10.5, color: "var(--text-faint)" }}>+{topics.length - 3} more</div>
                    )}
                  </div>
                ))}
              </div>
            </div>
          ) : (
            <div className="card">
              <div className="kmap-tree-row head">
                <span>Domain</span>
                <span>Topic</span>
                <span>Owner</span>
                <span>Status</span>
              </div>
              {rows.map((r, i) => (
                <div className="kmap-tree-row" key={i}>
                  <span className="mono">{r.domain}</span>
                  <span>{r.topic}</span>
                  <span style={{ color: "var(--text-muted)" }}>{r.owner}</span>
                  <span style={{ color: "var(--text-faint)", fontSize: 11 }}>{r.status}</span>
                </div>
              ))}
            </div>
          )}
        </>
      )}
    </section>
  );
}
