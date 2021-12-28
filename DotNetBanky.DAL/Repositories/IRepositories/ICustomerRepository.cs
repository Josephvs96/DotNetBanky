using DotNetBanky.Core.Entities;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<int> GetTotalNumberOfCustomersAsync();
    }
}
