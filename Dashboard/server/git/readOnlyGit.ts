// Read-only, by construction: the only function this module exposes to the
// rest of the app is getGitStatus(). No git write subcommand (add, commit,
// push, checkout, reset, branch -d, ...) is implemented anywhere below —
// not merely unused. There is no code path that could ever invoke one.

import { execFile } from "node:child_process";
import { promisify } from "node:util";
import type { GitStatus } from "../model/types.js";

const execFileAsync = promisify(execFile);

async function git(repoRoot: string, args: string[]): Promise<string> {
  const { stdout } = await execFileAsync("git", args, { cwd: repoRoot });
  return stdout.trim();
}

export async function getGitStatus(repoRoot: string): Promise<GitStatus> {
  try {
    const [branch, headSha, headSubject, porcelain] = await Promise.all([
      git(repoRoot, ["rev-parse", "--abbrev-ref", "HEAD"]),
      git(repoRoot, ["rev-parse", "--short", "HEAD"]),
      git(repoRoot, ["log", "-1", "--pretty=%s"]),
      git(repoRoot, ["status", "--porcelain"]),
    ]);

    let ahead = 0;
    let behind = 0;
    try {
      const counts = await git(repoRoot, [
        "rev-list",
        "--left-right",
        "--count",
        `origin/${branch}...HEAD`,
      ]);
      const [b, a] = counts.split(/\s+/).map((n) => parseInt(n, 10) || 0);
      behind = b;
      ahead = a;
    } catch {
      // no upstream configured, or offline — leave at 0/0 rather than fail the whole status
    }

    const modified: string[] = [];
    const untracked: string[] = [];
    for (const line of porcelain.split("\n").filter(Boolean)) {
      const code = line.slice(0, 2);
      const file = line.slice(3);
      if (code.includes("?")) untracked.push(file);
      else modified.push(file);
    }

    return {
      branch,
      headSha,
      headSubject,
      ahead,
      behind,
      modified,
      untracked,
      isClean: modified.length === 0 && untracked.length === 0,
    };
  } catch (err) {
    return {
      branch: "unknown",
      headSha: "unknown",
      headSubject: "",
      ahead: 0,
      behind: 0,
      modified: [],
      untracked: [],
      isClean: true,
    };
  }
}
