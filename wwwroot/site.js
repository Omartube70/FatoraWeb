/*
 * Behaviour of the public site: language cookie, dark / light switch, mobile menu, smooth anchors,
 * scroll progress, reveal-on-scroll, scroll-spy, card spotlight and hero tilt.
 *
 * Everything works through document-level delegation and `data-*` attributes (never classes), because
 * Blazor rebuilds the page after it takes over from the server-rendered HTML and re-renders parts of
 * it later - this script must never depend on node identity or on attributes Blazor owns.
 */
(function () {
    'use strict';

    var root = document.documentElement;
    var reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)');
    var LANG_COOKIE = 'fx-lang';
    var THEME_KEY = 'fx-theme';
    var THEME_COLORS = { light: '#fbfaff', dark: '#0a0916' };

    // ------------------------------------------------------------------ language

    function readCookie(name) {
        var match = document.cookie.match(new RegExp('(?:^|; )' + name + '=([^;]*)'));
        return match ? decodeURIComponent(match[1]) : null;
    }

    var swapTimer = 0;

    window.fx = {
        getLang: function () {
            return readCookie(LANG_COOKIE);
        },

        // Called by MainLayout when the visitor switches language. The new texts arrive from the
        // server a moment later, so the content fades out for a beat to hide the swap.
        setLang: function (lang) {
            lang = lang === 'en' ? 'en' : 'ar';
            document.cookie = LANG_COOKIE + '=' + lang + '; path=/; max-age=31536000; samesite=lax';

            root.classList.add('fx-swapping');
            root.setAttribute('lang', lang);
            root.setAttribute('dir', lang === 'ar' ? 'rtl' : 'ltr');

            clearTimeout(swapTimer);
            swapTimer = setTimeout(function () { root.classList.remove('fx-swapping'); }, 280);
        },

        // Keeps the selected screenshot thumbnail in view inside its own scroller (the page itself never moves).
        centerThumb: function () {
            var el = document.querySelector('.fx-thumb.is-active');
            if (!el || !el.parentElement) return;
            var box = el.parentElement.getBoundingClientRect();
            var rect = el.getBoundingClientRect();
            el.parentElement.scrollBy({
                left: (rect.left + rect.width / 2) - (box.left + box.width / 2),
                behavior: reduceMotion.matches ? 'auto' : 'smooth'
            });
        }
    };

    // ------------------------------------------------------------------ theme

    function applyTheme(theme) {
        root.setAttribute('data-theme', theme);
        try { localStorage.setItem(THEME_KEY, theme); } catch (e) { /* private mode: the choice just isn't remembered */ }

        var metas = document.querySelectorAll('meta[name="theme-color"]');
        for (var i = 0; i < metas.length; i++) {
            metas[i].removeAttribute('media');
            metas[i].setAttribute('content', THEME_COLORS[theme]);
        }
    }

    function toggleTheme(button) {
        var next = root.getAttribute('data-theme') === 'dark' ? 'light' : 'dark';

        if (!document.startViewTransition || reduceMotion.matches) {
            applyTheme(next);
            return;
        }

        // Circular reveal growing from the button that was pressed.
        var rect = button.getBoundingClientRect();
        var x = rect.left + rect.width / 2;
        var y = rect.top + rect.height / 2;
        var radius = Math.hypot(Math.max(x, innerWidth - x), Math.max(y, innerHeight - y));
        root.style.setProperty('--fx-vx', x + 'px');
        root.style.setProperty('--fx-vy', y + 'px');
        root.style.setProperty('--fx-vr', radius + 'px');

        document.startViewTransition(function () { applyTheme(next); });
    }

    // ------------------------------------------------------------------ mobile menu

    function setMenu(open) {
        if (open) root.setAttribute('data-menu-open', '');
        else root.removeAttribute('data-menu-open');

        var burger = document.querySelector('[data-fx-menu]');
        if (burger) burger.setAttribute('aria-expanded', open ? 'true' : 'false');
    }

    // ------------------------------------------------------------------ clicks

    document.addEventListener('click', function (e) {
        var target = e.target;
        if (!target || !target.closest) return;

        var themeButton = target.closest('[data-fx-theme]');
        if (themeButton) {
            toggleTheme(themeButton);
            return;
        }

        if (target.closest('[data-fx-menu]')) {
            setMenu(!root.hasAttribute('data-menu-open'));
            return;
        }

        // Clicking outside the open mobile menu closes it.
        if (root.hasAttribute('data-menu-open') && !target.closest('.fx-nav__menu')) setMenu(false);
    });

    // Smooth in-page anchors. Runs in the capture phase and stops the event so Blazor's own link
    // interception (which would push a history entry per section) never sees it.
    document.addEventListener('click', function (e) {
        if (e.defaultPrevented || e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey) return;

        var link = e.target && e.target.closest && e.target.closest('a[href^="#"]');
        if (!link) return;

        var id = link.getAttribute('href').slice(1);
        var section = id && id !== 'top' ? document.getElementById(id) : null;
        if (id && id !== 'top' && !section) return;

        e.preventDefault();
        e.stopPropagation();

        var behavior = reduceMotion.matches ? 'auto' : 'smooth';
        if (section) section.scrollIntoView({ behavior: behavior, block: 'start' });
        else window.scrollTo({ top: 0, behavior: behavior });

        try { history.replaceState(null, '', section ? '#' + id : location.pathname + location.search); } catch (err) { /* ignore */ }

        if (link.closest('.fx-nav__menu')) setMenu(false);
    }, true);

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && root.hasAttribute('data-menu-open')) setMenu(false);
    });

    window.addEventListener('resize', function () {
        if (innerWidth > 900 && root.hasAttribute('data-menu-open')) setMenu(false);
    });

    // ------------------------------------------------------------------ scroll state

    var scrollTicking = false;

    function updateScroll() {
        scrollTicking = false;
        var y = window.pageYOffset || root.scrollTop || 0;
        var max = root.scrollHeight - window.innerHeight;
        root.style.setProperty('--fx-scroll', max > 0 ? Math.min(1, y / max).toFixed(4) : '0');
        root.toggleAttribute('data-scrolled', y > 8);
        root.toggleAttribute('data-fab', y > 700);
    }

    window.addEventListener('scroll', function () {
        if (scrollTicking) return;
        scrollTicking = true;
        requestAnimationFrame(updateScroll);
    }, { passive: true });

    // ------------------------------------------------------------------ reveal on scroll

    // Elements Blazor swaps in after load are the "same" element as before; remembering which ones were
    // already revealed (by their position in the page) lets the new copies appear instantly instead of
    // playing their entrance a second time.
    var revealed = new Set();

    function keyOf(el) {
        var parts = [];
        for (var node = el; node && node !== document.body; node = node.parentElement) {
            if (node.id) { parts.push('#' + node.id); break; }
            if (!node.parentElement) break;
            parts.push(Array.prototype.indexOf.call(node.parentElement.children, node));
        }
        return parts.reverse().join('/');
    }

    var revealObserver = 'IntersectionObserver' in window
        ? new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;
                entry.target.setAttribute('data-in', '');
                revealed.add(entry.target.__fxKey);
                revealObserver.unobserve(entry.target);
            });
        }, { rootMargin: '0px 0px -8% 0px', threshold: 0.08 })
        : null;

    function registerReveal(el) {
        el.setAttribute('data-fx-r', '');
        el.__fxKey = keyOf(el);

        if (!revealObserver || reduceMotion.matches) {
            el.setAttribute('data-in', '');
            el.setAttribute('data-instant', '');
        } else if (revealed.has(el.__fxKey)) {
            el.setAttribute('data-in', '');
            el.setAttribute('data-instant', '');
        } else {
            revealObserver.observe(el);
        }
    }

    // ------------------------------------------------------------------ scroll-spy

    var spyObserver = 'IntersectionObserver' in window
        ? new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;
                var active = entry.target.getAttribute('data-spy');
                var links = document.querySelectorAll('[data-spy-link]');
                for (var i = 0; i < links.length; i++) {
                    var on = links[i].getAttribute('data-spy-link') === active;
                    if (on) { links[i].setAttribute('data-active', ''); links[i].setAttribute('aria-current', 'true'); }
                    else { links[i].removeAttribute('data-active'); links[i].removeAttribute('aria-current'); }
                }
            });
        }, { rootMargin: '-45% 0px -50% 0px', threshold: 0 })
        : null;

    function scan() {
        var pending = document.querySelectorAll('[data-reveal]:not([data-fx-r])');
        for (var i = 0; i < pending.length; i++) registerReveal(pending[i]);

        if (spyObserver) {
            var sections = document.querySelectorAll('[data-spy]:not([data-fx-s])');
            for (var j = 0; j < sections.length; j++) {
                sections[j].setAttribute('data-fx-s', '');
                spyObserver.observe(sections[j]);
            }
        }
    }

    var scanQueued = false;

    new MutationObserver(function () {
        if (scanQueued) return;
        scanQueued = true;
        // Runs before the next paint, so replaced elements never flash in their hidden state.
        Promise.resolve().then(function () { scanQueued = false; scan(); });
    }).observe(document.documentElement, { childList: true, subtree: true });

    // ------------------------------------------------------------------ pointer effects (spotlight, tilt)

    var tilted = null;

    function resetTilt() {
        if (!tilted) return;
        tilted.style.setProperty('--rx', '0deg');
        tilted.style.setProperty('--ry', '0deg');
        tilted = null;
    }

    document.addEventListener('pointermove', function (e) {
        if (e.pointerType === 'touch' || reduceMotion.matches || !e.target.closest) return;

        var spot = e.target.closest('[data-spot]');
        if (spot) {
            var box = spot.getBoundingClientRect();
            spot.style.setProperty('--mx', (e.clientX - box.left) + 'px');
            spot.style.setProperty('--my', (e.clientY - box.top) + 'px');
        }

        var tilt = e.target.closest('[data-tilt]');
        if (tilt) {
            var r = tilt.getBoundingClientRect();
            var px = (e.clientX - r.left) / r.width - 0.5;
            var py = (e.clientY - r.top) / r.height - 0.5;
            tilt.style.setProperty('--ry', (px * 7).toFixed(2) + 'deg');
            tilt.style.setProperty('--rx', (py * -6).toFixed(2) + 'deg');
            tilted = tilt;
        } else if (tilted) {
            resetTilt();
        }
    }, { passive: true });

    document.addEventListener('pointerleave', resetTilt);

    // ------------------------------------------------------------------ start

    scan();
    updateScroll();
})();
