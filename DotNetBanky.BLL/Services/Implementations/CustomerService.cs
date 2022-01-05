using AutoMapper;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Enums;
using DotNetBanky.Core.Exceptions;
using DotNetBanky.Core.Extentions;
using DotNetBanky.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task CreateCustomerAsync(CustomerCreateModel model)
        {
            var customer = _mapper.Map<Customer>(model);
            await _customerRepository.AddOneAsync(customer);
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditCustomerAsync(CustomerEditModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerListDTOModel>> GetCustomerListAsync(
            string? filter = null, CustomerSortColumn? sortColumn = null, SortDirection? sortDirection = null)
        {
            Expression<Func<Customer, bool>>? filterExp = null;
            if (filter != null) filterExp = x => x.Surname.Contains(filter);

            var customersList = await _customerRepository.GetListAsync(filter: filterExp, orderBy: x => x.SortBy(sortColumn, sortDirection));

            return _mapper.Map<IEnumerable<CustomerListDTOModel>>(customersList);
        }

        public async Task<PagedResult<CustomerListDTOModel>> GetPagedCustomerListAsync(
            int pageNumber, int pageSize, string? filter = null, CustomerSortColumn? sortColumn = null, SortDirection? sortDirection = null)
        {
            Expression<Func<Customer, bool>>? filterExp = null;
            if (filter != null) filterExp = x => x.Surname.Contains(filter);

            var customersList = await _customerRepository.GetPagedListAsync(
                page: pageNumber,
                pageSize: pageSize,
                filter: filterExp,
                orderBy: x => x.SortBy(sortColumn, sortDirection));

            return _mapper.Map<PagedResult<CustomerListDTOModel>>(customersList);
        }

        public async Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id)
        {
            var customer = await _customerRepository.GetOneByIdAsync(id);
            if (customer == null) throw new NotFoundException("Customer could not be found");
            return _mapper.Map<CustomerDetailsDTOModel>(customer);
        }

        public async Task<int> GetTotalNumberOfPages(int pageSize, string? filter = null)
        {
            Expression<Func<Customer, bool>>? filterExp = null;

            if (filter != null) filterExp = x => x.Givenname.Contains(filter) || x.Surname.Contains(filter);

            var records = await _customerRepository.GetNumberOfRecords(filterExp);
            return (records / pageSize) + 1;
        }
    }
}
