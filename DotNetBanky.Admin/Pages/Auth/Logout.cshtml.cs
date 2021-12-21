using DotNetBanky.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBanky.Admin.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly IUserService _userService;

        public LogoutModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await _userService.LogoutAsnyc();
            return LocalRedirect("~/");
        }
    }
}
