using ITBaza.Models.Enums;
using ITBaza.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class PhoneNumbersController : AccountingControllerBase<PhoneNumber, AppDbContext>
{
    public PhoneNumbersController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<PhoneNumber> BuildQuery(string? search, bool includeDismissed)
    {
        var query = _ctx.PhoneNumbers
            .Include(p => p.Operator)
            .AsQueryable(); 

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.Number.Contains(search) ||
                p.Operator.Name.Contains(search) ||
                p.SimSerialNumber.Contains(search)
            );
        }

        return query;
    }
    public override async Task<IActionResult> Details(int id)
    {
        var phoneNumber = await _ctx.PhoneNumbers
            .Include(p => p.Operator)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (phoneNumber == null)
            return NotFound();

        return View("Details", phoneNumber);
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.Operators = new SelectList(_ctx.MobileOperators, "Id", "Name");
    }

    protected override async Task<bool> ExistsAsync(PhoneNumber entity)
    {
        return await _ctx.PhoneNumbers.AnyAsync(p =>
            p.Number == entity.Number &&
            p.Id != entity.Id
        );
    }
}
