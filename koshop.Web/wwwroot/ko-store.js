/* ============================================================
   KO SHOP — Central Data Store
   Products, Cart, Wishlist, Search, Orders (localStorage)
   ============================================================ */

const KO = (function() {
  'use strict';

  /* ── PRODUCT DATA ──────────────────────────────────────── */
  const PRODUCTS = [
    { id:'p001', name:'هدفون بی‌سیم سونی WH-1000XMS', cat:'digital', brand:'Sony',
      price:15900000, oldPrice:18700000, discount:15, rating:4.9, reviews:124,
      badge:'جدید', color:['#1a1a1a','#c8b9a6','#4a4a4a'], sizes:[],
      img:'assets/products/p001.svg',
      imgs:['assets/products/p001.svg'],
      desc:'هدفون پرچم‌دار سونی با فناوری حذف نویز پیشرفته' },
    { id:'p002', name:'ایرپاد پرو ۲ نسل دوم اپل', cat:'digital', brand:'Apple',
      price:11900000, oldPrice:13500000, discount:12, rating:4.8, reviews:89,
      badge:'پرفروش', color:['#ffffff'], sizes:[],
      img:'assets/products/p002.svg',
      imgs:['assets/products/p002.svg'],
      desc:'ایرپاد پرو نسل دوم با چیپست H2 و حذف نویز فعال' },
    { id:'p003', name:'ساعت هوشمند اپل واچ سری 9', cat:'digital', brand:'Apple',
      price:22500000, oldPrice:25000000, discount:10, rating:4.7, reviews:56,
      badge:'', color:['#1a1a1a','#f5c5a3','#e8e8e8'], sizes:['40mm','44mm'],
      img:'assets/products/p003.svg',
      imgs:['assets/products/p003.svg'],
      desc:'ساعت هوشمند اپل با چیپست S9 و صفحه Always-On' },
    { id:'p004', name:'گوشی موبایل سامسونگ Galaxy S24 Ultra', cat:'digital', brand:'Samsung',
      price:58000000, oldPrice:65000000, discount:11, rating:4.8, reviews:203,
      badge:'جدید', color:['#1a1a1a','#b5a642','#c6c9c9'], sizes:['256GB','512GB'],
      img:'assets/products/p004.svg',
      imgs:['assets/products/p004.svg'],
      desc:'پرچم‌دار سامسونگ با قلم S Pen داخلی و دوربین ۲۰۰ مگاپیکسل' },
    { id:'p005', name:'لپ‌تاپ ایسوس ZenBook 14 OLED', cat:'digital', brand:'Asus',
      price:42000000, oldPrice:47000000, discount:11, rating:4.6, reviews:34,
      badge:'', color:['#2c2c2c'], sizes:['i5/16GB','i7/32GB'],
      img:'assets/products/p005.svg',
      imgs:['assets/products/p005.svg'],
      desc:'لپ‌تاپ فوق‌العاده با نمایشگر OLED 2.8K و پردازنده نسل ۱۳' },
    { id:'p006', name:'دوربین دیجیتال کانن EOS R50', cat:'digital', brand:'Canon',
      price:28500000, oldPrice:32000000, discount:11, rating:4.5, reviews:41,
      badge:'', color:['#1a1a1a','#ffffff'], sizes:[],
      img:'assets/products/p006.svg',
      imgs:['assets/products/p006.svg'],
      desc:'دوربین بدون آینه کانن مناسب مبتدیان و حرفه‌ای‌ها' },
    { id:'p007', name:'هودی مردانه نایک Sportswear Club', cat:'fashion', brand:'Nike',
      price:3200000, oldPrice:3800000, discount:16, rating:4.4, reviews:67,
      badge:'', color:['#1a1a1a','#ffffff','#666','#c8102e'], sizes:['S','M','L','XL','XXL'],
      img:'assets/products/p007.svg',
      imgs:['assets/products/p007.svg'],
      desc:'هودی کلاسیک نایک با پارچه نرم و گرم برای روزهای سرد' },
    { id:'p008', name:'کفش آدیداس Ultraboost 23', cat:'fashion', brand:'Adidas',
      price:8900000, oldPrice:10500000, discount:15, rating:4.7, reviews:112,
      badge:'پرفروش', color:['#ffffff','#1a1a1a','#c8102e'], sizes:['40','41','42','43','44','45'],
      img:'assets/products/p008.svg',
      imgs:['assets/products/p008.svg'],
      desc:'کفش دویدن حرفه‌ای آدیداس با فناوری Boost جدید' },
    { id:'p009', name:'اکشن فیگور Demon Slayer Tanjiro', cat:'action', brand:'WMF',
      price:6500000, oldPrice:7800000, discount:17, rating:4.6, reviews:28,
      badge:'تخفیف', color:['#c0c0c0'], sizes:[],
      img:'assets/products/p009.svg',
      imgs:['assets/products/p009.svg'],
      desc:'فیگور کالکتیبل تانجیرو کامادو از انیمه Demon Slayer با جزئیات دقیق و ارتفاع ۲۵ سانتی‌متر' },
    { id:'p010', name:'پاور بانک انکر PowerCore 26800', cat:'digital', brand:'Anker',
      price:2800000, oldPrice:3200000, discount:13, rating:4.8, reviews:189,
      badge:'پرفروش', color:['#1a1a1a','#ffffff'], sizes:[],
      img:'assets/products/p010.svg',
      imgs:['assets/products/p010.svg'],
      desc:'پاور بانک ۲۶۸۰۰ میلی‌آمپر با شارژ سریع PD 65W' },
    { id:'p011', name:'کرم ضدآفتاب Biore UV Aqua Rich', cat:'beauty', brand:'Biore',
      price:850000, oldPrice:1100000, discount:23, rating:4.9, reviews:445,
      badge:'تخفیف ویژه', color:[], sizes:['50ml','90ml'],
      img:'assets/products/p011.svg',
      imgs:['assets/products/p011.svg'],
      desc:'کرم ضدآفتاب ژاپنی با فاکتور SPF50+ بافت آبی سبک' },
    { id:'p012', name:'اکشن فیگور One Piece Monkey D. Luffy', cat:'action', brand:'LEGO',
      price:4200000, oldPrice:5000000, discount:16, rating:4.8, reviews:33,
      badge:'', color:['#c8102e'], sizes:[],
      img:'assets/products/p012.svg',
      imgs:['assets/products/p012.svg'],
      desc:'فیگور لوفی از انیمه One Piece، نسخه Gear 5 با بال و زنجیر قابل تنظیم، ارتفاع ۲۸ سانتی‌متر' },
,
    { id:'p013', name:'اکسسوری گیمینگ Razer Mouse Pad XXL', cat:'accessory', brand:'Razer',
      price:1850000, oldPrice:2200000, discount:16, rating:4.7, reviews:88,
      badge:'پرفروش', color:['#1a1a1a'], sizes:['Standard','XXL'],
      img:'assets/products/p013.svg',
      imgs:['assets/products/p013.svg'],
      desc:'ماوس‌پد XXL ضدآب Razer با لایه لاستیکی ضدلغزش و سطح بافت دقیق برای گیمینگ حرفه‌ای' },
    { id:'p014', name:'اکسسوری گوشی MagSafe Wallet', cat:'accessory', brand:'Apple',
      price:2100000, oldPrice:2600000, discount:19, rating:4.6, reviews:54,
      badge:'', color:['#1a1a1a','#f5c5a3','#e8e8e8'], sizes:[],
      img:'assets/products/p014.svg',
      imgs:['assets/products/p014.svg'],
      desc:'کیف کارت مگ‌سیف اپل با اتصال مغناطیسی، جای ۳ کارت، سازگار با iPhone 12 و بالاتر' }
  
  ];

  const CATS = {
    digital: 'کالای دیجیتال', fashion: 'مد و پوشاک',
    home: 'خانه و آشپزخانه', beauty: 'زیبایی و سلامت', action: 'اکشن فیگور', accessory: 'اکسسوری'
  };

  /* ── STORAGE HELPERS ───────────────────────────────────── */
  function get(key) {
    try { return JSON.parse(localStorage.getItem('ko_' + key) || 'null'); } catch { return null; }
  }
  function set(key, val) {
    try { localStorage.setItem('ko_' + key, JSON.stringify(val)); } catch {}
  }

  /* ── CART ──────────────────────────────────────────────── */
  const Cart = {
    get items() { return get('cart') || []; },
    add(productId, qty=1, opts={}) {
      let items = this.items;
      const existing = items.find(i => i.id === productId && i.color === opts.color && i.size === opts.size);
      if (existing) existing.qty += qty;
      else items.push({ id: productId, qty, ...opts });
      set('cart', items);
      this.updateBadge();
      if (window.showToast) showToast('✓ به سبد خرید افزوده شد', 'success');
    },
    remove(productId) {
      set('cart', this.items.filter(i => i.id !== productId));
      this.updateBadge();
    },
    updateQty(productId, qty) {
      let items = this.items;
      const item = items.find(i => i.id === productId);
      if (item) { item.qty = Math.max(1, qty); set('cart', items); }
    },
    clear() { set('cart', []); this.updateBadge(); },
    get count() { return this.items.reduce((s, i) => s + i.qty, 0); },
    get total() {
      return this.items.reduce((s, i) => {
        const p = PRODUCTS.find(p => p.id === i.id);
        return s + (p ? p.price * i.qty : 0);
      }, 0);
    },
    updateBadge() {
      document.querySelectorAll('.cart-badge').forEach(b => {
        const n = this.count;
        b.textContent = n;
        b.style.display = n > 0 ? '' : 'none';
        b.classList.remove('bump');
        void b.offsetWidth;
        b.classList.add('bump');
      });
    }
  };

  /* ── WISHLIST ───────────────────────────────────────────── */
  const Wishlist = {
    get items() { return get('wishlist') || []; },
    toggle(productId) {
      let items = this.items;
      const idx = items.indexOf(productId);
      if (idx >= 0) { items.splice(idx, 1); typeof showToast === 'function' && showToast('از علاقه‌مندی‌ها حذف شد'); }
      else { items.push(productId); typeof showToast === 'function' && showToast('♥ به علاقه‌مندی‌ها افزوده شد', 'success'); }
      set('wishlist', items);
      return idx < 0; // true = added
    },
    has(productId) { return (get('wishlist') || []).includes(productId); },
    get count() { return this.items.length; }
  };

  /* ── SEARCH ─────────────────────────────────────────────── */
  const Search = {
    query(q) {
      if (!q || q.trim().length < 2) return [];
      const term = q.trim().toLowerCase();
      return PRODUCTS.filter(p =>
        p.name.includes(q) ||
        p.brand.toLowerCase().includes(term) ||
        CATS[p.cat]?.includes(q) ||
        p.desc.includes(q)
      );
    },
    saveRecent(q) {
      let recent = get('searches') || [];
      recent = [q, ...recent.filter(r => r !== q)].slice(0, 8);
      set('searches', recent);
    },
    getRecent() { return get('searches') || []; }
  };

  /* ── PRODUCTS HELPERS ───────────────────────────────────── */
  const Products = {
    all: PRODUCTS,
    byId(id) { return PRODUCTS.find(p => p.id === id); },
    byCat(cat) { return cat === 'all' ? PRODUCTS : PRODUCTS.filter(p => p.cat === cat); },
    featured() { return PRODUCTS.slice(0, 6); },
    similar(id, limit=5) {
      const p = this.byId(id);
      if (!p) return [];
      return PRODUCTS.filter(x => x.id !== id && x.cat === p.cat).slice(0, limit);
    },
    fmt(price) {
      return price.toLocaleString('fa-IR') + ' تومان';
    },
    render(p, opts={}) {
      const wished = Wishlist.has(p.id);
      return `<div class="sp-card" onclick="window.location.href='product.html?id=${p.id}'" style="cursor:pointer">
        <div class="sp-thumb" style="position:relative">
          <img src="${p.img}"
               alt="${p.name}" style="width:80%;padding:12px;object-fit:contain;transition:transform .4s ease"
               loading="lazy"
               onerror="this.onerror=null;this.src='assets/ko-logo.png';this.style.opacity='.2'">
          ${p.badge ? `<span style="position:absolute;top:10px;right:10px;background:var(--orange);color:#fff;font-size:11px;font-weight:700;padding:3px 9px;border-radius:6px">${p.badge}</span>` : ''}
          ${p.discount ? `<span style="position:absolute;top:10px;left:10px;background:#e53935;color:#fff;font-size:11px;font-weight:700;padding:3px 8px;border-radius:6px">${p.discount}٪</span>` : ''}
        </div>
        <div class="sp-body">
          <div class="sp-name">${p.name}</div>
          <div style="display:flex;align-items:center;gap:8px;margin-bottom:4px">
            <div class="sp-price">${KO.Products.fmt(p.price)}</div>
            ${p.oldPrice ? `<div style="font-size:11px;color:#b3b4ba;text-decoration:line-through">${KO.Products.fmt(p.oldPrice)}</div>` : ''}
          </div>
          <div class="sp-rating">
            <svg viewBox="0 0 24 24" fill="#f4b400" width="12" height="12"><polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/></svg>
            ${p.rating} <span style="color:#b3b4ba">(${p.reviews})</span>
          </div>
        </div>
        <div class="sp-actions">
          <button class="sp-action-btn" title="علاقه‌مندی" 
            onclick="event.stopPropagation();this.style.color=KO.Wishlist.toggle('${p.id}')?'#e1452f':'';this.querySelector('path')?.setAttribute('fill',KO.Wishlist.has('${p.id}')?'#e1452f':'none')"
            style="color:${wished ? '#e1452f' : ''}">
            <svg viewBox="0 0 24 24" fill="${wished ? '#e1452f' : 'none'}" width="14" height="14">
              <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78Z" stroke="currentColor" stroke-width="1.5"/>
            </svg>
          </button>
          <button class="sp-action-btn" data-add-cart title="افزودن به سبد"
            onclick="event.stopPropagation();KO.Cart.add('${p.id}')">
            <svg viewBox="0 0 24 24" fill="none" width="14" height="14">
              <path d="M2 3h2l2.4 12.2a2 2 0 0 0 2 1.6h8.2a2 2 0 0 0 2-1.6L21 7H6" stroke="currentColor" stroke-width="1.5"/>
              <circle cx="9" cy="20" r="1.3" fill="currentColor"/><circle cx="17" cy="20" r="1.3" fill="currentColor"/>
            </svg>
          </button>
          <button class="sp-view-btn" onclick="event.stopPropagation();window.location.href='product.html?id=${p.id}'">مشاهده</button>
        </div>
      </div>`;
    }
  };

  /* ── AUTH ───────────────────────────────────────────────── */
  const Auth = {
    get user() { try { return JSON.parse(localStorage.getItem('ko_user')||'null'); } catch { return null; } },
    login(name, role='کاربر عادی') {
      localStorage.setItem('ko_user', JSON.stringify({name, role, loginTime: Date.now()}));
    },
    logout() { localStorage.removeItem('ko_user'); window.location.href = 'login.html'; },
    isLoggedIn() { return !!this.user; },
    guard() {
      if (!this.isLoggedIn()) {
        sessionStorage.setItem('ko_redirect', location.pathname.split('/').pop());
        location.href = 'login.html';
        return false;
      }
      return true;
    }
  };

  /* ── ORDERS (mock) ──────────────────────────────────────── */
  const Orders = {
    get list() { return get('orders') || []; },
    add(order) {
      let orders = this.list;
      order.id = 'KO-' + Date.now().toString().slice(-6);
      order.date = new Date().toLocaleDateString('fa-IR');
      order.status = 'در حال بررسی';
      orders.unshift(order);
      set('orders', orders);
      return order.id;
    },
    get count() { return this.list.length; }
  };

  /* ── INIT ───────────────────────────────────────────────── */
  function init() {
    // Update cart badge on load
    Cart.updateBadge();

    // Update sidebar username if present
    const sbu = document.getElementById('sidebar-username');
    if (sbu && Auth.user) sbu.textContent = Auth.user.name;
  }

  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
  else init();

  return { Cart, Wishlist, Search, Products, Auth, Orders, PRODUCTS, CATS };
})();
