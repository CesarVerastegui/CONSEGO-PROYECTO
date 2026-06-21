using CONSEGO.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CONSEGO.Controllers.Api
{
    [Route("api/auditoria")]
    [ApiController]
    public class AuditoriaApiController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AuditoriaApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetLogs()
        {
            var logs = _context.AuditLogs
                .OrderByDescending(a => a.TimestampUtc)
                .Take(100)
                .Select(a => new
                {
                    a.TimestampUtc,
                    a.Username,
                    a.Action,
                    a.Entity,
                    a.EntityId
                })
                .ToList();

            return Ok(logs);
        }
    
}
}
