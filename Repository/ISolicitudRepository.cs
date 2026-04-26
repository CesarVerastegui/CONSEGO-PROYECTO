using CONSEGO.Models;
using CONSEGO.Models.Enums;

namespace CONSEGO.Repository
{
    public interface ISolicitudRepository
    {
        IQueryable<SolicitudAcceso> GetQueryable();
        Task<SolicitudAcceso?> GetByIdAsync(int id);
        Task<string?> GetUltimoCodigoAsync(int anio);
        Task AddAsync(SolicitudAcceso solicitud);
        Task SaveChangesAsync();
        Task<int> CountTotalAsync();
        Task<int> CountByEstadoAsync(params EstadoSolicitud[] estados);
    }
}
