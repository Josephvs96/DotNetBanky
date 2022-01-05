using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Customer;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Enums;
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


        public PagedResult<CustomerListDTOModel> PagedResult { get; set; }

        public int PageSize { get; set; } = 10;
        public string? Filter { get; set; }
        public CustomerSortColumn? SortColumn { get; set; }
        public SortDirection? SortDirection { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, string? filter = null, CustomerSortColumn? sortColumn = null, SortDirection? sortDirection = null)
        {
            if (pageNumber <= 1) pageNumber = 1;

            Filter = filter;
            SortColumn = sortColumn;
            SortDirection = sortDirection;

            PagedResult = await _customerService.GetPagedCustomerListAsync(pageNumber, PageSize, filter, sortColumn, sortDirection);
        }
    }
}
