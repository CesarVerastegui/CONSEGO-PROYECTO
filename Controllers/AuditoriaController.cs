using CONSEGO.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CONSEGO.Controllers
{
    [Authorize(Roles = "Admin, Auditor")]
    public class AuditoriaController : Controller
    {
        private readonly IAuditRepository _auditRepository;

        public AuditoriaController(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _auditRepository.GetAllAsync();
            return View(logs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var log = await _auditRepository.GetByIdAsync(id);
            if (log == null) return NotFound();
            return View(log);
        }
    }
}
