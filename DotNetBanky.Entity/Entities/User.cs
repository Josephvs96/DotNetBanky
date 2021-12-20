using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.Entity.Entities
{
    public class User : IdentityUser
    {
        public string LoginName { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
