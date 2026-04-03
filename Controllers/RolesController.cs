using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CONSEGO.Data;
using CONSEGO.Models;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol)
        {
            if (!ModelState.IsValid)
                return View(rol);

            if (await _context.Roles.AnyAsync(r => r.Nombre == rol.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con ese nombre.");
                return View(rol);
            }

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Rol creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();
            return View(rol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol)
        {
            if (id != rol.Id) return NotFound();
            if (!ModelState.IsValid) return View(rol);

            if (await _context.Roles.AnyAsync(r => r.Nombre == rol.Nombre && r.Id != id))
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con ese nombre.");
                return View(rol);
            }

            _context.Update(rol);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Rol actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rol = await _context.Roles.Include(r => r.Usuarios).FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null) return NotFound();
            return View(rol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Roles.Include(r => r.Usuarios).FirstOrDefaultAsync(r => r.Id == id);
            if (rol == null) return NotFound();

            if (rol.Usuarios.Any())
            {
                TempData["Error"] = "No se puede eliminar el rol porque tiene usuarios asignados.";
                return RedirectToAction(nameof(Index));
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Rol eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
