using DotNetBanky.Core.Entities;
using DotNetBanky.DAL.Context;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DotNetBanky.DAL.Repositories
{
    public class DispositionsRepository : GenericRepository<Disposition>, IDispositionsRepository
    {
        private readonly ApplicationDbContext _db;

        public DispositionsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Disposition> GetAllDispostions()
        {
            return _db.Dispositions.Include(d => d.Account).Include(d => d.Customer).OrderBy(d => d.Customer.Country);
        }
    }
}
