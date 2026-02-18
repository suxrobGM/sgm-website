"use strict";

// Mobile navigation toggle (used by inline onclick in Razor)
function toggleNav() {
  document.getElementById("navLinks")?.classList.toggle("active");
}

// Navbar scroll effect
document.addEventListener("scroll", () => {
  document.getElementById("navbar")?.classList.toggle("scrolled", window.scrollY > 50);
});

// Smooth scroll for anchor links
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

// Windows XP startup sound - plays on first user click or keydown
function playXpStartupSound() {
  let played = false;

  const play = () => {
    if (played) return;
    played = true;
    const audio = new Audio("sounds/winxp.mp3");
    audio.volume = 0.5;
    audio.play().catch(() => {});
  };

  document.addEventListener("click", play, { once: true });
  document.addEventListener("keydown", play, { once: true });
}

// Windows XP taskbar clock
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
