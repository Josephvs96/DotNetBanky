using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetBanky.Common.AutomatedMigrations
{
    public static class DbInitilizer
    {
        public static async Task MigrateAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await ApplicationDbContextSeed.SeedDatabaseAsync(context, userManager, roleManager);
        }
    }
}
