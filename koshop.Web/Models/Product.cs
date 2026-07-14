using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoShop.Web.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(300)]
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal? OldPrice { get; set; }

    public int Stock { get; set; }

    // Active, Draft
    public string Status { get; set; } = "Draft";

    public string? Brand { get; set; }
    public string? Description { get; set; }

    // Cached aggregates, recalculated whenever a review is approved/rejected/deleted
    [Column(TypeName = "decimal(3,2)")]
    public decimal Rating { get; set; }
    public int ReviewCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ProductImage> Images { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}

public class ProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}
