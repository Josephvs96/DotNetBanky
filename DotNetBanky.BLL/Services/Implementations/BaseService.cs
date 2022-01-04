using DotNetBanky.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        private readonly IGenericRepository<TEntity> _genericRepository;

        public BaseService(IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> GetTotalNumberOfPages(int pageSize, Expression<Func<TEntity, bool>>? filter = null)
        {
            var records = await _genericRepository.GetNumberOfRecords(filter);
            return (records / pageSize) + 1;
        }
    }
}
