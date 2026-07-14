using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class AdminReviewsIndexViewModel
{
    public List<Review> Reviews { get; set; } = new();
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }
}
