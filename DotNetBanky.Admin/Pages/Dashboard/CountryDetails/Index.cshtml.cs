using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Dashboard.CountryDetails
{
    [Breadcrumb("Country Details")]
    [ResponseCache(Duration = 60, VaryByHeader = "countryDetails", Location = ResponseCacheLocation.Any, NoStore = false)]
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public IndexModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public DashboardCountryDetailsDTO CountryDetails { get; set; }
        public string CountryName { get; set; }

        public async Task OnGetAsync(string countryName)
        {
            CountryName = countryName;
            CountryDetails = await _dashboardService.GetDashboardCountryDetailsAsync(countryName);
        }
    }
}
