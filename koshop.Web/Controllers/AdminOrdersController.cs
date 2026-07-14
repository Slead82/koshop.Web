using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminOrdersController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminOrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /AdminOrders?status=Pending&q=KO-10
    public async Task<IActionResult> Index(string? status, string? q)
    {
        var query = _db.Orders.Include(o => o.User).AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(o => o.Status == status);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(o => o.OrderCode.Contains(q) ||
                                      (o.User != null && (o.User.FullName.Contains(q) || o.User.PhoneNumber.Contains(q))));

        var model = new AdminOrdersIndexViewModel
        {
            Orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync(),
            StatusFilter = status,
            SearchTerm = q
        };

        return View(model);
    }

    // GET /AdminOrders/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return NotFound();

        return View(new AdminOrderDetailViewModel { Order = order });
    }

    // POST /AdminOrders/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(AdminOrderUpdateViewModel model)
    {
        var order = await _db.Orders.FindAsync(model.OrderId);
        if (order == null) return NotFound();

        order.Status = model.Status;
        if (!string.IsNullOrWhiteSpace(model.TrackingCode))
            order.TrackingCode = model.TrackingCode.Trim();

        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _db.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = model.Status,
            ChangedAt = DateTime.UtcNow,
            ChangedByAdminId = adminId
        });

        await _db.SaveChangesAsync();
        TempData["Success"] = "وضعیت سفارش به‌روزرسانی شد";
        return RedirectToAction("Details", new { id = model.OrderId });
    }

    // POST /AdminOrders/UpdateNote
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateNote(AdminOrderNoteViewModel model)
    {
        var order = await _db.Orders.FindAsync(model.OrderId);
        if (order == null) return NotFound();

        order.AdminNote = model.AdminNote;
        await _db.SaveChangesAsync();

        TempData["Success"] = "یادداشت ذخیره شد";
        return RedirectToAction("Details", new { id = model.OrderId });
    }

    // POST /AdminOrders/BulkUpdateStatus  (checkbox selection + one status for all)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkUpdateStatus(int[] orderIds, string newStatus)
    {
        if (orderIds == null || orderIds.Length == 0 || string.IsNullOrEmpty(newStatus))
        {
            TempData["Error"] = "سفارش یا وضعیتی انتخاب نشده است";
            return RedirectToAction("Index");
        }

        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orders = await _db.Orders.Where(o => orderIds.Contains(o.Id)).ToListAsync();
        foreach (var order in orders)
        {
            order.Status = newStatus;
            _db.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = newStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedByAdminId = adminId
            });
        }
        await _db.SaveChangesAsync();

        TempData["Success"] = $"{orders.Count} سفارش به‌روزرسانی شد";
        return RedirectToAction("Index");
    }
}
