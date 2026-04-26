using CONSEGO.Models;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace CONSEGO.Service
{
    public class AuditService : IAuditService
    {

        private readonly IHttpContextAccessor _http;

        public AuditService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public void AddAuditLogs(ChangeTracker changeTracker)
        {
            var entries = changeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                // Evitar auditar la misma tabla de auditoría
                if (entry.Entity is AuditLog)
                    continue;

                var audit = new AuditLog
                {
                    TimestampUtc = DateTime.UtcNow,
                    Action = entry.State.ToString(),
                    Entity = entry.Entity.GetType().Name,
                    EntityId = GetPrimaryKey(entry),
                    Username = _http.HttpContext?.User?.Identity?.Name,
                    Changes = GetChanges(entry)
                };

    changeTracker.Context.Add(audit);
            }
        }

        private string GetPrimaryKey(EntityEntry entry)
{
    var key = entry.Properties
        .FirstOrDefault(p => p.Metadata.IsPrimaryKey());

    return key?.CurrentValue?.ToString() ?? "";
}

private string GetChanges(EntityEntry entry)
{
    var changes = new Dictionary<string, object?>();

    foreach (var prop in entry.Properties)
    {
        if (prop.Metadata.IsPrimaryKey())
            continue;

        if (entry.State == EntityState.Added)
            changes[prop.Metadata.Name] = prop.CurrentValue;

        if (entry.State == EntityState.Deleted)
            changes[prop.Metadata.Name] = prop.OriginalValue;

        if (entry.State == EntityState.Modified &&
            !Equals(prop.OriginalValue, prop.CurrentValue))
        {
            changes[prop.Metadata.Name] = new
            {
                Old = prop.OriginalValue,
                New = prop.CurrentValue
            };
        }
    }

    return JsonSerializer.Serialize(changes);
}

    }
}
