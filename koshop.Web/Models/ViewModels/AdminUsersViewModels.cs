using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class AdminUsersIndexViewModel
{
    public List<User> Users { get; set; } = new();
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }
}
