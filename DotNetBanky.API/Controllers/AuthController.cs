using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBanky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _userService.LoginAsync(model);
                return Ok(new { accessToken = token });
            }
            return BadRequest();
        }
    }
}
