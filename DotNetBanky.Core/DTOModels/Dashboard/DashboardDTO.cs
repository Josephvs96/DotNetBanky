using DotNetBanky.Core.DTOModels.Customer;

namespace DotNetBanky.Core.DTOModels.Dashboard
{
    public class DashboardSummeryDTO
    {
        public int? TotalNumberOfAccounts { get; set; }
        public int? TotalNumberOfCustomers { get; set; }
        public decimal? TotalSumOfAllAccounts { get; set; }
    }
    public class DashboardCountryCardDTO
    {
        public string CountryName { get; set; } = null!;
        public string FlagURL { get; set; } = null!;
        public int TotalNumberOfAccounts { get; set; }
        public int TotalNumberOfCustomers { get; set; }
        public decimal TotalSumOfAllAccounts { get; set; }
    }
    public class DashboardCountryDetailsDTO
    {
        public IEnumerable<CustomerCountryDetailsDTO> TopCustomers { get; set; } = null!;
    }
}
