using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Transaction;

namespace DotNetBanky.BLL.Services
{
    public interface IAccountService
    {
        Task<AccountDetailsDTO> GetAccountDetailsByAccountId(int accountId);
        Task<List<AccountSummeryDTO>> GetAccountsListByCustomerId(int customerId);

        Task<PagedResult<TransactionDTO>> GetPagedTransactions(int accountId, int page, int pageSize);

        Task CreateAccountAndAssignToCustomer(AccountCreateModel model);

        Task CreateAccountTransaction(TransactionCreateDTO model);
    }
}
