using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ResourceTypesController : DirectoryControllerBase<ResourceType, AppDbContext>
{
    public ResourceTypesController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<ResourceType> BuildQuery(string? search)
    {
        var query = _ctx.ResourceTypes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r => r.Name.Contains(search));
        }

        return query;
    }

    protected override Task<bool> ExistsAsync(ResourceType entity)
    {
        return _ctx.ResourceTypes.AnyAsync(r => r.Name == entity.Name && r.Id != entity.Id);
    }
}
