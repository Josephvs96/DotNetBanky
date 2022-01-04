using DotNetBanky.Core.DTOModels.Customer;

namespace DotNetBanky.BLL.Services
{
    public interface ICustomerService
    {
        public Task CreateCustomerAsync(CustomerCreateModel model);
        public Task EditCustomerAsync(CustomerEditModel model);
        public Task DeleteCustomerAsync(int id);
        public Task<IEnumerable<CustomerListDTOModel>> GetAllCustomerListAsync(int pageNumber, int pageSize);
        public Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id);
        public Task<int> GetTotalNumberOfPages(int pageSize);
    }
}
