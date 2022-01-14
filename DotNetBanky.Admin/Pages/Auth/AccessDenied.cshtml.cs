using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBanky.Admin.Pages.Auth
{
    [AllowAnonymous]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
