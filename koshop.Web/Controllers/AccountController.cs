using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(ApplicationDbContext db, IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    // ---------------- REGISTER ----------------

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Uniqueness checks (Username / Email / PhoneNumber must each be unique, matching the DB constraints)
        bool usernameTaken = await _db.Users.AnyAsync(u => u.Username == model.Username);
        bool emailTaken = await _db.Users.AnyAsync(u => u.Email == model.Email);
        bool phoneTaken = await _db.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber);

        if (usernameTaken) ModelState.AddModelError(nameof(model.Username), "این نام کاربری قبلاً ثبت شده است");
        if (emailTaken) ModelState.AddModelError(nameof(model.Email), "این ایمیل قبلاً ثبت شده است");
        if (phoneTaken) ModelState.AddModelError(nameof(model.PhoneNumber), "این شماره موبایل قبلاً ثبت شده است");

        if (!ModelState.IsValid) return View(model);

        var user = new User
        {
            FullName = model.FullName.Trim(),
            Username = model.Username.Trim(),
            Email = model.Email.Trim(),
            PhoneNumber = model.PhoneNumber.Trim(),
            Role = "Customer",
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        await SignInUserAsync(user, rememberMe: false);

        return RedirectToAction("Index", "Home");
    }

    // ---------------- LOGIN ----------------

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var input = model.UsernameOrEmailOrPhone.Trim();
        var user = await _db.Users.FirstOrDefaultAsync(u =>
            u.Username == input || u.Email == input || u.PhoneNumber == input);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "نام کاربری/ایمیل/موبایل یا رمز عبور اشتباه است");
            return View(model);
        }

        if (user.Status == "Blocked")
        {
            ModelState.AddModelError(string.Empty, "حساب کاربری شما مسدود شده است. با پشتیبانی تماس بگیرید.");
            return View(model);
        }

        var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "نام کاربری/ایمیل/موبایل یا رمز عبور اشتباه است");
            return View(model);
        }

        await SignInUserAsync(user, model.RememberMe);

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            return Redirect(model.ReturnUrl);

        return RedirectToAction("Index", "Home");
    }

    // ---------------- LOGOUT ----------------

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("SiteCookieAuth");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();

    // ---------------- helpers ----------------

    private async Task SignInUserAsync(User user, bool rememberMe)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role),
            new("Username", user.Username),
        };

        var identity = new ClaimsIdentity(claims, "SiteCookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("SiteCookieAuth", principal, new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
        });
    }
}
