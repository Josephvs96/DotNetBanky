using DotNetBanky.BLL.Services;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Search;
using DotNetBanky.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace DotNetBanky.Admin.Pages.Search
{
    [IgnoreAntiforgeryToken]
    [Breadcrumb("Search Results")]
    public class SearchResultsModel : PageModel
    {
        private readonly ISearchService _searchService;

        public SearchResultsModel(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public PagedResult<CustomerSearchDTO> PagedResult { get; set; }
        public int PageSize { get; set; }
        public string SearchWord { get; set; }
        public async Task<IActionResult> OnGetAsync(
            string searchWord,
            int pageNumber = 1,
            int pageSize = 50,
            SearchSortColumn sortColumn = SearchSortColumn.Relevance,
            SortDirection sortDirection = SortDirection.None)
        {
            if (string.IsNullOrWhiteSpace(searchWord))
                return LocalRedirect("/Customers");

            SearchWord = searchWord;
            PageSize = pageSize;
            PagedResult = await _searchService.Search(searchWord, sortDirection, sortColumn, pageNumber, pageSize);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string searchWord)
        {
            return RedirectToPage("SearchResults", new { searchWord = searchWord });
        }
    }
}
