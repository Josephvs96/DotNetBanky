using DotNetBanky.Core.Entities;

namespace DotNetBanky.BLL.Services
{
    public interface IDashboardService
    {
        public Task<List<Customer>> GetAllCustomersAsync();
        public Task<List<Account>> GetAllAccountsAsync();
        public Task<decimal> GetTotalAccountsBalanceAsync();
        Task<int> GetTotalNumberOfAccountsAsync();
        Task<int> GetTotalNumberOfCustomersAsync();
    }
}
