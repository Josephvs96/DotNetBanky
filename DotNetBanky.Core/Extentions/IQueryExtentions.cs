using DotNetBanky.Core.Entities;
using DotNetBanky.Core.Enums;
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
                CustomerSortColumn.Name => x => x.Surname,
                _ => x => x.CustomerId,
            };

            sortDirection ??= SortDirection.Asc;

            return sortDirection == SortDirection.Asc ? query.OrderBy(exp) : query.OrderByDescending(exp);
        }
    }
}
