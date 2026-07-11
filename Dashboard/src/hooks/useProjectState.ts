import { useEffect, useRef, useState } from "react";
import type { ProjectSnapshot } from "../../server/model/types";

interface State {
  snapshot: ProjectSnapshot | null;
  connected: boolean;
}

export function useProjectState(): State {
  const [snapshot, setSnapshot] = useState<ProjectSnapshot | null>(null);
  const [connected, setConnected] = useState(false);
  const retryRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    let socket: WebSocket | null = null;
    let cancelled = false;

    // Initial fetch so the UI has data even before the socket connects.
    fetch("/api/snapshot")
      .then((r) => r.json())
      .then((data: ProjectSnapshot) => {
        if (!cancelled) setSnapshot(data);
      })
      .catch(() => {});

    function connect() {
      const proto = window.location.protocol === "https:" ? "wss:" : "ws:";
      socket = new WebSocket(`${proto}//${window.location.host}/ws/snapshot`);

      socket.onopen = () => setConnected(true);
      socket.onclose = () => {
        setConnected(false);
        if (!cancelled) retryRef.current = setTimeout(connect, 1500);
      };
      socket.onerror = () => socket?.close();
      socket.onmessage = (event) => {
        try {
          setSnapshot(JSON.parse(event.data));
        } catch {
          // ignore malformed frame
        }
      };
    }

    connect();

    return () => {
      cancelled = true;
      if (retryRef.current) clearTimeout(retryRef.current);
      socket?.close();
    };
  }, []);

  return { snapshot, connected };
}
