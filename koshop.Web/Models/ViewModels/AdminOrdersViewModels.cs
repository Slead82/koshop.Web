using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class AdminOrdersIndexViewModel
{
    public List<Order> Orders { get; set; } = new();
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }
}

public class AdminOrderDetailViewModel
{
    public Order Order { get; set; } = null!;
}

public class AdminOrderUpdateViewModel
{
    public int OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TrackingCode { get; set; }
}

public class AdminOrderNoteViewModel
{
    public int OrderId { get; set; }
    public string? AdminNote { get; set; }
}
