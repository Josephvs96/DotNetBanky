using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Enums;
using DotNetBanky.Core.Extentions;
using DotNetBanky.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IAccountService accountService,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task CreateCustomerAsync(CustomerCreateModel model)
        {
            var customer = _mapper.Map<Customer>(model);
            customer.CountryCode = CountryConstants.CountryCodes[model.Country];
            customer.Dispositions = new List<Disposition>()
            {
                new Disposition()
                {
                    Account = new Account()
                    {
                        Balance = 0,
                        Frequency = FrequencyConstants.Monthly,
                        Created = DateTime.UtcNow,
                    },
                    Type = DispostionsConstants.Owner
                }
            };

            await _customerRepository.AddOneAsync(customer);
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task EditCustomerAsync(CustomerEditModel model)
        {
            model.CountryCode = CountryConstants.CountryCodes[model.Country];
            await _customerRepository.UpdateOneAsync(_mapper.Map<Customer>(model));
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
            if (filter != null)
                filterExp = x =>
                x.Surname.Contains(filter) || x.Givenname.Contains(filter) || x.CustomerId.ToString() == filter;

            var customersList = await _customerRepository.GetPagedListAsync(
                page: pageNumber,
                pageSize: pageSize,
                filter: filterExp,
                orderBy: x => x.SortBy(sortColumn, sortDirection));

            return _mapper.Map<PagedResult<CustomerListDTOModel>>(customersList);
        }

        public async Task<CustomerDetailsDTOModel> GetCustomerDetailsAsync(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                var mappedCustomer = _mapper.Map<CustomerDetailsDTOModel>(customer);
                mappedCustomer.TotalBalance = await _customerRepository.GetTotalAccountsBalanceByCustomerId(id);
                mappedCustomer.Accounts = await _accountService.GetAccountsListByCustomerId(id);
                return mappedCustomer;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
