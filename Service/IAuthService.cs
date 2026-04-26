using CONSEGO.Models;

namespace CONSEGO.Service
{
    public interface IAuthService
    {
        Task<Usuario?> ValidarUsuarioAsync(string email, string password);
    }
}
