using KoShop.Web.Data;
using KoShop.Web.Helpers;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Controllers;

public class CartController : Controller
{
    private const string CartSessionKey = "Cart";
    private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db)
    {
        _db = db;
    }

    private Dictionary<int, int> GetCartDict() =>
        HttpContext.Session.GetObject<Dictionary<int, int>>(CartSessionKey) ?? new Dictionary<int, int>();

    private void SaveCartDict(Dictionary<int, int> cart) =>
        HttpContext.Session.SetObject(CartSessionKey, cart);

    // GET /Cart
    public async Task<IActionResult> Index()
    {
        var cart = GetCartDict();
        var model = await BuildViewModel(cart);
        return View(model);
    }

    // POST /Cart/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null || product.Status != "Active")
        {
            TempData["CartError"] = "این محصول در دسترس نیست";
            return RedirectToAction("Index", "Shop");
        }

        var cart = GetCartDict();
        cart[productId] = cart.GetValueOrDefault(productId, 0) + Math.Max(1, quantity);
        SaveCartDict(cart);

        TempData["CartSuccess"] = "به سبد خرید اضافه شد";
        return RedirectToAction("Index", "Cart");
    }

    // POST /Cart/UpdateQuantity
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        var cart = GetCartDict();
        if (quantity <= 0) cart.Remove(productId);
        else cart[productId] = quantity;
        SaveCartDict(cart);
        return RedirectToAction("Index");
    }

    // POST /Cart/Remove
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        var cart = GetCartDict();
        cart.Remove(productId);
        SaveCartDict(cart);
        return RedirectToAction("Index");
    }

    private async Task<CartIndexViewModel> BuildViewModel(Dictionary<int, int> cart)
    {
        var model = new CartIndexViewModel();
        if (cart.Count == 0) return model;

        var productIds = cart.Keys.ToList();
        var products = await _db.Products.Include(p => p.Images)
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

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
