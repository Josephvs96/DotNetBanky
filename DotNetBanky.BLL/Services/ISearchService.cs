using DotNetBanky.Core.SearchEntities;

namespace DotNetBanky.BLL.Services
{
    public interface ISearchService
    {
        Task CreateAndPopulateIndex();
        Task<List<CustomerSearch>> Search(string searchTerm);
    }
}
