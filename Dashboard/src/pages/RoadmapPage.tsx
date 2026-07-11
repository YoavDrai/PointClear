import type { ProjectSnapshot, Sprint } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

export default function RoadmapPage({ snapshot }: { snapshot: ProjectSnapshot }) {
  const { roadmap } = snapshot;
  const phase2 = roadmap.phases.find((p) => p.id === "2");

  return (
    <section>
      <h1 className="page-title">Roadmap</h1>
      <p className="page-sub">
        {phase2 ? `Phase 2 — ${phase2.title}` : "Phase 2"}, generated live from ROADMAP.md. A research plan, not a
        fixed schedule — gates may send work backward, not just forward.
      </p>

      <IssueBanner issues={snapshot.issues} />

      {roadmap.clusters.length === 0 && <p className="page-sub">No clusters parsed — check ROADMAP.md's structure.</p>}

      {roadmap.clusters.map((cluster) => {
        const sprints = roadmap.sprints.filter((s) => s.cluster === cluster.id);
        return (
          <div className="cluster" key={cluster.id}>
            <div className="cluster-head">
              <div>
                <h3>
                  Cluster {cluster.id} — {cluster.title}
                </h3>
                {cluster.purpose && <div className="cluster-q">{cluster.purpose}</div>}
              </div>
            </div>
            <div className="cluster-body">
              {sprints.map((sprint) => (
                <SprintRow key={sprint.id} sprint={sprint} />
              ))}
              {cluster.gate && (
                <div className="gate">
                  <div className="gq">Gate — {cluster.gate.question}</div>
                  <div className="gd">{cluster.gate.decision}</div>
                  {cluster.gate.extra && <div className="gd" style={{ marginTop: 4 }}>{cluster.gate.extra}</div>}
                </div>
              )}
            </div>
          </div>
        );
      })}
    </section>
  );
}

function SprintRow({ sprint }: { sprint: Sprint }) {
  return (
    <details className="sprint-row">
      <summary>
        <span className="sid mono">{sprint.id}</span>
        <span className="sname">{sprint.title}</span>
        <span className="mono" style={{ fontSize: 10.5, color: "var(--text-faint)" }}>
          {sprint.fields.length} fields
        </span>
      </summary>
      <div className="sprint-fields">
        {sprint.fields.map((f, i) => (
          <div className="f" key={i}>
            <b>{f.label}</b>
            {f.text}
          </div>
        ))}
      </div>
    </details>
  );
}
