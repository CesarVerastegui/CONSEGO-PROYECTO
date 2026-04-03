using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONSEGO.Data;
using CONSEGO.Models;
using CONSEGO.Models.ViewModels;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre");
                return View(model);
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario con ese email.");
                ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = AppDbContext.HashPassword(model.Password),
                RolId = model.RolId,
                Activo = model.Activo,
                FechaCreacion = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Usuario creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var model = new UsuarioEditViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                RolId = usuario.RolId,
                Activo = usuario.Activo
            };

            ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre", model.RolId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre", model.RolId);
                return View(model);
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email && u.Id != id))
            {
                ModelState.AddModelError("Email", "Ya existe un usuario con ese email.");
                ViewBag.Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Nombre", model.RolId);
                return View(model);
            }

            usuario.Nombre = model.Nombre;
            usuario.Email = model.Email;
            usuario.RolId = model.RolId;
            usuario.Activo = model.Activo;

            if (!string.IsNullOrWhiteSpace(model.Password))
                usuario.PasswordHash = AppDbContext.HashPassword(model.Password);

            _context.Update(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Usuario actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var tieneSolicitudes = await _context.SolicitudesAcceso
                .AnyAsync(s => s.UsuarioSolicitanteId == id || s.AnalistaId == id);

            if (tieneSolicitudes)
            {
                TempData["Error"] = "No se puede eliminar el usuario porque tiene solicitudes asociadas.";
                return RedirectToAction(nameof(Index));
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Usuario eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
