using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace CONSEGO.Service
{
    public interface IAuditService
    {
        void AddAuditLogs(ChangeTracker changeTracker);
    }
}
