using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CONSEGO.Data;

namespace CONSEGO.Controllers.Api
{
    [ApiController]
    [Route("api/solicitudes")]
    public class SolicitudesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitudesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/solicitudes
        [HttpGet]
        public IActionResult Get()
        {
            var solicitudes = _context.SolicitudesAcceso
                .Include(s => s.UsuarioSolicitante)
                .Include(s => s.Plataforma)
                .Select(s => new
                {
                    s.Id,
                    Usuario = s.UsuarioSolicitante.Nombre,
                    Plataforma = s.Plataforma.Nombre,
                    s.TipoAcceso,
                    s.Justificacion,
                    s.Estado,
                    s.FechaSolicitud
                })
                .ToList();

            return Ok(solicitudes);
        }

        // GET: api/solicitudes/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var s = _context.SolicitudesAcceso
                .Include(x => x.UsuarioSolicitante)
                .Include(x => x.Plataforma)
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    Usuario = x.UsuarioSolicitante.Nombre,
                    Plataforma = x.Plataforma.Nombre,
                    x.TipoAcceso,
                    x.Justificacion,
                    x.Estado,
                    x.FechaSolicitud
                })
                .FirstOrDefault();

            if (s == null) return NotFound();

            return Ok(s);
        }
    }
}