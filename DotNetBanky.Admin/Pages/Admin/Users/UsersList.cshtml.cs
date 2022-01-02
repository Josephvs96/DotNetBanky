using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Admin.Users
{
    [Breadcrumb("Users", FromPage = typeof(Dashboard.IndexModel))]
    public class UsersListModel : PageModel
    {
        private readonly IUserService _userService;

        public UsersListModel(IUserService userService)
        {
            _userService = userService;
        }

        public List<UserDTOModel> UsersList { get; set; }

        public async Task OnGetAsync()
        {
            UsersList = await _userService.GetAllUsersAsync();
        }
    }
}
