export function saveDarkModePreference(isDarkMode) {
    localStorage.setItem('darkmode', isDarkMode ? 'true' : 'false');
}

export function getDarkModePreference() {
    const saved = localStorage.getItem('darkmode');
    if (saved !== null) {
        return saved === 'true';
    }
    // Check system preference
    return window.matchMedia('(prefers-color-scheme: dark)').matches;
}
