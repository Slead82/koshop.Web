using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminProductsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? status, string? q)
    {
        var query = _db.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(p => p.Status == status);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q));

        var model = new AdminProductsIndexViewModel
        {
            Products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync(),
            StatusFilter = status,
            SearchTerm = q
        };
        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new AdminProductFormViewModel { Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync() };
        return View("Form", model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        var model = new AdminProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Price = product.Price,
            OldPrice = product.OldPrice,
            Stock = product.Stock,
            Status = product.Status,
            Brand = product.Brand,
            Description = product.Description,
            ImageUrl = product.Images.OrderBy(i => i.SortOrder).FirstOrDefault()?.ImageUrl,
            Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync()
        };
        return View("Form", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(AdminProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            return View("Form", model);
        }

        Product product;
        if (model.Id.HasValue)
        {
            product = await _db.Products.Include(p => p.Images).FirstAsync(p => p.Id == model.Id.Value);
        }
        else
        {
            product = new Product { CreatedAt = DateTime.UtcNow };
            _db.Products.Add(product);
        }

        product.Name = model.Name.Trim();
        product.CategoryId = model.CategoryId;
        product.Price = model.Price;
        product.OldPrice = model.OldPrice;
        product.Stock = model.Stock;
        product.Status = model.Status;
        product.Brand = model.Brand;
        product.Description = model.Description;

        if (!string.IsNullOrWhiteSpace(model.ImageUrl))
        {
            var firstImage = product.Images.OrderBy(i => i.SortOrder).FirstOrDefault();
            if (firstImage != null)
                firstImage.ImageUrl = model.ImageUrl.Trim();
            else
                product.Images.Add(new ProductImage { ImageUrl = model.ImageUrl.Trim(), SortOrder = 0 });
        }

        await _db.SaveChangesAsync();
        TempData["Success"] = model.Id.HasValue ? "محصول ویرایش شد" : "محصول جدید اضافه شد";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        TempData["Success"] = "محصول حذف شد";
        return RedirectToAction("Index");
    }
}
