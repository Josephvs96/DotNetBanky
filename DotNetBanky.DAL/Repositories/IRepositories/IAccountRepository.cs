using DotNetBanky.Core.Entities;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<decimal> GetTotalBalanceOfAllAccountsAsync();
        Task<int> GetTotalNumberOfAccountsAsync();

        Task CreateNewDepositTransaction(Account account);

        Task CreateNewWithdrawTransaction(Account account);

        Task CreateNewTransaction(Account accountFrom, Account accountTo);
    }
}
