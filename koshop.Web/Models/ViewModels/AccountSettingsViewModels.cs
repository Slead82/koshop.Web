using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class AccountSettingsViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string Role { get; set; } = "Customer";

    public List<Address> Addresses { get; set; } = new();
}

public class UpdateProfileViewModel
{
    [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
    [StringLength(150)]
    [Display(Name = "نام و نام خانوادگی")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل را وارد کنید")]
    [EmailAddress(ErrorMessage = "ایمیل معتبر نیست")]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
    [Phone(ErrorMessage = "شماره موبایل معتبر نیست")]
    [Display(Name = "شماره موبایل")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "تاریخ تولد")]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "جنسیت")]
    public string? Gender { get; set; }
}

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "رمز عبور فعلی را وارد کنید")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور فعلی")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور جدید را وارد کنید")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور جدید باید حداقل ۶ کاراکتر باشد")]
    [DataType(DataType.Password)]
    [Display(Name = "رمز عبور جدید")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "تکرار رمز عبور جدید را وارد کنید")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "رمز عبور جدید و تکرار آن یکسان نیستند")]
    [Display(Name = "تکرار رمز عبور جدید")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class AddressesViewModel
{
    public List<Address> Addresses { get; set; } = new();
}

public class AddAddressViewModel
{
    [Required(ErrorMessage = "عنوان آدرس را وارد کنید")]
    [StringLength(100)]
    [Display(Name = "عنوان آدرس")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "آدرس کامل را وارد کنید")]
    [Display(Name = "آدرس کامل")]
    public string FullAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره تماس را وارد کنید")]
    [StringLength(20)]
    [Display(Name = "شماره تماس")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "آدرس پیش‌فرض")]
    public bool IsDefault { get; set; }
}
