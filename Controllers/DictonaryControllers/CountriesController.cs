using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITBaza.Controllers.DictonaryControllers
{
    public class CountriesController : DirectoryControllerBase<Country, AppDbContext>
    {
        public CountriesController(AppDbContext ctx) : base(ctx) { }

        protected override IQueryable<Country> BuildQuery(string? search)
        {
            return _ctx.Countries
                       .Where(c => string.IsNullOrEmpty(search) ||
                                   c.Name.Contains(search));
        }
    }
}
