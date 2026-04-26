using CONSEGO.Models;

namespace CONSEGO.Repository
{
    public interface IPlataformaRepository
    {
        Task<IEnumerable<Plataforma>> GetAllAsync();
        Task<Plataforma?> GetByIdAsync(int id);
        Task<Plataforma?> GetByNombreAsync(string nombre);
        Task AddAsync(Plataforma plataforma);
        void Update(Plataforma plataforma);
        void Delete(Plataforma plataforma);
        Task SaveChangesAsync();
        Task<IEnumerable<Plataforma>> GetAllActivasAsync();
    }
}
