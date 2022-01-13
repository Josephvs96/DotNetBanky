using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Search;
using DotNetBanky.Core.Enums;

namespace DotNetBanky.BLL.Services
{
    public interface ISearchService
    {
        Task CreateAndPopulateIndex();
        Task CreatOrUpdateDocumentAsync(CustomerSearchDTO customerSearch);
        Task<PagedResult<CustomerSearchDTO>> Search(
            string searchTerm,
            SortDirection sortDirection = SortDirection.Asc,
            SearchSortColumn sortColumn = SearchSortColumn.Id,
            int pageNumber = 1,
            int pageSize = 20);
    }
}
