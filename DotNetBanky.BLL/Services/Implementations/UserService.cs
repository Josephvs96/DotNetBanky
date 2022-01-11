using AutoMapper;
using DotNetBanky.Core.DTOModels.User;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Exceptions;
using DotNetBanky.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNetBanky.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task CreateAsync(UserCreateModel model)
        {
            var user = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()!.Description);

            await _userManager.AddToRoleAsync(user, model.Role);

            var accountConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Send CONFIRMATION EMAIL
            // Delete the next line when implementing the email validation
            await _userManager.ConfirmEmailAsync(user, accountConfirmationToken);

            // await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task<Customer> CreateWithCustomerAsync(UserCreateModel model)
        {
            await CreateAsync(model);

            var createdUser = await _userManager.Users.Include(u => u.Customer).FirstAsync(u => u.Email == model.Email);

            return createdUser.Customer;
        }

        public async Task<string> LoginAsync(UserLoginModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                throw new NotFoundException("Username or password is incorrect");

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (!signInResult.Succeeded)
                throw new BadRequestException("Username or password is incorrect");

            var token = JwtHelper.GenerateToken(user, _configuration);

            return token.ToString();
        }

        public async Task LogoutAsnyc()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ChangePasswordAsync(UserChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) throw new NotFoundException("User does not exist anymore");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()!.Description);
        }

        public async Task<List<UserDTOModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new UserDTOModel
            {
                Email = u.Email,
                Id = u.Id,
                DisplayName = u.UserName,
                FullName = u.FullName,
                Role = _userManager.GetRolesAsync(u).GetAwaiter().GetResult().FirstOrDefault()!,
            }).ToList();
        }

        public async Task<UserDTOModel> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) throw new NotFoundException("User does not exist");
            var userToBeReturned = _mapper.Map<UserDTOModel>(user);
            userToBeReturned.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()!;
            return userToBeReturned;
        }

        public async Task UpdateUserInfoAsync(UserDTOModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null) throw new NotFoundException("User does not exist");

            user.UserName = model.DisplayName;
            user.FullName = model.FullName;

            var currentUserRoles = await _userManager.GetRolesAsync(user);
            if (currentUserRoles.Any() && !currentUserRoles.Contains(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()!.Description);
        }

        public async Task DeleteUserAsync(UserDTOModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null) throw new NotFoundException("User does not exist");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()!.Description);
        }

        public async Task<List<IdentityRole>> GetAvailableRollesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
