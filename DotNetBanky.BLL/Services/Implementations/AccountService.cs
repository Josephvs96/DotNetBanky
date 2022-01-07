using AutoMapper;
using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Transaction;
using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IGenericRepository<Transaction> transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<AccountDetailsDTO> GetAccountDetailsByAccountId(int accountId)
        {
            var account = await _accountRepository.GetOneAsync(
                filter: a => a.AccountId == accountId);

            var transactions = await _transactionRepository.GetListAsync(
                filter: t => t.AccountNavigation.AccountId == accountId,
                orderBy: query => query.OrderByDescending(t => t.Date),
                limit: 20);

            account.Transactions = transactions;
            return _mapper.Map<AccountDetailsDTO>(account);
        }

        public async Task<List<AccountSummeryDTO>> GetAccountsListByCustomerId(int customerId)
        {
            return _mapper.Map<List<AccountSummeryDTO>>(await _accountRepository.GetListAsync(
                filter: a => a.Dispositions.Any(d => d.Customer.CustomerId == customerId)));
        }

        public async Task<PagedResult<TransactionDTO>> GetPagedTransactions(int accountId, int page = 2, int pageSize = 20)
        {
            var transactions = await _transactionRepository.GetPagedListAsync(
                filter: t => t.AccountNavigation.AccountId == accountId,
                include: query => query.Include(t => t.AccountNavigation),
                orderBy: query => query.OrderByDescending(t => t.Date),
                page: page,
                pageSize: pageSize);

            return _mapper.Map<PagedResult<TransactionDTO>>(transactions);
        }
    }
}
