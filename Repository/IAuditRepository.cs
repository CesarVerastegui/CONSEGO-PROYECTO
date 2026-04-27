using CONSEGO.Models;

namespace CONSEGO.Repository
{
    public interface IAuditRepository
    {
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(int id);
    }
}
