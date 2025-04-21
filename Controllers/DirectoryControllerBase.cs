using ITBaza.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ITBaza.Models;
using Microsoft.AspNetCore.Mvc.Filters;

[Route("Dictonary/[controller]/{action=Index}")]
public abstract class DirectoryControllerBase<TEntity, TContext> : Controller
    where TEntity : class
    where TContext : DbContext
{
    protected readonly TContext _ctx;

    protected DirectoryControllerBase(TContext ctx)
    {
        _ctx = ctx;
    }

    protected virtual string ViewFolder => $"Views/DictonaryView/{GetFolderName()}";
    protected virtual Task<bool> ExistsAsync(TEntity entity) => Task.FromResult(false);

    private string GetFolderName()
    {
        string name = typeof(TEntity).Name;
        return name switch
        {
            "Country" => "Countries",
            "Person" => "People",
            _ => name + "s"
        };
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.SidebarMenu = DictionaryMenuItem.GetDictionaryMenu();
        base.OnActionExecuting(context);
    }

    protected abstract IQueryable<TEntity> BuildQuery(string? search);

    // ----- INDEX (GET) -----
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> Index(string? search)
    {
        var data = await BuildQuery(search).Take(500).ToListAsync();
        ViewBag.Search = search;
        return View("Index", data);
    }

    // ----- CREATE (GET) -----
    [HttpGet]
    public virtual IActionResult Create()
    {
        var entity = Activator.CreateInstance<TEntity>();
        PrepSelectLists();
        return View("Create", entity);
    }

    // ----- CREATE (POST) -----
    [HttpPost]
    [ValidateAntiForgeryToken]
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
        return View("Create", entity);
    }

    // ----- EDIT (GET) -----
    [HttpGet]
    public virtual async Task<IActionResult> Edit(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        PrepSelectLists(entity);
        return View("Edit", entity);
    }

    // ----- EDIT (POST) -----
    [HttpPost]
    [ValidateAntiForgeryToken]
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
        TempData["Success"] = "Зміни збережено";
        return View("Edit", entity);
    }

    // ----- DELETE (GET) -----
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null) return NotFound();

        return View("Delete", entity);
    }

    // ----- DELETE (POST) -----
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity != null) _ctx.Remove(entity);

        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Запис вилучено";
        return RedirectToAction("Index");
    }

    // ----- DETAILS (GET) -----
    [HttpGet]
    public virtual async Task<IActionResult> Details(int id)
    {
        var entity = await _ctx.Set<TEntity>().FindAsync(id);
        if (entity == null)
            return NotFound();

        PrepSelectLists(entity);
        return View("Details", entity);
    }

    // ----- Віртуальний метод для SelectList -----
    protected virtual void PrepSelectLists(object? entity = null) { }
}
