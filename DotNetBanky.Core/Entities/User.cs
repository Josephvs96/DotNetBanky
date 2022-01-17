using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.Core.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public string DisplayName { get; set; } = null!;
        public Customer? Customer { get; set; }
        public int? CustomerId { get; set; }
    }
}
