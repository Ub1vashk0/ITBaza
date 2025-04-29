using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ITBaza.Models;

[Route("Accounting/[controller]/{action=Index}")]
public abstract class AccountingControllerBase<TEntity, TContext> : Controller
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _ctx;

    protected AccountingControllerBase(TContext ctx)
    {
        _ctx = ctx;
    }

    protected virtual string ViewFolder => $"Views/AccountingView/{GetFolderName()}";

    private string GetFolderName()
    {
        string name = typeof(TEntity).Name;
        return name switch
        {
            "Person" => "People",
            _ => name + "s"
        };
    }

    protected abstract IQueryable<TEntity> BuildQuery(string? search, bool includeDismissed);

    protected virtual Task<bool> ExistsAsync(TEntity entity) => Task.FromResult(false);

    protected virtual void PrepSelectLists(object? entity = null) { }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
        base.OnActionExecuting(context);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index(string? search, bool includeDismissed = false)
    {
        var query = BuildQuery(search, includeDismissed);

        var data = await query
            .Take(500)
            .ToListAsync();

        ViewBag.Search = search;
        ViewBag.IncludeDismissed = includeDismissed;

        return View("Index", data);
    }

    public virtual IActionResult Create()
    {
        var entity = Activator.CreateInstance<TEntity>();
        PrepSelectLists();
        return View("Create", entity);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TEntity entity)
    {
        if (!ModelState.IsValid)
        {
            PrepSelectLists(entity);
            return View("Create", entity);
        }

        if (await ExistsAsync(entity))
        {
            ModelState.AddModelError(string.Empty, "Такий запис уже існує.");
            PrepSelectLists(entity);
            return View("Create", entity);
        }

        _ctx.Add(entity);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Створено успішно!";
        return RedirectToAction(nameof(Index));
    }

    public virtual async Task<IActionResult> Edit(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        PrepSelectLists(entity);
        return View("Edit", entity);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit(int id, TEntity entity)
    {
        if (id != (int)typeof(TEntity).GetProperty("Id")!.GetValue(entity)!)
            return NotFound();

        if (!ModelState.IsValid)
        {
            PrepSelectLists(entity);
            return View("Edit", entity);
        }

        _ctx.Update(entity);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Зміни збережено!";
        return RedirectToAction(nameof(Index));
    }

    public virtual async Task<IActionResult> Details(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        PrepSelectLists(entity);
        return View("Details", entity);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        return View("Delete", entity);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity != null)
        {
            _ctx.Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        TempData["Success"] = "Запис успішно видалено.";
        return RedirectToAction(nameof(Index));
    }
}
