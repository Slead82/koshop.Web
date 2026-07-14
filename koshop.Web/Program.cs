using KoShop.Web.Data;
using KoShop.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- Services ----------

// MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

// Database (Entity Framework Core -> SQL Server, using the tables already created in SSMS)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Password hashing utility (standalone, does not require full ASP.NET Identity)
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Cookie-based authentication for the SITE (customers)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "SiteCookieAuth";
})
.AddCookie("SiteCookieAuth", options =>
{
    options.Cookie.Name = "KoShop.Auth";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
})
// Separate cookie scheme for the ADMIN panel, so an admin session and a
// customer session on the same browser never get mixed up with each other.
.AddCookie("AdminCookieAuth", options =>
{
    options.Cookie.Name = "KoShop.AdminAuth";
    options.LoginPath = "/AdminAccount/Login";
    options.AccessDeniedPath = "/AdminAccount/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// ---------- Middleware pipeline ----------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // serves everything under /wwwroot (css, js, images — the existing KO Shop front-end assets)

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
