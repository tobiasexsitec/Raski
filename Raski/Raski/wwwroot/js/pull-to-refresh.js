const threshold = 70;
const maxPull = 110;

function isStandalone() {
    return window.matchMedia("(display-mode: standalone)").matches || window.navigator.standalone === true;
}

function isBlocked() {
    if (document.querySelector('[aria-modal="true"]')) {
        return true;
    }

    // Recreating an editor page would throw away unsaved input.
    if (/\/(ny|redigera)\/?$/.test(location.pathname)) {
        return true;
    }

    const active = document.activeElement;
    return active instanceof HTMLElement && active.matches("input, textarea, select, [contenteditable]");
}

export function init(indicator, dotNetRef) {
    if (!isStandalone()) {
        return { dispose() { } };
    }

    let startY = null;
    let distance = 0;
    let refreshing = false;

    const render = () => {
        indicator.style.transform = `translate(-50%, ${distance - 48}px) rotate(${distance * 3}deg)`;
        indicator.style.opacity = String(Math.min(distance / threshold, 1));
        indicator.classList.toggle("ready", distance >= threshold);
    };

    const reset = () => {
        startY = null;
        distance = 0;
        indicator.classList.remove("pulling");
        render();
    };

    const onStart = e => {
        if (refreshing || e.touches.length !== 1 || window.scrollY > 0 || isBlocked()) {
            return;
        }
        startY = e.touches[0].clientY;
    };

    const onMove = e => {
        if (startY === null) {
            return;
        }

        const delta = e.touches[0].clientY - startY;
        if (delta <= 0 || window.scrollY > 0) {
            reset();
            return;
        }

        // Resistance makes the pull feel elastic, like native lists.
        distance = Math.min(delta * 0.5, maxPull);
        indicator.classList.add("pulling");
        render();
        e.preventDefault();
    };

    const onEnd = async () => {
        if (startY === null) {
            return;
        }

        if (distance < threshold) {
            reset();
            return;
        }

        startY = null;
        refreshing = true;
        distance = threshold;
        indicator.classList.remove("pulling");
        indicator.classList.add("refreshing");
        render();

        try {
            await dotNetRef.invokeMethodAsync("RefreshAsync");
        } finally {
            refreshing = false;
            indicator.classList.remove("refreshing");
            reset();
        }
    };

    document.addEventListener("touchstart", onStart, { passive: true });
    document.addEventListener("touchmove", onMove, { passive: false });
    document.addEventListener("touchend", onEnd);
    document.addEventListener("touchcancel", reset);

    return {
        dispose() {
            document.removeEventListener("touchstart", onStart);
            document.removeEventListener("touchmove", onMove);
            document.removeEventListener("touchend", onEnd);
            document.removeEventListener("touchcancel", reset);
        }
    };
}
