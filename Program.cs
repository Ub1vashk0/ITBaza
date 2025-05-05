using ITBaza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Додати MVC та шляхи для Views
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/DictonaryView/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/AccountingView/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/AdminView/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
    });

// Хешування паролів
builder.Services.AddScoped<IPasswordHasher<SystemUser>, PasswordHasher<SystemUser>>();

// Контекст бази даних
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додати аутентифікацію через Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Куди скеровувати, якщо не авторизований
        options.AccessDeniedPath = "/Account/AccessDenied"; // Якщо немає прав
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // тривалість авторизації
        options.SlidingExpiration = true; // продовжувати час при активності
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

var app = builder.Build();

// Обробка помилок
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Додано для входу/виходу користувачів
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // атрибутивна маршрутизація

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Account}/{action=Login}/"
    //pattern: "{controller=Home}/{action=Index}/"
    ); // fallback

app.Run();
