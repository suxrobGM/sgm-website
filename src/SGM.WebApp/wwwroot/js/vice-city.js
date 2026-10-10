/**
 * Vice City theme behavior: HUD clock and money, wanted level, radar minimap, station banner,
 * pause menu, scroll reveals with mission stamps and loading tips.
 *
 * Loaded on every page from App.razor as a module (strict and deferred, nothing leaks to window).
 * It does nothing unless the page has a `[data-vc]` root, and re-runs after Blazor enhanced
 * navigation, tearing down the previous run first.
 */
const TIPS = [
  "Tip: Press Esc anytime to open the menu.",
  "Tip: Click the blips on the radar to fast travel.",
  "Tip: The cassette in the corner plays music.",
  "Tip: MorphoCLIP trains on a single consumer GPU.",
  "Tip: Meat.gg has 50K+ registered players.",
  "Tip: Want the academic version? The research page has papers and BibTeX.",
];

/** @type {AbortController | null} */
let current = null;

function init() {
  current?.abort();
  current = null;

  const root = document.querySelector("[data-vc]");
  if (!root) return;

  current = new AbortController();
  const { signal } = current;
  /** @type {Array<() => void>} */
  const cleanups = [];
  signal.addEventListener("abort", () => cleanups.forEach((fn) => fn()));

  document.documentElement.classList.add("vc-js");
  cleanups.push(() => document.documentElement.classList.remove("vc-js"));

  const game = createGame(cleanups);
  startClock(cleanups);
  startLoadingTips(cleanups);
  setupReveals(root, game, cleanups);
  setupStations(root, cleanups, signal);
  setupPauseMenu(signal, cleanups);
  setupTitleMenu(signal);
}

/** Shared HUD state: money counter and wanted stars. */
function createGame(cleanups) {
  const moneyEl = document.getElementById("hudMoney");
  const hudStars = [...document.querySelectorAll("#hudStars i")];
  const bannerStars = [...document.querySelectorAll("#wantedStars i")];
  const wantedSection = document.getElementById("hire");
  let money = 0;
  let shown = 0;
  let moneyFrame = 0;
  /** @type {number[]} */
  let starTimers = [];

  cleanups.push(() => {
    cancelAnimationFrame(moneyFrame);
    starTimers.forEach(clearTimeout);
  });

  const renderMoney = (value) => {
    if (moneyEl) moneyEl.textContent = `$${String(Math.round(value)).padStart(8, "0")}`;
  };

  return {
    addMoney(amount) {
      const from = shown;
      money += amount;
      const to = money;
      const start = performance.now();
      cancelAnimationFrame(moneyFrame);
      moneyEl?.classList.remove("bump");
      void moneyEl?.offsetWidth;
      moneyEl?.classList.add("bump");
      const step = (now) => {
        const t = Math.min(1, (now - start) / 1200);
        shown = from + (to - from) * (1 - Math.pow(1 - t, 3));
        renderMoney(shown);
        if (t < 1) moneyFrame = requestAnimationFrame(step);
      };
      moneyFrame = requestAnimationFrame(step);
    },

    /** Lights stars one by one, like the wanted level climbing. */
    setWanted(level) {
      starTimers.forEach(clearTimeout);
      starTimers = [];
      wantedSection?.classList.toggle("alarm", level > 0);
      [hudStars, bannerStars].forEach((stars) =>
        stars.forEach((star, i) => {
          if (i >= level) {
            star.classList.remove("on");
            return;
          }
          starTimers.push(setTimeout(() => star.classList.add("on"), i * 180));
        }),
      );
    },
  };
}

/** Game clock: starts at the visitor's local time and runs one minute per second. */
function startClock(cleanups) {
  const el = document.getElementById("hudClock");
  if (!el) return;

  const now = new Date();
  let minutes = now.getHours() * 60 + now.getMinutes();
  const render = () => {
    const h = String(Math.floor(minutes / 60)).padStart(2, "0");
    const m = String(minutes % 60).padStart(2, "0");
    el.textContent = `${h}:${m}`;
  };

  render();
  const id = setInterval(() => {
    minutes = (minutes + 1) % 1440;
    render();
  }, 1000);
  cleanups.push(() => clearInterval(id));
}

