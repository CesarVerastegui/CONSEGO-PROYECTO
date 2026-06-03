using CONSEGO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

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
                .Where(e => e.Entity is not AuditLog &&
                           (e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted))
                .ToList();

            if (!entries.Any()) return;

            var httpContext = _http.HttpContext;

            foreach (var entry in entries)
            {
                var audit = new AuditLog
                {
                    TimestampUtc = DateTime.UtcNow,
                    Action = entry.State.ToString(),
                    Entity = entry.Entity.GetType().Name,
                    EntityId = GetPrimaryKey(entry),
                    Username = httpContext?.User?.Identity?.Name ?? "System",
                    IpAddress = httpContext?.Connection?.RemoteIpAddress?.ToString(),
                    UserAgent = httpContext?.Request?.Headers["User-Agent"].ToString(),
                    Changes = GetChanges(entry)
                };

                changeTracker.Context.Add(audit);
            }
        }

        private string GetPrimaryKey(EntityEntry entry)
        {
            var key = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
            return key?.CurrentValue?.ToString() ?? "N/A";
        }

        private string GetChanges(EntityEntry entry)
        {
            var changes = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                if (prop.Metadata.IsPrimaryKey()) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        changes[prop.Metadata.Name] = prop.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        changes[prop.Metadata.Name] = prop.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (!Equals(prop.OriginalValue, prop.CurrentValue))
                        {
                            changes[prop.Metadata.Name] = new { Old = prop.OriginalValue, New = prop.CurrentValue };
                        }
                        break;
                }
            }
            return JsonSerializer.Serialize(changes);
        }
    }
}