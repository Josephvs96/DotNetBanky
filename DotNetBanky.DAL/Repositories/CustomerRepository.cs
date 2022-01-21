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

        public async Task<IEnumerable<Customer>> GetTopCustomersWithAccountsAsync(string countryName)
        {
            return await GetListAsync(
                filter: c => c.Country == countryName,
                include: q => q.Include(x => x.Dispositions).ThenInclude(d => d.Account),
                orderBy: q => q.OrderByDescending(c => c.Dispositions.Sum(d => d.Account.Balance)),
                limit: 10);
        }

        public async Task<int> GetTotalNumberOfCustomersAsync()
        {
            return await _db.Customers.CountAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _db.Customers
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account).FirstAsync(c => c.CustomerId == id);
        }

        public async Task<decimal> GetTotalAccountsBalanceByCustomerId(int id)
        {
            return await _db.Customers.Where(c => c.CustomerId == id).Select(c => c.Dispositions.Sum(d => d.Account.Balance)).FirstAsync();
        }

        public Task<List<string>> GetAllCountries()
        {
            return Task.FromResult(_db.Customers.Select(c => c.Country).Distinct().ToList());
        }
    }
}
