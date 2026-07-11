import chokidar, { type FSWatcher } from "chokidar";
import { readdirSync } from "node:fs";
import { join } from "node:path";

export interface WatcherHandle {
  watcher: FSWatcher;
  watchedPaths: string[];
}

/**
 * chokidar v4 removed glob support entirely — passing "*.md" or
 * "Documentation/**\/*.md" resolves to ZERO paths and silently watches
 * nothing. So we watch real directory trees + explicitly enumerated
 * root-level .md files instead, and filter events by extension in the
 * handler. This was the cause of the live-update failure in the first
 * PC-006 build.
 */
export function watchProjectFiles(
  repoRoot: string,
  onChange: (event: string, path: string) => void,
): WatcherHandle {
  // Root-level .md files, enumerated now (chokidar v4 can't glob them).
  // A brand-new root .md added after startup won't be watched until the
  // server restarts — an accepted, documented limitation.
  let rootMdFiles: string[] = [];
  try {
    rootMdFiles = readdirSync(repoRoot)
      .filter((f) => f.toLowerCase().endsWith(".md"))
      .map((f) => join(repoRoot, f));
  } catch {
    rootMdFiles = [];
  }

  const paths = [
    ...rootMdFiles,
    join(repoRoot, "Documentation"),
    join(repoRoot, "Tasks"),
    join(repoRoot, ".git", "HEAD"),
  ];

  const watcher = chokidar.watch(paths, {
    ignoreInitial: true,
    // Belt-and-suspenders: these never appear under the roots above, but
    // guard against Node/editor scratch files if the roots ever widen.
    ignored: (p: string) => /[\\/](node_modules|\.vite|dist)[\\/]/.test(p),
  });

  let timer: NodeJS.Timeout | null = null;
  const relevant = (p: string) => p.toLowerCase().endsWith(".md") || p.endsWith("HEAD");

  const handle = (event: string) => (p: string) => {
    console.log(`[dashboard] fs ${event}: ${p}`);
    if (!relevant(p)) return;
    if (timer) clearTimeout(timer);
    timer = setTimeout(() => onChange(event, p), 200);
  };

  watcher
    .on("add", handle("add"))
    .on("change", handle("change"))
    .on("unlink", handle("unlink"));

  return { watcher, watchedPaths: paths };
}
