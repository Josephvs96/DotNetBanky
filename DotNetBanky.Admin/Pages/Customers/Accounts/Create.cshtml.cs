using AspNetCoreHero.ToastNotification.Abstractions;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Nodes;

namespace DotNetBanky.Admin.Pages.Customers.Accounts
{
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
        public AccountCreateModel InputModel { get; set; } = null!;
        public SelectList FrequencyList { get; set; } = new SelectList(FrequencyConstants.FrequencyList);
        public SelectList TypeList { get; set; } = new SelectList(DispostionsConstants.DispotionsList, "Value", "Key");

        public void OnGet(int customerId)
        {
            InputModel.CustomerId = customerId;
            SetupBreadcrumb(customerId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _accountService.CreateAccountAndAssignToCustomer(InputModel);
                _notyfService.Success("Account created successfully!");
                return LocalRedirect($"/Customers/Accounts/Account/{InputModel.CustomerId}");
            }
            catch (Exception)
            {
                throw;
                _notyfService.Error("Error while creating new account");
            }

            return Page();
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
