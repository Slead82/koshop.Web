using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

public class ShopController : Controller
{
    private readonly ApplicationDbContext _db;

    public ShopController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /Shop  or  /Shop?categoryId=2&q=ساعت
    public async Task<IActionResult> Index(int? categoryId, string? q)
    {
        var query = _db.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Where(p => p.Status == "Active")
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q));

        var model = new ShopIndexViewModel
        {
            Products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync(),
            Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync(),
            SelectedCategoryId = categoryId,
            SearchTerm = q
        };

        return View(model);
    }

    // GET /Shop/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.Status == "Active");

        if (product == null) return NotFound();

        var approvedReviews = await _db.Reviews
            .Include(r => r.User)
            .Where(r => r.ProductId == id && r.Status == "Approved")
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var related = await _db.Products
            .Include(p => p.Images)
            .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.Status == "Active")
            .Take(4)
            .ToListAsync();

        var model = new ProductDetailsViewModel
        {
            Product = product,
            ApprovedReviews = approvedReviews,
            RelatedProducts = related,
            ProductId = id
        };

        return View(model);
    }

    // POST /Shop/SubmitReview  — logged-in users only (guests can't submit through this form;
    // matches the "UserId nullable for guest reviews" design, but requiring login here keeps
    // moderation simpler and prevents anonymous spam)
    [HttpPost]
    [Authorize(AuthenticationSchemes = "SiteCookieAuth")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitReview(SubmitReviewViewModel model)
    {
        if (model.Rating < 1 || model.Rating > 5 || string.IsNullOrWhiteSpace(model.Comment))
        {
            TempData["ReviewError"] = "لطفاً امتیاز و متن نظر را کامل وارد کنید";
            return RedirectToAction("Details", new { id = model.ProductId });
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        _db.Reviews.Add(new Review
        {
            ProductId = model.ProductId,
            UserId = userId,
            Rating = model.Rating,
            Comment = model.Comment.Trim(),
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();

        TempData["ReviewSuccess"] = "نظر شما ثبت شد و پس از تأیید نمایش داده می‌شود";
        return RedirectToAction("Details", new { id = model.ProductId });
    }
}
