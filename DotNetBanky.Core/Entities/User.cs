using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.Core.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public Customer? Customer { get; set; }
    }
}
