using AspNetCoreHero.ToastNotification.Abstractions;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Admin.Users
{
    [Breadcrumb("Create User", FromPage = typeof(UsersListModel))]
    public class CreateUserModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly INotyfService _notifyService;

        public CreateUserModel(IUserService userService, INotyfService notifyService)
        {
            _userService = userService;
            _notifyService = notifyService;
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
                    _notifyService.Success("User created successfully!");
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
    }
}
