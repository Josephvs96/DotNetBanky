using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.Entities;

namespace DotNetBanky.BLL.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        public Task CreateCustomerAsync(CustomerCreateModel model);
        public Task EditCustomerAsync(CustomerEditModel model);
        public Task DeleteCustomerAsync(int id);
        public Task<IEnumerable<CustomerListDTOModel>> GetAllCustomerListAsync(int pageNumber, int pageSize, string? filter = null);
        public Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id);
    }
}
