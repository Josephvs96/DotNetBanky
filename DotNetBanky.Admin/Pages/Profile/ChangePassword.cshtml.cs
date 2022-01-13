using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SmartBreadcrumbs.Attributes;
using System.Security.Claims;

namespace DotNetBanky.Admin.Pages.Profile
{
    [Breadcrumb("Change Password", FromPage = (typeof(IndexModel)))]
    public class ChangePassword : PageModel
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        public ChangePassword(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public UserChangePasswordModel InputModel { get; set; }

        public void OnGet()
        {
            InputModel = new();
            InputModel.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        await _userService.ChangePasswordAsync(InputModel);
                        _toastNotification.AddSuccessToastMessage("Password updated!");
                        return LocalRedirect("~/");
                    }
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage($"While changing the password Error: {e.Message}");
                }
            }

            return Page();
        }
    }
}
