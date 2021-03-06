using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Dashboard;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.Extensions.Caching.Memory;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class DashBoardService : IDashboardService
    {
        private readonly IDispositionsRepository _dispositionsRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICountryService _countryService;
        private readonly IMemoryCache _memoryCache;

        public DashBoardService(
            IDispositionsRepository dispositionsRepository,
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository,
            ICountryService countryService,
            IMemoryCache memoryCache)
        {
            _dispositionsRepository = dispositionsRepository;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _countryService = countryService;
            _memoryCache = memoryCache;
        }

        public async Task<int> GetTotalNumberOfAccountsAsync()
        {
            return await _accountRepository.GetTotalNumberOfAccountsAsync();
        }

        public async Task<decimal> GetTotalAccountsBalanceAsync()
        {
            return await _accountRepository.GetTotalBalanceOfAllAccountsAsync();
        }

        public async Task<int> GetTotalNumberOfCustomersAsync()
        {
            return await _customerRepository.GetTotalNumberOfCustomersAsync();
        }

        public async Task<DashboardSummeryDTO> GetDashboardSummeryAsync()
        {
            return new DashboardSummeryDTO
            {
                TotalNumberOfAccounts = await _accountRepository.GetTotalNumberOfAccountsAsync(),
                TotalNumberOfCustomers = await _customerRepository.GetTotalNumberOfCustomersAsync(),
                TotalSumOfAllAccounts = await _accountRepository.GetTotalBalanceOfAllAccountsAsync(),
            };
        }

        public async Task<List<DashboardCountryCardDTO>> GetDashboardCountriesSummeryAsync()
        {
            var allDisposionData = _dispositionsRepository.GetAllDispostions();

            var dataToReturn = allDisposionData.GroupBy(d => d.Customer.Country).Select(async c => new DashboardCountryCardDTO
            {
                CountryName = c.Key,
                FlagURL = await GetCountryFlagUrl(c.Key),
                TotalNumberOfAccounts = c.
                Where(c => c.Type == DispostionsConstants.Owner)
                .DistinctBy(c => c.Account.AccountId)
                .Count(),

                TotalNumberOfCustomers = c
                .DistinctBy(c => c.Customer.CustomerId)
                .Count(),

                TotalSumOfAllAccounts = c
                .Where(c => c.Type == DispostionsConstants.Owner)
                .DistinctBy(c => c.Account.AccountId)
                .Sum(c => c.Account.Balance),
            });

            return (await Task.WhenAll(dataToReturn)).Where(result => result != null).ToList();
        }

        private async Task<string> GetCountryFlagUrl(string countryName)
        {
            return await _memoryCache.GetOrCreateAsync(countryName, async x =>
            {
                x.SlidingExpiration = TimeSpan.FromDays(1);
                return await _countryService.GetCountryFlagUrl(countryName);
            });
        }

        public async Task<DashboardCountryDetailsDTO> GetDashboardCountryDetailsAsync(string countryName)
        {

            var topCustomers = await _customerRepository.GetTopCustomersWithAccountsAsync(countryName);

            return new DashboardCountryDetailsDTO
            {
                TopCustomers = topCustomers.Select(c => new CustomerCountryDetailsDTO
                {
                    CustomerId = c.CustomerId,
                    EmailAddress = c.Emailaddress ?? "No email address available",
                    GivenName = $"{c.Givenname} {c.Surname}",
                    AccountBalance = c.Dispositions.Sum(x => x.Account.Balance)
                })
            };
        }
    }
}
