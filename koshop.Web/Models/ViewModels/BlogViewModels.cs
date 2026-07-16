namespace KoShop.Web.Models.ViewModels;

public class BlogIndexViewModel
{
    public List<BlogPost> Posts { get; set; } = new();
    public string? SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}

public class BlogPostViewModel
{
    public BlogPost Post { get; set; } = null!;
    public List<BlogPost> LatestPosts { get; set; } = new();
}
