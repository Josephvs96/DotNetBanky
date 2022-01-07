using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Customer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Customer Details", FromPage = typeof(IndexModel))]
    public class CustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;

        }
        public CustomerDetailsDTOModel InputModel { get; set; } = null!;

        public async Task OnGetAsync(int customerId)
        {
            InputModel = await _customerService.GetCustomerDetailsAsync(customerId);
        }


    }
}
