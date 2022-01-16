using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetBanky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(RoleConstants.Customer)]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("/api/Me")]
        public async Task<IActionResult> GetCustomerProfileAsync()
        {
            var userId = int.Parse(User.FindFirstValue("CustomerId"));
            var customerDetails = await _customerService.GetCustomerDetailsAsync(userId);

            return Ok(customerDetails);
        }
    }
}