/** Rotates the hero tips once the loading bar has filled. */
function startLoadingTips(cleanups) {
  const el = document.getElementById("loadingTip");
  if (!el) return;

  let index = 0;
  let swapTimer = 0;
  let rotateTimer = 0;
  const next = () => {
    el.classList.add("fade");
    swapTimer = setTimeout(() => {
      index = (index + 1) % TIPS.length;
      el.textContent = TIPS[index];
      el.classList.remove("fade");
    }, 400);
  };

  const startTimer = setTimeout(() => {
    next();
    rotateTimer = setInterval(next, 5000);
  }, 4000);

  cleanups.push(() => {
    clearTimeout(startTimer);
    clearTimeout(swapTimer);
    clearInterval(rotateTimer);
  });
}

/** Fades sections in, stamps missions as passed and raises the wanted level at the hire banner. */
function setupReveals(root, game, cleanups) {
  const items = [...root.querySelectorAll(".reveal")];

  const onReveal = (el) => {
    el.classList.add("is-revealed");

    if (el.classList.contains("mission")) {
      setTimeout(() => {
        el.classList.add("passed");
        const reward = Number(el.dataset.reward);
        if (reward > 0) game.addMoney(reward);
      }, 450);
    }

    if (el.classList.contains("wanted-banner")) {
      setTimeout(() => game.setWanted(5), 300);
    }
  };

  const observer = new IntersectionObserver(
    (entries) => {
      for (const entry of entries) {
        if (!entry.isIntersecting) continue;
        observer.unobserve(entry.target);
        onReveal(entry.target);
      }
    },
    { threshold: 0.15, rootMargin: "0px 0px -8% 0px" },
  );

  items.forEach((el) => observer.observe(el));
  cleanups.push(() => observer.disconnect());
}

/**
 * Tracks which section is under the middle of the viewport: shows the station banner,
 * marks the pause menu entry, and scrolls the radar map so the player arrow sits on it.
 */
function setupStations(root, cleanups, signal) {
  const sections = [...root.querySelectorAll("[data-station]")];
  const radar = document.querySelector(".radar");
  const map = document.getElementById("radarMap");
  const banner = document.getElementById("stationBanner");
  const freqEl = document.getElementById("stationFreq");
  const nameEl = document.getElementById("stationName");
  const menuLinks = [...document.querySelectorAll(".pause-list a")];
  if (!sections.length) return;

  /** @type {number[]} */
  let tops = [];
  let active = -1;
  let frame = 0;
  let bannerTimer = 0;

  const measure = () => {
    tops = sections.map((s) => s.getBoundingClientRect().top + window.scrollY);
    update();
  };

  const showBanner = (section) => {
    if (!banner || !freqEl || !nameEl) return;
    freqEl.textContent = section.dataset.station ?? "";
    nameEl.textContent = section.dataset.stationName ?? "";
    banner.classList.add("show");
    clearTimeout(bannerTimer);
    bannerTimer = setTimeout(() => banner.classList.remove("show"), 2200);
  };

  const update = () => {
    frame = 0;
    const probe = window.scrollY + window.innerHeight / 2;
    let index = 0;
    while (index < tops.length - 1 && probe >= tops[index + 1]) index++;

    const start = tops[index];
    const end = tops[index + 1] ?? document.documentElement.scrollHeight;
    const fraction = Math.min(1, Math.max(0, (probe - start) / Math.max(1, end - start)));

    if (index !== active) {
      active = index;
      showBanner(sections[index]);
      menuLinks.forEach((a, i) => a.classList.toggle("current", i === index));
    }

    if (radar && map) {
      const style = getComputedStyle(radar);
      const size = parseFloat(style.getPropertyValue("--size"));
      const scale = parseFloat(style.getPropertyValue("--map-scale"));
      // Blips sit at y = 60 + i * 100 in map units (see Hud.razor).
      const y = 60 + (index + fraction - 0.5) * 100;
      map.style.setProperty("--map-offset", `${size / 2 - y * scale}px`);
    }
  };

  const schedule = () => {
    if (!frame) frame = requestAnimationFrame(update);
  };

  window.addEventListener("scroll", schedule, { passive: true, signal });
  window.addEventListener("resize", measure, { signal });

  const resizeObserver = new ResizeObserver(measure);
  resizeObserver.observe(document.body);

  cleanups.push(() => {
    resizeObserver.disconnect();
    cancelAnimationFrame(frame);
    clearTimeout(bannerTimer);
  });

  measure();
}

