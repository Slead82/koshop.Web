using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /
    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            HomeContent = await _db.HomePageContents.FirstOrDefaultAsync(),
            Setting = await _db.SiteSettings.FirstOrDefaultAsync(),

            // Root categories with active-product counts (own products + direct children's)
            Categories = await _db.Categories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.Name)
                .Select(c => new HomeCategoryItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = _db.Products.Count(p =>
                        p.Status == "Active" &&
                        (p.CategoryId == c.Id || (p.Category != null && p.Category.ParentId == c.Id)))
                })
                .ToListAsync(),

            // Featured = latest 8 active products
            FeaturedProducts = await _db.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .ToListAsync(),

            // Active banners placed on the home page (Placement values are Persian strings
            // set in AdminBanners: "صفحه اصلی - اسلایدر" / "صفحه اصلی - نوار بالا")
            Banners = await _db.Banners
                .Where(b => b.IsActive && b.Placement.StartsWith("صفحه اصلی"))
                .OrderBy(b => b.Id)
                .ToListAsync()
        };

        return View(model);
    }

    // GET /Home/About
    public async Task<IActionResult> About()
    {
        var content = await _db.AboutPageContents.FirstOrDefaultAsync() ?? new AboutPageContent();
        return View(content);
    }

    // GET /Home/Contact
    public async Task<IActionResult> Contact()
    {
        var model = new ContactViewModel
        {
            Content = await _db.ContactPageContents.FirstOrDefaultAsync(),
            Setting = await _db.SiteSettings.FirstOrDefaultAsync()
        };
        return View(model);
    }

    // POST /Home/ContactSubmit — there is no Tbl_* table for contact messages,
    // so the form only acknowledges receipt (see setup notes).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ContactSubmit(string? fullName, string? email, string? phone, string? message)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(message))
        {
            TempData["Error"] = "لطفاً نام و متن پیام را کامل وارد کنید";
            return RedirectToAction("Contact");
        }

        TempData["Success"] = "پیام شما دریافت شد؛ کارشناسان ما در اسرع وقت با شما تماس خواهند گرفت";
        return RedirectToAction("Contact");
    }

    // GET /Home/Faq
    public async Task<IActionResult> Faq()
    {
        var content = await _db.FaqPageContents.FirstOrDefaultAsync() ?? new FaqPageContent();
        return View(content);
    }

    // GET /Home/Terms
    public async Task<IActionResult> Terms()
    {
        var content = await _db.TermsPageContents.FirstOrDefaultAsync() ?? new TermsPageContent();
        return View(content);
    }

    // GET /Home/Privacy
    public async Task<IActionResult> Privacy()
    {
        var content = await _db.PrivacyPageContents.FirstOrDefaultAsync() ?? new PrivacyPageContent();
        return View(content);
    }

    public IActionResult Error()
    {
        return View();
    }
}
