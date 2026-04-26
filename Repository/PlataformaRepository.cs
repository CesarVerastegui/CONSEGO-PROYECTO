using CONSEGO.Data;
using CONSEGO.Models;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Repository
{
    public class PlataformaRepository : IPlataformaRepository
    {
        private readonly AppDbContext _context;

        public PlataformaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plataforma>> GetAllAsync()
        {
            return await _context.Plataformas.Include(p => p.Solicitudes).ToListAsync();
        }

        public async Task<Plataforma?> GetByIdAsync(int id)
        {
            return await _context.Plataformas.Include(p => p.Solicitudes).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Plataforma?> GetByNombreAsync(string nombre)
        {
            return await _context.Plataformas.FirstOrDefaultAsync(p => p.Nombre == nombre);
        }

        public async Task AddAsync(Plataforma plataforma)
        {
            await _context.Plataformas.AddAsync(plataforma);
        }

        public void Update(Plataforma plataforma)
        {
            _context.Plataformas.Update(plataforma);
        }

        public void Delete(Plataforma plataforma)
        {
            _context.Plataformas.Remove(plataforma);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plataforma>> GetAllActivasAsync()
        {
            return await _context.Plataformas
                .Where(p => p.Activa)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<int> CountActivasAsync() => await _context.Plataformas.CountAsync(p => p.Activa);
    }
}
