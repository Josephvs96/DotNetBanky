using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Customers")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
