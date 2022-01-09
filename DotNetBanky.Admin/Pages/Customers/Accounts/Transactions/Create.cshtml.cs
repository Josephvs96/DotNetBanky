using AspNetCoreHero.ToastNotification.Abstractions;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using DotNetBanky.Core.DTOModels.Transaction;
using DotNetBanky.Core.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;

namespace DotNetBanky.Admin.Pages.Customers.Accounts.Transactions
{
    [Breadcrumb("New Transaction", FromPage = typeof(CustomerModel))]
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly INotyfService _notyfService;

        public CreateModel(IAccountService accountService, INotyfService notyfService)
        {
            _accountService = accountService;
            _notyfService = notyfService;
        }

        [BindProperty(SupportsGet = true)]
        public TransactionCreateDTO InputModel { get; set; } = null!;
        public List<AccountSummeryDTO> CustomerAccounts { get; set; }
        public SelectList AccountsList { get; set; }
        public SelectList TypeList { get; set; } = new(TransactionConstants.TransactionTypes);
        public SelectList OperationList { get; set; } = new(TransactionConstants.TransactionOperations);

        public async Task OnGetAsync(int customerId, int? accountId = null)
        {
            //InputModel = new TransactionCreateDTO();
            SetupBreadcrumb(customerId);
            await PopulateAccountList(customerId);

            InputModel.CustomerId = customerId;
            if (accountId != null)
                InputModel.AccountFrom = accountId.Value;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _accountService.CreateAccountTransaction(InputModel);
                _notyfService.Success("New transaction created successfully!");
                return LocalRedirect($"/Customer/{InputModel.CustomerId}/Account/{InputModel.AccountFrom}");
            }
            catch (Exception e)
            {
                _notyfService.Error(e.Message.ToString(), durationInSeconds: 5);
                await PopulateAccountList(InputModel.CustomerId);
            }

            return Page();
        }

        private void SetupBreadcrumb(int customerId)
        {

            var currentNode = new RazorPageBreadcrumbNode("/Customers/Accounts/Account", "New Transaction")
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

        private async Task PopulateAccountList(int customerId)
        {
            CustomerAccounts = await _accountService.GetAccountsListByCustomerId(customerId);

            AccountsList = new(
                CustomerAccounts
                .Select(a => new
                {
                    AccountId = a.AccountId,
                    Text = $"{a.AccountId} - {a.AccountType.ToPascalCase()} - {a.Balance.ToStringFormated()}"
                }), "AccountId", "Text");
        }
    }
}
