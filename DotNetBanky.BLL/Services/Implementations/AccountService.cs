using AutoMapper;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Transaction;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Exceptions;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace DotNetBanky.BLL.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public AccountService(
            IAccountRepository accountRepository,
            IGenericRepository<Transaction> transactionRepository,
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task CreateAccountAndAssignToCustomer(AccountCreateModel model)
        {
            var customer = await _customerRepository.GetOneByIdAsync(model.CustomerId);
            var newAccount = _mapper.Map<Account>(model);
            newAccount.Dispositions = new List<Disposition>()
            {
                new Disposition()
                {
                    Account = newAccount,
                    Customer = customer,
                    Type = model.AccountType
                }
            };

            await _accountRepository.AddOneAsync(newAccount);
        }

        public async Task CreateAccountTransaction(TransactionCreateDTO model)
        {
            var accountFrom = await _accountRepository.GetOneByIdAsync(model.AccountFrom);

            if (!accountFrom.Dispositions.Any(d => d.Customer.CustomerId == model.CustomerId))
                throw new AccountNotOwnedByCustomerException();

            switch (model.Operation)
            {
                case TransactionConstants.Deposit:
                    {
                        var transaction = _mapper.Map<Transaction>(model);
                        transaction.Balance += accountFrom.Balance;
                        accountFrom.Transactions.Add(transaction);
                        accountFrom.Balance += transaction.Amount;
                        await _accountRepository.CreateNewDepositTransaction(accountFrom);
                        break;
                    }
                case TransactionConstants.Withdraw:
                    {
                        var transaction = _mapper.Map<Transaction>(model);
                        transaction.Balance = accountFrom.Balance - transaction.Amount;
                        transaction.Amount *= -1;
                        if (transaction.Balance < 0) throw new TransactionAmountLargerThanBalanceException();

                        accountFrom.Transactions.Add(transaction);
                        accountFrom.Balance = transaction.Balance;
                        await _accountRepository.CreateNewDepositTransaction(accountFrom);
                        break;
                    }
                case TransactionConstants.Transaction:
                    {
                        if (!model.AccountTo.HasValue) throw new AccountToNotProvided();

                        var accountTo = await _accountRepository.GetOneByIdAsync(model.AccountTo.Value);
                        if (accountTo == null) throw new NotFoundException("The To Account could not be found in the local system");

                        var transactionFrom = _mapper.Map<Transaction>(model);
                        transactionFrom.Amount *= -1;
                        transactionFrom.Balance = accountFrom.Balance + transactionFrom.Amount;
                        transactionFrom.Account = model.AccountTo.Value.ToString();
                        accountFrom.Transactions.Add(transactionFrom);
                        accountFrom.Balance = transactionFrom.Balance;

                        var transactionTo = _mapper.Map<Transaction>(model);
                        transactionTo.Balance = accountTo.Balance + transactionTo.Amount;
                        transactionTo.Account = model.AccountFrom.ToString();
                        accountTo.Transactions.Add(transactionTo);
                        accountTo.Balance = transactionTo.Balance;

                        await _accountRepository.CreateNewTransaction(accountFrom, accountTo);
                        break;
                    }
                default:
                    break;
            }
        }

        public async Task<AccountDetailsDTO> GetAccountDetailsByAccountId(int accountId)
        {
            var account = await _accountRepository.GetOneAsync(
                filter: a => a.AccountId == accountId);

            var transactions = await _transactionRepository.GetPagedListAsync(
                filter: t => t.AccountNavigation.AccountId == accountId,
                orderBy: q => q.OrderByDescending(t => t.Date),
                page: 1,
                pageSize: 20);

            var accountDetailsDto = _mapper.Map<AccountDetailsDTO>(account);
            accountDetailsDto.PagedTransactions = _mapper.Map<PagedResult<TransactionDTO>>(transactions);

            return accountDetailsDto;
        }

        public async Task<List<AccountSummeryDTO>> GetAccountsListByCustomerId(int customerId)
        {
            var accounts = await _accountRepository.GetListAsync(
                filter: a => a.Dispositions.Any(d => d.Customer.CustomerId == customerId),
                include: q => q.Include(a => a.Dispositions));
            return _mapper.Map<List<AccountSummeryDTO>>(accounts);
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
