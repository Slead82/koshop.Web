/* ============================================================
   KO Shop — Animations JS (scroll reveal + page transitions)
   ============================================================ */

(function () {
  'use strict';

  /* ---- SCROLL PROGRESS BAR ---- */
  const progressBar = document.createElement('div');
  progressBar.className = 'scroll-progress';
  document.body.prepend(progressBar);

  function updateProgress() {
    const scrolled = window.scrollY;
    const total = document.documentElement.scrollHeight - window.innerHeight;
    const pct = total > 0 ? scrolled / total : 0;
    progressBar.style.transform = `scaleX(${pct})`;
    progressBar.style.transformOrigin = 'right';
  }
  window.addEventListener('scroll', updateProgress, { passive: true });

  /* ---- SCROLL REVEAL (IntersectionObserver) ---- */
  const revealClasses = [
    '.reveal-from-bottom',
    '.reveal-from-right',
    '.reveal-from-left',
    '.reveal-expand',
    '.stagger-children',
  ];

  // Skip top-animated elements (header uses CSS animation)
  const allReveal = document.querySelectorAll(revealClasses.join(','));

  const revealObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('revealed');
        revealObserver.unobserve(entry.target);
      }
    });
  }, {
    threshold: 0.12,
    rootMargin: '0px 0px -40px 0px',
  });

  allReveal.forEach(el => {
    // Skip header (it has its own CSS keyframe)
    if (!el.classList.contains('reveal-from-top')) {
      revealObserver.observe(el);
    }
  });

  /* ---- FEATURE STRIP STAGGER ---- */
  // Add stagger class to feature strip
  document.querySelectorAll('.feature-strip-inner').forEach(el => {
    el.classList.add('stagger-children');
    revealObserver.observe(el);
  });

  /* ---- PAGE LINK TRANSITIONS ---- */
  // Fade out on navigation
  document.querySelectorAll('a[href]').forEach(link => {
    const href = link.getAttribute('href');
    if (!href || href.startsWith('#') || href.startsWith('javascript') || href.startsWith('mailto')) return;

    link.addEventListener('click', function (e) {
      const target = this.getAttribute('href');
      if (target && !this.hasAttribute('target')) {
        e.preventDefault();
        // Slide in the overlay
        const overlay = document.querySelector('.page-transition-overlay');
        if (overlay) {
          overlay.style.animation = 'none';
          overlay.style.transform = 'scaleY(0)';
          overlay.style.opacity = '1';
          overlay.style.transformOrigin = 'bottom';
          requestAnimationFrame(() => {
            overlay.style.transition = 'transform 0.4s cubic-bezier(0.25,1,0.5,1)';
            overlay.style.transform = 'scaleY(1)';
          });
        }
        setTimeout(() => { window.location.href = target; }, 380);
      }
    });
  });

  /* ---- HEADER SHADOW ON SCROLL ---- */
  const header = document.querySelector('.site-header');
  if (header) {
    window.addEventListener('scroll', () => {
      if (window.scrollY > 10) {
        header.style.boxShadow = '0 4px 24px rgba(20,20,30,0.08)';
      } else {
        header.style.boxShadow = '';
      }
    }, { passive: true });
  }

  /* ---- COUNTER ANIMATIONS (for any .count-up elements) ---- */
  document.querySelectorAll('[data-count-up]').forEach(el => {
    const target = parseInt(el.dataset.countUp, 10);
    const duration = 1500;
    let start = null;

    const countObs = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          start = null;
          const animate = (timestamp) => {
            if (!start) start = timestamp;
            const progress = Math.min((timestamp - start) / duration, 1);
            const ease = 1 - Math.pow(1 - progress, 3);
            el.textContent = Math.floor(ease * target).toLocaleString('fa-IR');
            if (progress < 1) requestAnimationFrame(animate);
          };
          requestAnimationFrame(animate);
          countObs.unobserve(entry.target);
        }
      });
    }, { threshold: 0.5 });
    countObs.observe(el);
  });

  /* ---- RIPPLE EFFECT on buttons ---- */
  document.querySelectorAll('.btn-cta, .btn-cta-small, .btn-support').forEach(btn => {
    btn.addEventListener('click', function (e) {
      const ripple = document.createElement('span');
      ripple.style.cssText = `
        position:absolute; border-radius:50%; pointer-events:none;
        background:rgba(255,255,255,0.35);
        transform:scale(0); animation:ripple-anim 0.5s ease-out forwards;
        width:80px; height:80px;
      `;
      const rect = this.getBoundingClientRect();
      ripple.style.left = (e.clientX - rect.left - 40) + 'px';
      ripple.style.top  = (e.clientY - rect.top  - 40) + 'px';

      if (getComputedStyle(this).position === 'static') {
        this.style.position = 'relative';
      }
      this.style.overflow = 'hidden';
      this.appendChild(ripple);
      setTimeout(() => ripple.remove(), 550);
    });
  });

  // Inject ripple keyframe
  const rippleStyle = document.createElement('style');
  rippleStyle.textContent = `@keyframes ripple-anim { to { transform: scale(3); opacity: 0; } }`;
  document.head.appendChild(rippleStyle);

  /* ---- MOBILE NAV ---- */
  // (Already handled by existing template; this is just for future hamburger)

})();

