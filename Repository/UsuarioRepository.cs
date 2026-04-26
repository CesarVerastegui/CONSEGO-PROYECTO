using CONSEGO.Data;
using CONSEGO.Models;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Usuario>> GetAllWithRolesAsync() =>
            await _context.Usuarios.Include(u => u.Rol).ToListAsync();

        public async Task<Usuario?> GetByIdAsync(int id) =>
            await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<Usuario?> GetByEmailAsync(string email) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<bool> HasSolicitudesAsync(int id) =>
            await _context.SolicitudesAcceso.AnyAsync(s => s.UsuarioSolicitanteId == id || s.AnalistaId == id);

        public async Task AddAsync(Usuario usuario) => await _context.Usuarios.AddAsync(usuario);
        public void Update(Usuario usuario) => _context.Usuarios.Update(usuario);
        public void Delete(Usuario usuario) => _context.Usuarios.Remove(usuario);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
