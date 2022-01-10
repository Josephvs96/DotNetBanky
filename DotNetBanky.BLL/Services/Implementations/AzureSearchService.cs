using AutoMapper;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using DotNetBanky.Core.DTOModels.Paging;
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
                var customersPaged = _mapper.Map<PagedResult<CustomerSearch>>(await _customerRepository.GetPagedListAsync(page: currentPage, pageSize: 1000));

                List<IndexDocumentsAction<CustomerSearch>> actionsList = new List<IndexDocumentsAction<CustomerSearch>>();
                for (int i = 0; i < customersPaged.Results.Count; i++)
                {
                    actionsList.Add(IndexDocumentsAction.Upload(customersPaged.Results[i]));
                }
                IndexDocumentsBatch<CustomerSearch> batch = IndexDocumentsBatch.Create(actionsList.ToArray());
                await _searchClient.IndexDocumentsAsync(batch);

                currentPage++;
                if (currentPage > customersPaged.PageCount) break;
            }
        }

        public Task<List<CustomerSearch>> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
