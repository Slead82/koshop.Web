namespace KoShop.Web.Models.ViewModels;

// Stored in session as Dictionary<int productId, int quantity> under the key "Cart"
public class CartLineViewModel
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int Stock { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public class CartIndexViewModel
{
    public List<CartLineViewModel> Lines { get; set; } = new();
    public decimal Subtotal => Lines.Sum(l => l.LineTotal);
}
