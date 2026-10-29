const buttons = document.querySelectorAll("[data-copy]");

for (const button of buttons) {
    button.addEventListener("click", async () => {
        const value = button.getAttribute("data-copy") ?? "";
        const previous = button.textContent;
        try {
            await navigator.clipboard.writeText(value);
            button.textContent = "Copied";
        } catch {
            button.textContent = "Copy failed";
        }
        window.setTimeout(() => {
            button.textContent = previous;
        }, 1600);
    });
}
