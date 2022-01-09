using DotNetBanky.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBanky.Admin.Pages.Auth
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LogoutModel : PageModel
    {
        private readonly IUserService _userService;

        public LogoutModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnGetAsync()
        {
            await _userService.LogoutAsnyc();
        }

        public async Task<IActionResult> OnPost()
        {
            await _userService.LogoutAsnyc();
            return LocalRedirect("~/");
        }
    }
}
