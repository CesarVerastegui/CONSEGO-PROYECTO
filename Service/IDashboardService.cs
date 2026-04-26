using CONSEGO.Models.ViewModels;

namespace CONSEGO.Service
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardStatsAsync();
    }
}
