using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetBanky.Admin.Pages.Auth
{
    [AllowAnonymous]
    public class SignupModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public SignupModel(RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            _roleManager = roleManager;
            _userService = userService;
        }

        [BindProperty]
        public UserCreateModel InputModel { get; set; }
        public string? ReturnUrl { get; set; }

        public async Task OnGetAsync(string? returnUrl)
        {
            InputModel = new UserCreateModel { Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name") };
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.CreateAsync(InputModel);
                    return LocalRedirect(returnUrl);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    InputModel.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                    return Page();
                }
            }

            InputModel.Roles = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");

            return Page();
        }
    }
}
