using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DotNetBanky.Core.Identity
{
    public class AddDisplayNameClaimsTransformation : IClaimsTransformation
    {

        private readonly UserManager<User> _userManager;

        public AddDisplayNameClaimsTransformation(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            // Support AD and local accounts
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
                                                              c.Type == ClaimTypes.Name);
            if (nameId == null)
            {
                return principal;
            }

            // Get user from database
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                return principal;
            }

            newIdentity.AddClaim(new Claim("DisplayName", user.DisplayName));
            newIdentity.AddClaim(new Claim("CustomerId", user.CustomerId.ToString()));
            return clone;
        }
    }
}
