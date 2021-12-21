using DotNetBanky.Core.DTOModels.User;

namespace DotNetBanky.BLL.Services
{
    public interface IUserService
    {
        Task ChangePasswordAsync(Guid userId, UserChangePasswordModel model);

        Task CreateAsync(UserCreateModel model);

        Task<string> LoginAsync(UserLoginModel model);

        Task LogoutAsnyc();
    }
}
