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
    document.documentElement.setAttribute("data-theme", resolve(theme));

    try {
        localStorage.setItem(storageKey, theme);
    } catch {
        // Private mode can block storage; the attribute is still applied.
    }
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
