using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Repositories.IRepositories;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class DashBoardService : IDashboardService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;

        public DashBoardService(
            IAccountRepository accountRepository,
            ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetListAsync();
        }

        public async Task<int> GetTotalNumberOfAccountsAsync()
        {
            return await _accountRepository.GetTotalNumberOfAccountsAsync();
        }

        public async Task<decimal> GetTotalAccountsBalanceAsync()
        {
            return await _accountRepository.GetTotalBalanceOfAllAccountsAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetListAsync();
        }

        public async Task<int> GetTotalNumberOfCustomersAsync()
        {
            return await _accountRepository.GetTotalNumberOfAccountsAsync();
        }
    }
}
