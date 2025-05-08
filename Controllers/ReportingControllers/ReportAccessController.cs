using ITBaza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITBaza.Controllers.ReportingControllers
{
    [Authorize]
    public class ReportAccessController : Controller
    {
        private readonly AppDbContext _context;

        public ReportAccessController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string login)
        {
            var access = await _context.Accesses
                .Include(a => a.Resource)
                .Include(a => a.ResourceRole)
                .FirstOrDefaultAsync(a => a.Login == login);

            if (access == null)
            {
                TempData["Error"] = "Доступ з таким логіном не знайдено.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = access.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var access = await _context.Accesses
                .Include(a => a.Resource)
                .Include(a => a.ResourceRole)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (access == null) return NotFound();

            var current = await _context.AccessOperations
                .Include(op => op.Person)
                .ThenInclude(p => p.Vacation)
                    .ThenInclude(v => v.Department)
                .Include(op => op.Person)
                    .ThenInclude(p => p.Vacation)
                        .ThenInclude(v => v.Division)
                .Where(op => op.AccessId == id && op.Action == "надати")
                .ToListAsync();

            var history = await _context.AccessOperations
                .Include(op => op.Person)
                .Include(op => op.Executor)
                .Where(op => op.AccessId == id)
                .OrderByDescending(op => op.ActionDate)
                .ToListAsync();

            ViewBag.Current = current;
            ViewBag.History = history;

            return View(access);
        }
    }
}
