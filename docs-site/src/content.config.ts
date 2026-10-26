import { defineCollection } from "astro:content";
import { docsLoader } from "@astrojs/starlight/loaders";
import { docsSchema } from "@astrojs/starlight/schema";

export const collections = {
    docs: defineCollection({
        loader: docsLoader({
            // Keep namespace case and dots (api/TypeSafe.AI.Sdk). The default slugger
            // lowercases and deletes them, so TypeSafe.AI.Sdk.DependencyInjection collapses.
            generateId({ entry }) {
                const normalized = entry.replace(/\\/g, "/");
                const withoutExt = normalized.replace(/\.(md|mdx|markdown|mdown|mkdn|mkd|mdwn|mdoc)$/i, "");
                if (withoutExt === "index" || withoutExt.endsWith("/index")) {
                    return withoutExt.slice(0, -"index".length).replace(/\/$/, "") || "index";
                }
                return withoutExt;
            },
        }),
        schema: docsSchema(),
    }),
};
