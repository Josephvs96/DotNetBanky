using DotNetBanky.DAL.Repositories.IRepositories;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        private readonly IGenericRepository<T> _genericRepository;

        public BaseService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }
        public async Task<int> GetTotalNumberOfPages(int pageSize)
        {
            var records = await _genericRepository.GetNumberOfRecords();
            return records / pageSize;
        }
    }
}
