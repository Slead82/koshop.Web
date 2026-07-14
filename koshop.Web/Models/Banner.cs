using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class Banner
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Placement { get; set; } = string.Empty;

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public string? LinkUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public int Clicks { get; set; }
}
