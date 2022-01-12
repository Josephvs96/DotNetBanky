using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SmartBreadcrumbs.Nodes;

namespace DotNetBanky.Admin.Pages.Customers.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IToastNotification _toastNotification;

        public CreateModel(IAccountService accountService, IToastNotification toastNotification)
        {
            _accountService = accountService;
            _toastNotification = toastNotification;
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
                if (ModelState.IsValid)
                {
                    await _accountService.CreateAccountAndAssignToCustomer(InputModel);
                    _toastNotification.AddSuccessToastMessage("Account created successfully!");
                    return LocalRedirect($"/Customer/{InputModel.CustomerId}");
                }
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage($"Error while creating new account {e.Message}");
            }

            return Page();
        }

        private void SetupBreadcrumb(int customerId)
        {

            var currentNode = new RazorPageBreadcrumbNode("/Customers/Accounts/Account", "Create Account")
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
