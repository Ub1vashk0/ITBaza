using ITBaza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITBaza.Controllers.ReportingControlers
{
    [Authorize]
    public class ReportPersonController : Controller
    {
        private readonly AppDbContext _context;

        public ReportPersonController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var persons = string.IsNullOrWhiteSpace(search)
                ? new List<Person>()
                : await _context.People
                    .Include(p => p.Vacation).ThenInclude(v => v.Department)
                    .Include(p => p.Vacation).ThenInclude(v => v.Division)
                    .Include(p => p.Placement).ThenInclude(pl => pl.Country)
                    .Where(p =>
                        p.LastName.Contains(search) ||
                        p.FirstName.Contains(search) ||
                        p.MiddleName.Contains(search))
                    .ToListAsync();

            ViewBag.Search = search;
            return View(persons);
        }

        public async Task<IActionResult> Details(int id)
        {
            var person = await _context.People
                .Include(p => p.Vacation).ThenInclude(v => v.Department)
                .Include(p => p.Vacation).ThenInclude(v => v.Division)
                .Include(p => p.Placement).ThenInclude(pl => pl.Country)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return NotFound();

            var accesses = await _context.AccessOperations
                .Where(a => a.PersonId == id && a.Action == "надати")
                .Include(a => a.Access).ThenInclude(ac => ac.Resource)
                .Include(a => a.Access).ThenInclude(ac => ac.ResourceRole)
                .ToListAsync();

            var phoneNumbers = await _context.PhoneNumberOperations
                .Where(p => p.PersonId == id && p.Action == "надати")
                .Include(p => p.PhoneNumber).ThenInclude(pn => pn.Operator)
                .ToListAsync();

            ViewBag.Accesses = accesses;
            ViewBag.PhoneNumbers = phoneNumbers;

            return View(person);
        }
    }
}
