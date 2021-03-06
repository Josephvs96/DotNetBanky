using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Enums;

namespace DotNetBanky.BLL.Services
{
    public interface ICustomerService
    {
        public Task CreateCustomerAsync(CustomerCreateModel model);
        public Task EditCustomerAsync(CustomerEditModel model);
        public Task DeleteCustomerAsync(int id);
        public Task<IEnumerable<CustomerListDTOModel>> GetCustomerListAsync(
            string? filter = null, CustomerSortColumn? sortColumn = null, SortDirection? sortDirection = null);

        public Task<PagedResult<CustomerListDTOModel>> GetPagedCustomerListAsync(
            int pageNumber, int pageSize, string? filter = null, CustomerSortColumn? sortColumn = null, SortDirection? sortDirection = null);
        public Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id);
    }
}
