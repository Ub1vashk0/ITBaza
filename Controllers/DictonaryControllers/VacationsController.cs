using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class VacationsController : DirectoryControllerBase<Vacation, AppDbContext>
{
    public VacationsController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Vacation> BuildQuery(string? search)
    {
        IQueryable<Vacation> query = _ctx.Vacations
                        .Include(v => v.Department)
                        .Include(v => v.Division);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(v =>
                v.Name.Contains(search) ||
                v.Department.Name.Contains(search) ||
                v.Division.Name.Contains(search));
        }

        return query;
    }

    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.Departments = new SelectList(_ctx.Departments, "Id", "Name");

        int? selectedDepartmentId = (entity as Vacation)?.DepartmentId;
        int? selectedDivisionId = (entity as Vacation)?.DivisionId;

        var divisions = selectedDepartmentId.HasValue
            ? _ctx.Divisions
                .Where(d => d.DepartmentId == selectedDepartmentId)
                .ToList()
            : new List<Division>();
        ViewBag.Divisions = new SelectList(divisions, "Id", "Name", selectedDivisionId);
    }


    protected override async Task<bool> ExistsAsync(Vacation entity)
    {
        return await _ctx.Vacations.AnyAsync(v =>
            v.Name == entity.Name &&
            v.DepartmentId == entity.DepartmentId &&
            v.DivisionId == entity.DivisionId &&
            v.Id != entity.Id);
    }

    [HttpGet("/api/vacations/divisions/{departmentId}")]
    public IActionResult GetDivisionsByDepartment(int departmentId)
    {
        if (!_ctx.Departments.Any(d => d.Id == departmentId))
            return NotFound("Департамент не знайдено");

        var divisions = _ctx.Divisions
            .Where(d => d.DepartmentId == departmentId)
            .Select(d => new { d.Id, d.Name })
            .ToList();

        return Ok(divisions);
    }
}
