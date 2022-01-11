using System.ComponentModel;

namespace DotNetBanky.Core.Enums
{
    public enum CustomerSortColumn
    {
        [Description("CustomerId")]
        Id,
        [Description("Givenname")]
        Name,
    }

    public enum SearchSortColumn
    {
        [Description("CustomerId")]
        Id,
        [Description("Givenname")]
        Name,
        [Description("City")]
        City,
    }
}
