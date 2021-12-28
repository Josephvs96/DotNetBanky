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

        public async Task OnGetAsync()
        {
            DashboardModel = new DashboardSummeryDTO
            {
                TotalNumberOfAccounts = await _dashboardService.GetTotalNumberOfAccountsAsync(),
                TotalNumberOfCustomers = await _dashboardService.GetTotalNumberOfCustomersAsync(),
                TotalSumOfAllAccounts = await _dashboardService.GetTotalAccountsBalanceAsync(),
            };
        }
    }
}