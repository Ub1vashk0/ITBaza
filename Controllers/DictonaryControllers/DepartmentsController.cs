using ITBaza.Models;
using Microsoft.EntityFrameworkCore;

public class DepartmentsController : DirectoryControllerBase<Department, AppDbContext>
{
    public DepartmentsController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Department> BuildQuery(string? search)
    {
        var query = _ctx.Departments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(d => d.Name.Contains(search));

        return query;
    }

    protected override Task<bool> ExistsAsync(Department entity)
    {
        return _ctx.Departments.AnyAsync(d => d.Name == entity.Name);
    }
}
