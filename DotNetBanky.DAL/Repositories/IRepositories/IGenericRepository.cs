using DotNetBanky.Core.DTOModels.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DotNetBanky.DAL.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetOneByIdAsync(int id);
        Task<List<T>> GetListAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? page = null,
            int? pageSize = null);
        Task<PagedResult<T>> GetPagedListAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? page = null,
            int? pageSize = null);
        Task<T> AddOneAsync(T entity);
        Task<T> UpdateOneAsync(T entity);
        Task<int> DeleteOneAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entity);
        Task<List<T>> UpdateRangeAsync(List<T> entity);
        Task<int> GetNumberOfRecords(Expression<Func<T, bool>>? filter = null);
    }
}
