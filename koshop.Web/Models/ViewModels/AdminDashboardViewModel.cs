using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingReviews { get; set; }
    public int OpenTickets { get; set; }
    public List<Order> RecentOrders { get; set; } = new();
}
