using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace DotNetBanky.Core.SearchEntities
{
    public class CustomerSearch
    {
        [SimpleField(IsKey = true, IsFilterable = true, IsSortable = true)]
        public string CustomerId { get; set; } = null!;
        [SearchableField(IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.SvMicrosoft)]
        public string Givenname { get; set; } = null!;

        [SearchableField(IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.SvMicrosoft)]
        public string Surname { get; set; } = null!;
        [SimpleField(IsSortable = true)]
        public string Streetaddress { get; set; } = null!;

        [SearchableField(IsSortable = true, AnalyzerName = LexicalAnalyzerName.Values.SvMicrosoft)]
        public string City { get; set; } = null!;

        [SimpleField(IsSortable = true)]
        public string? NationalId { get; set; }
    }
}
