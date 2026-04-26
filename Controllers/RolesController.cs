using CONSEGO.Data;
using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _rolService.ListarRolesAsync();
            return View(roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolCreateDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var exito = await _rolService.CrearRolAsync(dto);
            if (!exito)
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con ese nombre.");
                return View(dto);
            }

            TempData["Success"] = "Rol creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _rolService.ObtenerParaEditarAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RolUpdateDTO dto)
        {
            if (id != dto.Id) return NotFound();
            if (!ModelState.IsValid) return View(dto);

            var exito = await _rolService.ActualizarRolAsync(dto);
            if (!exito)
            {
                ModelState.AddModelError("Nombre", "Ya existe un rol con ese nombre.");
                return View(dto);
            }

            TempData["Success"] = "Rol actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _rolService.ObtenerDetallesAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var error = await _rolService.EliminarRolAsync(id);
            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Rol eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
