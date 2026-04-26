using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using CONSEGO.Repository;

namespace CONSEGO.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly ISolicitudRepository _solicitudRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPlataformaRepository _plataformaRepo;

        public DashboardService(
            ISolicitudRepository solicitudRepo,
            IUsuarioRepository usuarioRepo,
            IPlataformaRepository plataformaRepo)
        {
            _solicitudRepo = solicitudRepo;
            _usuarioRepo = usuarioRepo;
            _plataformaRepo = plataformaRepo;
        }

        public async Task<DashboardViewModel> GetDashboardStatsAsync()
        {
            // El servicio coordina múltiples fuentes de datos
            return new DashboardViewModel
            {
                TotalSolicitudes = await _solicitudRepo.CountTotalAsync(),
                TotalPlataformasActivas = await _plataformaRepo.CountActivasAsync(),
                TotalUsuariosActivos = await _usuarioRepo.CountActivosAsync(),

                SolicitudesAprobadas = await _solicitudRepo.CountByEstadoAsync(EstadoSolicitud.Aprobado),
                SolicitudesImplementadas = await _solicitudRepo.CountByEstadoAsync(EstadoSolicitud.Implementado),
                SolicitudesRechazadas = await _solicitudRepo.CountByEstadoAsync(EstadoSolicitud.Rechazado),
                SolicitudesPendientes = await _solicitudRepo.CountByEstadoAsync(EstadoSolicitud.Registrado, EstadoSolicitud.EnAnalisis)
            };
        }
    }
}
