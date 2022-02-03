using AutoMapper;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using DotNetBanky.Core.DTOModels.Paging;
using DotNetBanky.Core.DTOModels.Search;
using DotNetBanky.Core.Enums;
using DotNetBanky.Core.Extentions;
using DotNetBanky.Core.SearchEntities;
using DotNetBanky.DAL.Repositories.IRepositories;
using Microsoft.Extensions.Configuration;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class AzureSearchService : ISearchService
    {
        private readonly SearchClient _searchClient;
        private readonly SearchIndexClient _searchIndexClient;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly string _indexName;

        public AzureSearchService(
            SearchClient searchClient,
            SearchIndexClient searchIndexClient,
            ICustomerRepository customerRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _searchClient = searchClient;
            _searchIndexClient = searchIndexClient;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _indexName = configuration.GetSection("AzureSearch:indexName").Value;
        }

        public async Task CreateAndPopulateIndex()
        {
            FieldBuilder fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(CustomerSearch));

            var definition = new SearchIndex(_indexName, searchFields);

            await _searchIndexClient.CreateOrUpdateIndexAsync(definition);

            var documentsCount = await _searchClient.GetDocumentCountAsync();

            if (documentsCount.Value <= 0)
                await PopulateIndexFromDatabase();
        }

        private async Task PopulateIndexFromDatabase()
        {
            int currentPage = 1;
            while (true)
            {
                var customersPaged = _mapper.Map<PagedResult<CustomerSearch>>(
                    await _customerRepository.GetPagedListAsync(page: currentPage, pageSize: 1000));

                await _searchClient.MergeOrUploadDocumentsAsync(customersPaged.Results);

                currentPage++;
                if (currentPage > customersPaged.PageCount) break;
            }
        }

        public async Task CreatOrUpdateDocumentAsync(CustomerSearchDTO customerSearch)
        {
            var documentsList = new List<CustomerSearchDTO> { customerSearch };
            await _searchClient.MergeOrUploadDocumentsAsync(documentsList);
        }

        public async Task<PagedResult<CustomerSearchDTO>> Search(
            string searchTerm,
            SortDirection sortDirection = SortDirection.None,
            SearchSortColumn sortColumn = SearchSortColumn.Relevance,
            int pageNumber = 1,
            int pageSize = 50)
        {
            SearchOptions options = new SearchOptions
            {
                IncludeTotalCount = true,
                QueryType = SearchQueryType.Full,
                Size = pageSize,
                Skip = (pageNumber - 1) * pageSize,
                SearchMode = SearchMode.All
            };

            if (!string.IsNullOrEmpty(sortColumn.GetValueAsString()))
                options.OrderBy.Add($"{sortColumn.GetValueAsString()} {sortDirection.GetValueAsString()}");

            SearchResults<CustomerSearch> results;
            results = await _searchClient.SearchAsync<CustomerSearch>(searchTerm + "~1", options);

            var resultsIds = results.GetResults().Select(r => int.Parse(r.Document.CustomerId)).ToList();
            var resultListFromDb = await _customerRepository.GetListAsync(c => resultsIds.Contains(c.CustomerId));

            var pagedResults = _mapper.Map<PagedResult<CustomerSearchDTO>>(results.GetPaged(pageNumber, pageSize));
            pagedResults.Results = _mapper.Map<List<CustomerSearchDTO>>(resultListFromDb);

            return pagedResults;
        }
    }
}
