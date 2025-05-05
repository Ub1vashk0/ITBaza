using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

public class PeopleController : AccountingControllerBase<Person, AppDbContext>
{
    public PeopleController(AppDbContext ctx) : base(ctx) { }

    protected override IQueryable<Person> BuildQuery(string? search, bool includeDismissed)
    {
        var query = _ctx.People
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Department)  // Підтягуємо департамент
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Division)    // Підтягуємо відділ
            .Include(p => p.Placement)   
            .AsQueryable();

        if (!includeDismissed)
        {
            query = query.Where(p => p.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                p.LastName.Contains(search) ||
                p.FirstName.Contains(search) ||
                (p.MiddleName != null && p.MiddleName.Contains(search)) ||
                (p.Vacation != null && p.Vacation.Name.Contains(search)) ||
                (p.Placement != null && p.Placement.City.Contains(search))
            );
        }

        return query;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Create(Person person)
    {
        if (ModelState.IsValid)
        {
            // Автоматичне встановлення IsActive
            person.IsActive = !person.DismissalDate.HasValue;

            _ctx.Add(person);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(person);
    }
    protected override void PrepSelectLists(object? entity = null)
    {
        ViewBag.Departments = new SelectList(_ctx.Departments, "Id", "Name");
        ViewBag.Countries = new SelectList(_ctx.Countries, "Id", "Name");
    }
    public override async Task<IActionResult> Details(int id)
    {
        var person = await _ctx.People
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Department)
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Division)
            .Include(p => p.Placement)
                .ThenInclude(p => p.Country)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
        {
            return NotFound();
        }

        return View("Details", person);
    }
    public override async Task<IActionResult> Edit(int id)
    {
        var person = await _ctx.People
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Division)
            .Include(p => p.Vacation)
                .ThenInclude(v => v.Department)
            .Include(p => p.Placement)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null)
            return NotFound();

        await LoadEditSelectLists(person);
        return View("Edit", person);
    }

    protected override async Task<bool> ExistsAsync(Person entity)
    {
        return await _ctx.People.AnyAsync(p =>
            p.FirstName == entity.FirstName &&
            p.LastName == entity.LastName &&
            p.BirthDate == entity.BirthDate &&
            p.Id != entity.Id
        );
    }
    private async Task LoadEditSelectLists(Person person)
    {
        PrepSelectLists();
        Console.WriteLine("WORK METHOD");
        int? selectedDepartmentId = person?.Vacation?.Division?.DepartmentId;
        int? selectedDivisionId = person?.Vacation?.DivisionId;
        int? selectedVacationId = person?.VacationId;

        ViewBag.SelectedDepartmentId = selectedDepartmentId?.ToString();
        ViewBag.SelectedDivisionId = selectedDivisionId?.ToString();
        ViewBag.SelectedVacationId = selectedVacationId?.ToString();

        if (selectedDepartmentId.HasValue)
        {
            var divisions = await _ctx.Divisions
                .Where(d => d.DepartmentId == selectedDepartmentId)
                .ToListAsync();
            ViewBag.Divisions = new SelectList(divisions, "Id", "Name", selectedDivisionId);
        }

        if (selectedDivisionId.HasValue)
        {
            var vacations = await _ctx.Vacations
                .Where(v => v.DivisionId == selectedDivisionId)
                .ToListAsync();
            ViewBag.Vacations = new SelectList(vacations, "Id", "Name", selectedVacationId);
        }

        ViewBag.Countries = new SelectList(_ctx.Countries, "Id", "Name");

        int? selectedCountryId = person?.Placement?.CountryId;
        string? selectedCityName = person?.Placement?.City;
        string? selectedOfficeName = person?.Placement?.Office;

        ViewBag.SelectedCountryId = selectedCountryId?.ToString();
        ViewBag.SelectedCityName = selectedCityName;
        ViewBag.SelectedOfficeName = selectedOfficeName;

        if (selectedCountryId.HasValue)
        {
            var cities = await _ctx.Placements
                .Where(p => p.CountryId == selectedCountryId)
                .ToListAsync();

            ViewBag.Cities = new SelectList(cities, "City", "City", selectedCityName);
        }

        if (!string.IsNullOrEmpty(selectedCityName))
        {
            var offices = await _ctx.Placements
                .Where(p => p.City == selectedCityName)
                .ToListAsync();

            ViewBag.Offices = new SelectList(offices, "Office", "Office", selectedOfficeName);
        }
    }

}
[Route("api/people")]
[ApiController]
public class PeopleApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public PeopleApiController(AppDbContext context)
    {
        _context = context;
    }

    // --- Отримати відділи за департаментом (залишаємо для повноти) ---
    [HttpGet("get-divisions/{departmentId}")]
    public async Task<IActionResult> GetDivisionsByDepartment(int departmentId)
    {
        var divisions = await _context.Divisions
            .Where(d => d.DepartmentId == departmentId)
            .Select(d => new { d.Id, d.Name })
            .ToListAsync();

        return Ok(divisions);
    }

    // --- Отримати вакансії за відділом ---
    [HttpGet("get-vacations/{divisionId}")]
    public async Task<IActionResult> GetVacationsByDivision(int divisionId)
    {
        var vacations = await _context.Vacations
            .Where(v => v.DivisionId == divisionId)
            .Select(v => new { v.Id, v.Name })
            .ToListAsync();

        return Ok(vacations);
    }
    [HttpGet("get-cities/{countryId}")]
    public async Task<IActionResult> GetCitiesByCountry(int countryId)
    {
        var cities = await _context.Placements
            .Where(p => p.CountryId == countryId)
            .Select(p => p.City)
            .Distinct()
            .ToListAsync();

        var cityList = cities.Select(city => new { name = city }).ToList();

        return Ok(cityList);
    }

    [HttpGet("get-offices/{city}")]
    public async Task<IActionResult> GetOfficesByCity(string city)
    {
        var offices = await _context.Placements
            .Where(p => p.City == city && p.Office != null)
            .Select(p => p.Office)
            .Distinct()
            .ToListAsync();

        var officeList = offices.Select(office => new { name = office }).ToList();

        return Ok(officeList);
    }

    [HttpGet("get-placement")]
    public async Task<IActionResult> GetPlacement([FromQuery] int countryId, [FromQuery] string city, [FromQuery] string? office)
    {
        if (string.IsNullOrEmpty(city))
            return BadRequest("Місто обов'язкове для пошуку Placement");

        var query = _context.Placements
            .Where(p => p.CountryId == countryId && p.City == city);

        if (string.IsNullOrEmpty(office))
        {
            query = query.Where(p => p.Office == null);
        }
        else
        {
            query = query.Where(p => p.Office == office);
        }

        var placement = await query
            .Select(p => new { id = p.Id, city = p.City, office = p.Office })
            .FirstOrDefaultAsync();

        if (placement == null)
        {
            return NotFound(new { message = "Placement не знайдено." });
        }

        return Ok(placement);
    }
}
