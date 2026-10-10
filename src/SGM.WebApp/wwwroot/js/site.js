"use strict";

/** @type {HTMLAudioElement | null} */
let vcAudio = null;

/** Plays or pauses the Vice City theme music. Called from the inline onclick in ViceCity/CassettePlayer.razor. */
function toggleVcMusic() {
  const btn = document.getElementById("cassetteBtnIcon");
  const player = document.getElementById("cassettePlayer");
  if (!btn) return;

  if (!vcAudio) {
    vcAudio = new Audio("sounds/vc-theme-music.m4a");
    vcAudio.volume = 0.4;
    vcAudio.loop = true;
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

/** Blazor re-renders the form after the post, which resets the busy state. */
function setFormBusy(form, busy) {
  const button = form.querySelector('button[type="submit"]');
  button.disabled = busy;
  button.querySelector(".btn-loading").hidden = !busy;
  button.querySelector(".btn-label").textContent = busy ? "Sending..." : "Send Message";
}

/** Browsers block autoplay, so the startup sound waits for the first click or keypress. */
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

/** @param {HTMLElement} clockEl */
function startXpClock(clockEl) {
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

/** Runs on full loads and after enhanced navigation; a no-op off the XP page or once started. */
function initXpPage() {
  const clockEl = document.getElementById("xpClock");
  if (!clockEl || clockEl.dataset.started) return;
  clockEl.dataset.started = "true";
  startXpClock(clockEl);
  playXpStartupSound();
}

document.addEventListener("DOMContentLoaded", initXpPage);
Blazor.addEventListener("enhancedload", initXpPage);
