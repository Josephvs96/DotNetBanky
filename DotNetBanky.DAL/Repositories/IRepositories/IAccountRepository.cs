using DotNetBanky.Core.Entities;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<decimal> GetTotalBalanceOfAllAccountsAsync();
        Task<int> GetTotalNumberOfAccountsAsync();
    }
}
