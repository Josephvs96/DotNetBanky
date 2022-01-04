using System.Linq.Expressions;

namespace DotNetBanky.BLL.Services
{
    public interface IBaseService<T> where T : class, new()
    {
        public Task<int> GetTotalNumberOfPages(int pageSize, Expression<Func<T, bool>>? filter = null);
    }
}
