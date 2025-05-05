using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AccessesController : AccountingControllerBase<Access, AppDbContext>
{
    public AccessesController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Access> BuildQuery(string? search, bool includeDismissed)
    {
        var query = _ctx.Accesses
            .Include(a => a.Resource)
            .Include(a => a.ResourceRole)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(a =>
                a.Login.Contains(search) ||
                a.Resource.Name.Contains(search) ||
                a.ResourceRole.RoleName.Contains(search) ||
                a.Id.ToString().Contains(search)
            );
        }

        return query;
    }
    public override async Task<IActionResult> Details(int id)
    {
        var access = await _ctx.Accesses
            .Include(a => a.Resource)
                .ThenInclude(r => r.ResourceType)
            .Include(a => a.Resource)
                .ThenInclude(r => r.ResponsiblePerson)
            .Include(a => a.ResourceRole)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (access == null)
            return NotFound();

        return View("Details", access);
    }
    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.Resources = new SelectList(_ctx.Resources, "Id", "Name");
        ViewBag.ResourceRoles = new SelectList(_ctx.ResourceRoles, "Id", "RoleName");
    }

    protected override async Task<bool> ExistsAsync(Access entity)
    {
        return await _ctx.Accesses.AnyAsync(a =>
            a.Login == entity.Login &&
            a.ResourceId == entity.ResourceId &&
            a.ResourceRoleId == entity.ResourceRoleId &&
            a.Id != entity.Id);
    }
    
}
[Route("api/resources")]
[ApiController]
public class ResourcesApiController : ControllerBase
{
    private readonly AppDbContext _context;
    public ResourcesApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetResourceInfo(int id)
    {
        var resource = await _context.Resources
            .Include(r => r.ResourceType)
            .Include(r => r.ResponsiblePerson)
            .FirstOrDefaultAsync(r => r.Id == id);
        Console.WriteLine(resource.Name);
        if (resource == null) return NotFound();

        return Ok(new
        {
            resource.Id,
            resource.Name,
            Description = resource.Description,
            ResourceType = resource.ResourceType?.Name,
            Location = resource.MainLocation,
            Responsible = $"{resource.ResponsiblePerson?.LastName} {resource.ResponsiblePerson?.FirstName}"
        });
    }
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRolesForResource(int id)
    {
        var roles = await _context.ResourceRoles
            .Where(rr => rr.ResourceId == id)
            .Select(rr => new { rr.Id, rr.RoleName })
            .ToListAsync();

        return Ok(roles);
    }

}
