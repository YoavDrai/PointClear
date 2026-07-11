import { defineConfig } from "vitest/config";

// Kept separate from vite.config.ts: tests only target server/ parser
// modules (no React), and mixing @vitejs/plugin-react's Plugin type with
// vitest's own nested vite dependency causes a type-identity collision
// that has nothing to do with either config's actual behavior.
export default defineConfig({
  test: {
    environment: "node",
    include: ["server/**/*.test.ts"],
  },
});
