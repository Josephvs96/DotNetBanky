using DotNetBanky.Core.DTOModels.User;
using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.BLL.Services
{
    public interface IUserService
    {
        Task ChangePasswordAsync(UserChangePasswordModel model);
        Task CreateAsync(UserCreateModel model);
        Task<Customer> CreateWithCustomerAsync(UserCreateModel model);
        Task DeleteUserAsync(UserDTOModel model);
        Task<List<UserDTOModel>> GetAllUsersAsync();
        Task<List<IdentityRole>> GetAvailableRollesAsync();
        Task<UserDTOModel> GetUserByIdAsync(string userId);
        Task<string> LoginAsync(UserLoginModel model);

        Task LogoutAsnyc();
        Task UpdateUserInfoAsync(UserDTOModel model);
    }
}
