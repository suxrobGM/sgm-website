"use strict";

/**
 * Toggles the mobile navigation menu visibility.
 * Called via inline `onclick` in Razor components.
 * @returns {void}
 */
function toggleNav() {
  document.getElementById("navLinks")?.classList.toggle("active");
}

// Navbar scroll effect - adds "scrolled" class when page is scrolled past 50px
document.addEventListener("scroll", () => {
  document
    .getElementById("navbar")
    ?.classList.toggle("scrolled", window.scrollY > 50);
});

// Smooth scroll for anchor links - intercepts `#hash` clicks and scrolls smoothly
document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
    anchor.addEventListener("click", (e) => {
      e.preventDefault();
      const target = document.querySelector(anchor.getAttribute("href"));
      if (target) {
        target.scrollIntoView({ behavior: "smooth", block: "start" });
        document.getElementById("navLinks")?.classList.remove("active");
      }
    });
  });
});

/** @type {HTMLAudioElement | null} Vice City theme music audio instance */
let vcAudio = null;

/**
 * Toggles Vice City theme music playback.
 * Lazily creates the audio element on first invocation.
 * Updates the cassette player UI (spinning reels, play/pause icon).
 * Called via inline `onclick` on the cassette button in HomeViceCity.razor.
 */
function toggleVcMusic() {
  const btn = document.getElementById("cassetteBtnIcon");
  const player = document.getElementById("cassettePlayer");
  if (!btn) return;

  if (!vcAudio) {
    vcAudio = new Audio("sounds/vc-theme-music.m4a");
    vcAudio.volume = 0.4;
    vcAudio.loop = true;
    vcAudio.addEventListener("ended", () => {
      btn.className = "fas fa-play";
      player?.classList.remove("playing");
    });
  }

  if (vcAudio.paused) {
    vcAudio
      .play()
      .then(() => {
        btn.className = "fas fa-pause";
        player?.classList.add("playing");
      })
      .catch(() => {});
  } else {
    vcAudio.pause();
    btn.className = "fas fa-play";
    player?.classList.remove("playing");
  }
}

/**
 * Registers event listeners to play the Windows XP startup sound
 * on the user's first click or keydown interaction.
 * Called via Blazor JS interop from `HomeWindowsXP.OnAfterRenderAsync`.
 * @returns {void}
 */
function playXpStartupSound() {
  let played = false;

  const play = () => {
    if (played) return;
    played = true;
    const audio = new Audio("sounds/winxp-startup.mp3");
    audio.volume = 0.5;
    audio.play().catch(() => {});
  };

  document.addEventListener("click", play, { once: true });
  document.addEventListener("keydown", play, { once: true });
}

/**
 * Starts the Windows XP taskbar clock, updating the `#xpClock` element
 * with the current time in 12-hour format every 30 seconds.
 * Called via Blazor JS interop from `HomeWindowsXP.OnAfterRenderAsync`.
 * @returns {void}
 */
function startXpClock() {
  const clockEl = document.getElementById("xpClock");
  if (!clockEl) return;

  const update = () => {
    const now = new Date();
    const minutes = now.getMinutes().toString().padStart(2, "0");
    const rawHours = now.getHours();
    const ampm = rawHours >= 12 ? "PM" : "AM";
    const hours = rawHours % 12 || 12;
    clockEl.textContent = `${hours}:${minutes} ${ampm}`;
  };

  update();
  setInterval(update, 30000);
}
