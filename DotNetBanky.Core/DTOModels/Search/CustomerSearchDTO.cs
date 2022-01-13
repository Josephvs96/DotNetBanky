namespace DotNetBanky.Core.DTOModels.Search
{
    public class CustomerSearchDTO
    {
        public string CustomerId { get; set; } = null!;
        public string Givenname { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Streetaddress { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? NationalId { get; set; }
    }
}
