using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DotNetBanky.Core.Identity
{
    public class IdentityClaimsTransformation : IClaimsTransformation
    {

        private readonly UserManager<User> _userManager;

        public IdentityClaimsTransformation(UserManager<User> userManager)
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

            // Added the display name and the customer id for the claims list
            newIdentity.AddClaim(new Claim("DisplayName", user.DisplayName));
            newIdentity.AddClaim(new Claim("CustomerId", user.CustomerId.ToString()));

            // Adding the user roles as we don't want to expose the roles in the public token sent to the user
            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                newIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return clone;
        }
    }
}
