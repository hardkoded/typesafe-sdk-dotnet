// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";
import { satteri } from "@astrojs/markdown-satteri";
import { baseLinksPlugin } from "./src/base-links.mjs";
import { siteBase, siteOrigin } from "./site.mjs";
import apiSidebar from "./src/generated/api-sidebar.json";

// https://astro.build/config
export default defineConfig({
    site: siteOrigin,
    base: siteBase,
    trailingSlash: "always",
    markdown: {
        processor: satteri({
            mdastPlugins: [baseLinksPlugin()],
        }),
    },
    integrations: [
        starlight({
            title: "TypeSafe.AI.Sdk",
            description:
                "Community .NET client for TypeSafe AI. Port of @typesafe-ai/sdk. Not an official TypeSafe product.",
            logo: {
                src: "./src/assets/logo.png",
                alt: "Hardkoded",
            },
            favicon: "/favicon.png",
            customCss: [
                "@fontsource/inter/400.css",
                "@fontsource/inter/500.css",
                "@fontsource/inter/600.css",
                "@fontsource/inter/700.css",
                "@fontsource/dm-mono/400.css",
                "./src/styles/custom.css",
            ],
            expressiveCode: {
                themes: ["github-light", "github-dark"],
                useStarlightDarkModeSwitch: true,
                useStarlightUiThemeColors: true,
            },
            social: [
                { icon: "github", label: "GitHub", href: "https://github.com/hardkoded/typesafe-sdk-dotnet" },
            ],
            editLink: {
                baseUrl: "https://github.com/hardkoded/typesafe-sdk-dotnet/edit/main/docs-site/",
            },
            credits: false,
            components: {
                Footer: "./src/components/Footer.astro",
            },
            sidebar: [
                { label: "Docs", slug: "docs" },
                {
                    label: "Guides",
                    items: [
                        { label: "Getting started", slug: "getting-started" },
                        { label: "Questions", slug: "questions" },
                        { label: "Images", slug: "images" },
                        { label: "Configuration", slug: "configuration" },
                        { label: "Dependency injection", slug: "dependency-injection" },
                        { label: "Errors", slug: "errors" },
                    ],
                },
                {
                    label: "API",
                    collapsed: true,
                    items: apiSidebar,
                },
            ],
        }),
    ],
});
