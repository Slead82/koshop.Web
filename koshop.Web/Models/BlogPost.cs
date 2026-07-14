using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class BlogPost
{
    public int Id { get; set; }

    [Required, StringLength(300)]
    public string Title { get; set; } = string.Empty;

    public string? Summary { get; set; }

    [Required]
    public string Body { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }

    // Draft, Published
    public string Status { get; set; } = "Draft";

    public int Views { get; set; }

    public DateTime? PublishedAt { get; set; }

    public int AuthorAdminId { get; set; }
    public User? AuthorAdmin { get; set; }
}
