using DotNetBanky.Core.Constants;
using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.DAL.Context
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDatabaseAsync(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var adminUser = new User { UserName = "StefanAdmin", Email = "stefan.holmberg@systementor.se", EmailConfirmed = true };
                var cashierUser = new User { UserName = "StefanCashier", Email = "stefan.holmberg@nackademin.se", EmailConfirmed = true };

                await userManager.CreateAsync(adminUser, "Hejsan123#");
                await userManager.CreateAsync(cashierUser, "Hejsan123#");
            }

            if (!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleConstants.Admin,
                    NormalizedName = RoleConstants.Admin.ToUpper(),
                };
                var cashierRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleConstants.Cahsier,
                    NormalizedName = RoleConstants.Cahsier.ToUpper()
                };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(cashierRole);
            }


            await db.SaveChangesAsync();
        }
    }
}