/* ============================================================
   AUTH STATE MANAGEMENT
   KO uses localStorage key "ko_user" to simulate login state.
   Set ko_user = JSON like {name:"علی",role:"کاربر عادی"} on login.
   Clear it on logout.
   ============================================================ */

(function initAuthState() {
  // Simulate: change this to false to see guest buttons
  // In real app this comes from server session / token check
  const user = getKOUser();

  const guestEl = document.getElementById('header-auth-guest');
  const userEl  = document.getElementById('header-auth-user');
  const nameEl  = document.getElementById('udm-name');
  const roleEl  = document.getElementById('udm-role');

  if (!guestEl || !userEl) return;

  if (user) {
    userEl.style.display  = 'flex';
    guestEl.style.display = 'none';
    if (nameEl) nameEl.textContent = user.name || 'کاربر';
    if (roleEl) roleEl.textContent = user.role || 'کاربر عادی';
  } else {
    guestEl.style.display = 'flex';
    userEl.style.display  = 'none';
  }

  // "داشبورد" in the header dropdown must only be visible to admins, and must
  // point to the real admin panel — regular customers should not see this option at all.
  const dashLink = document.querySelector('#user-dropdown-menu a[href="dashboard.html"]');
  if (dashLink) {
    const isAdmin = !!(user && ['ادمین', 'مدیر', 'مدیر کل', 'Admin'].includes(user.role));
    if (isAdmin) {
      dashLink.setAttribute('href', 'admin-dashboard.html');
      dashLink.style.display = '';
    } else {
      dashLink.style.display = 'none';
    }
  }
})();

function getKOUser() {
  try {
    const raw = localStorage.getItem('ko_user');
    return raw ? JSON.parse(raw) : null;
  } catch(e) { return null; }
}

/* ============================================================
   SITE FOOTER — swapped automatically based on login state.
   Guest footer: homepage-style, with the "join / newsletter" CTA.
   Logged-in footer: blog-style extended footer, no CTA (a logged-in
   user is already a member, so prompting them to join again was a bug).
   Runs on every page that has <footer class="site-footer">.
   ============================================================ */
