using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _db;

    public OrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /Orders/MyOrders
    [Authorize(AuthenticationSchemes = "SiteCookieAuth")]
    public async Task<IActionResult> MyOrders(string? status)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = _db.Orders.Where(o => o.UserId == userId).AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(o => o.Status == status);

        var model = new MyOrdersIndexViewModel
        {
            Orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync(),
            StatusFilter = status
        };
        return View(model);
    }

    // GET /Orders/Details/5  — must belong to the current user
    [Authorize(AuthenticationSchemes = "SiteCookieAuth")]
    public async Task<IActionResult> Details(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var order = await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null) return NotFound();
        return View(new OrderDetailViewModel { Order = order });
    }

    // GET /Orders/Track  — public, no login required, search by order code
    [HttpGet]
    public IActionResult Track() => View(new TrackOrderViewModel());

    [HttpGet]
    public async Task<IActionResult> TrackResult(string orderCode)
    {
        var model = new TrackOrderViewModel { OrderCode = orderCode, Searched = true };
        if (!string.IsNullOrWhiteSpace(orderCode))
        {
            model.FoundOrder = await _db.Orders
                .Include(o => o.StatusHistory)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode.Trim());
        }
        return View("Track", model);
    }

    // GET/POST /Orders/Submit — request an item from an external reference store (Taobao/1688/Tmall)
    [Authorize(AuthenticationSchemes = "SiteCookieAuth")]
    [HttpGet]
    public IActionResult Submit() => View(new SubmitOrderViewModel());

    [Authorize(AuthenticationSchemes = "SiteCookieAuth")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(SubmitOrderViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var order = new Order
        {
            OrderCode = "KO-" + DateTime.UtcNow.Ticks.ToString().Substring(10),
            UserId = userId,
            SourceStore = model.SourceStore,
            TotalAmount = 0, // price isn't known yet — admin fills it in after checking the reference store
            ShippingCost = 0,
            Status = OrderStatus.Pending,
            AdminNote = model.Notes,
            CreatedAt = DateTime.UtcNow
        };
        order.Items.Add(new OrderItem
        {
            ProductNameSnapshot = model.ProductDescription.Trim(),
            Quantity = model.Quantity,
            UnitPrice = 0
        });
        order.StatusHistory.Add(new OrderStatusHistory { Status = OrderStatus.Pending, ChangedAt = DateTime.UtcNow });

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"سفارش شما با کد {order.OrderCode} ثبت شد. تیم ما به‌زودی قیمت نهایی را بررسی و اعلام می‌کند.";
        return RedirectToAction("Details", new { id = order.Id });
    }
}
