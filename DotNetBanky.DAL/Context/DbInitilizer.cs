using DotNetBanky.Utility.Constents;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.DAL.Context
{
    public class DbInitilizer
    {
        private readonly ApplicationDbContext _db;

        public DbInitilizer(ApplicationDbContext db)
        {
            _db = db;
        }

        public void InitilizeDB()
        {
            _db.Database.Migrate();
            SeedRolls();
            SeedUsers();
        }

        private void SeedUsers()
        {
            if (_db.Roles.Any()) return;
            // TODO: seed the default two users....
        }

        private void SeedRolls()
        {
            if (_db.Roles.Any()) return;

            _db.AddRange(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleConstents.Admin,
                    NormalizedName = RoleConstents.Admin.ToUpper(),
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RoleConstents.Cahsier,
                    NormalizedName = RoleConstents.Cahsier.ToUpper()
                });
            _db.SaveChanges();
        }
    }
}
