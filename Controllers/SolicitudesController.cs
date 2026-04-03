using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CONSEGO.Data;
using CONSEGO.Models;
using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using System.Security.Claims;

namespace CONSEGO.Controllers
{
    [Authorize]
    public class SolicitudesController : Controller
    {
        private readonly AppDbContext _context;

        public SolicitudesController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string GetRol() =>
            User.FindFirstValue(ClaimTypes.Role)!;

        // GET: Solicitudes (con filtros y paginación)
        public async Task<IActionResult> Index(SolicitudFiltroViewModel filtro)
        {
            var query = _context.SolicitudesAcceso
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.Plataforma)
                .Include(s => s.Analista)
                .AsQueryable();

            var rol = GetRol();
            var userId = GetUsuarioId();

            // Solicitante solo ve sus propias solicitudes
            if (rol == "Solicitante")
                query = query.Where(s => s.UsuarioSolicitanteId == userId);

            // Infra solo ve aprobadas e implementadas
            if (rol == "Infra")
                query = query.Where(s => s.Estado == EstadoSolicitud.Aprobado || s.Estado == EstadoSolicitud.Implementado);

            // Filtros
            if (filtro.Estado.HasValue)
                query = query.Where(s => s.Estado == filtro.Estado.Value);

            if (filtro.PlataformaId.HasValue)
                query = query.Where(s => s.PlataformaId == filtro.PlataformaId.Value);

            if (filtro.FechaDesde.HasValue)
                query = query.Where(s => s.FechaSolicitud >= filtro.FechaDesde.Value);

            if (filtro.FechaHasta.HasValue)
                query = query.Where(s => s.FechaSolicitud <= filtro.FechaHasta.Value);

            filtro.TotalRegistros = await query.CountAsync();

            filtro.Solicitudes = await query
                .OrderByDescending(s => s.FechaSolicitud)
                .Skip((filtro.Pagina - 1) * filtro.TamañoPagina)
                .Take(filtro.TamañoPagina)
                .ToListAsync();

            filtro.PlataformasDisponibles = await _context.Plataformas.Where(p => p.Activa).ToListAsync();

            return View(filtro);
        }

        // GET: Solicitudes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var solicitud = await _context.SolicitudesAcceso
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.Plataforma)
                .Include(s => s.Analista)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null) return NotFound();

            var rol = GetRol();
            var userId = GetUsuarioId();

            if (rol == "Solicitante" && solicitud.UsuarioSolicitanteId != userId)
                return Forbid();

            return View(solicitud);
        }

        // GET: Solicitudes/Create (solo Solicitante y Admin)
        [Authorize(Roles = "Solicitante,Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Plataformas = new SelectList(
                await _context.Plataformas.Where(p => p.Activa).ToListAsync(), "Id", "Nombre");
            return View();
        }

        // POST: Solicitudes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Solicitante,Admin")]
        public async Task<IActionResult> Create(SolicitudAcceso solicitud)
        {
            // Quitar validación de navegación
            ModelState.Remove("UsuarioSolicitante");
            ModelState.Remove("Plataforma");
            ModelState.Remove("Analista");
            ModelState.Remove("Codigo");

            if (!ModelState.IsValid)
            {
                ViewBag.Plataformas = new SelectList(
                    await _context.Plataformas.Where(p => p.Activa).ToListAsync(), "Id", "Nombre");
                return View(solicitud);
            }

            solicitud.UsuarioSolicitanteId = GetUsuarioId();
            solicitud.Estado = EstadoSolicitud.Registrado;
            solicitud.FechaSolicitud = DateTime.Now;
            solicitud.Codigo = await GenerarCodigo();

            _context.SolicitudesAcceso.Add(solicitud);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Solicitud {solicitud.Codigo} creada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Solicitudes/Tomar/5 (Analista toma la solicitud ? EnAnalisis)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AnalistaSeguridad,Admin")]
        public async Task<IActionResult> Tomar(int id)
        {
            var solicitud = await _context.SolicitudesAcceso.FindAsync(id);
            if (solicitud == null) return NotFound();

            if (solicitud.Estado != EstadoSolicitud.Registrado)
            {
                TempData["Error"] = "Solo se pueden tomar solicitudes en estado Registrado.";
                return RedirectToAction(nameof(Index));
            }

            solicitud.Estado = EstadoSolicitud.EnAnalisis;
            solicitud.AnalistaId = GetUsuarioId();
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Solicitud {solicitud.Codigo} tomada para análisis.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Solicitudes/Resolver/5 (formulario para aprobar/rechazar)
        [Authorize(Roles = "AnalistaSeguridad,Admin")]
        public async Task<IActionResult> Resolver(int id)
        {
            var solicitud = await _context.SolicitudesAcceso
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.Plataforma)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null) return NotFound();

            if (solicitud.Estado != EstadoSolicitud.EnAnalisis)
            {
                TempData["Error"] = "Solo se pueden resolver solicitudes en estado En Análisis.";
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
            var solicitud = await _context.SolicitudesAcceso.FindAsync(id);
            if (solicitud == null) return NotFound();

            if (solicitud.Estado != EstadoSolicitud.EnAnalisis)
            {
                TempData["Error"] = "Solo se pueden resolver solicitudes en estado En Análisis.";
                return RedirectToAction(nameof(Index));
            }

            solicitud.ObservacionesSeguridad = observacionesSeguridad;
            solicitud.FechaDecision = DateTime.Now;

            if (decision == "Aprobar")
            {
                solicitud.Estado = EstadoSolicitud.Aprobado;
                TempData["Success"] = $"Solicitud {solicitud.Codigo} aprobada.";
            }
            else
            {
                solicitud.Estado = EstadoSolicitud.Rechazado;
                solicitud.MotivoRechazo = motivoRechazo;
                TempData["Success"] = $"Solicitud {solicitud.Codigo} rechazada.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Solicitudes/Implementar/5 (Infra/Admin marca implementada)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Infra,Admin")]
        public async Task<IActionResult> Implementar(int id)
        {
            var solicitud = await _context.SolicitudesAcceso.FindAsync(id);
            if (solicitud == null) return NotFound();

            if (solicitud.Estado != EstadoSolicitud.Aprobado)
            {
                TempData["Error"] = "Solo se pueden implementar solicitudes aprobadas.";
                return RedirectToAction(nameof(Index));
            }

            solicitud.Estado = EstadoSolicitud.Implementado;
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Solicitud {solicitud.Codigo} marcada como implementada.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GenerarCodigo()
        {
            var año = DateTime.Now.Year;
            var ultimoNumero = await _context.SolicitudesAcceso
                .Where(s => s.Codigo.StartsWith($"ACC-{año}-"))
                .OrderByDescending(s => s.Codigo)
                .Select(s => s.Codigo)
                .FirstOrDefaultAsync();

            int siguiente = 1;
            if (ultimoNumero != null)
            {
                var partes = ultimoNumero.Split('-');
                if (partes.Length == 3 && int.TryParse(partes[2], out int num))
                    siguiente = num + 1;
            }

            return $"ACC-{año}-{siguiente:D4}";
        }
    }
}
