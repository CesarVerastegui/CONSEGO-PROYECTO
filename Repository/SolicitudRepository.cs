using CONSEGO.Data;
using CONSEGO.Models;
using CONSEGO.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Repository
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly AppDbContext _context;
        public SolicitudRepository(AppDbContext context) => _context = context;

        public IQueryable<SolicitudAcceso> GetQueryable() =>
            _context.SolicitudesAcceso
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.Plataforma)
                .Include(s => s.Analista);

        public async Task<SolicitudAcceso?> GetByIdAsync(int id) =>
            await GetQueryable().FirstOrDefaultAsync(s => s.Id == id);

        public async Task<string?> GetUltimoCodigoAsync(int anio) =>
            await _context.SolicitudesAcceso
            .Where(s => s.Codigo.StartsWith($"ACC-{anio}-"))
            .OrderByDescending(s => s.Codigo)
            .Select(s => s.Codigo)
            .FirstOrDefaultAsync();

        public async Task AddAsync(SolicitudAcceso solicitud) => await _context.SolicitudesAcceso.AddAsync(solicitud);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();


        public async Task<int> CountTotalAsync()
            => await _context.SolicitudesAcceso.CountAsync();

        public async Task<int> CountByEstadoAsync(params EstadoSolicitud[] estados)
            => await _context.SolicitudesAcceso.CountAsync(s => estados.Contains(s.Estado));
    }
}
