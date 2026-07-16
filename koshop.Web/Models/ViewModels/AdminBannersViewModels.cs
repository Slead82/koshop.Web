using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class AdminBannerFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "عنوان بنر را وارد کنید")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "محل نمایش را انتخاب کنید")]
    public string Placement { get; set; } = "صفحه اصلی - اسلایدر";

    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
