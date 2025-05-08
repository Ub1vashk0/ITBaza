using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITBaza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ITBaza.Controllers.AccountingControllers
{
    [Authorize]
    [Route("Accounting/[controller]/{action=Index}")]
    public class PhoneNumberOperationsController : Controller
    {
        private readonly AppDbContext _context;

        public PhoneNumberOperationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PhoneNumberOperations
        public async Task<IActionResult> Index()
        {
            var operations = await _context.PhoneNumberOperations
                .Include(p => p.Person)
                .Include(p => p.PhoneNumber)
                .Include(p => p.Executor)
                .OrderByDescending(p => p.ActionDate)
                .ToListAsync();

            return View(operations);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var operation = await _context.PhoneNumberOperations
                .Include(p => p.Person)
                    .ThenInclude(p => p.Vacation)
                        .ThenInclude(v => v.Department)
                .Include(p => p.Person)
                    .ThenInclude(p => p.Vacation)
                        .ThenInclude(v => v.Division)
                .Include(p => p.Person)
                    .ThenInclude(p => p.Placement)
                        .ThenInclude(pl => pl.Country)
                .Include(p => p.PhoneNumber)
                    .ThenInclude(n => n.Operator)
                        .ThenInclude(op => op.IdCountryNavigation)
                .Include(p => p.Executor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (operation == null)
                return NotFound();

            ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
            return View(operation);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName");

            ViewBag.PhoneNumbers = new SelectList(
                await _context.PhoneNumbers.ToListAsync(), "Id", "Number");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhoneNumberOperation model)
        {
            bool exists = await _context.PhoneNumberOperations
                        .AnyAsync(op =>
                            op.PersonId == model.PersonId &&
                            op.PhoneNumberId == model.PhoneNumberId &&
                            op.Action == "надати");

            if (exists)
            {
                ModelState.AddModelError("", "Цей номер вже було надано цьому працівнику.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                model.Action = "надати";
                model.ActionDate = DateTime.Now;
                model.ExecutorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                _context.PhoneNumberOperations.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Persons = new SelectList(
                await _context.People
                    .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                    .ToListAsync(),
                "Id", "FullName", model.PersonId);

            ViewBag.PhoneNumbers = new SelectList(await _context.PhoneNumbers.ToListAsync(), "Id", "Number", model.PhoneNumberId);

            return View(model);
        }

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
        public async Task<IActionResult> Revoke(PhoneNumberOperation model)
        {
            if (model.PersonId == 0 || model.PhoneNumberId == 0)
            {
                ModelState.AddModelError("", "Потрібно вибрати працівника та номер телефону.");
            }

            bool exists = await _context.PhoneNumberOperations
                .AnyAsync(op => op.PersonId == model.PersonId && op.PhoneNumberId == model.PhoneNumberId && op.Action == "надати");

            if (!exists)
            {
                ModelState.AddModelError("", "Цей номер не був наданий цьому працівнику.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Persons = new SelectList(
                    await _context.People
                        .Select(p => new { p.Id, FullName = p.LastName + " " + p.FirstName + " " + p.MiddleName })
                        .ToListAsync(),
                    "Id", "FullName", model.PersonId);

                return View(model);
            }

            model.Action = "скасовано";
            model.ActionDate = DateTime.Now;
            model.ExecutorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            _context.PhoneNumberOperations.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool PhoneNumberOperationExists(int id)
        {
            return _context.PhoneNumberOperations.Any(e => e.Id == id);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
            base.OnActionExecuting(context);
        }
    }
}
[Route("api/phonenumbers")]
[ApiController]
public class PhoneNumbersApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public PhoneNumbersApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("get-granted/{personId}")]
    public async Task<IActionResult> GetGrantedNumbers(int personId)
    {
        var granted = await _context.PhoneNumberOperations
            .Where(op => op.PersonId == personId && op.Action == "надати")
            .Include(op => op.PhoneNumber)
            .Select(op => new
            {
                phoneNumberId = op.PhoneNumberId,
                label = op.PhoneNumber.Number
            })
            .Distinct()
            .ToListAsync();

        return Ok(granted);
    }
    [HttpGet("info/{id}")]
    public async Task<IActionResult> GetPhoneNumberInfo(int id)
    {
        var number = await _context.PhoneNumbers
            .Include(p => p.Operator)
                .ThenInclude(o => o.IdCountryNavigation)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (number == null)
            return NotFound();
        Console.WriteLine($"Country: {number.Operator?.IdCountryNavigation?.Name}");
        return Ok(new
        {
            operatorName = number.Operator?.Name,
            corporation = number.Operator?.Corporation,
            country = number.Operator?.IdCountryNavigation?.Name

        });
        

    }
}

