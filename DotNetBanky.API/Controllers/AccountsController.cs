using DotNetBanky.BLL.Services;
using DotNetBanky.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBanky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(RoleConstants.CashierAndAbove)]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountTransactions(int accountId, int limit = 20, int offset = 1)
        {
            var details = await _accountService.GetPagedTransactions(accountId, offset, limit);

            return Ok(details.Results);
        }
    }
}
