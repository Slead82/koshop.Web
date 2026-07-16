using KoShop.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class AdminBlogIndexViewModel
{
    public List<BlogPost> Posts { get; set; } = new();
}

public class AdminBlogFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "عنوان مقاله را وارد کنید")]
    [StringLength(300)]
    public string Title { get; set; } = string.Empty;

    public string? Summary { get; set; }

    [Required(ErrorMessage = "متن مقاله را وارد کنید")]
    public string Body { get; set; } = string.Empty;

    public string? CoverImageUrl { get; set; }
}
