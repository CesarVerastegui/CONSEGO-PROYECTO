using CONSEGO.Models;

namespace CONSEGO.Repository
{
    public interface ISolicitudRepository
    {
        IQueryable<SolicitudAcceso> GetQueryable();
        Task<SolicitudAcceso?> GetByIdAsync(int id);
        Task<string> GetUltimoCodigoAsync(int anio);
        Task AddAsync(SolicitudAcceso solicitud);
        Task SaveChangesAsync();
    }
}