function footerGuestHTML() {
  return `
  <div class="newsletter">
    <div class="newsletter-cart">
      <div class="hero-glow small"></div>
      <img src="assets/ko-logo.png" alt="KO Logo" class="newsletter-cart-img" style="object-fit:contain;padding:10px;">
    </div>
    <div class="newsletter-content">
      <h3>از جدیدترین محصولات و تخفیف‌ها باخبر شوید!</h3>
      <p>ایمیل خود را وارد کنید تا از آخرین تخفیف‌ها و جدیدترین محصولات ویژه مطلع شوید.</p>
      <form class="newsletter-form" onsubmit="if(this.querySelector('input').value){showToast('عضویت شما ثبت شد!', 'success');}else{showToast('ایمیل خود را وارد کنید', 'error');}return false;">
        <button type="submit" class="btn-cta">عضویت</button>
        <input type="email" placeholder="ایمیل شما">
      </form>
    </div>
    <div class="newsletter-brand">
      <a class="logo footer-logo" href="index.html"><img src="assets/ko-logo.png" alt="KO" style="width:80px;filter:brightness(0) invert(1)"></a>
      <p>مرجع کامل مطمئن برای خرید محصولات از تمامی فروشگاه ها و ارسال سریع به سراسر ایران.</p>
      <div class="social-icons">
        <a href="#" aria-label="تلگرام"><svg viewBox="0 0 24 24" fill="none"><path d="m21 4-19 8 6 2m13-10-3.5 16-9.5-6m13-10L7.5 14" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg></a>
        <a href="#" aria-label="اینستاگرام"><svg viewBox="0 0 24 24" fill="none"><rect x="3" y="3" width="18" height="18" rx="5" stroke="currentColor" stroke-width="1.6"/><circle cx="12" cy="12" r="4" stroke="currentColor" stroke-width="1.6"/><circle cx="17.5" cy="6.5" r="1" fill="currentColor"/></svg></a>
        <a href="#" aria-label="واتساپ"><svg viewBox="0 0 24 24" fill="none"><path d="M20 12a8 8 0 1 1-3.8-6.8L20 4l-1 3.5A7.96 7.96 0 0 1 20 12Z" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/><path d="M9 9.5c0 3 2.5 5.5 5.5 5.5" stroke="currentColor" stroke-width="1.6" stroke-linecap="round"/></svg></a>
      </div>
    </div>
  </div>
  <div class="footer-bottom">
    <div class="footer-links">
      <a href="faq.html">سوالات متداول</a>
      <a href="blog.html">مقالات</a>
      <a href="terms.html">قوانین و مقررات</a>
      <a href="privacy.html">حریم خصوصی</a>
      <a href="contact.html">تماس با ما</a>
      <a href="about.html">درباره ما</a>
    </div>
    <p class="copyright">تمامی حقوق این سایت محفوظ است.</p>
  </div>`;
}

function footerUserHTML() {
  return `<div class="footer-ext-grid"><div class="footer-ext-brand"><a class="logo footer-logo" href="index.html"><img src="assets/ko-logo.png" alt="KO" style="width:80px;filter:brightness(0) invert(1)"></a><p>سفارش هر کالای خارجی دلخواه شما از سایت‌های بین‌المللی</p><div class="social-icons"><a href="#"><svg viewBox="0 0 24 24" fill="none"><rect x="3" y="3" width="18" height="18" rx="5" stroke="currentColor" stroke-width="1.6"/><circle cx="12" cy="12" r="4" stroke="currentColor" stroke-width="1.6"/><circle cx="17.5" cy="6.5" r="1" fill="currentColor"/></svg></a><a href="#"><svg viewBox="0 0 24 24" fill="none"><path d="m21 4-19 8 6 2m13-10-3.5 16-9.5-6m13-10L7.5 14" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg></a><a href="#"><svg viewBox="0 0 24 24" fill="none"><path d="M20 12a8 8 0 1 1-3.8-6.8L20 4l-1 3.5A7.96 7.96 0 0 1 20 12Z" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/></svg></a></div></div><div class="footer-ext-col"><h4>دسترسی سریع</h4><a href="index.html">صفحه اصلی</a><a href="shop.html">فروشگاه</a><a href="blog.html">مقالات</a><a href="about.html">درباره ما</a><a href="contact.html">تماس با ما</a></div><div class="footer-ext-col"><h4>دسته‌بندی‌ها</h4><a href="category.html">کالای دیجیتال</a><a href="category.html">مد و پوشاک</a><a href="category.html">زیبایی و سلامت</a><a href="category.html">اکسسوری</a><a href="category.html">اکشن فیگور</a></div><div class="footer-ext-col"><h4>خدمات مشتریان</h4><a href="track-order.html">پیگیری سفارش</a><a href="faq.html">سوالات متداول</a><a href="terms.html">قوانین و مقررات</a><a href="privacy.html">حریم خصوصی</a><a href="contact.html">تماس با ما</a></div></div><div class="footer-bottom"><div class="footer-links"><a href="faq.html">سوالات متداول</a><a href="terms.html">قوانین و مقررات</a><a href="contact.html">تماس با ما</a><a href="about.html">درباره ما</a></div><p class="copyright">تمامی حقوق این سایت محفوظ است.</p></div>`;
}

function renderSiteFooter() {
  const footer = document.querySelector('footer.site-footer');
  if (!footer) return; // this page has no site footer (e.g. admin panel) — nothing to do
  const user = getKOUser();
  footer.innerHTML = user ? footerUserHTML() : footerGuestHTML();
  footer.className = user ? 'site-footer site-footer-ext' : 'site-footer';
}
// NOTE (backend integration): the footer is now rendered server-side in
// _Layout.cshtml based on the real authentication state, so this old
// localStorage-driven footer builder is intentionally NOT invoked anymore.
// The function is left here only in case an unconverted static page still needs it.
// renderSiteFooter();

