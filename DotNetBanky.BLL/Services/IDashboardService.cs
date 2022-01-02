using DotNetBanky.Core.DTOModels.Dashboard;

namespace DotNetBanky.BLL.Services
{
    public interface IDashboardService
    {
        public Task<DashboardSummeryDTO> GetDashboardSummeryAsync();
        public Task<List<DashboardCountryCardDTO>> GetDashboardCountriesSummeryAsync();
        public Task<DashboardCountryDetailsDTO> GetDashboardCountryDetailsAsync(string countryName);
    }
}
