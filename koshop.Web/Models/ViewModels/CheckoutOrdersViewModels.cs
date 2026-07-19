using KoShop.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models.ViewModels;

public class CheckoutViewModel
{
    public List<CartLineViewModel> Lines { get; set; } = new();
    public decimal Subtotal => Lines.Sum(l => l.LineTotal);
    public decimal ShippingCost { get; set; } = 350000;
    public decimal Total => Subtotal + ShippingCost;

    [Required(ErrorMessage = "نام گیرنده را وارد کنید")]
    [Display(Name = "نام گیرنده")]
    public string ReceiverName { get; set; } = string.Empty;

    [Required(ErrorMessage = "شماره تماس را وارد کنید")]
    [Display(Name = "شماره تماس")]
    public string ReceiverPhone { get; set; } = string.Empty;

    [Required(ErrorMessage = "آدرس را وارد کنید")]
    [Display(Name = "آدرس کامل")]
    public string ShippingAddress { get; set; } = string.Empty;
}

public class MyOrdersIndexViewModel
{
    public List<Order> Orders { get; set; } = new();
    public string? StatusFilter { get; set; }
}

public class OrderDetailViewModel
{
    public Order Order { get; set; } = null!;
}

public class TrackOrderViewModel
{
    [Required(ErrorMessage = "کد سفارش را وارد کنید")]
    [Display(Name = "کد سفارش")]
    public string OrderCode { get; set; } = string.Empty;

    public Order? FoundOrder { get; set; }
    public bool Searched { get; set; }
}

public class SubmitOrderViewModel
{
    [Required(ErrorMessage = "نام فروشگاه مبدا را انتخاب کنید")]
    [Display(Name = "فروشگاه مبدا")]
    public string SourceStore { get; set; } = "Taobao";

    [Required(ErrorMessage = "لینک یا مشخصات کالا را وارد کنید")]
    [Display(Name = "لینک محصول یا توضیحات کامل کالا")]
    public string ProductDescription { get; set; } = string.Empty;

    [Range(1, 100)]
    [Display(Name = "تعداد")]
    public int Quantity { get; set; } = 1;

    public string? Notes { get; set; }
}
