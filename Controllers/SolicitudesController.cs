using System.Data;
using System.Security.Claims;
using ClosedXML.Excel;
using CONSEGO.Data;
using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using CONSEGO.Repository;
using CONSEGO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Controllers
{
    [Authorize]
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IPlataformaRepository _plataformaRepo; // Para llenar los select lists

        public SolicitudesController(ISolicitudService solicitudService, IPlataformaRepository plataformaRepo)
        {
            _solicitudService = solicitudService;
            _plataformaRepo = plataformaRepo;
        }

        private int GetUsuarioId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private string GetRol() => User.FindFirstValue(ClaimTypes.Role)!;

        // GET: Solicitudes
        public async Task<IActionResult> Index(SolicitudFiltroViewModel filtro)
        {
            var model = await _solicitudService.ListarFiltradoAsync(filtro, GetUsuarioId(), GetRol());

            // Cargamos plataformas para el filtro del Index
            var plataformas = await _plataformaRepo.GetAllActivasAsync();
            model.PlataformasDisponibles = plataformas.Select(p => new Models.Plataforma { Id = p.Id, Nombre = p.Nombre }).ToList();

            return View(model);
        }

        // GET: Solicitudes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Nota: Aquí podrías agregar un método GetByIdAsync en el Service que devuelva un ResponseDTO
            // Por ahora, asumimos que el Service o Repositorio maneja la obtención
            var solicitud = await _solicitudService.ObtenerDetalleAsync(id);

            if (solicitud == null) return NotFound();

            // Validación de seguridad: El solicitante solo ve lo suyo
            if (GetRol() == "Solicitante" && solicitud.UsuarioSolicitanteId != GetUsuarioId())
                return Forbid();

            return View(solicitud);
        }

        // GET: Solicitudes/Create
        [Authorize(Roles = "Solicitante,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Plataformas = new SelectList(await _plataformaRepo.GetAllActivasAsync(), "Id", "Nombre");
            return View();
        }

        // POST: Solicitudes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Solicitante,Admin")]
        public async Task<IActionResult> Create(SolicitudCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Plataformas = new SelectList(await _plataformaRepo.GetAllActivasAsync(), "Id", "Nombre");
                return View(dto);
            }

            var codigo = await _solicitudService.CrearSolicitudAsync(dto, GetUsuarioId());
            TempData["Success"] = $"Solicitud {codigo} creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Solicitudes/Tomar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AnalistaSeguridad,Admin")]
        public async Task<IActionResult> Tomar(int id)
        {
            var exito = await _solicitudService.TomarSolicitudAsync(id, GetUsuarioId());

            if (exito)
                TempData["Success"] = "Solicitud tomada para análisis.";
            else
                TempData["Error"] = "No se pudo tomar la solicitud (estado incorrecto o no encontrada).";

            return RedirectToAction(nameof(Index));
        }

        // GET: Solicitudes/Resolver/5
        [Authorize(Roles = "AnalistaSeguridad,Admin")]
        public async Task<IActionResult> Resolver(int id)
        {
            var solicitud = await _solicitudService.ObtenerDetalleAsync(id);
            if (solicitud == null) return NotFound();

            if (solicitud.Estado != Models.Enums.EstadoSolicitud.EnAnalisis)
            {
                TempData["Error"] = "Solo se pueden resolver solicitudes en análisis.";
                return RedirectToAction(nameof(Index));
            }

            return View(solicitud);
        }

        // POST: Solicitudes/Resolver/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AnalistaSeguridad,Admin")]
        public async Task<IActionResult> Resolver(int id, string decision, string? observacionesSeguridad, string? motivoRechazo)
        {
            var exito = await _solicitudService.ResolverSolicitudAsync(id, decision, observacionesSeguridad, motivoRechazo);

            if (exito)
                TempData["Success"] = $"Solicitud procesada como: {decision}.";
            else
                TempData["Error"] = "Error al intentar resolver la solicitud.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Solicitudes/Implementar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Infra,Admin")]
        public async Task<IActionResult> Implementar(int id)
        {
            var exito = await _solicitudService.ImplementarSolicitudAsync(id);

            if (exito)
                TempData["Success"] = "La solicitud ha sido marcada como implementada.";
            else
                TempData["Error"] = "No se pudo implementar (debe estar aprobada primero).";

            return RedirectToAction(nameof(Index));
        }

        // POST: Solicitudes/ExportarExcel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportarExcel(SolicitudFiltroViewModel filtro)
        {
            var content = await _solicitudService.ExportarExcelAsync(filtro, GetUsuarioId(), GetRol());

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Solicitudes_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            );
        }
    }
}