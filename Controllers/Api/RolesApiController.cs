using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using CONSEGO.Data;
using CONSEGO.Models;

namespace CONSEGO.Controllers.Api
{
    [ApiController]
    [Route("api/roles")]
    public class RolesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/roles
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Roles.ToList());
        }

        // GET: api/roles/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var rol = _context.Roles.Find(id);
            if (rol == null) return NotFound();
            return Ok(rol);
        }

        // POST: api/roles
        [HttpPost]
        public IActionResult Post([FromBody] Rol rol)
        {
            _context.Roles.Add(rol);
            _context.SaveChanges();
            return Ok(rol);
        }

        // PUT: api/roles/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Rol rol)
        {
            var existing = _context.Roles.Find(id);
            if (existing == null) return NotFound();

            existing.Nombre = rol.Nombre;

            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/roles/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rol = _context.Roles.Find(id);
            if (rol == null) return NotFound();

            _context.Roles.Remove(rol);
            _context.SaveChanges();

            return Ok();
        }
    }
}