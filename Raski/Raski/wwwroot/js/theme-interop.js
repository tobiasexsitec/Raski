// Theme helpers. Kept separate from firebase-interop.js so the theme can be
// applied without loading the Firebase SDK.

const storageKey = "raski-theme";

function resolve(theme) {
    if (theme === "light" || theme === "dark") {
        return theme;
    }

    return window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
}

export function getStoredTheme() {
    try {
        return localStorage.getItem(storageKey);
    } catch {
        return null;
    }
}

export function applyTheme(theme) {
    const resolved = resolve(theme);
    document.documentElement.setAttribute("data-theme", resolved);
    updateStatusBar(resolved);

    try {
        localStorage.setItem(storageKey, theme);
    } catch {
        // Private mode can block storage; the attribute is still applied.
    }
}

function updateStatusBar(resolved) {
    const surface = getComputedStyle(document.documentElement).getPropertyValue("--surface").trim();
    const themeColor = document.querySelector('meta[name="theme-color"]');
    if (surface && themeColor) {
        themeColor.setAttribute("content", surface);
    }

    // iOS only reads this at launch, but keeping it in sync avoids a stale value on reload.
    const appleStatusBar = document.querySelector('meta[name="apple-mobile-web-app-status-bar-style"]');
    appleStatusBar?.setAttribute("content", resolved === "dark" ? "black" : "default");
}

export function getResolvedTheme(theme) {
    return resolve(theme);
}

// Notifies C# when the OS preference changes so "system" stays in sync.
export function listenToSystemTheme(dotNetRef, methodName) {
    const media = window.matchMedia("(prefers-color-scheme: dark)");
    const handler = () => dotNetRef.invokeMethodAsync(methodName);
    media.addEventListener("change", handler);
    return {
        dispose: () => media.removeEventListener("change", handler)
    };
}
