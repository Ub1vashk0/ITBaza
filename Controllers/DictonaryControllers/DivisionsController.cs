using ITBaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class DivisionsController : DirectoryControllerBase<Division, AppDbContext>
{
    public DivisionsController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Division> BuildQuery(string? search)
    {
        IQueryable<Division> query = _ctx.Divisions.Include(d => d.Department);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(d =>
                d.Name.Contains(search) ||
                d.Department.Name.Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.DepartmentId = new SelectList(_ctx.Departments, "Id", "Name");
    }

    protected override async Task<bool> ExistsAsync(Division entity)
    {
        return await _ctx.Divisions.AnyAsync(d =>
            d.Name == entity.Name &&
            d.DepartmentId == entity.DepartmentId &&
            d.Id != (entity.Id == 0 ? -1 : entity.Id));
    }
}
