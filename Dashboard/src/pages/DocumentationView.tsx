import { useEffect, useMemo, useState } from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import type { DocEntry, ProjectSnapshot } from "../../server/model/types";
import IssueBanner from "../components/IssueBanner";

export default function DocumentationView({ snapshot }: { snapshot: ProjectSnapshot }) {
  const [filter, setFilter] = useState("");
  const [selected, setSelected] = useState<DocEntry | null>(null);
  const [content, setContent] = useState<string>("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const grouped = useMemo(() => {
    const q = filter.trim().toLowerCase();
    const map = new Map<string, DocEntry[]>();
    for (const d of snapshot.docs) {
      if (q && !`${d.title} ${d.path}`.toLowerCase().includes(q)) continue;
      if (!map.has(d.group)) map.set(d.group, []);
      map.get(d.group)!.push(d);
    }
    return [...map.entries()].sort(([a], [b]) => a.localeCompare(b));
  }, [snapshot.docs, filter]);

  useEffect(() => {
    if (!selected) return;
    let cancelled = false;
    setLoading(true);
    setError(null);
    fetch(`/api/doc?path=${encodeURIComponent(selected.path)}`)
      .then((r) => r.json())
      .then((data: { content?: string; error?: string }) => {
        if (cancelled) return;
        if (data.error) setError(data.error);
        else setContent(data.content ?? "");
      })
      .catch(() => !cancelled && setError("Failed to load document."))
      .finally(() => !cancelled && setLoading(false));
    return () => {
      cancelled = true;
    };
  }, [selected]);

  return (
    <section>
      <h1 className="page-title">Documentation</h1>
      <p className="page-sub">
        Browsable tree with source-of-truth ownership. Click any document to open its rendered Markdown — read-only,
        loaded on demand from the repository.
      </p>

      <IssueBanner issues={snapshot.issues} />

      <div className="doc-layout">
        <div className="doc-sidebar">
          <div className="search-box" style={{ marginBottom: 14, width: "100%" }}>
            <span style={{ color: "var(--text-faint)", fontSize: 12 }}>⌕</span>
            <input placeholder="Filter documents…" value={filter} onChange={(e) => setFilter(e.target.value)} />
          </div>
          {grouped.map(([group, docs]) => (
            <div className="doc-group" key={group}>
              <h4>{group}</h4>
              {docs.map((d) => (
                <button
                  type="button"
                  className={`doc-link ${selected?.path === d.path ? "sel" : ""}`}
                  key={d.path}
                  title={d.path}
                  onClick={() => setSelected(d)}
                >
                  <span>{d.title}</span>
                  {d.ownership && <span className="own">{d.ownership}</span>}
                </button>
              ))}
            </div>
          ))}
        </div>

        <div className="doc-reader">
          {!selected && <p className="page-sub">Select a document on the left to read it here.</p>}
          {selected && loading && <p className="page-sub">Loading {selected.title}…</p>}
          {selected && error && <div className="issue-banner">{error}</div>}
          {selected && !loading && !error && (
            <article className="markdown-body">
              <div className="doc-reader-path mono">{selected.path}</div>
              <ReactMarkdown remarkPlugins={[remarkGfm]}>{content}</ReactMarkdown>
            </article>
          )}
        </div>
      </div>
    </section>
  );
}
