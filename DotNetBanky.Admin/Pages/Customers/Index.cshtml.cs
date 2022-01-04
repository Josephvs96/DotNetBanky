using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Customer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Customers")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public List<CustomerListDTOModel> CustomersList { get; set; } = null!;
        public int NumberOfPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; }
        public string? Filter { get; set; }

        public async Task OnGetAsync(int pageNumber, string? filter = null)
        {
            Filter = filter;

            if (filter != null) NumberOfPages = await _customerService.GetTotalNumberOfPages(PageSize, c => c.Surname.Contains(filter));
            else NumberOfPages = await _customerService.GetTotalNumberOfPages(PageSize);

            CurrentPage = (pageNumber > 0 && pageNumber <= NumberOfPages) ? pageNumber : 1;

            CustomersList = (await _customerService.GetAllCustomerListAsync(CurrentPage, PageSize, filter)).ToList();
        }
    }
}
