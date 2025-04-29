using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITBaza.Models;

namespace ITBaza.Controllers.AccountingControllers
{
    public class AccessesController : Controller
    {
        private readonly AppDbContext _context;

        public AccessesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Accesses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Accesses.Include(a => a.Resource).Include(a => a.ResourceRole);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Accesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Accesses
                .Include(a => a.Resource)
                .Include(a => a.ResourceRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (access == null)
            {
                return NotFound();
            }

            return View(access);
        }

        // GET: Accesses/Create
        public IActionResult Create()
        {
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id");
            ViewData["ResourceRoleId"] = new SelectList(_context.ResourceRoles, "Id", "Id");
            return View();
        }

        // POST: Accesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResourceId,Login,ResourceRoleId")] Access access)
        {
            if (ModelState.IsValid)
            {
                _context.Add(access);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", access.ResourceId);
            ViewData["ResourceRoleId"] = new SelectList(_context.ResourceRoles, "Id", "Id", access.ResourceRoleId);
            return View(access);
        }

        // GET: Accesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Accesses.FindAsync(id);
            if (access == null)
            {
                return NotFound();
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", access.ResourceId);
            ViewData["ResourceRoleId"] = new SelectList(_context.ResourceRoles, "Id", "Id", access.ResourceRoleId);
            return View(access);
        }

        // POST: Accesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceId,Login,ResourceRoleId")] Access access)
        {
            if (id != access.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(access);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessExists(access.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ResourceId"] = new SelectList(_context.Resources, "Id", "Id", access.ResourceId);
            ViewData["ResourceRoleId"] = new SelectList(_context.ResourceRoles, "Id", "Id", access.ResourceRoleId);
            return View(access);
        }

        // GET: Accesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var access = await _context.Accesses
                .Include(a => a.Resource)
                .Include(a => a.ResourceRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (access == null)
            {
                return NotFound();
            }

            return View(access);
        }

        // POST: Accesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var access = await _context.Accesses.FindAsync(id);
            if (access != null)
            {
                _context.Accesses.Remove(access);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessExists(int id)
        {
            return _context.Accesses.Any(e => e.Id == id);
        }
    }
}
