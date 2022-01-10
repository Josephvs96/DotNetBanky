using DotNetBanky.Core.DTOModels.Search;

namespace DotNetBanky.BLL.Services
{
    public interface ISearchService
    {
        Task CreateAndPopulateIndex();
        Task CreatOrUpdateDocumentAsync(CustomerSearchDTO customerSearch);
        Task<List<CustomerSearchDTO>> Search(string searchTerm);
    }
}
