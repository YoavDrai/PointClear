import type { Plugin, ViteDevServer } from "vite";
import { WebSocketServer, WebSocket } from "ws";
import { readFile } from "node:fs/promises";
import { resolve, sep } from "node:path";
import { buildSnapshot } from "./model/buildSnapshot.js";
import { watchProjectFiles } from "./watcher.js";

const WS_PATH = "/ws/snapshot";

export function pointClearDashboardPlugin(): Plugin {
  const repoRoot = resolve(process.cwd(), "..");
  let snapshotJson = "";
  const clients = new Set<WebSocket>();

  async function rebuild(reason: string) {
    const snapshot = await buildSnapshot(repoRoot);
    snapshotJson = JSON.stringify(snapshot);
    const latest = snapshot.changelog[0]?.summary ?? "(none)";
    console.log(
      `[dashboard] snapshot rebuilt (${reason}) — ${snapshot.changelog.length} changelog entries, latest: "${latest}"; broadcasting to ${clients.size} client(s)`,
    );
    for (const client of clients) {
      if (client.readyState === WebSocket.OPEN) client.send(snapshotJson);
    }
  }

  return {
    name: "point-clear-dashboard",

    configureServer(server: ViteDevServer) {
      const wss = new WebSocketServer({ noServer: true });

      wss.on("connection", (socket) => {
        clients.add(socket);
        if (snapshotJson) socket.send(snapshotJson);
        socket.on("close", () => clients.delete(socket));
      });

      server.httpServer?.on("upgrade", (req, socket, head) => {
        if (req.url === WS_PATH) {
          wss.handleUpgrade(req, socket, head, (ws) => wss.emit("connection", ws, req));
        }
        // any other path (including Vite's own HMR upgrade) is left alone
      });

      server.middlewares.use("/api/snapshot", async (_req, res) => {
        if (!snapshotJson) await rebuild("http /api/snapshot (cold)");
        res.setHeader("Content-Type", "application/json");
        res.end(snapshotJson);
      });

      // Read-only, path-validated markdown reader for the Documentation
      // viewer. Reads only .md files strictly inside the repo root; any
      // traversal attempt or non-.md path is rejected. No write path exists.
      server.middlewares.use("/api/doc", async (req, res) => {
        const sendErr = (code: number, msg: string) => {
          res.statusCode = code;
          res.setHeader("Content-Type", "application/json");
          res.end(JSON.stringify({ error: msg }));
        };
        try {
          const raw = (req as any).originalUrl ?? req.url ?? "";
          const url = new URL(raw, "http://localhost");
          const rel = url.searchParams.get("path") ?? "";
          const abs = resolve(repoRoot, rel);
          if (!abs.startsWith(repoRoot + sep) || !abs.toLowerCase().endsWith(".md")) {
            return sendErr(400, "Path must be a .md file inside the repository.");
          }
          const content = await readFile(abs, "utf8");
          res.setHeader("Content-Type", "application/json");
          res.end(JSON.stringify({ path: rel, content }));
        } catch {
          sendErr(404, "Document not found.");
        }
      });

      rebuild("initial").catch((err) => console.error("[dashboard] initial snapshot build failed:", err));

      const { watcher, watchedPaths } = watchProjectFiles(repoRoot, (event, path) => {
        rebuild(`fs ${event}: ${path}`).catch((err) =>
          console.error("[dashboard] snapshot rebuild failed:", err),
        );
      });

      console.log("[dashboard] watching for changes under:");
      for (const p of watchedPaths) console.log(`  • ${p}`);

      server.httpServer?.on("close", () => {
        watcher.close();
        wss.close();
      });
    },
  };
}
