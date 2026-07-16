using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminBannersController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminBannersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var banners = await _db.Banners.OrderByDescending(b => b.Id).ToListAsync();
        return View(banners);
    }

    public IActionResult Create() => View("Form", new AdminBannerFormViewModel());

    public async Task<IActionResult> Edit(int id)
    {
        var b = await _db.Banners.FindAsync(id);
        if (b == null) return NotFound();

        return View("Form", new AdminBannerFormViewModel
        {
            Id = b.Id,
            Title = b.Title,
            Placement = b.Placement,
            ImageUrl = b.ImageUrl,
            LinkUrl = b.LinkUrl,
            IsActive = b.IsActive
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(AdminBannerFormViewModel model)
    {
        if (!ModelState.IsValid) return View("Form", model);

        Banner banner;
        if (model.Id.HasValue)
        {
            banner = await _db.Banners.FindAsync(model.Id.Value) ?? throw new InvalidOperationException("Banner not found");
        }
        else
        {
            banner = new Banner();
            _db.Banners.Add(banner);
        }

        banner.Title = model.Title.Trim();
        banner.Placement = model.Placement;
        banner.ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl) ? "/assets/ko-logo.png" : model.ImageUrl.Trim();
        banner.LinkUrl = model.LinkUrl;
        banner.IsActive = model.IsActive;

        await _db.SaveChangesAsync();
        TempData["Success"] = "بنر ذخیره شد";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var b = await _db.Banners.FindAsync(id);
        if (b == null) return NotFound();

        b.IsActive = !b.IsActive;
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _db.Banners.FindAsync(id);
        if (b == null) return NotFound();

        _db.Banners.Remove(b);
        await _db.SaveChangesAsync();
        TempData["Success"] = "بنر حذف شد";
        return RedirectToAction("Index");
    }
}
