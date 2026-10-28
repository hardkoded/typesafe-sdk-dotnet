import { cp, mkdir, readdir } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { siteBase } from "../site.mjs";

// GitHub Pages mounts dist/ at `base`. Vercel serves dist/ at `/`, so the same
// URLs 404 unless the files also exist under that prefix. Rewrites cover the
// rest; see vercel.json.
if (!process.env.VERCEL) {
    console.log("Skipping base mount (not a Vercel build).");
} else {
    const docsSite = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
    const dist = path.join(docsSite, "dist");
    const prefix = siteBase.replace(/^\/+|\/+$/g, "");
    if (!prefix || prefix.split("/").some((segment) => segment === "" || segment === "." || segment === "..")) {
        throw new Error(`Refusing to mount site base: ${siteBase}`);
    }

    const target = path.join(dist, prefix);
    await mkdir(target, { recursive: true });
    for (const entry of await readdir(dist, { withFileTypes: true })) {
        if (entry.name === prefix) continue;
        await cp(path.join(dist, entry.name), path.join(target, entry.name), { recursive: true });
    }

    console.log(`Mounted dist at /${prefix}/`);
}