/** Esc or the Menu button opens the pause menu; it traps focus and locks page scroll while open. */
function setupPauseMenu(signal, cleanups) {
  const menu = document.getElementById("pauseMenu");
  const button = document.getElementById("pauseBtn");
  if (!menu || !button) return;

  const focusables = () => [...menu.querySelectorAll("a[href], button")];
  const isOpen = () => !menu.hidden;

  const open = () => {
    menu.hidden = false;
    button.setAttribute("aria-expanded", "true");
    document.body.style.overflow = "hidden";
    (menu.querySelector(".pause-list a.current") ?? menu.querySelector(".pause-list a"))?.focus();
  };

  const close = (restoreFocus = true) => {
    if (!isOpen()) return;
    menu.hidden = true;
    button.setAttribute("aria-expanded", "false");
    document.body.style.overflow = "";
    if (restoreFocus) button.focus({ preventScroll: true });
  };

  cleanups.push(() => close(false));

  button.addEventListener("click", () => (isOpen() ? close() : open()), { signal });

  menu.addEventListener(
    "click",
    (e) => {
      const target = /** @type {HTMLElement} */ (e.target);
      if (target.closest("[data-close-menu]")) close();
      else if (target.closest("a")) close(false);
    },
    { signal },
  );

  document.addEventListener(
    "keydown",
    (e) => {
      if (e.key === "Escape") {
        if (isOpen()) close();
        else if (!isTyping(e.target)) open();
        return;
      }

      if (e.key === "Tab" && isOpen()) {
        const items = focusables();
        const first = items[0];
        const last = items[items.length - 1];
        if (e.shiftKey && document.activeElement === first) {
          e.preventDefault();
          last.focus();
        } else if (!e.shiftKey && document.activeElement === last) {
          e.preventDefault();
          first.focus();
        }
      }
    },
    { signal },
  );
}

/** Start menu: arrow keys move between items, and hover or focus moves the highlight. */
function setupTitleMenu(signal) {
  const menu = document.getElementById("titleMenu");
  if (!menu) return;

  const items = [...menu.querySelectorAll(".title-menu-item")];
  const select = (item) => items.forEach((i) => i.classList.toggle("selected", i === item));
  select(items[0]);

  items.forEach((item) => {
    item.addEventListener("mouseenter", () => select(item), { signal });
    item.addEventListener("focus", () => select(item), { signal });
  });

  menu.addEventListener(
    "keydown",
    (e) => {
      const step = { ArrowRight: 1, ArrowDown: 1, ArrowLeft: -1, ArrowUp: -1 }[e.key];
      if (!step) return;
      e.preventDefault();
      const index = items.indexOf(/** @type {HTMLAnchorElement} */ (document.activeElement));
      items[(index + step + items.length) % items.length].focus();
    },
    { signal },
  );
}

/** @param {EventTarget | null} target */
function isTyping(target) {
  return target instanceof HTMLElement && (target.isContentEditable || /^(INPUT|TEXTAREA|SELECT)$/.test(target.tagName));
}

// Modules run after the document is parsed, so the DOM is ready here.
init();
Blazor.addEventListener("enhancedload", init);
