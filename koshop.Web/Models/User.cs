using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class User
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    // Never store or display the raw password — only the hash produced by IPasswordHasher<User>
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? AvatarUrl { get; set; }

    // Customer, VipCustomer, Support, Admin — deliberately separate from Status
    public string Role { get; set; } = "Customer";

    // Active, Blocked — independent of Role (matches the admin panel's
    // "role change" vs "block/unblock" being two separate actions)
    public string Status { get; set; } = "Active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Address> Addresses { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}
