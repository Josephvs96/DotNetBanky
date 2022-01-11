using Azure.Search.Documents.Models;
using DotNetBanky.Core.DTOModels.Paging;

namespace DotNetBanky.Core.Extentions
{
    public static class SearchResultsExtentions
    {
        public static PagedResult<T> GetPaged<T>(this SearchResults<T> results,
                                        int? page, int? pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = page.Value;
            result.PageSize = pageSize.Value;
            result.RowCount = (int)results.TotalCount.Value;

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount.Value);

            result.Results = results.GetResults().Select(r => r.Document).ToList();

            return result;
        }
    }
}
