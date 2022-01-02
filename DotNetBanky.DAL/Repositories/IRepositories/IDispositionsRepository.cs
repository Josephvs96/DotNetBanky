using DotNetBanky.Core.Entities;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface IDispositionsRepository : IGenericRepository<Disposition>
    {
        public IEnumerable<Disposition> GetAllDispostions();
    }
}
