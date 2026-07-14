using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class AdminLoginViewModel
{
    [Required(ErrorMessage = "نام کاربری را وارد کنید")]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;
}
