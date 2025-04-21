using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ResourceRolesController : DirectoryControllerBase<ResourceRole, AppDbContext>
{
    public ResourceRolesController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<ResourceRole> BuildQuery(string? search)
    {
        IQueryable<ResourceRole> query = _ctx.ResourceRoles.Include(r => r.Resource);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                r.RoleName.Contains(search) ||
                r.Resource.Name.Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        var selectedResourceId = (entity as ResourceRole)?.ResourceId;
        ViewBag.Resources = new SelectList(_ctx.Resources, "Id", "Name", selectedResourceId);
    }

    protected override async Task<bool> ExistsAsync(ResourceRole entity)
    {
        return await _ctx.ResourceRoles.AnyAsync(r =>
            r.ResourceId == entity.ResourceId &&
            r.RoleName == entity.RoleName &&
            r.Id != entity.Id);
    }
}
