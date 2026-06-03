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
            // CRÍTICO: Usamos .ToList() para que la enumeración no falle 
            // al agregar nuevos registros (AuditLog) al contexto.
            var entries = changeTracker.Entries()
                .Where(e =>
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                // Evitar auditar la misma tabla de auditoría (por seguridad extra)
                if (entry.Entity is AuditLog)
                    continue;

                var audit = new AuditLog
                {
                    TimestampUtc = DateTime.UtcNow,
                    Action = entry.State.ToString(),
                    Entity = entry.Entity.GetType().Name,
                    EntityId = GetPrimaryKey(entry),
                    Username = _http.HttpContext?.User?.Identity?.Name ?? "System",
                    Changes = GetChanges(entry)
                };

                // Ahora podemos agregar al contexto porque estamos iterando sobre una copia (.ToList)
                changeTracker.Context.Add(audit);
            }
        }

        private string GetPrimaryKey(EntityEntry entry)
        {
            var key = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey());

            return key?.CurrentValue?.ToString() ?? "N/A";
        }

        private string GetChanges(EntityEntry entry)
        {
            var changes = new Dictionary<string, object?>();

            foreach (var prop in entry.Properties)
            {
                // No guardamos la PK en el JSON de cambios para no redundar
                if (prop.Metadata.IsPrimaryKey())
                    continue;

                if (entry.State == EntityState.Added)
                {
                    changes[prop.Metadata.Name] = prop.CurrentValue;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    changes[prop.Metadata.Name] = prop.OriginalValue;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Solo guardamos si el valor realmente cambió
                    if (!Equals(prop.OriginalValue, prop.CurrentValue))
                    {
                        changes[prop.Metadata.Name] = new
                        {
                            Old = prop.OriginalValue,
                            New = prop.CurrentValue
                        };
                    }
                }
            }

            return JsonSerializer.Serialize(changes);
        }
    }
}