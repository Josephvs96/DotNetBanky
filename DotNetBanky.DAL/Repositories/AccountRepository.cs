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
    }
}
