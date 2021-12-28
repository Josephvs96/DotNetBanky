using System.Linq.Expressions;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> AddOneAsync(T entity);
        Task<T> UpdateOneAsync(T entity);
        Task<int> DeleteOneAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entity);
        Task<List<T>> UpdateRangeAsync(List<T> entity);
    }
}
