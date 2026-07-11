import { useMemo, useState } from "react";
import type { ProjectSnapshot, Task, TaskColumn } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

const COLUMNS: TaskColumn[] = ["TODO", "IN_PROGRESS", "REVIEW", "DONE", "ARCHIVED"];
const LABELS: Record<TaskColumn, string> = {
  TODO: "Todo",
  IN_PROGRESS: "In Progress",
  REVIEW: "Review",
  DONE: "Done",
  ARCHIVED: "Archived",
};

export default function TasksView({ snapshot }: { snapshot: ProjectSnapshot }) {
  const [filter, setFilter] = useState("");

  const byColumn = useMemo(() => {
    const q = filter.trim().toLowerCase();
    const map: Record<TaskColumn, Task[]> = { TODO: [], IN_PROGRESS: [], REVIEW: [], DONE: [], ARCHIVED: [] };
    for (const t of snapshot.tasks) {
      if (q && !`${t.id} ${t.title}`.toLowerCase().includes(q)) continue;
      map[t.column].push(t);
    }
    return map;
  }, [snapshot.tasks, filter]);

  return (
    <section>
      <h1 className="page-title">Tasks</h1>
      <p className="page-sub">
        One card per file in <span className="mono">Tasks/</span> — the column is the file's actual folder, not a
        typed status field.
      </p>

      <IssueBanner issues={snapshot.issues} />

      <div className="search-box" style={{ marginBottom: 16, width: 280 }}>
        <span style={{ color: "var(--text-faint)", fontSize: 12 }}>⌕</span>
        <input placeholder="Filter tasks…" value={filter} onChange={(e) => setFilter(e.target.value)} />
      </div>

      <div className="board">
        {COLUMNS.map((col) => (
          <div key={col}>
            <div className="col-head">
              <span>{LABELS[col]}</span>
              <span>{byColumn[col].length}</span>
            </div>
            {byColumn[col].length === 0 && <div className="empty-col">empty</div>}
            {byColumn[col].map((t) => (
              <TaskCard key={t.id} task={t} />
            ))}
          </div>
        ))}
      </div>
    </section>
  );
}

function TaskCard({ task }: { task: Task }) {
  const acPassed = task.acceptanceCriteria.filter((i) => i.checked).length;
  const acTotal = task.acceptanceCriteria.length;
  const pct = acTotal > 0 ? Math.round((acPassed / acTotal) * 100) : 0;
  const mismatch = task.statusField && task.statusField.toUpperCase().split(/[\s(]/)[0] !== task.column;

  return (
    <div className="task-card">
      <div className="task-id">{task.id}</div>
      <div className="task-title">{task.title}</div>
      <div className="task-meta">
        <span>{task.priority ?? "—"}</span>
        <span>{task.owner ?? "unassigned"}</span>
      </div>
      {acTotal > 0 && (
        <>
          <div className="task-progress">
            <i style={{ width: `${pct}%` }} />
          </div>
          <div className="task-meta" style={{ marginTop: 4 }}>
            <span>
              {acPassed}/{acTotal} criteria
            </span>
            <span>{task.approved ? "approved" : "pending"}</span>
          </div>
        </>
      )}
      {mismatch && (
        <div style={{ marginTop: 6, fontSize: 10.5, color: "var(--warn)" }}>
          folder ({task.column}) ≠ status field ("{task.statusField}")
        </div>
      )}
    </div>
  );
}
