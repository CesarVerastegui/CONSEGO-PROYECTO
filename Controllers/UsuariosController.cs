using CONSEGO.Data;
using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Models.ViewModels;
using CONSEGO.Repository;
using CONSEGO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _service;
        private readonly IRolRepository _rolRepo; // Para llenar los combos de roles

        public UsuariosController(IUsuarioService service, IRolRepository rolRepo)
        {
            _service = service;
            _rolRepo = rolRepo;
        }

        public async Task<IActionResult> Index() => View(await _service.ListarUsuariosAsync());

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre");
                return View(dto);
            }

            if (!await _service.CrearUsuarioAsync(dto))
            {
                ModelState.AddModelError("Email", "Email ya registrado.");
                ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre");
                return View(dto);
            }

            TempData["Success"] = "Usuario creado.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.ObtenerParaEditarAsync(id);
            if (dto == null) return NotFound();
            ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre", dto.RolId);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioUpdateDTO dto)
        {
            if (id != dto.Id || !ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre", dto.RolId);
                return View(dto);
            }

            if (!await _service.ActualizarUsuarioAsync(dto))
            {
                ModelState.AddModelError("Email", "Email ya en uso.");
                ViewBag.Roles = new SelectList(await _rolRepo.GetAllAsync(), "Id", "Nombre", dto.RolId);
                return View(dto);
            }

            TempData["Success"] = "Usuario actualizado.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _service.ToggleEstadoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var error = await _service.EliminarUsuarioAsync(id);
            if (error != null) TempData["Error"] = error;
            else TempData["Success"] = "Usuario eliminado.";
            return RedirectToAction(nameof(Index));
        }
    }
}