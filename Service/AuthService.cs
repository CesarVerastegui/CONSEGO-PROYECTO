using CONSEGO.Data;
using CONSEGO.Models;
using CONSEGO.Repository;

namespace CONSEGO.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _repo;

        public AuthService(IUsuarioRepository repo) => _repo = repo;

        public async Task<Usuario?> ValidarUsuarioAsync(string email, string password)
        {
            // Usamos el método estático que ya tienes en el DbContext
            var hash = AppDbContext.HashPassword(password);

            var usuario = await _repo.ObtenerPorEmailYPasswordAsync(email, hash);

            // Retornamos el usuario (será null si los datos están mal o el rol no existe)
            return (usuario != null && usuario.Rol != null) ? usuario : null;
        }
    }
}