function loginUser(name, role) {
  localStorage.setItem('ko_user', JSON.stringify({ name: name || 'کاربر', role: role || 'کاربر عادی' }));
  location.reload();
}

function logoutUser() {
  localStorage.removeItem('ko_user');
  window.location.href = 'login.html';
}

function toggleUserDropdown() {
  const menu = document.getElementById('user-dropdown-menu');
  if (!menu) return;

  const isOpen = menu.classList.contains('open');

  // Close any open dropdown first
  document.querySelectorAll('.user-dropdown-menu.open').forEach(m => m.classList.remove('open'));
  document.querySelectorAll('.dropdown-backdrop').forEach(b => { b.style.display = 'none'; });

  if (!isOpen) {
    menu.classList.add('open');
    // Backdrop
    let backdrop = document.querySelector('.dropdown-backdrop');
    if (!backdrop) {
      backdrop = document.createElement('div');
      backdrop.className = 'dropdown-backdrop';
      document.body.appendChild(backdrop);
    }
    backdrop.style.display = 'block';
    backdrop.onclick = () => {
      menu.classList.remove('open');
      backdrop.style.display = 'none';
    };
  }
}

// Close dropdown on Escape
document.addEventListener('keydown', e => {
  if (e.key === 'Escape') {
    document.querySelectorAll('.user-dropdown-menu.open').forEach(m => m.classList.remove('open'));
    document.querySelectorAll('.dropdown-backdrop').forEach(b => { b.style.display = 'none'; });
  }
});

/* ============================================================
   SCROLL TO TOP BUTTON
   ============================================================ */
(function() {
  const btn = document.createElement('button');
  btn.className = 'scroll-top-btn';
  btn.setAttribute('aria-label', 'بازگشت به بالا');
  btn.innerHTML = '<svg viewBox="0 0 24 24" fill="none" width="20" height="20"><path d="M18 15l-6-6-6 6" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>';
  btn.onclick = () => window.scrollTo({ top: 0, behavior: 'smooth' });
  document.body.appendChild(btn);

  window.addEventListener('scroll', () => {
    btn.classList.toggle('visible', window.scrollY > 400);
  }, { passive: true });
})();

/* ============================================================
   AUTO ACTIVE NAV LINK
   ============================================================ */
(function() {
  function setActiveNav() {
    const current = window.location.pathname.split('/').pop() || 'index.html';
    document.querySelectorAll('.main-nav a').forEach(a => {
      a.classList.remove('active');
      if (a.getAttribute('href') === current) {
        a.classList.add('active');
      }
    });
  }
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', setActiveNav);
  } else {
    setActiveNav();
  }
})();

/* ============================================================
   TOAST NOTIFICATION SYSTEM
   ============================================================ */
function showToast(msg, type = '') {
  let toast = document.querySelector('.ko-toast');
  if (!toast) {
    toast = document.createElement('div');
    toast.className = 'ko-toast';
    document.body.appendChild(toast);
  }
  toast.textContent = msg;
  toast.className = 'ko-toast' + (type ? ' ' + type : '');
  void toast.offsetWidth; // reflow
  toast.classList.add('show');
  clearTimeout(toast._t);
  toast._t = setTimeout(() => toast.classList.remove('show'), 2800);
}

/* ============================================================
   ADD TO CART FEEDBACK
   ============================================================ */
document.addEventListener('click', function(e) {
  const btn = e.target.closest('[data-add-cart], .btn-add-to-cart');
  if (!btn) return;
  showToast('✓ به سبد خرید افزوده شد', 'success');
  // Bump cart badge
  const badge = document.querySelector('.cart-badge');
  if (badge) {
    badge.classList.remove('bump');
    void badge.offsetWidth;
    badge.classList.add('bump');
    const current = parseInt(badge.textContent) || 0;
    badge.textContent = current + 1;
  }
});

/* ============================================================
   WISHLIST TOGGLE FEEDBACK
   ============================================================ */
document.addEventListener('click', function(e) {
  const btn = e.target.closest('[data-wishlist]');
  if (!btn) return;
  const isActive = btn.classList.toggle('wishlist-active');
  btn.style.color = isActive ? '#e1452f' : '';
  showToast(isActive ? '♥ به علاقه‌مندی‌ها افزوده شد' : 'از علاقه‌مندی‌ها حذف شد');
});

