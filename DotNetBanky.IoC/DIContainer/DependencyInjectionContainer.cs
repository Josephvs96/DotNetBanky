using DotNetBanky.DAL.Context;
using DotNetBanky.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static DotNetBanky.Utility.AutoMapperProfiles.AutoMapperProfiles;

namespace DotNetBanky.IoC.DIContainer
{
    public static class DependencyInjectionContainer
    {
        public static void AddBankyDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaulConnection"));
            });

            services.AddScoped<DbInitilizer>();
            services.AddDefaultIdentity<User>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
