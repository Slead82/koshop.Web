using KoShop.Web.Data;
using KoShop.Web.Helpers;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "SiteCookieAuth")]
public class CheckoutController : Controller
{
    private const string CartSessionKey = "Cart";
    private readonly ApplicationDbContext _db;

    public CheckoutController(ApplicationDbContext db)
    {
        _db = db;
    }

    private Dictionary<int, int> GetCartDict() =>
        HttpContext.Session.GetObject<Dictionary<int, int>>(CartSessionKey) ?? new Dictionary<int, int>();

    public async Task<IActionResult> Index()
    {
        var cart = GetCartDict();
        if (cart.Count == 0) return RedirectToAction("Index", "Cart");

        var model = await BuildModel(cart);

        // pre-fill receiver info from the user's profile / default address if available
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        var defaultAddress = await _db.Addresses.FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);

        model.ReceiverName = user?.FullName ?? "";
        model.ReceiverPhone = user?.PhoneNumber ?? "";
        model.ShippingAddress = defaultAddress?.FullAddress ?? "";

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
    {
        var cart = GetCartDict();
        if (cart.Count == 0) return RedirectToAction("Index", "Cart");

        if (!ModelState.IsValid)
        {
            var refilled = await BuildModel(cart);
            model.Lines = refilled.Lines;
            return View("Index", model);
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var products = await _db.Products.Where(p => cart.Keys.Contains(p.Id)).ToListAsync();

        var order = new Order
        {
            OrderCode = "KO-" + DateTime.UtcNow.Ticks.ToString().Substring(10),
            UserId = userId,
            SourceStore = "KO Shop",
            TotalAmount = products.Sum(p => p.Price * cart[p.Id]) + 350000,
            ShippingCost = 350000,
            Status = OrderStatus.Pending,
            ReceiverName = model.ReceiverName.Trim(),
            ReceiverPhone = model.ReceiverPhone.Trim(),
            ShippingAddress = model.ShippingAddress.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        foreach (var p in products)
        {
            order.Items.Add(new OrderItem
            {
                ProductNameSnapshot = p.Name,
                ProductId = p.Id,
                Quantity = cart[p.Id],
                UnitPrice = p.Price
            });
            // reduce stock
            p.Stock = Math.Max(0, p.Stock - cart[p.Id]);
        }

        order.StatusHistory.Add(new OrderStatusHistory { Status = OrderStatus.Pending, ChangedAt = DateTime.UtcNow });

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        HttpContext.Session.Remove(CartSessionKey);

        return RedirectToAction("Details", "Orders", new { id = order.Id });
    }

    private async Task<CheckoutViewModel> BuildModel(Dictionary<int, int> cart)
    {
        var products = await _db.Products.Include(p => p.Images).Where(p => cart.Keys.Contains(p.Id)).ToListAsync();
        var model = new CheckoutViewModel();
        foreach (var p in products)
        {
            model.Lines.Add(new CartLineViewModel
            {
                ProductId = p.Id,
                Name = p.Name,
                ImageUrl = p.Images.OrderBy(i => i.SortOrder).FirstOrDefault()?.ImageUrl ?? "/assets/ko-logo.png",
                UnitPrice = p.Price,
                Quantity = cart[p.Id],
                Stock = p.Stock
            });
        }
        return model;
    }
}
