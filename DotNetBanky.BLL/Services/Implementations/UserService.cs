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
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task CreateAsync(UserCreateModel model)
        {
            var user = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

            var accountConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Send CONFIRMATION EMAIL
            // Delete the next line when implementing the email validation
            await _userManager.ConfirmEmailAsync(user, accountConfirmationToken);
        }

        public async Task<string> LoginAsync(UserLoginModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                throw new NotFoundException("Username or password is incorrect");

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!signInResult.Succeeded)
                throw new BadRequestException("Username or password is incorrect");

            var token = JwtHelper.GenerateToken(user, _configuration);

            return token.ToString();
        }

        public async Task LogoutAsnyc()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ChangePasswordAsync(Guid userId, UserChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null) throw new NotFoundException("User does not exist anymore");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
        }
    }
}
