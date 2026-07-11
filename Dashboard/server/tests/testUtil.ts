import { mkdtemp, writeFile, rm } from "node:fs/promises";
import { tmpdir } from "node:os";
import { join } from "node:path";

export async function withFixtureFile(name: string, content: string): Promise<string> {
  const dir = await mkdtemp(join(tmpdir(), "pcd-test-"));
  const path = join(dir, name);
  await writeFile(path, content, "utf8");
  return path;
}

export async function cleanupFixture(filePath: string): Promise<void> {
  await rm(join(filePath, ".."), { recursive: true, force: true });
}
