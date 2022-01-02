using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Context;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.DAL.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Customer> GetTopCustomersWithAccountsAsync(string countryName)
        {
            return _db.Customers.Include(c => c.Dispositions).ThenInclude(d => d.Account).Where(c => c.Country == countryName).OrderByDescending(c => c.Dispositions.Sum(d => d.Account.Balance)).Take(10);
        }

        public async Task<int> GetTotalNumberOfCustomersAsync()
        {
            return await _db.Customers.CountAsync();
        }
    }
}
