using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CONSEGO.Data;
using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                TotalSolicitudes = await _context.SolicitudesAcceso.CountAsync(),
                TotalPlataformasActivas = await _context.Plataformas.CountAsync(p => p.Activa),
                TotalUsuariosActivos = await _context.Usuarios.CountAsync(u => u.Activo),

                SolicitudesAprobadas = await _context.SolicitudesAcceso.CountAsync(s => s.Estado == EstadoSolicitud.Aprobado),
                SolicitudesImplementadas = await _context.SolicitudesAcceso.CountAsync(s => s.Estado == EstadoSolicitud.Implementado),
                SolicitudesRechazadas = await _context.SolicitudesAcceso.CountAsync(s => s.Estado == EstadoSolicitud.Rechazado),
                SolicitudesPendientes = await _context.SolicitudesAcceso.CountAsync(s =>
                    s.Estado == EstadoSolicitud.Registrado || s.Estado == EstadoSolicitud.EnAnalisis)
            };

            return View(dashboard);
        }
    }
}