using ITBaza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITBaza.Controllers.Accounting
{
    [Authorize]
    [Route("Accounting/[controller]/{action=Index}")]
    public class AccessOperationsController : Controller
    {
        private readonly AppDbContext _context;

        public AccessOperationsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var operations = await _context.AccessOperations
                .Include(a => a.Person)
                .Include(a => a.Access)
                .Include(a => a.Executor)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            return View(operations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var operation = await _context.AccessOperations
                .Include(a => a.Person)
                    .ThenInclude(p => p.Vacation)
                        .ThenInclude(v => v.Department)
                .Include(a => a.Person)
                    .ThenInclude(p => p.Vacation)
                        .ThenInclude(v => v.Division)
                .Include(a => a.Person)
                    .ThenInclude(p => p.Placement)
                        .ThenInclude(p => p.Country)
                .Include(a => a.Access)
                    .ThenInclude(a => a.Resource)
                        .ThenInclude(r => r.ResourceType)
                .Include(a => a.Access)
                    .ThenInclude(a => a.Resource)
                        .ThenInclude(r => r.ResponsiblePerson)
                .Include(a => a.Access)
                    .ThenInclude(a => a.ResourceRole)
                .Include(a => a.Executor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (operation == null)
            {
                return NotFound();
            }

            ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
            return View(operation);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName");
            ViewBag.Accesses = await _context.Accesses
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Id} - {a.Login}"
                })
                .ToListAsync();
            ViewBag.Executors = new SelectList(await _context.SystemUsers.Where(u => u.IsActive == true).ToListAsync(), "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccessOperation model)
        {
            // Перевірка наявності такого вже наданого доступу
            bool exists = await _context.AccessOperations
                .AnyAsync(op =>
                    op.PersonId == model.PersonId &&
                    op.AccessId == model.AccessId &&
                    op.Action == "надати");

            if (exists)
            {
                ModelState.AddModelError("", "Цей доступ вже було надано цьому працівнику.");
            }

            if (ModelState.IsValid)
            {
                model.Action = "надати";
                model.ActionDate = DateTime.Now;
                model.ExecutorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                _context.AccessOperations.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Повторне завантаження ViewBag
            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName", model.PersonId);

            ViewBag.Accesses = await _context.Accesses
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Id} - {a.Login}"
                })
                .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Revoke()
        {
            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(AccessOperation model)
        {
            if (ModelState.IsValid)
            {
                model.ActionDate = DateTime.Now;
                model.Action = "скасувати";

                var executorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                model.ExecutorId = executorId;

                _context.AccessOperations.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName");

            return View(model);
        }

        [HttpGet("/api/accessoperations/get-granted/{personId}")]
        public async Task<IActionResult> GetGrantedAccesses(int personId)
        {
            var accesses = await _context.AccessOperations
                .Include(a => a.Access)
                .Where(a => a.PersonId == personId && a.Action == "надати")
                .Select(a => new {
                    a.AccessId,
                    Label = a.Access.Id + " - " + a.Access.Login
                })
                .Distinct()
                .ToListAsync();

            return Ok(accesses);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
            base.OnActionExecuting(context);
        }
    }
}
