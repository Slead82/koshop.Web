// ============== ICON LIBRARY (inline SVG strings) ==============
const icons = {
  headset: `<svg viewBox="0 0 24 24" fill="none"><path d="M4 13v-1a8 8 0 0 1 16 0v1" stroke="currentColor" stroke-width="1.6" stroke-linecap="round"/><rect x="3" y="13" width="4" height="6" rx="1.6" stroke="currentColor" stroke-width="1.6"/><rect x="17" y="13" width="4" height="6" rx="1.6" stroke="currentColor" stroke-width="1.6"/><path d="M19 19v.5a3 3 0 0 1-3 3h-2.5" stroke="currentColor" stroke-width="1.6" stroke-linecap="round"/></svg>`,
  shield: `<svg viewBox="0 0 24 24" fill="none"><path d="M12 3.5 19 6v5.5c0 4.6-3 7.9-7 9-4-1.1-7-4.4-7-9V6l7-2.5Z" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/><path d="m9 12 2 2 4-4.2" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
  globe: `<svg viewBox="0 0 24 24" fill="none"><circle cx="12" cy="12" r="8.5" stroke="currentColor" stroke-width="1.6"/><path d="M3.5 12h17M12 3.5c2.4 2.4 3.6 5.3 3.6 8.5s-1.2 6.1-3.6 8.5c-2.4-2.4-3.6-5.3-3.6-8.5s1.2-6.1 3.6-8.5Z" stroke="currentColor" stroke-width="1.6"/></svg>`,
  doc: `<svg viewBox="0 0 24 24" fill="none"><path d="M7 3.5h7l4 4V20a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V4.5a1 1 0 0 1 1-1Z" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/><path d="M14 3.5V8h4M9 12.5h6M9 16h6" stroke="currentColor" stroke-width="1.6" stroke-linecap="round"/></svg>`,
  box: `<svg viewBox="0 0 24 24" fill="none"><path d="M12 3 4 7v10l8 4 8-4V7l-8-4Z" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/><path d="M4 7l8 4 8-4M12 11v10" stroke="currentColor" stroke-width="1.6" stroke-linejoin="round"/></svg>`,
  check: `<svg viewBox="0 0 24 24" fill="none"><circle cx="12" cy="12" r="8.5" stroke="currentColor" stroke-width="1.6"/><path d="m8.5 12.3 2.4 2.4 4.6-5" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
  cart: `<svg viewBox="0 0 24 24" fill="none"><path d="M2 3h2l2.4 12.2a2 2 0 0 0 2 1.6h8.2a2 2 0 0 0 2-1.6L21 7H6" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" stroke-linejoin="round"/><circle cx="9" cy="20" r="1.3" fill="currentColor"/><circle cx="17" cy="20" r="1.3" fill="currentColor"/></svg>`,
  jacket: `<svg viewBox="0 0 24 24" fill="none"><path d="M9 3.8 6.5 5.5 4 9.2l2.2 1.8V20h11.6v-9l2.2-1.8-2.5-3.7L15 3.8l-1.7 1.6h-2.6L9 3.8Z" stroke="currentColor" stroke-width="1.5" stroke-linejoin="round"/><path d="M11 5.8v5.4l1 1.3 1-1.3V5.8" stroke="currentColor" stroke-width="1.3" stroke-linejoin="round"/></svg>`,
  appliance: `<svg viewBox="0 0 24 24" fill="none"><rect x="4" y="4" width="9" height="16" rx="1.4" stroke="currentColor" stroke-width="1.5"/><circle cx="8.5" cy="14.3" r="2.6" stroke="currentColor" stroke-width="1.3"/><path d="M6.5 7h4M6.5 9.3h4" stroke="currentColor" stroke-width="1.2" stroke-linecap="round"/><path d="M15 9h5v9.5a1.5 1.5 0 0 1-1.5 1.5H16a1.5 1.5 0 0 1-1.5-1.5V9.8" stroke="currentColor" stroke-width="1.4" stroke-linejoin="round"/><path d="M16 9V6.5a1.5 1.5 0 0 1 3 0V9" stroke="currentColor" stroke-width="1.4"/></svg>`,
  electronics: `<svg viewBox="0 0 24 24" fill="none"><rect x="3" y="5" width="18" height="3.6" rx="1" stroke="currentColor" stroke-width="1.4"/><rect x="3" y="10.5" width="11" height="8.7" rx="1.4" stroke="currentColor" stroke-width="1.4"/><circle cx="17.7" cy="14" r="2.4" stroke="currentColor" stroke-width="1.3"/><path d="M17.7 16.4v2.8" stroke="currentColor" stroke-width="1.3" stroke-linecap="round"/></svg>`,
  beauty: `<svg viewBox="0 0 24 24" fill="none"><rect x="9.3" y="9.5" width="5.4" height="10.5" rx="1.6" stroke="currentColor" stroke-width="1.4"/><path d="M10.3 9.5V6.8a1.7 1.7 0 0 1 1.7-1.7h0a1.7 1.7 0 0 1 1.7 1.7v2.7" stroke="currentColor" stroke-width="1.4" stroke-linejoin="round"/><path d="M9.3 13h5.4" stroke="currentColor" stroke-width="1.2"/><circle cx="18.5" cy="6" r="2.2" stroke="currentColor" stroke-width="1.3"/><path d="M5 17.5c1.3-1 2.6-1 3.3 0" stroke="currentColor" stroke-width="1.2" stroke-linecap="round"/></svg>`,
  sport: `<svg viewBox="0 0 24 24" fill="none"><rect x="6" y="4" width="12" height="17" rx="2.4" stroke="currentColor" stroke-width="1.5"/><path d="M6 9h12M9 4V2.6M15 4V2.6" stroke="currentColor" stroke-width="1.4" stroke-linecap="round"/><path d="M9.5 13h5" stroke="currentColor" stroke-width="1.3" stroke-linecap="round"/></svg>`,
  toy: `<svg viewBox="0 0 24 24" fill="none"><circle cx="12" cy="13" r="6.3" stroke="currentColor" stroke-width="1.5"/><path d="M8.2 7.6c-1-1.6-.7-3.4.6-4 1.3-.6 2.9.4 3.4 2.1M15.8 7.6c1-1.6.7-3.4-.6-4-1.3-.6-2.9.4-3.4 2.1" stroke="currentColor" stroke-width="1.4" stroke-linecap="round"/><circle cx="9.6" cy="12" r=".9" fill="currentColor"/><circle cx="14.4" cy="12" r=".9" fill="currentColor"/><path d="M9.8 15.6c.7.7 3.7.7 4.4 0" stroke="currentColor" stroke-width="1.3" stroke-linecap="round"/></svg>`,
  watch: `<svg viewBox="0 0 24 24" fill="none"><rect x="7.5" y="7.5" width="9" height="9" rx="2" stroke="currentColor" stroke-width="1.5"/><path d="M9.5 7.5V4.8a1 1 0 0 1 1-1h3a1 1 0 0 1 1 1V7.5M9.5 16.5v2.7a1 1 0 0 0 1 1h3a1 1 0 0 0 1-1v-2.7" stroke="currentColor" stroke-width="1.4"/><path d="M10.3 10.3h3.4v3.4h-3.4z" stroke="currentColor" stroke-width="1.2"/></svg>`,
};

