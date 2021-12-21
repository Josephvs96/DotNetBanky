using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetBanky.Admin.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserLoginModel UserLogin { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _userService.LoginAsync(UserLogin);
                return LocalRedirect("/");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }

        }
    }
}
