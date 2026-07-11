import { useMemo, useState } from "react";
import { useProjectState } from "./hooks/useProjectState";
import { searchSnapshot } from "./lib/search";
import MainDashboard from "./pages/MainDashboard";
import RoadmapPage from "./pages/RoadmapPage";
import TasksView from "./pages/TasksView";
import DecisionsView from "./pages/DecisionsView";
import KnowledgeMapPage from "./pages/KnowledgeMapPage";
import DocumentationView from "./pages/DocumentationView";

type PageId = "main" | "roadmap" | "tasks" | "decisions" | "kmap" | "docs";

const NAV: { id: PageId; label: string; icon: string }[] = [
  { id: "main", label: "Main", icon: "◆" },
  { id: "roadmap", label: "Roadmap", icon: "▤" },
  { id: "tasks", label: "Tasks", icon: "☐" },
  { id: "decisions", label: "Decisions", icon: "§" },
  { id: "kmap", label: "Knowledge Map", icon: "◈" },
  { id: "docs", label: "Documentation", icon: "▦" },
];

export default function App() {
  const { snapshot, connected } = useProjectState();
  const [page, setPage] = useState<PageId>("main");
  const [query, setQuery] = useState("");

  const results = useMemo(() => searchSnapshot(snapshot, query), [snapshot, query]);

  return (
    <div className="shell">
      <div className="topbar">
        <div className="brand">
          <span className="brand-dot" />
          {(snapshot?.projectName ?? "POINT CLEAR").toUpperCase()} // DASHBOARD
        </div>
        <div className="sep" />
        {snapshot ? (
          <>
            <div className="item">
              branch <b>{snapshot.git.branch}</b>
            </div>
            <div className="item">
              HEAD <b>{snapshot.git.headSha}</b>
            </div>
            <div className="item">
              {snapshot.git.isClean
                ? "clean"
                : `${snapshot.git.modified.length} modified · ${snapshot.git.untracked.length} untracked`}
            </div>
          </>
        ) : (
          <div className="item">loading repository state…</div>
        )}

        <div style={{ position: "relative", marginLeft: "auto", display: "flex", alignItems: "center", gap: 12 }}>
          <div className="search-box">
            <span style={{ color: "var(--text-faint)", fontSize: 12 }}>⌕</span>
            <input
              placeholder="Search sprints, tasks, decisions…"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
            />
          </div>
          {query.trim() && (
            <div className="search-results">
              {results.length === 0 ? (
                <div className="sr-empty">No matches for "{query}"</div>
              ) : (
                results.map((r, i) => (
                  <div className="sr-item" key={i}>
                    <div className="sr-kind">{r.kind}</div>
                    <div style={{ fontWeight: 500 }}>{r.title}</div>
                    <div style={{ color: "var(--text-muted)" }}>{r.detail}</div>
                  </div>
                ))
              )}
            </div>
          )}
          <span className={`live-pill ${connected ? "live" : "off"}`}>
            <span className="live-dot" />
            {connected ? "Live" : "Reconnecting"}
          </span>
        </div>
      </div>

      <nav className="sidebar">
        {NAV.map((n) => (
          <button
            key={n.id}
            className={`nav-btn ${page === n.id ? "active" : ""}`}
            onClick={() => setPage(n.id)}
          >
            <span className="ic">{n.icon}</span>
            <span className="label">{n.label}</span>
          </button>
        ))}
      </nav>

      <main className="main">
        {!snapshot ? (
          <p className="page-sub">Waiting for the first snapshot from the server…</p>
        ) : (
          <>
            {page === "main" && <MainDashboard snapshot={snapshot} />}
            {page === "roadmap" && <RoadmapPage snapshot={snapshot} />}
            {page === "tasks" && <TasksView snapshot={snapshot} />}
            {page === "decisions" && <DecisionsView snapshot={snapshot} />}
            {page === "kmap" && <KnowledgeMapPage snapshot={snapshot} />}
            {page === "docs" && <DocumentationView snapshot={snapshot} />}
          </>
        )}
      </main>
    </div>
  );
}
