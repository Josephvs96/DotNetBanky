using DotNetBanky.Core.Constants;
using DotNetBanky.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DotNetBanky.DAL.Context
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDatabaseAsync(ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
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
                var customerRole = new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleConstants.Customer,
                    NormalizedName = RoleConstants.Customer,
                };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(cashierRole);
                await roleManager.CreateAsync(customerRole);
            }
            await db.SaveChangesAsync();

            if (!userManager.Users.Any())
            {
                var adminUser = new User { FullName = "Stefan Admin", UserName = "Stefan Admin", Email = "stefan.holmberg@systementor.se", EmailConfirmed = true };
                var cashierUser = new User { FullName = "Stefan Cashier", UserName = "Stefan Cashier", Email = "stefan.holmberg@nackademin.se", EmailConfirmed = true };

                await userManager.CreateAsync(adminUser, "Hejsan123#");
                await userManager.CreateAsync(cashierUser, "Hejsan123#");

                await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
                await userManager.AddToRoleAsync(cashierUser, RoleConstants.Cahsier);
            }

            await db.SaveChangesAsync();
        }
    }
}
