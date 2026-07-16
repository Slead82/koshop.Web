using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class HomeIndexViewModel
{
    public HomePageContent? HomeContent { get; set; }
    public SiteSetting? Setting { get; set; }
    public List<HomeCategoryItem> Categories { get; set; } = new();
    public List<Product> FeaturedProducts { get; set; } = new();
    public List<Banner> Banners { get; set; } = new();
}

// Root category + count of active products (its own + direct children's)
public class HomeCategoryItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}

public class ContactViewModel
{
    public ContactPageContent? Content { get; set; }
    public SiteSetting? Setting { get; set; }
}
