using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<SystemUser> _hasher;

    public AccountController(AppDbContext context, IPasswordHasher<SystemUser> hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        Console.ForegroundColor= ConsoleColor.Red;
        Console.WriteLine("Login POST called");
        Console.ResetColor();
        if (!ModelState.IsValid)
            return View(model); ;

        var user = await _context.SystemUsers.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == model.Login && u.IsActive == true);

        if (user == null || _hasher.VerifyHashedPassword(user, user.PasswordHash ?? "", model.Password) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Невірний логін або пароль");
            return View(model);
        }
        Console.WriteLine(user.FullName);
        // Оновити дату входу
        user.LastLoginDate = DateTime.Now;
        await _context.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? ""),
            new Claim("FullName", user.FullName ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
