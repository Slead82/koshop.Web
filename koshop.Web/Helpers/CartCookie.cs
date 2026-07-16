using System.Text.Json;

namespace KoShop.Web.Helpers;

// One line of the cookie cart: which product and how many.
public record CartLine(int ProductId, int Qty);

// Cookie-based cart storage — no Session, no DB table. The cookie value is a
// JSON array of { ProductId, Qty } pairs; product data is always re-read from
// the DB when the cart is displayed, so prices/stock are never stale.
public static class CartCookie
{
    public const string CookieName = "KoShop.Cart";

    public static List<CartLine> Read(HttpRequest request)
    {
        var raw = request.Cookies[CookieName];
        if (string.IsNullOrWhiteSpace(raw)) return new List<CartLine>();

        try
        {
            var lines = JsonSerializer.Deserialize<List<CartLine>>(raw);
            return lines?.Where(l => l.ProductId > 0 && l.Qty > 0).ToList() ?? new List<CartLine>();
        }
        catch (JsonException)
        {
            // Corrupted/hand-edited cookie — treat as an empty cart.
            return new List<CartLine>();
        }
    }

    public static void Write(HttpResponse response, List<CartLine> lines)
    {
        response.Cookies.Append(CookieName, JsonSerializer.Serialize(lines), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,   // server-rendered only; no client JS needs to read it
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });
    }

    public static void Clear(HttpResponse response)
    {
        response.Cookies.Delete(CookieName, new CookieOptions { Path = "/" });
    }
}
