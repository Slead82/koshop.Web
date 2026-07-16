using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

// Persian labels + badge classes for the 6 order status codes (Models/Order.cs).
public static class OrderStatusFa
{
    public static string Label(string status) => status switch
    {
        OrderStatus.Pending => "در انتظار بررسی",
        OrderStatus.Processing => "در حال پردازش",
        OrderStatus.Purchased => "خریداری شده",
        OrderStatus.Shipped => "ارسال شده",
        OrderStatus.Done => "تحویل شده",
        OrderStatus.Cancelled => "لغو شده",
        _ => status
    };

    // Badge classes used by the my-orders / order-detail front templates.
    public static string BadgeClass(string status) => status switch
    {
        OrderStatus.Pending => "status-pending",
        OrderStatus.Done => "status-done",
        OrderStatus.Cancelled => "status-cancelled",
        _ => "status-active" // Processing / Purchased / Shipped = "in progress"
    };

    // The normal (non-cancelled) lifecycle, in timeline order.
    public static readonly string[] Timeline =
    {
        OrderStatus.Pending, OrderStatus.Processing, OrderStatus.Purchased,
        OrderStatus.Shipped, OrderStatus.Done
    };
}

public class SubmitOrderViewModel
{
    [Required(ErrorMessage = "انتخاب فروشگاه مرجع الزامی است")]
    [StringLength(50)]
    public string StoreName { get; set; } = "Taobao"; // Taobao / 1688 / Tmall / AliExpress / سایر

    [Required(ErrorMessage = "لینک محصول الزامی است")]
    public string ProductLink { get; set; } = string.Empty;

    [StringLength(300)]
    public string? ProductTitle { get; set; }

    [Range(1, 1000, ErrorMessage = "تعداد باید حداقل ۱ باشد")]
    public int Quantity { get; set; } = 1;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class TrackOrderViewModel
{
    public string? OrderCode { get; set; }
    public string? Phone { get; set; }

    public bool Searched { get; set; }
    public string? Error { get; set; }
    public Order? Order { get; set; }
}

public class MyOrdersViewModel
{
    public List<Order> Orders { get; set; } = new();
    public string? StatusFilter { get; set; }

    // Tab counters (computed over ALL of the user's orders, not just the filtered list)
    public int CountAll { get; set; }
    public int CountInProgress { get; set; }
    public int CountPending { get; set; }
    public int CountDone { get; set; }
    public int CountCancelled { get; set; }
}

public class OrderDetailsViewModel
{
    public Order Order { get; set; } = null!;
}
