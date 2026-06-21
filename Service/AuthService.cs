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
            var hash = AppDbContext.HashPassword(password);

            var usuario = await _repo.ObtenerPorEmailYPasswordAsync(email, hash);

            return (usuario != null && usuario.Rol != null) ? usuario : null;
        }
    }
}
