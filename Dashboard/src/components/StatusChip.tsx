type ChipKind = "done" | "review" | "todo" | "blocked" | "active" | "progress";

export default function StatusChip({ kind, label }: { kind: ChipKind; label: string }) {
  return (
    <span className={`chip ${kind}`}>
      <span className="chip-dot" />
      {label}
    </span>
  );
}

export function columnToChip(column: string): { kind: ChipKind; label: string } {
  switch (column) {
    case "DONE":
      return { kind: "done", label: "Done" };
    case "REVIEW":
      return { kind: "review", label: "Review" };
    case "IN_PROGRESS":
      return { kind: "progress", label: "In Progress" };
    case "ARCHIVED":
      return { kind: "blocked", label: "Archived" };
    default:
      return { kind: "todo", label: "Todo" };
  }
}
