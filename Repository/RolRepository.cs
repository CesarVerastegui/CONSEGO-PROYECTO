using CONSEGO.Data;
using CONSEGO.Models;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Repository
{
    public class RolRepository : IRolRepository
    {
        private readonly AppDbContext _context;

        public RolRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Roles.Include(r => r.Usuarios).ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Roles.Include(r => r.Usuarios).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rol?> GetByNombreAsync(string nombre)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task AddAsync(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
        }

        public void Update(Rol rol)
        {
            _context.Roles.Update(rol);
        }

        public void Delete(Rol rol)
        {
            _context.Roles.Remove(rol);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