/* ============================================================
   ADDRESS / PAYMENT METHOD CARD ACTIONS
   (edit + delete icon buttons had no handler at all — now wired)
   ============================================================ */
document.addEventListener('click', function(e) {
  const addBtn = e.target.closest('.btn-add-addr');
  if (addBtn && !addBtn.hasAttribute('onclick')) {
    window.location.href = 'addresses.html';
    return;
  }
  const delBtn = e.target.closest('.addr-btn.del');
  if (delBtn) {
    const card = delBtn.closest('.address-card');
    if (card) { card.remove(); showToast('آدرس حذف شد', 'success'); }
    return;
  }
  const editBtn = e.target.closest('.addr-btn:not(.del)');
  if (editBtn) {
    showToast('این ویژگی به زودی اضافه می‌شود');
    return;
  }
});

/* ============================================================
   FORM SUBMIT LOADING STATE
   ============================================================ */
document.addEventListener('submit', function(e) {
  const form = e.target;
  const submitBtn = form.querySelector('button[type=submit], .btn-auth, .btn-save');
  if (!submitBtn || form.hasAttribute('data-no-loading')) return;
  submitBtn.classList.add('btn-loading');
  setTimeout(() => submitBtn.classList.remove('btn-loading'), 1800);
}, true);

/* ============================================================
   SMOOTH ANCHOR SCROLL
   ============================================================ */
document.querySelectorAll('a[href^="#"]').forEach(a => {
  a.addEventListener('click', e => {
    const target = document.querySelector(a.getAttribute('href'));
    if (target) {
      e.preventDefault();
      target.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  });
});

/* ============================================================
   COPY TO CLIPBOARD HELPER
   ============================================================ */
function copyToClipboard(text, btn) {
  navigator.clipboard.writeText(text).then(() => {
    showToast('✓ کپی شد!', 'success');
    if (btn) {
      const orig = btn.innerHTML;
      btn.innerHTML = '<svg viewBox="0 0 24 24" fill="none" width="14" height="14"><path d="m5 13 4 4L19 7" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg> کپی شد!';
      setTimeout(() => { btn.innerHTML = orig; }, 2000);
    }
  }).catch(() => showToast('خطا در کپی', 'error'));
}

/* ============================================================
   LAZY IMAGE LOADING
   ============================================================ */
if ('IntersectionObserver' in window) {
  const imgObs = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const img = entry.target;
        if (img.dataset.src) {
          img.src = img.dataset.src;
          img.removeAttribute('data-src');
        }
        imgObs.unobserve(img);
      }
    });
  }, { rootMargin: '200px' });

  document.querySelectorAll('img[data-src]').forEach(img => imgObs.observe(img));
}

/* ============================================================
   PASSWORD VISIBILITY TOGGLE
   ============================================================ */
document.querySelectorAll('.pwd-toggle').forEach(btn => {
  btn.addEventListener('click', () => {
    const input = btn.previousElementSibling || btn.parentElement.querySelector('input');
    if (!input) return;
    const isText = input.type === 'text';
    input.type = isText ? 'password' : 'text';
    btn.innerHTML = isText
      ? '<svg viewBox="0 0 24 24" fill="none" width="16" height="16"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8Z" stroke="currentColor" stroke-width="1.5"/><circle cx="12" cy="12" r="3" stroke="currentColor" stroke-width="1.5"/></svg>'
      : '<svg viewBox="0 0 24 24" fill="none" width="16" height="16"><path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24" stroke="currentColor" stroke-width="1.5"/><line x1="1" y1="1" x2="23" y2="23" stroke="currentColor" stroke-width="1.5"/></svg>';
  });
});

/* ============================================================
   QUANTITY COUNTER (cart, product page)
   ============================================================ */
document.querySelectorAll('.qty-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    const wrap = btn.closest('.cart-qty, .product-qty-ctrl');
    if (!wrap) return;
    const valEl = wrap.querySelector('.qty-val');
    if (!valEl) return;
    let val = parseInt(valEl.textContent) || 1;
    if (btn.textContent.trim() === '+') val = Math.min(val + 1, 99);
    else val = Math.max(val - 1, 1);
    valEl.textContent = val;
  });
});

/* ============================================================
   DARK MODE TOGGLE
   ============================================================ */
(function() {
    
})();


