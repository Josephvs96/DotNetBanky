using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotNetBanky.Core.Extentions
{
    public static class IQueryExtentions
    {
        public static IOrderedQueryable<Customer> SortBy(
            this IQueryable<Customer> query,
            CustomerSortColumn? sortByColumn = null,
            SortDirection? sortDirection = null)
        {
            Expression<Func<Customer, object>> exp = sortByColumn switch
            {
                CustomerSortColumn.Id => x => x.CustomerId,
                CustomerSortColumn.Name => x => x.Givenname,
                _ => x => x.CustomerId,
            };

            sortDirection ??= SortDirection.Asc;

            return sortDirection == SortDirection.Asc ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }

        public static async Task<PagedResult<T>> GetPagedResult<T>(this IQueryable<T> query,
                                         int? page, int? pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page.Value;
            result.PageSize = pageSize.Value;
            result.RowCount = query.Count();


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount.Value);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip.Value).Take(pageSize.Value).ToListAsync();

            return result;
        }
    }
}
