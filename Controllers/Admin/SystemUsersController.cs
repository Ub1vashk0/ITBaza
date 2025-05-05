using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITBaza.Models;
using Microsoft.AspNetCore.Identity;

namespace ITBaza.Controllers.Admin
{
    [Route("Admin/[controller]/{action=Index}")]
    public class SystemUsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<SystemUser> _passwordHasher;

        public SystemUsersController(AppDbContext context, IPasswordHasher<SystemUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var users = _context.SystemUsers.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                users = users.Where(u => u.Login.Contains(search) || u.FullName.Contains(search));
            }

            ViewBag.Search = search;
            return View(await users.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUser systemUser)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", systemUser.RoleId);
                return View(systemUser);
            }

            if (!string.IsNullOrEmpty(systemUser.PasswordHash))
            {
                systemUser.PasswordHash = _passwordHasher.HashPassword(systemUser, systemUser.PasswordHash);
            }

            _context.Add(systemUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.SystemUsers.FindAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SystemUser systemUser)
        {
            if (id != systemUser.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(_context.Roles, "Id", "Name", systemUser.RoleId);
                return View(systemUser);
            }

            if (!string.IsNullOrEmpty(systemUser.PasswordHash))
            {
                systemUser.PasswordHash = _passwordHasher.HashPassword(systemUser, systemUser.PasswordHash);
            }
            else
            {
                var existing = await _context.SystemUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                systemUser.PasswordHash = existing?.PasswordHash;
            }

            _context.Update(systemUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.SystemUsers.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }
    }
}
