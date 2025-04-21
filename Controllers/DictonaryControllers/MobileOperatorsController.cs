using ITBaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class MobileOperatorsController : DirectoryControllerBase<MobileOperator, AppDbContext>
{
    public MobileOperatorsController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<MobileOperator> BuildQuery(string? search)
    {
        IQueryable<MobileOperator> query = _ctx.MobileOperators
                        .Include(m => m.IdCountryNavigation);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(m =>
                m.Name.Contains(search) ||
                m.Corporation.Contains(search) ||
                m.Code.Contains(search) ||
                m.IdCountryNavigation.Name.Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        int? selectedCountryId = (entity as MobileOperator)?.IdCountry;
        ViewBag.Countries = new SelectList(_ctx.Countries, "Id", "Name", selectedCountryId);
    }

    protected override async Task<bool> ExistsAsync(MobileOperator entity)
    {
        return await _ctx.MobileOperators.AnyAsync(m =>
            m.Name == entity.Name &&
            m.Code == entity.Code &&
            m.IdCountry == entity.IdCountry &&
            m.Id != entity.Id);
    }
}
