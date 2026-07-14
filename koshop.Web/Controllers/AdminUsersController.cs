using KoShop.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoShop.Web.Models.ViewModels;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminUsersController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminUsersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? status, string? q)
    {
        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(u => u.Status == status);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(u => u.FullName.Contains(q) || u.PhoneNumber.Contains(q) || u.Email.Contains(q));

        var model = new AdminUsersIndexViewModel
        {
            Users = await query.OrderByDescending(u => u.CreatedAt).ToListAsync(),
            StatusFilter = status,
            SearchTerm = q
        };

        return View(model);
    }

    // POST /AdminUsers/ChangeRole — deliberately separate from block/unblock (two independent actions)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeRole(int userId, string newRole)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var allowedRoles = new[] { "Customer", "VipCustomer", "Support", "Admin" };
        if (!allowedRoles.Contains(newRole))
        {
            TempData["Error"] = "نقش نامعتبر است";
            return RedirectToAction("Index");
        }

        user.Role = newRole;
        await _db.SaveChangesAsync();

        TempData["Success"] = $"نقش «{user.FullName}» به‌روزرسانی شد";
        return RedirectToAction("Index");
    }

    // POST /AdminUsers/ToggleBlock — independent of role
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleBlock(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();

        user.Status = user.Status == "Active" ? "Blocked" : "Active";
        await _db.SaveChangesAsync();

        TempData["Success"] = $"وضعیت «{user.FullName}» به‌روزرسانی شد";
        return RedirectToAction("Index");
    }
}
