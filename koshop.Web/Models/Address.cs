using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty; // e.g. "آدرس منزل"

    [Required]
    public string FullAddress { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    public bool IsDefault { get; set; }
}
