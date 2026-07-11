import type { ProjectSnapshot } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

export default function MainDashboard({ snapshot }: { snapshot: ProjectSnapshot }) {
  const { focus, health, activity, git } = snapshot;
  const doneSprints = snapshot.tasks.filter((t) => t.column === "DONE" && /Sprint \d+\.\d+/.test(t.title)).length;

  return (
    <section>
      <h1 className="page-title">Project Headquarters</h1>
      <p className="page-sub">
        Where {snapshot.projectName} stands right now, derived live from the repository — nothing here is typed in
        by hand.
      </p>

      <IssueBanner issues={snapshot.issues} />

      <div className="grid g-stats">
        <div className="card stat-card">
          <div className="stat-label">Phase</div>
          <div className="stat-value">{focus.phaseId}</div>
          <div className="stat-note">Vertical Slice</div>
        </div>
        <div className="card stat-card">
          <div className="stat-label">Cluster</div>
          <div className="stat-value">{focus.clusterId ?? "—"}</div>
          <div className="stat-note">
            {snapshot.roadmap.clusters.find((c) => c.id === focus.clusterId)?.title ?? "unresolved"}
          </div>
        </div>
        <div className="card stat-card">
          <div className="stat-label">Sprint</div>
          <div className="stat-value">{focus.sprintId ?? "—"}</div>
          <div className="stat-note">{focus.sprintTitle ?? "—"}</div>
        </div>
        <div className="card stat-card">
          <div className="stat-label">Overall progress</div>
          <div className="stat-value">
            {doneSprints} / {snapshot.roadmap.sprints.length}
          </div>
          <div className="stat-note">sprints complete, Phase 2</div>
        </div>
      </div>

      <div className="focus-card">
        <div className="focus-eyebrow">Current Development Focus</div>
        <div className="focus-title">
          {focus.sprintId ? `Sprint ${focus.sprintId} — ${focus.sprintTitle}` : "No active sprint inferred"}
        </div>
        {focus.sprintQuestion && <p className="focus-q">"{focus.sprintQuestion}"</p>}
        <div className="focus-meta">
          <span>
            Gate ahead <b>{focus.gate ? `Cluster ${focus.gate.cluster} Decision Gate` : "none"}</b>
          </span>
          <span>
            Branch <b>{git.branch}</b>
          </span>
          <span>
            HEAD <b>{git.headSubject || git.headSha}</b>
          </span>
        </div>
        <div className="focus-basis">Inferred: {focus.basis || "no basis available"}</div>
      </div>

      <div className="grid g-2">
        <div className="card">
          <div className="section-title">Activity Timeline</div>
          <div className="timeline">
            {activity.length === 0 && <p className="page-sub">No changelog entries parsed yet.</p>}
            {activity.slice(0, 8).map((a, i) => (
              <div className="tl-item" key={i}>
                <div className="tl-marker">
                  <i />
                </div>
                <div className="tl-body">
                  <div className="tl-date mono">{a.date}</div>
                  <div className="tl-text">{a.text}</div>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="card">
          <div className="section-title">Project Health</div>
          <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
            <HealthRow label="Parse failures" value={health.parseFailures} bad={health.parseFailures > 0} />
            <HealthRow label="Tasks in review" value={health.tasksInReview} />
            <HealthRow label="Tasks blocked" value={health.tasksBlocked} bad={health.tasksBlocked > 0} />
            <HealthRow label="Unresolved decisions" value={health.unresolvedDecisions} />
            <HealthRow
              label="Documentation coverage"
              value={`${health.documentationWritten} docs tracked`}
            />
            <hr style={{ border: "none", borderTop: "1px solid var(--border)", margin: "2px 0" }} />
            <div>
              <div style={{ color: "var(--text-muted)", fontSize: 11.5, marginBottom: 4 }}>
                {focus.gate ? `Cluster ${focus.gate.cluster} Decision Gate` : "No gate ahead"}
              </div>
              {focus.gate && <div style={{ fontSize: 12.5, fontStyle: "italic" }}>"{focus.gate.question}"</div>}
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

function HealthRow({ label, value, bad }: { label: string; value: string | number; bad?: boolean }) {
  return (
    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
      <span style={{ color: "var(--text-muted)", fontSize: 12 }}>{label}</span>
      <span className="mono" style={{ fontSize: 12, color: bad ? "var(--danger)" : "var(--text)" }}>
        {value}
      </span>
    </div>
  );
}
