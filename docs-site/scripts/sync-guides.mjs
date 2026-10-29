import { mkdir, readFile, writeFile } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { redirectHtml } from "./redirect.mjs";

const docsSite = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const repoRoot = path.resolve(docsSite, "..");
const contentDir = path.join(docsSite, "src/content/docs");
const editBase = "https://github.com/hardkoded/typesafe-sdk-dotnet/edit/main/docs-site/";

const pages = [
    {
        source: "guides/getting-started.md",
        output: "getting-started.md",
        title: "Getting started",
        label: "Getting started",
        legacy: "articles/getting-started.html",
        target: "/getting-started/",
    },
    {
        source: "guides/questions.md",
        output: "questions.md",
        title: "Questions",
        label: "Questions",
        legacy: "articles/questions.html",
        target: "/questions/",
    },
    {
        source: "guides/images.md",
        output: "images.md",
        title: "Images",
        label: "Images",
        legacy: "articles/images.html",
        target: "/images/",
    },
    {
        source: "guides/configuration.md",
        output: "configuration.md",
        title: "Configuration",
        label: "Configuration",
        legacy: "articles/configuration.html",
        target: "/configuration/",
    },
    {
        source: "guides/dependency-injection.md",
        output: "dependency-injection.md",
        title: "Dependency injection",
        label: "Dependency injection",
        legacy: "articles/dependency-injection.html",
        target: "/dependency-injection/",
    },
    {
        source: "guides/errors.md",
        output: "errors.md",
        title: "Errors",
        label: "Errors",
        legacy: "articles/errors.html",
        target: "/errors/",
    },
];

const pageTargets = new Map(pages.map((page) => [path.posix.basename(page.source), page.target]));

const sectionRedirects = [
    // Old DocFX table of contents and README pointed at the Starlight home.
    // The site root is the marketing page. Those URLs land on the docs home.
    ["toc.html", "/docs/"],
    ["articles/toc.html", "/docs/"],
    ["README.html", "/docs/"],
];

await mkdir(contentDir, { recursive: true });

for (const page of pages) {
    const raw = await readFile(path.join(docsSite, page.source), "utf8");
    const body = rewriteLinks(stripTitle(raw.replace(/^\uFEFF/, "")));
    const description = describe(body);
    const frontmatter = [
        "---",
        `title: ${JSON.stringify(page.title)}`,
        `description: ${JSON.stringify(description)}`,
        `editUrl: ${JSON.stringify(`${editBase}${page.source}`)}`,
        "sidebar:",
        `  label: ${JSON.stringify(page.label)}`,
        "---",
        "",
    ].join("\n");
    await writeFile(path.join(contentDir, page.output), `${frontmatter}${body.trim()}\n`);
    await writeLegacy(page.legacy, page.target);
}

for (const [legacy, target] of sectionRedirects) {
    await writeLegacy(legacy, target);
}

console.log(`Synced ${pages.length} guide pages into ${path.relative(repoRoot, contentDir)}`);

function stripTitle(markdown) {
    return markdown.replace(/^#\s+[^\n]*\n+/, "");
}

function describe(markdown) {
    const paragraph = markdown
        .split(/\n\s*\n/)
        .map((block) => block.trim())
        .find((block) => block && !block.startsWith("#") && !block.startsWith("```") && !block.startsWith("|") && !block.startsWith("<"));
    const text = (paragraph ?? "TypeSafe.AI.Sdk")
        .replace(/\[([^\]]+)\]\([^)]+\)/g, "$1")
        .replace(/[`*_]/g, "")
        .replace(/\s+/g, " ")
        .trim();
    return text.length > 160 ? `${text.slice(0, 157).trimEnd()}...` : text;
}

function rewriteLinks(markdown) {
    return markdown.replace(/\[([^\]]+)\]\(([^)\s]+)\)/g, (full, text, href) => {
        if (/^[a-z][a-z0-9+.-]*:/i.test(href) || href.startsWith("#") || href.startsWith("/")) return full;
        const hashAt = href.indexOf("#");
        const file = hashAt >= 0 ? href.slice(0, hashAt) : href;
        const hash = hashAt >= 0 ? href.slice(hashAt) : "";
        const target = pageTargets.get(file);
        if (!target) return full;
        return `[${text}](${target}${hash})`;
    });
}

async function writeLegacy(relativePath, target) {
    const destination = path.join(docsSite, "public", relativePath);
    await mkdir(path.dirname(destination), { recursive: true });
    await writeFile(destination, redirectHtml(target));
}
