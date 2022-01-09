using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Context;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.DAL.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<int> GetTotalNumberOfAccountsAsync()
        {
            return await _db.Accounts.CountAsync();
        }

        public async Task<decimal> GetTotalBalanceOfAllAccountsAsync()
        {
            return await _db.Accounts.SumAsync(a => a.Balance);
        }

        public new async Task<Account> GetOneByIdAsync(int id)
        {
            return await _db.Accounts
                .Include(a => a.Transactions)
                .Include(a => a.Dispositions).ThenInclude(d => d.Customer)
                .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task CreateNewDepositTransaction(Account account)
        {
            await UpdateOneAsync(account);
        }

        public async Task CreateNewWithdrawTransaction(Account account)
        {
            await UpdateOneAsync(account);
        }

        public async Task CreateNewTransaction(Account accountFrom, Account accountTo)
        {
            await _db.Database.BeginTransactionAsync();

            await UpdateOneAsync(accountFrom);

            await UpdateOneAsync(accountTo);

            await _db.Database.CommitTransactionAsync();
        }
    }
}
