using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class Category
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }
    public Category? Parent { get; set; }

    public List<Category> Children { get; set; } = new();
    public List<Product> Products { get; set; } = new();
}
