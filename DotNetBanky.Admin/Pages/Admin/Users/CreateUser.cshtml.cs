
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Admin.Users
{
    [Breadcrumb("Create User", FromPage = typeof(UsersListModel))]
    public class CreateUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IToastNotification _toastNotification;

        public CreateUserModel(IUserService userService, IToastNotification toastNotification)
        {
            _userService = userService;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public UserCreateModel? InputModel { get; set; }

        public async Task OnGetAsync()
        {
            InputModel = new();
            InputModel.Roles = new SelectList(await _userService.GetAvailableRollesAsync(), "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.CreateAsync(InputModel);
                    _toastNotification.AddSuccessToastMessage("User created successfully!");
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
    }
}
