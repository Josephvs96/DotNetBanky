using AutoMapper;
using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using DotNetBanky.Core.DTOModels.User;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Exceptions;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MockQueryable.NSubstitute;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DotNetBanky.TEST.UnitTests.Services
{
    public class UserServiceTests
    {
        protected readonly IMapper _mapper;
        protected readonly IConfiguration Configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        public UserServiceTests()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserCreateModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();
                cfg.CreateMap<UserDTOModel, User>().ForMember(usr => usr.UserName, opt => opt.MapFrom(dto => dto.DisplayName)).ReverseMap();

            }).CreateMapper();

            var configurationBody = new Dictionary<string, string>
            {
                {"JwtConfiguration:SecretKey", "Super secret token key"},
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationBody)
                .Build();

            var contextAccessor = Substitute.For<IHttpContextAccessor>();
            var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<User>>();
            var userStore = Substitute.For<IUserStore<User>>();
            var identityStore = Substitute.For<IRoleStore<IdentityRole>>();
            _userManager = Substitute.For<UserManager<User>>(userStore, null, null, null, null, null, null, null, null);
            _roleManager = Substitute.For<RoleManager<IdentityRole>>(identityStore, null, null, null, null);
            _signInManager = Substitute.For<SignInManager<User>>(_userManager, contextAccessor, userPrincipalFactory, null, null, null);
            _userService = new UserService(_mapper, _userManager, _signInManager, _roleManager, Configuration);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_User_To_Database_And_Assign_Role()
        {
            // Arrange
            var createUserModel = Builder<UserCreateModel>.CreateNew().Build();
            var applicationUser = Builder<User>.CreateNew().Build();
            _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Success);
            _userManager.FindByNameAsync(Arg.Any<string>()).Returns(applicationUser);

            // Act
            await _userService.CreateAsync(createUserModel);

            // Assert
            await _userManager.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<string>());
            await _userManager.Received(1).AddToRoleAsync(Arg.Any<User>(), Arg.Any<string>());
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_Exception_If_Adding_To_Db_Fails()
        {
            // Arrange
            var createUserModel = Builder<UserCreateModel>.CreateNew().Build();
            var identityErros = Builder<IdentityError>.CreateListOfSize(5).Build().ToArray();
            _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(IdentityResult.Failed(identityErros));

            // Act
            Func<Task> callCreateAsync = async () => await _userService.CreateAsync(createUserModel);

            // Assert
            await callCreateAsync.Should().ThrowAsync<BadRequestException>().WithMessage(identityErros.FirstOrDefault()?.Description);
            await _userManager.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<string>());
        }

        [Fact]
        public async Task LoginAsync_Should_Throw_Excetpion_If_User_Does_Not_Exisit()
        {
            // Arrange
            var logInModel = Builder<UserLoginModel>.CreateNew().Build();
            var emptyUsersList = new List<User>().AsQueryable().BuildMock();
            _userManager.Users.Returns(emptyUsersList);

            // Act
            Func<Task> callLoginAsync = async () => await _userService.LoginAsync(logInModel);

            // Assert
            await callLoginAsync.Should().ThrowAsync<NotFoundException>().WithMessage("Username or password is incorrect");
        }

        [Fact]
        public async Task LoginAsync_Should_Throw_Exception_If_Provided_Wrong_Login_Information()
        {
            // Arrange
            var loginUserModel = Builder<UserLoginModel>.CreateNew().Build();
            var usersListQueryable = Builder<User>.CreateListOfSize(1)
                .All().With(u => u.Email = loginUserModel.Email)
                .Build().AsQueryable().BuildMock();
            _userManager.Users.Returns(usersListQueryable);
            _signInManager
                .PasswordSignInAsync(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Failed);

            // Act
            Func<Task> callCreateAsync = async () => await _userService.LoginAsync(loginUserModel);

            // Assert
            await callCreateAsync.Should().ThrowAsync<BadRequestException>().WithMessage("Username or password is incorrect");
            await _signInManager.Received(1).PasswordSignInAsync(Arg.Any<User>(), Arg.Any<string>(),
                Arg.Any<bool>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Login_Response_If_Credentials_Are_Good()
        {
            // Arrange
            var loginUserModel = Builder<UserLoginModel>.CreateNew().Build();
            var usersList = Builder<User>.CreateListOfSize(1).All()
                .With(u => u.Email = loginUserModel.Email).Build();
            var usersListQueryable = usersList.AsQueryable().BuildMock();
            _userManager.Users.Returns(usersListQueryable);
            _signInManager
                .PasswordSignInAsync(Arg.Any<User>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(SignInResult.Success);

            // Act
            var result = await _userService.LoginAsync(loginUserModel);

            // Assert
            result.Should().NotBeNullOrEmpty();
            await _signInManager.Received(1).PasswordSignInAsync(Arg.Any<User>(), Arg.Any<string>(),
                Arg.Any<bool>(), Arg.Any<bool>());
        }

        [Fact]
        public async Task GetAllUsersAsync_Should_Return_List_Of_UserDTOModel_Of_All_Users_With_Admin_Role()
        {
            // Arrange
            var usersList = Builder<User>.CreateListOfSize(5).Build().AsQueryable().BuildMock(); ;
            var usersRole = new List<string> { RoleConstants.Admin };
            _userManager.Users.Returns(usersList);
            _userManager.GetRolesAsync(Arg.Any<User>()).Returns(usersRole);

            // Act
            var users = await _userService.GetAllUsersAsync();

            // Assert
            users.Should().HaveCount(5);
            users.Should().ContainItemsAssignableTo<UserDTOModel>();
            users.All(u => u.Role == RoleConstants.Admin).Should().BeTrue();
        }

        [Fact]
        public async Task GetAvailableRollesAsync_Should_Return_Two_Roles()
        {
            // Arrange
            var rolesList = Builder<IdentityRole>.CreateListOfSize(2).Build().AsQueryable().BuildMock();
            _roleManager.Roles.Returns(rolesList);

            // Act
            var roles = await _userService.GetAvailableRollesAsync();

            // Assert
            roles.Should().HaveCount(2);
            roles.Should().BeEquivalentTo(rolesList);
        }

    }
}
