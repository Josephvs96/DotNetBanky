using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Search;
using DotNetBanky.Core.DTOModels.User;
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
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ISearchService _searchService;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IAccountService accountService,
            IUserService userService,
            ISearchService searchService,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _accountService = accountService;
            _userService = userService;
            _searchService = searchService;
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

            var user = _mapper.Map<UserCreateModel>(model);
            user.Customer = customer;
            var createdCustomer = await _userService.CreateWithCustomerAsync(user);
            await _searchService.CreatOrUpdateDocumentAsync(_mapper.Map<CustomerSearchDTO>(createdCustomer));
        }

        public Task DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task EditCustomerAsync(CustomerEditModel model)
        {
            model.CountryCode = CountryConstants.CountryCodes[model.Country];
            var updatedCustomer = await _customerRepository.UpdateOneAsync(_mapper.Map<Customer>(model));
            await _searchService.CreatOrUpdateDocumentAsync(_mapper.Map<CustomerSearchDTO>(updatedCustomer));
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
                throw new NotFoundException("Customer could not be found");
            }

        }
    }
}
