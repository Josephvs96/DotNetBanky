using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Create", FromPage = typeof(IndexModel))]
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IToastNotification _toastNotification;

        public CreateModel(ICustomerService customerService, IToastNotification toastNotification)
        {
            _customerService = customerService;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public CustomerCreateModel InputModel { get; set; } = null!;

        public SelectList CountryList { get; set; } = new SelectList(CountryConstants.CountryList);
        public SelectList GenderList { get; set; } = new SelectList(GenderConstants.GenderList, "Value", "Key");

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _customerService.CreateCustomerAsync(InputModel);
                    _toastNotification.AddSuccessToastMessage("New customer added successfully!");
                    return LocalRedirect("/Customers/Index");
                }
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }

            return Page();
        }
    }
}
