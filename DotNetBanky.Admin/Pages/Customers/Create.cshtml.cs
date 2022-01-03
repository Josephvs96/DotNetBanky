using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Create", FromPage = typeof(IndexModel))]
    public class CreateModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
