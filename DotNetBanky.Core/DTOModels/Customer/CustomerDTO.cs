namespace DotNetBanky.Core.DTOModels.Customer
{
    public class CustomerCountryDetailsDTO
    {
        public int CustomerId { get; set; }
        public string GivenName { get; set; } = null!;
        public string? TelephoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
