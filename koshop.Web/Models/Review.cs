using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class Review
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    // Null = guest reviewer (no account)
    public int? UserId { get; set; }
    public User? User { get; set; }

    [Range(1, 5)]
    public byte Rating { get; set; }

    [Required]
    public string Comment { get; set; } = string.Empty;

    // Pending, Approved, Rejected
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ModeratedByAdminId { get; set; }
    public User? ModeratedByAdmin { get; set; }

    // Only set when the reviewer had no account (UserId is null) — display name only
    [StringLength(150)]
    public string? GuestName { get; set; }
}
