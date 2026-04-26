using CONSEGO.Models;

namespace CONSEGO.Repository
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllWithRolesAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> HasSolicitudesAsync(int id);
        Task AddAsync(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
        Task SaveChangesAsync();
        Task<int> CountActivosAsync();
        Task<Usuario?> ObtenerPorEmailYPasswordAsync(string email, string passwordHash);
    }
}
