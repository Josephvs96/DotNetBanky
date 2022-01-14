using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Customers
{
    [Breadcrumb("Customers")]
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost(int customerId)
        {
            return RedirectToPage("/Customers/Customer", new { customerId = customerId });
        }

    }
}
