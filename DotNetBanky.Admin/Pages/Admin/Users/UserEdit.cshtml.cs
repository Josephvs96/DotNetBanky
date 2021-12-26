using AspNetCoreHero.ToastNotification.Abstractions;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Admin.Users
{
    [Breadcrumb("Edit User", FromPage = typeof(UsersListModel))]
    public class UserEditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly INotyfService _notifyService;

        public UserEditModel(IUserService userService, INotyfService notifyService)
        {
            _userService = userService;
            _notifyService = notifyService;
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
                    _notifyService.Success("User information updated successfully!");
                    return LocalRedirect("/Admin/Users/UsersList");
                }
                catch (Exception e)
                {
                    _notifyService.Error(e.Message);
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
                _notifyService.Success("User deleted successfully!");
                return LocalRedirect("/Admin/Users/UsersList");
            }
            catch (Exception e)
            {
                _notifyService.Error(e.Message);
            }

            InputModel.Roles = new SelectList(await _userService.GetAvailableRollesAsync(), "Name", "Name");

            return Page();
        }
    }
}
