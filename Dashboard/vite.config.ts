import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { pointClearDashboardPlugin } from "./server/index.js";

export default defineConfig({
  plugins: [react(), pointClearDashboardPlugin()],
  server: {
    port: 5183,
    strictPort: false,
    open: true,
  },
});
