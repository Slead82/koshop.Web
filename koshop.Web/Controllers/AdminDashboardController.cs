using KoShop.Web.Data;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminDashboardController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminDashboardController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalUsers = await _db.Users.CountAsync(u => u.Status == "Active"),
            TotalOrders = await _db.Orders.CountAsync(),
            TotalRevenue = await _db.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0,
            PendingReviews = await _db.Reviews.CountAsync(r => r.Status == "Pending"),
            OpenTickets = await _db.SupportTickets.CountAsync(t => t.Status == "Open"),
            RecentOrders = await _db.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}
