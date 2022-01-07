using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Nodes;

namespace DotNetBanky.Admin.Pages.Customers.Accounts
{
    //[Breadcrumb("Account", FromPage = typeof(CustomerModel))]
    public class AccountModel : PageModel
    {
        private readonly IAccountService _accountService;

        public AccountModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public AccountDetailsDTO InputModel { get; set; } = null!;
        public int AccountId { get; set; }

        public async Task OnGetAsync(int customerId, int accountId)
        {
            AccountId = accountId;
            SetupBreadcrumb(customerId);

            InputModel = await _accountService.GetAccountDetailsByAccountId(accountId);
        }

        public async Task<IActionResult> OnGetMore(int currnetPage, int accountId)
        {
            var jsonTransactions = await _accountService.GetPagedTransactions(accountId, currnetPage, 20);
            return new JsonResult(jsonTransactions.Results);
        }

        private void SetupBreadcrumb(int customerId)
        {

            var currentNode = new RazorPageBreadcrumbNode("/Customers/Accounts/Account", "Account")
            {
                Parent = new RazorPageBreadcrumbNode("/Customers/Customer", "Customer Details")
                {
                    RouteValues = new Dictionary<string, object>()
                {
                    { "customerId", customerId }
                },
                    Parent = new RazorPageBreadcrumbNode("/Customers/Index", "Customers")
                },
            };

            ViewData["BreadcrumbNode"] = currentNode;
        }
    }
}
