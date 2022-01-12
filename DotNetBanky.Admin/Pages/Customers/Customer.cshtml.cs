using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Customer Details", FromPage = typeof(IndexModel))]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IToastNotification _toastNotification;

        public CustomerModel(ICustomerService customerService, IToastNotification toastNotification)
        {
            _customerService = customerService;
            _toastNotification = toastNotification;
        }
        public CustomerDetailsDTOModel InputModel { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            try
            {
                InputModel = await _customerService.GetCustomerDetailsAsync(customerId);
                return Page();
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }
            return RedirectToPage("/Customers/Index");
        }


    }
}
