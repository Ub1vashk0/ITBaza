using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ResourcesController : DirectoryControllerBase<Resource, AppDbContext>
{
    public ResourcesController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Resource> BuildQuery(string? search)
    {
        IQueryable<Resource> query = _ctx.Resources
            .Include(r => r.ResourceType)
            .Include(r => r.ResponsiblePerson);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(r =>
                r.Name.Contains(search) ||
                r.MainLocation.Contains(search) ||
                r.AltLocation.Contains(search) ||
                r.ResourceType.Name.Contains(search) ||
                (r.ResponsiblePerson.FirstName + " " +
                 r.ResponsiblePerson.LastName + " " +
                 r.ResponsiblePerson.MiddleName).Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.ResourceTypes = new SelectList(_ctx.ResourceTypes, "Id", "Name");
        ViewBag.ResponsiblePeople = new SelectList(
            _ctx.People
                .Where(p => p.IsActive)
                .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName }),
            "Id", "FullName");
    }

    protected override async Task<bool> ExistsAsync(Resource entity)
    {
        return await _ctx.Resources.AnyAsync(r =>
            r.Name == entity.Name &&
            r.MainLocation == entity.MainLocation &&
            r.ResponsiblePersonId == entity.ResponsiblePersonId &&
            r.ResourceTypeId == entity.ResourceTypeId &&
            r.Id != entity.Id);
    }
}