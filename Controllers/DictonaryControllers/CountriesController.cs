using ITBaza.Models;
using Microsoft.EntityFrameworkCore;

public class CountriesController : DirectoryControllerBase<Country, AppDbContext>
{
    public CountriesController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Country> BuildQuery(string? search)
    {
        var query = _ctx.Countries.AsQueryable();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search));
        return query;
    }
    protected override async Task<bool> ExistsAsync(Country entity)
    {
        return await _ctx.Countries
            .AnyAsync(c => c.Name.ToLower() == entity.Name.ToLower());
    }

}
