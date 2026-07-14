using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class ShopIndexViewModel
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public int? SelectedCategoryId { get; set; }
    public string? SearchTerm { get; set; }
}

public class ProductDetailsViewModel
{
    public Product Product { get; set; } = null!;
    public List<Review> ApprovedReviews { get; set; } = new();
    public List<Product> RelatedProducts { get; set; } = new();

    // For the "write a review" form
    public int ProductId { get; set; }
}

public class SubmitReviewViewModel
{
    public int ProductId { get; set; }
    public byte Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}
