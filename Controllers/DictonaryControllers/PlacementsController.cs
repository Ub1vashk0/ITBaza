using ITBaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class PlacementsController : DirectoryControllerBase<Placement, AppDbContext>
{
    public PlacementsController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Placement> BuildQuery(string? search)
    {
        IQueryable<Placement> query = _ctx.Placements.Include(p => p.Country);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.City.Contains(search) ||
                p.Office.Contains(search) ||
                p.Country.Name.Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.CountryId = new SelectList(_ctx.Countries, "Id", "Name");
    }
    protected override Task<bool> ExistsAsync(Placement entity)
    {
        return _ctx.Placements.AnyAsync(p =>
            p.CountryId == entity.CountryId &&
            p.City == entity.City &&
            p.Office == entity.Office
        );
    }
}
