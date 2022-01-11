using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Edit", FromPage = typeof(IndexModel))]
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public EditModel(ICustomerService customerService, IMapper mapper, INotyfService notyfService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        [BindProperty]
        public CustomerEditModel InputModel { get; set; } = null!;
        public SelectList CountryList { get; set; } = new SelectList(CountryConstants.CountryList);
        public SelectList GenderList { get; set; } = new SelectList(GenderConstants.GenderList, "Value", "Key");

        public async Task OnGet(int customerId)
        {
            InputModel = _mapper.Map<CustomerEditModel>(await _customerService.GetCustomerDetailsAsync(customerId));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _customerService.EditCustomerAsync(InputModel);
                    _notyfService.Success("Customer updated successfully!");
                    return RedirectToPage("Customer", new { customerId = InputModel.CustomerId });
                }
            }
            catch (Exception e)
            {
                _notyfService.Error($"Could not update customer Error: {e.Message[0..50]}...");
            }

            return Page();
        }
    }
}
