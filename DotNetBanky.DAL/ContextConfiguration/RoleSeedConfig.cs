using DotNetBanky.Utility.Constents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetBanky.DAL.ContextConfiguration
{
    public class RoleSeedConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = new Guid().ToString(),
                    Name = RoleConstents.Admin,
                    NormalizedName = RoleConstents.Admin.ToUpper(),
                },
                new IdentityRole
                {
                    Id = new Guid().ToString(),
                    Name = RoleConstents.Cahsier,
                    NormalizedName = RoleConstents.Cahsier.ToUpper()
                }
           );
        }
    }
}
