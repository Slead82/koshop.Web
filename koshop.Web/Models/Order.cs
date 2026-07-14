using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoShop.Web.Models;

// Kept as plain strings (not a C# enum) on purpose: this project favors simple,
// readable string status codes end-to-end (matches the admin panel's JS layer,
// which already works with these exact string values).
public static class OrderStatus
{
    public const string Pending = "Pending";
    public const string Processing = "Processing";
    public const string Purchased = "Purchased";
    public const string Shipped = "Shipped";
    public const string Done = "Done";
    public const string Cancelled = "Cancelled";
}

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string OrderCode { get; set; } = string.Empty; // e.g. KO-1042

    public int UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(50)]
    public string SourceStore { get; set; } = string.Empty; // Taobao / 1688 / Tmall

    [Column(TypeName = "decimal(18,0)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal ShippingCost { get; set; }

    public string Status { get; set; } = OrderStatus.Pending;

    public string? TrackingCode { get; set; }
    public string? AdminNote { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> Items { get; set; } = new();
    public List<OrderStatusHistory> StatusHistory { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    [Required, StringLength(300)]
    public string ProductNameSnapshot { get; set; } = string.Empty;

    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;

    [Column(TypeName = "decimal(18,0)")]
    public decimal UnitPrice { get; set; }
}

public class OrderStatusHistory
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    [Required, StringLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public int? ChangedByAdminId { get; set; }
    public User? ChangedByAdmin { get; set; }
}
