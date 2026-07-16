using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminBlogController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminBlogController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminBlogIndexViewModel
        {
            Posts = await _db.BlogPosts.OrderByDescending(p => p.Id).ToListAsync()
        };
        return View(model);
    }

    public IActionResult Create() => View("Form", new AdminBlogFormViewModel());

    public async Task<IActionResult> Edit(int id)
    {
        var post = await _db.BlogPosts.FindAsync(id);
        if (post == null) return NotFound();

        return View("Form", new AdminBlogFormViewModel
        {
            Id = post.Id,
            Title = post.Title,
            Summary = post.Summary,
            Body = post.Body,
            CoverImageUrl = post.CoverImageUrl
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(AdminBlogFormViewModel model, string action)
    {
        if (!ModelState.IsValid) return View("Form", model);

        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var publish = action == "publish";

        BlogPost post;
        if (model.Id.HasValue)
        {
            post = await _db.BlogPosts.FindAsync(model.Id.Value) ?? throw new InvalidOperationException("مقاله یافت نشد");
        }
        else
        {
            post = new BlogPost { AuthorAdminId = adminId };
            _db.BlogPosts.Add(post);
        }

        post.Title = model.Title.Trim();
        post.Summary = model.Summary;
        post.Body = model.Body;
        post.CoverImageUrl = model.CoverImageUrl;
        post.Status = publish ? "Published" : "Draft";
        if (publish && post.PublishedAt == null) post.PublishedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        TempData["Success"] = publish ? "مقاله منتشر شد" : "پیش‌نویس ذخیره شد";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _db.BlogPosts.FindAsync(id);
        if (post == null) return NotFound();

        _db.BlogPosts.Remove(post);
        await _db.SaveChangesAsync();

        TempData["Success"] = "مقاله حذف شد";
        return RedirectToAction("Index");
    }
}
