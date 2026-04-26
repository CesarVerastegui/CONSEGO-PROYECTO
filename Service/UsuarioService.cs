using CONSEGO.Data;
using CONSEGO.DTOs;
using CONSEGO.Models;
using CONSEGO.Repository;

namespace CONSEGO.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        public UsuarioService(IUsuarioRepository repo) => _repo = repo;

        public async Task<IEnumerable<UsuarioResponseDTO>> ListarUsuariosAsync()
        {
            var usuarios = await _repo.GetAllWithRolesAsync();
            return usuarios.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                RolNombre = u.Rol?.Nombre ?? "Sin Rol",
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion
            });
        }

        public async Task<bool> CrearUsuarioAsync(UsuarioCreateDTO dto)
        {
            if (await _repo.GetByEmailAsync(dto.Email) != null) return false;

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = AppDbContext.HashPassword(dto.Password), // Tu método estático
                RolId = dto.RolId,
                Activo = true,
                FechaCreacion = DateTime.Now
            };
            await _repo.AddAsync(usuario);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioUpdateDTO?> ObtenerParaEditarAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;
            return new UsuarioUpdateDTO { Id = u.Id, Nombre = u.Nombre, Email = u.Email, RolId = u.RolId };
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioUpdateDTO dto)
        {
            var u = await _repo.GetByIdAsync(dto.Id);
            if (u == null) return false;

            var existeEmail = await _repo.GetByEmailAsync(dto.Email);
            if (existeEmail != null && existeEmail.Id != dto.Id) return false;

            u.Nombre = dto.Nombre;
            u.Email = dto.Email;
            u.RolId = dto.RolId;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                u.PasswordHash = AppDbContext.HashPassword(dto.Password);

            _repo.Update(u);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleEstadoAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return false;
            u.Activo = !u.Activo;
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<string?> EliminarUsuarioAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return "Usuario no encontrado.";
            if (await _repo.HasSolicitudesAsync(id)) return "No se puede eliminar: tiene solicitudes asociadas.";

            _repo.Delete(u);
            await _repo.SaveChangesAsync();
            return null;
        }
    }
}