// ============== TOP / BOTTOM FEATURE STRIP ==============
const features = [
  { icon: 'headset', title: 'پشتیبانی ۲۴ ساعته', desc: 'پشتیبانی دائمی در تلگرام' },
  { icon: 'shield', title: 'پرداخت امن', desc: 'پرداخت امن و مطمئن بدون واسطه' },
  { icon: 'globe', title: 'ارسال از خارج', desc: 'ارسال از سریع‌ترین و بهترین منابع' },
  { icon: 'doc', title: 'ثبت سفارش کالا', desc: 'سفارش هر کالا که بخواهید' },
];

function renderFeatureStrip(containerId) {
  const el = document.getElementById(containerId);
  if (!el) return; // page may not have this particular strip container — nothing to do
  el.innerHTML = features.map(f => `
    <div class="feature-item">
      <span class="feature-icon">${icons[f.icon]}</span>
      <span class="feature-text">
        <strong>${f.title}</strong>
        <small>${f.desc}</small>
      </span>
    </div>
  `).join('');
}
renderFeatureStrip('features-top');
renderFeatureStrip('features-bottom');

// ============== CATEGORIES ==============
const categories = [
  { icon: 'jacket', label: 'مد و پوشاک' },
  { icon: 'appliance', label: 'خانه و آشپزخانه' },
  { icon: 'electronics', label: 'لوازم الکترونیکی' },
  { icon: 'beauty', label: 'زیبایی و سلامت' },
  { icon: 'sport', label: 'ورزش و سفر' },
  { icon: 'toy', label: 'اسباب‌بازی و کودک' },
];

const categoriesGridEl = document.getElementById('categories-grid');
if (categoriesGridEl) categoriesGridEl.innerHTML = categories.map(c => `
  <a class="category-card" href="#">
    <span class="category-icon">${icons[c.icon]}</span>
    <span class="category-label">${c.label}</span>
  </a>
`).join('');

// ============== PRODUCTS ==============
const products = [
  { name: 'هدفون بی‌سیم سونی WH-100RMS', price: '15,900,000', glyph: 'headset' },
  { name: 'ساعت هوشمند شیائومی Mi Band 8', price: '2,350,000', glyph: 'watch' },
  { name: 'دوربین دیجیتال کانن EOS 8500', price: '35,500,000', glyph: 'electronics' },
  { name: 'کوله‌پشتی ضد آب شیائومی', price: '1,850,000', glyph: 'box' },
];

const productsGridEl = document.getElementById('products-grid');
if (productsGridEl) productsGridEl.innerHTML = products.map(p => `
  <div class="product-card">
    <div class="product-thumb">
      <span class="product-glyph">${icons[p.glyph]}</span>
    </div>
    <p class="product-name">${p.name}</p>
    <div class="product-bottom">
      <span class="product-price">${p.price}<small> تومان</small></span>
      <button class="product-add" aria-label="افزودن به سبد">${icons.cart}</button>
    </div>
  </div>
`).join('');

// ============== TAOBAO STEPS ==============
const steps = [
  { icon: 'box', title: 'دریافت کالا', desc: 'کالای شما در سریع‌ترین زمان به دستتان می‌رسد' },
  { icon: 'check', title: 'تایید و پرداخت', desc: 'قیمت نهایی را تایید و پرداخت کنید' },
  { icon: 'doc', title: 'ثبت سفارش', desc: 'ازطریق سایت ثبت سفارش کنید' },
  { icon: 'cart', title: 'انتخاب کالا', desc: 'کالای مورد نظر خود را در اینترنت پیدا کنید' },
];

const stepsRowEl = document.getElementById('steps-row');
if (stepsRowEl) stepsRowEl.innerHTML = steps.map((s, i) => `
  <div class="step-item">
    <span class="step-icon">${icons[s.icon]}</span>
    <strong class="step-title">${s.title}</strong>
    <p class="step-desc">${s.desc}</p>
  </div>
  ${i < steps.length - 1 ? '<span class="step-arrow">←</span>' : ''}
`).join('');
