using DotNetBanky.Core.Entities;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<int> GetTotalNumberOfCustomersAsync();

        Task<IEnumerable<Customer>> GetTopCustomersWithAccountsAsync(string countryName);
    }
}
