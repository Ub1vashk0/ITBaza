// ---------------- «using»  ----------------------------------------------
using Microsoft.AspNetCore.Mvc;          // дає базовий клас Controller
using Microsoft.EntityFrameworkCore;     // IQueryable, Include, ToListAsync

namespace ITBaza.Controllers;

/// <summary>
/// Базовий CRUD‑контролер для всіх довідників (Country, Placement …).
/// TEntity — клас‑сутність із первинним ключем Id.
/// TContext — ваш DbContext (AppDbContext).
/// </summary>
public abstract class DirectoryControllerBase<TEntity, TContext> : Controller
    where TEntity : class
    where TContext : DbContext
{
    // ----------------------- DI‑контекст ---------------------------------
    protected readonly TContext _ctx;                       // доступ до БД
    protected DirectoryControllerBase(TContext ctx) => _ctx = ctx;

    // ------------------ абстрактний «Конструктор LINQ» -------------------
    /// <remarks>
    /// Повертає IQueryable із потрібними Include / Where.
    /// Реальна реалізація — у підкласі (CountriesController, …).
    /// </remarks>
    protected abstract IQueryable<TEntity> BuildQuery(string? search);

    /// Назва папки з представленнями.
    /// За замовчанням = ім’я сутності + 's' (Country → Countries).
    protected virtual string ViewFolder =>
        typeof(TEntity).Name + "s";

    // ==========================   ACTION‑МЕТОДИ   ========================

    // ---------- Index (список + пошук) -----------------------------------
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        var data = await BuildQuery(search)   // 1) формуємо запит
                         .Take(500)           // 2) примітивний ліміт на 500
                         .ToListAsync();      // 3) виконуємо SQL
        ViewBag.Search = search;              // повертаємо введений текст
        return View($"{ViewFolder}/Index", data);   // 4) повертаємо View
    }

    // ---------- Create (GET) --------------------------------------------
    public virtual IActionResult Create()
    {
        PrepSelectLists();                    // заповнити <select> якщо треба
        return View($"{ViewFolder}/Create",
                    Activator.CreateInstance<TEntity>()); // порожня модель
    }

    // ---------- Create (POST) -------------------------------------------
    [HttpPost, ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TEntity entity)
    {
        if (!ModelState.IsValid)              // серверна валідація
        {
            PrepSelectLists(entity);
            return View($"{ViewFolder}/Create", entity);
        }

        _ctx.Add(entity);                     // insert
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Запис створено";
        return RedirectToAction(nameof(Index));
    }

    // ---------- Edit (GET) ----------------------------------------------
    public virtual async Task<IActionResult> Edit(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        PrepSelectLists(entity);
        return View($"{ViewFolder}/Edit", entity);
    }

    // ---------- Edit (POST) ---------------------------------------------
    [HttpPost, ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(int id, TEntity entity)
    {
        // Перевіряємо, що hidden‑поле Id і маршрутний id збігаються
        if (id != (int) typeof(TEntity).GetProperty("Id")!
                                     .GetValue(entity)!)
            return NotFound();

        if (!ModelState.IsValid)
        {
            PrepSelectLists(entity);
            return View($"{ViewFolder}/Edit", entity);
        }

        _ctx.Update(entity);                  // update
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Зміни збережено";
        return RedirectToAction(nameof(Index));
    }

    // ---------- Delete (GET) (підтвердження) -----------------------------
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        return View($"{ViewFolder}/Delete", entity);
    }

    // ---------- Delete (POST) -------------------------------------------
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity != null) _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Запис вилучено";
        return RedirectToAction(nameof(Index));
    }

    // ---------- SelectList‑хелпер (перевизначається в підкласі) ----------
    protected virtual void PrepSelectLists(object? entity = null) { }
}
