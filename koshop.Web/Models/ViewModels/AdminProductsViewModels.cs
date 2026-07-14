using KoShop.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class AdminProductsIndexViewModel
{
    public List<Product> Products { get; set; } = new();
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }
}

public class AdminProductFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "نام محصول را وارد کنید")]
    [StringLength(300)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "دسته‌بندی را انتخاب کنید")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "قیمت را وارد کنید")]
    [Range(0, double.MaxValue, ErrorMessage = "قیمت نامعتبر است")]
    public decimal Price { get; set; }

    public decimal? OldPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public string Status { get; set; } = "Active";
    public string? Brand { get; set; }
    public string? Description { get; set; }

    // Simple: one image URL per product for now (multi-image gallery can be a later refinement)
    public string? ImageUrl { get; set; }

    public List<Category> Categories { get; set; } = new();
}
