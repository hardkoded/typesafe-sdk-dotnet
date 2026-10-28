export const config = {
    matcher: ["/typesafe-sdk-dotnet", "/typesafe-sdk-dotnet/:path*"],
};

export default function middleware(request: Request) {
    const url = new URL(request.url);
    const base = "/typesafe-sdk-dotnet";

    if (url.pathname !== base && !url.pathname.startsWith(`${base}/`)) {
        return;
    }

    const path = url.pathname.slice(base.length);
    url.pathname = path === "" || path === "/"
        ? "/index.html"
        : path.endsWith("/") ? `${path}index.html` : path;

    return new Response(null, {
        headers: { "x-middleware-rewrite": url.toString() },
    });
}
