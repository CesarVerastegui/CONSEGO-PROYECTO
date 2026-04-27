using CONSEGO.Data;
using CONSEGO.Models;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.TimestampUtc)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetByIdAsync(int id)
        {
            return await _context.AuditLogs.FindAsync(id);
        }
    }
}
