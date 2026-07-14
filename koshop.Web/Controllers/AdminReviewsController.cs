using KoShop.Web.Data;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminReviewsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminReviewsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? status, string? q)
    {
        var query = _db.Reviews.Include(r => r.Product).Include(r => r.User).AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(r => r.Status == status);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(r => (r.Product != null && r.Product.Name.Contains(q)) ||
                                      (r.User != null && r.User.FullName.Contains(q)));

        var model = new AdminReviewsIndexViewModel
        {
            Reviews = await query.OrderByDescending(r => r.CreatedAt).ToListAsync(),
            StatusFilter = status,
            SearchTerm = q
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetStatus(int reviewId, string status)
    {
        var review = await _db.Reviews.FindAsync(reviewId);
        if (review == null) return NotFound();

        review.Status = status;
        review.ModeratedByAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _db.SaveChangesAsync();

        await RecalculateProductRating(review.ProductId);

        TempData["Success"] = status == "Approved" ? "نظر تأیید و در سایت نمایش داده می‌شود" : "نظر رد شد";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int reviewId)
    {
        var review = await _db.Reviews.FindAsync(reviewId);
        if (review == null) return NotFound();

        var productId = review.ProductId;
        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();

        await RecalculateProductRating(productId);

        TempData["Success"] = "نظر حذف شد";
        return RedirectToAction("Index");
    }

    // Keeps Product.Rating / Product.ReviewCount in sync with approved reviews only
    private async Task RecalculateProductRating(int productId)
    {
        var approved = await _db.Reviews.Where(r => r.ProductId == productId && r.Status == "Approved").ToListAsync();
        var product = await _db.Products.FindAsync(productId);
        if (product == null) return;

        product.ReviewCount = approved.Count;
        product.Rating = approved.Count > 0 ? (decimal)approved.Average(r => r.Rating) : 0;
        await _db.SaveChangesAsync();
    }
}
