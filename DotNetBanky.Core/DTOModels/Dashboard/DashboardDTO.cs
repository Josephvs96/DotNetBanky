using DotNetBanky.Core.Entities;

namespace DotNetBanky.Core.DTOModels.Dashboard
{
    public class DashboardSummeryDTO
    {
        public List<Account>? Accounts { get; set; }
        public List<Customer>? Customers { get; set; }
        public int? TotalNumberOfAccounts { get; set; }
        public int? TotalNumberOfCustomers { get; set; }
        public decimal? TotalSumOfAllAccounts { get; set; }
    }
}
