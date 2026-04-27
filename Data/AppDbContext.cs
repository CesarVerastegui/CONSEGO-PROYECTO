using CONSEGO.Models;
using CONSEGO.Models.Enums;
using CONSEGO.Service;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CONSEGO.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IAuditService _auditService;

        // UN SOLO CONSTRUCTOR: Recibe opciones y el servicio de auditoría
        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IAuditService auditService)
            : base(options)
        {
            _auditService = auditService;
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<SolicitudAcceso> SolicitudesAcceso { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // Sobrescribir SaveChanges para automatizar la auditoría
        public override int SaveChanges()
        {
            _auditService.AddAuditLogs(ChangeTracker);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _auditService.AddAuditLogs(ChangeTracker);
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índices Únicos
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Plataforma>().HasIndex(p => p.Nombre).IsUnique();
            modelBuilder.Entity<SolicitudAcceso>().HasIndex(s => s.Codigo).IsUnique();

            // Relaciones de SolicitudAcceso (Evitar borrado en cascada)
            modelBuilder.Entity<SolicitudAcceso>()
                .HasOne(s => s.UsuarioSolicitante)
                .WithMany(u => u.SolicitudesComoSolicitante)
                .HasForeignKey(s => s.UsuarioSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitudAcceso>()
                .HasOne(s => s.Analista)
                .WithMany(u => u.SolicitudesComoAnalista)
                .HasForeignKey(s => s.AnalistaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitudAcceso>()
                .HasOne(s => s.Plataforma)
                .WithMany(p => p.Solicitudes)
                .HasForeignKey(s => s.PlataformaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Conversión de Enums a String
            modelBuilder.Entity<SolicitudAcceso>().Property(s => s.Estado).HasConversion<string>();
            modelBuilder.Entity<SolicitudAcceso>().Property(s => s.TipoAcceso).HasConversion<string>();
            modelBuilder.Entity<Plataforma>().Property(p => p.Tipo).HasConversion<string>();
            modelBuilder.Entity<Plataforma>().Property(p => p.Criticidad).HasConversion<string>();

            // ==================== SEED DATA ====================

            // Roles (Añadido Auditor)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin", Descripcion = "Acceso total" },
                new Rol { Id = 2, Nombre = "AnalistaSeguridad", Descripcion = "Revisa solicitudes" },
                new Rol { Id = 3, Nombre = "Solicitante", Descripcion = "Crea solicitudes" },
                new Rol { Id = 4, Nombre = "Infra", Descripcion = "Implementa accesos" },
                new Rol { Id = 5, Nombre = "Auditor", Descripcion = "Solo lectura y revisión de logs" }
            );

            var hash = HashPassword("Demo123!");
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Administrador", Email = "admin@idmtechnology.pe", PasswordHash = hash, RolId = 1, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new Usuario { Id = 2, Nombre = "Ana García", Email = "analista@idmtechnology.pe", PasswordHash = hash, RolId = 2, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new Usuario { Id = 3, Nombre = "Juan Asto", Email = "solicitante@idmtechnology.pe", PasswordHash = hash, RolId = 3, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new Usuario { Id = 4, Nombre = "Pedro Castro", Email = "auditor@idmtechnology.pe", PasswordHash = hash, RolId = 5, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) }
            );

            // Plataformas Demo
            modelBuilder.Entity<Plataforma>().HasData(
                new Plataforma { Id = 1, Nombre = "GitHub Organization", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 2, Nombre = "AWS", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 3, Nombre = "Azure", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 8, Nombre = "VMs On-Premise", Tipo = TipoPlataforma.Infra, Criticidad = Criticidad.Alta, Activa = true }
            );
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}