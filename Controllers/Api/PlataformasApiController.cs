using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using CONSEGO.Data;
using CONSEGO.Models;

namespace CONSEGO.Controllers.Api
{
    [ApiController]
    [Route("api/plataformas")]
    public class PlataformasApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlataformasApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/plataformas
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Plataformas.ToList());
        }

        // GET: api/plataformas/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var plataforma = _context.Plataformas.Find(id);
            if (plataforma == null) return NotFound();
            return Ok(plataforma);
        }

        // POST
        [HttpPost]
        public IActionResult Post([FromBody] Plataforma plataforma)
        {
            _context.Plataformas.Add(plataforma);
            _context.SaveChanges();
            return Ok(plataforma);
        }

        // PUT
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Plataforma plataforma)
        {
            var existing = _context.Plataformas.Find(id);
            if (existing == null) return NotFound();

            existing.Nombre = plataforma.Nombre;
            existing.Tipo = plataforma.Tipo;

            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var plataforma = _context.Plataformas.Find(id);
            if (plataforma == null) return NotFound();

            _context.Plataformas.Remove(plataforma);
            _context.SaveChanges();

            return Ok();
        }
    }
}