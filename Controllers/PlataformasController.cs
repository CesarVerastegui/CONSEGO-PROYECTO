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
    public class PlataformasController : Controller
    {
        private readonly IPlataformaService _service;

        public PlataformasController(IPlataformaService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var plataformas = await _service.ListarTodoAsync();
            return View(plataformas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlataformaCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var exito = await _service.CrearAsync(dto);
            if (!exito)
            {
                ModelState.AddModelError("Nombre", "Ya existe una plataforma con ese nombre.");
                return View(dto);
            }

            TempData["Success"] = "Plataforma creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenerParaEditarAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlataformaUpdateDTO dto)
        {
            if (id != dto.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            var exito = await _service.ActualizarAsync(dto);
            if (!exito)
            {
                ModelState.AddModelError("Nombre", "Ya existe una plataforma con ese nombre.");
                return View(dto);
            }

            TempData["Success"] = "Plataforma actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.ObtenerDetallesAsync(id);
            if (dto == null) return NotFound();

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mensajeError = await _service.EliminarAsync(id);

            if (mensajeError != null)
            {
                TempData["Error"] = mensajeError;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Plataforma eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
