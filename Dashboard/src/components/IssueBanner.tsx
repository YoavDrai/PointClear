import type { ParseIssue } from "../../server/model/types";

export default function IssueBanner({ issues }: { issues: ParseIssue[] }) {
  if (issues.length === 0) return null;
  return (
    <div className="issue-banner">
      <strong>{issues.length} parse issue{issues.length > 1 ? "s" : ""}</strong> — shown here rather than
      hidden, per the read-only design rule: bad data should never render silently.
      <ul style={{ margin: "6px 0 0", paddingLeft: 18 }}>
        {issues.map((i, idx) => (
          <li key={idx}>
            <span className="mono">{i.source}</span>: {i.message}
          </li>
        ))}
      </ul>
    </div>
  );
}
