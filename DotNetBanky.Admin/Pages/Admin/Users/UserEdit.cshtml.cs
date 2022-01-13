
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Admin.Users
{
    [Breadcrumb("Edit User", FromPage = typeof(UsersListModel))]
    public class UserEditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        public UserEditModel(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public UserDTOModel? InputModel { get; set; }

        public async Task OnGetAsync(string userId)
        {
            InputModel = await _userService.GetUserByIdAsync(userId);
            InputModel.Roles = new SelectList(await _userService.GetAvailableRollesAsync(), "Name", "Name");
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUserInfoAsync(InputModel);
                    _toastNotification.AddSuccessToastMessage("User information updated successfully!");
                    return LocalRedirect("/Admin/Users/UsersList");
                }
                catch (Exception e)
                {
                    _toastNotification.AddErrorToastMessage(e.Message);
                }
            }

            InputModel.Roles = new SelectList(await _userService.GetAvailableRollesAsync(), "Name", "Name");

            return Page();

        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            try
            {
                await _userService.DeleteUserAsync(InputModel);
                _toastNotification.AddSuccessToastMessage("User deleted successfully!");
                return LocalRedirect("/Admin/Users/UsersList");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }

            InputModel.Roles = new SelectList(await _userService.GetAvailableRollesAsync(), "Name", "Name");

            return Page();
        }
    }
}
