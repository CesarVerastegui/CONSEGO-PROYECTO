using Microsoft.EntityFrameworkCore;
using CONSEGO.Models;
using CONSEGO.Models.Enums;
using System.Security.Cryptography;
using System.Text;

namespace CONSEGO.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<SolicitudAcceso> SolicitudesAcceso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único para Email de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Índice único para Nombre de Plataforma
            modelBuilder.Entity<Plataforma>()
                .HasIndex(p => p.Nombre)
                .IsUnique();

            // Índice único para Código de Solicitud
            modelBuilder.Entity<SolicitudAcceso>()
                .HasIndex(s => s.Codigo)
                .IsUnique();

            // Relaciones de SolicitudAcceso
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

            // Convertir enums a string en la BD
            modelBuilder.Entity<SolicitudAcceso>()
                .Property(s => s.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<SolicitudAcceso>()
                .Property(s => s.TipoAcceso)
                .HasConversion<string>();

            modelBuilder.Entity<Plataforma>()
                .Property(p => p.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Plataforma>()
                .Property(p => p.Criticidad)
                .HasConversion<string>();

            // ==================== SEED DATA ====================

            // Roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin", Descripcion = "Administrador del sistema con acceso total" },
                new Rol { Id = 2, Nombre = "AnalistaSeguridad", Descripcion = "Analista de seguridad que revisa solicitudes" },
                new Rol { Id = 3, Nombre = "Solicitante", Descripcion = "Usuario que crea solicitudes de acceso" },
                new Rol { Id = 4, Nombre = "Infra", Descripcion = "Equipo de infraestructura que implementa accesos aprobados" }
            );

            // Usuarios demo (password = "Demo123!")
            var hash = HashPassword("Demo123!");
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nombre = "Administrador", Email = "admin@idmtechnology.pe", PasswordHash = hash, RolId = 1, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new Usuario { Id = 2, Nombre = "Ana García (Analista)", Email = "analista@idmtechnology.pe", PasswordHash = hash, RolId = 2, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) },
                new Usuario { Id = 3, Nombre = "Carlos López (Solicitante)", Email = "solicitante@idmtechnology.pe", PasswordHash = hash, RolId = 3, Activo = true, FechaCreacion = new DateTime(2025, 1, 1) }
            );

            // Plataformas demo
            modelBuilder.Entity<Plataforma>().HasData(
                new Plataforma { Id = 1, Nombre = "GitHub Organization", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 2, Nombre = "AWS", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 3, Nombre = "Azure", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Alta, Activa = true },
                new Plataforma { Id = 4, Nombre = "Microsoft 365", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Media, Activa = true },
                new Plataforma { Id = 5, Nombre = "Cloudflare", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Media, Activa = true },
                new Plataforma { Id = 6, Nombre = "WordPress", Tipo = TipoPlataforma.App, Criticidad = Criticidad.Baja, Activa = true },
                new Plataforma { Id = 7, Nombre = "GoDaddy", Tipo = TipoPlataforma.Cloud, Criticidad = Criticidad.Media, Activa = true },
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
