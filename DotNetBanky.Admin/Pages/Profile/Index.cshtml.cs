using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Profile
{
    [Breadcrumb("Profile")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
