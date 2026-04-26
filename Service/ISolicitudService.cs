using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Models.ViewModels;

namespace CONSEGO.Service
{
    public interface ISolicitudService
    {
        Task<SolicitudFiltroViewModel> ListarFiltradoAsync(SolicitudFiltroViewModel filtro, int userId, string rol);
        Task<byte[]> ExportarExcelAsync(SolicitudFiltroViewModel filtro, int userId, string rol);
        Task<string> CrearSolicitudAsync(SolicitudCreateDTO dto, int userId);
        Task<bool> TomarSolicitudAsync(int id, int analistaId);
        Task<bool> ResolverSolicitudAsync(int id, string decision, string? obs, string? motivo);
        Task<bool> ImplementarSolicitudAsync(int id);
        Task<SolicitudAcceso?> ObtenerDetalleAsync(int id);
    }
}
