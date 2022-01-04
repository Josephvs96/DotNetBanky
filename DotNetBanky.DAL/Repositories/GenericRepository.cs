using DotNetBanky.DAL.Context;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DotNetBanky.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly ApplicationDbContext _db;

        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<TEntity> AddOneAsync(TEntity entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
        {
            await _db.AddRangeAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteOneAsync(TEntity entity)
        {
            _db.Remove(entity);
            return await _db.SaveChangesAsync();
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await _db.Set<TEntity>().FirstAsync(filter);
        }

        public async Task<TEntity> GetOneByIdAsync(int id)
        {
            return await _db.Set<TEntity>().FindAsync(id);
        }

        public async Task<List<TEntity>> GetListAsync(
                    Expression<Func<TEntity, bool>>? filter = null,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                    int? page = null,
                    int? pageSize = null)

        {
            var query = _db.Set<TEntity>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> UpdateOneAsync(TEntity entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
        {
            _db.UpdateRange(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<int> GetNumberOfRecords(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter != null ? await _db.Set<TEntity>().Where(filter).CountAsync() : await _db.Set<TEntity>().CountAsync();
        }
    }
}
