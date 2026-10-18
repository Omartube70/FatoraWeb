// Admin panel helpers (imported by AdminLayout / AuthLayout as an ES module).
// The theme is shared with the public site: same localStorage key and the same data-theme attribute.

const KEY = 'fx-theme';

function systemTheme() {
    return window.matchMedia && matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
}

export function getTheme() {
    let t = null;
    try { t = localStorage.getItem(KEY); } catch (e) { }
    return t === 'light' || t === 'dark' ? t : systemTheme();
}

export function setTheme(theme) {
    try { localStorage.setItem(KEY, theme); } catch (e) { }

    const root = document.documentElement;
    const change = () => root.setAttribute('data-theme', theme);

    // The same circle-grows-out-of-the-button switch the public site uses, when the browser supports it.
    if (document.startViewTransition && !matchMedia('(prefers-reduced-motion: reduce)').matches) {
        const btn = document.querySelector('[data-ad-theme]');
        if (btn) {
            const r = btn.getBoundingClientRect();
            const x = r.left + r.width / 2, y = r.top + r.height / 2;
            root.style.setProperty('--fx-vx', x + 'px');
            root.style.setProperty('--fx-vy', y + 'px');
            root.style.setProperty('--fx-vr', Math.hypot(Math.max(x, innerWidth - x), Math.max(y, innerHeight - y)) + 'px');
        }
        document.startViewTransition(change);
    } else {
        change();
    }
}

/** Minutes the browser's clock is ahead of UTC (Cairo: 120 or 180). */
export function getTimezoneOffset() {
    return -new Date().getTimezoneOffset();
}

export async function copyText(text) {
    try {
        await navigator.clipboard.writeText(text);
        return true;
    } catch (e) {
        return false;
    }
}
