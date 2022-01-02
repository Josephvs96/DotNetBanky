using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Dashboard;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages
{
    [DefaultBreadcrumb("Dashboard")]
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public DashboardSummeryDTO DashboardModel { get; set; }
        public List<DashboardCountryCardDTO> DashboardCountryList { get; set; }

        public async Task OnGetAsync()
        {
            DashboardModel = await _dashboardService.GetDashboardSummeryAsync();
            DashboardCountryList = await _dashboardService.GetDashboardCountriesSummeryAsync();
        }
    }
}