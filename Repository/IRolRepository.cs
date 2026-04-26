using CONSEGO.Models;

namespace CONSEGO.Repository
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> GetAllAsync();
        Task<Rol?> GetByIdAsync(int id);
        Task<Rol?> GetByNombreAsync(string nombre);
        Task AddAsync(Rol rol);
        void Update(Rol rol);
        void Delete(Rol rol);
        Task SaveChangesAsync();
    }
}
