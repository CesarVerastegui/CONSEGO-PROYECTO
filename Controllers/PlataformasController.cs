using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CONSEGO.Data;
using CONSEGO.Models;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlataformasController : Controller
    {
        private readonly AppDbContext _context;

        public PlataformasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var plataformas = await _context.Plataformas.ToListAsync();
            return View(plataformas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plataforma plataforma)
        {
            if (!ModelState.IsValid)
                return View(plataforma);

            if (await _context.Plataformas.AnyAsync(p => p.Nombre == plataforma.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe una plataforma con ese nombre.");
                return View(plataforma);
            }

            _context.Plataformas.Add(plataforma);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Plataforma creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var plataforma = await _context.Plataformas.FindAsync(id);
            if (plataforma == null) return NotFound();
            return View(plataforma);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plataforma plataforma)
        {
            if (id != plataforma.Id) return NotFound();
            if (!ModelState.IsValid) return View(plataforma);

            if (await _context.Plataformas.AnyAsync(p => p.Nombre == plataforma.Nombre && p.Id != id))
            {
                ModelState.AddModelError("Nombre", "Ya existe una plataforma con ese nombre.");
                return View(plataforma);
            }

            _context.Update(plataforma);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Plataforma actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var plataforma = await _context.Plataformas
                .Include(p => p.Solicitudes)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (plataforma == null) return NotFound();
            return View(plataforma);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plataforma = await _context.Plataformas
                .Include(p => p.Solicitudes)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (plataforma == null) return NotFound();

            if (plataforma.Solicitudes.Any())
            {
                TempData["Error"] = "No se puede eliminar la plataforma porque tiene solicitudes asociadas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Plataformas.Remove(plataforma);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Plataforma eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
