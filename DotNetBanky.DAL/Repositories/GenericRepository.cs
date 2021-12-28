using DotNetBanky.DAL.Context;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
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
            return await _db.Set<TEntity>().FirstOrDefaultAsync(filter) ?? new TEntity();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await (filter == null ? _db.Set<TEntity>().ToListAsync() : _db.Set<TEntity>().Where(filter).ToListAsync());
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
    }
}
