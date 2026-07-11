import type { ProjectSnapshot } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

export default function DecisionsView({ snapshot }: { snapshot: ProjectSnapshot }) {
  const decisions = [...snapshot.decisions].reverse(); // newest DEC number first

  return (
    <section>
      <h1 className="page-title">Decisions</h1>
      <p className="page-sub">Timeline of accepted facts from DECISIONS.md — newest first, {decisions.length} total.</p>

      <IssueBanner issues={snapshot.issues} />

      <div className="grid g-2">
        <div className="card">
          {decisions.map((d) => (
            <div className="dec-row" key={d.id}>
              <div className="dec-id mono">{d.id}</div>
              <div>
                <div className="dec-title">{d.title}</div>
                <div className="dec-body">{d.body}</div>
                {d.boundaries && <div className="dec-boundaries">Boundaries: {d.boundaries}</div>}
              </div>
            </div>
          ))}
        </div>

        <div className="card">
          <div className="section-title">Unresolved ({snapshot.unresolvedDecisions.length})</div>
          <ul style={{ margin: 0, paddingLeft: 18, display: "flex", flexDirection: "column", gap: 8 }}>
            {snapshot.unresolvedDecisions.map((u, i) => (
              <li key={i} style={{ fontSize: 12, color: "var(--text-muted)" }}>
                {u}
              </li>
            ))}
          </ul>
        </div>
      </div>
    </section>
  );
}
