"use strict";

/** @type {HTMLAudioElement | null} Vice City theme music audio instance */
let vcAudio = null;

/**
 * Toggles Vice City theme music playback.
 * Lazily creates the audio element on first invocation.
 * Updates the cassette player UI (spinning reels, play/pause icon).
 * Called via inline `onclick` on the cassette button in ViceCity/CassettePlayer.razor.
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
 * Contact form: before each submit, fetches a fresh reCAPTCHA token into the hidden field, then
 * resubmits so Blazor's enhanced form post carries it. The server verifies the token.
 * Runs in the capture phase so it sees the submit before Blazor does.
 */
document.addEventListener(
  "submit",
  async (e) => {
    const form = e.target;
    if (!(form instanceof HTMLFormElement) || !form.dataset.recaptchaKey) return;

    // Second pass: the token is in place, let Blazor send the form.
    if (form.dataset.tokenReady) {
      delete form.dataset.tokenReady;
      return;
    }

    e.preventDefault();
    e.stopImmediatePropagation();
    setFormBusy(form, true);

    try {
      form.querySelector("[data-recaptcha-token]").value = await getRecaptchaToken(form.dataset.recaptchaKey);
      form.dataset.tokenReady = "true";
      form.requestSubmit();
    } catch (error) {
      console.error("reCAPTCHA failed", error);
      setFormBusy(form, false);
    }
  },
  true,
);

/**
 * Shows the sending state on the contact form's submit button.
 * Blazor re-renders the form after the post, which resets it.
 * @param {HTMLFormElement} form
 * @param {boolean} busy
 */
function setFormBusy(form, busy) {
  const button = form.querySelector('button[type="submit"]');
  button.disabled = busy;
  button.querySelector(".btn-loading").hidden = !busy;
  button.querySelector(".btn-label").textContent = busy ? "Sending..." : "Send Message";
}

/**
 * Registers event listeners to play the Windows XP startup sound
 * on the user's first click or keydown interaction.
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
 * @returns {void}
 */
function startXpClock() {
  const clockEl = document.getElementById("xpClock");
  if (!clockEl || clockEl.dataset.started) return;
  clockEl.dataset.started = "true";

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

/**
 * Windows XP page setup: starts the taskbar clock and arms the startup sound.
 * Runs on full loads and after Blazor enhanced navigation; a no-op on other pages.
 * @returns {void}
 */
function initXpPage() {
  const clockEl = document.getElementById("xpClock");
  if (!clockEl || clockEl.dataset.started) return;
  startXpClock();
  playXpStartupSound();
}

document.addEventListener("DOMContentLoaded", initXpPage);
Blazor.addEventListener("enhancedload", initXpPage);
