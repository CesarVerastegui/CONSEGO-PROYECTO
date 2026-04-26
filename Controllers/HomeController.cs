using CONSEGO.Data;
using CONSEGO.Models.Enums;
using CONSEGO.Models.ViewModels;
using CONSEGO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CONSEGO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // Solo pide el objeto y lo envía a la vista
            var viewModel = await _dashboardService.GetDashboardStatsAsync();
            return View(viewModel);
        }
    }
}