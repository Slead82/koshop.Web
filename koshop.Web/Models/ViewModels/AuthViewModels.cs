using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "شماره موبایل یا ایمیل را وارد کنید")]
    [Display(Name = "شماره موبایل یا ایمیل")]
    public string UsernameOrEmailOrPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }

    // Where to send the user back to after a successful login
    // (set automatically when an anonymous user is redirected here from a protected page)
    public string? ReturnUrl { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
    [StringLength(150)]
    [Display(Name = "نام و نام خانوادگی")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "نام کاربری را وارد کنید")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "نام کاربری باید حداقل ۳ کاراکتر باشد")]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
    [Phone(ErrorMessage = "شماره موبایل معتبر نیست")]
    [Display(Name = "شماره موبایل")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور را وارد کنید")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "رمز عبور و تکرار آن یکسان نیستند")]
    [Display(Name = "تکرار رمز عبور")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
