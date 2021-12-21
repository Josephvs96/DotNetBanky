using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [MaxLength(6)]
        public string Gender { get; set; } = null!;

        [MaxLength(100)]
        public string Givenname { get; set; } = null!;

        [MaxLength(100)]
        public string Surname { get; set; } = null!;

        [MaxLength(100)]
        public string Streetaddress { get; set; } = null!;

        [MaxLength(100)]
        public string City { get; set; } = null!;

        [MaxLength(15)]
        public string Zipcode { get; set; } = null!;

        [MaxLength(100)]
        public string Country { get; set; } = null!;

        [MaxLength(2)]
        public string CountryCode { get; set; } = null!;
        public DateTime? Birthday { get; set; }

        [MaxLength(20)]
        public string? NationalId { get; set; }

        [MaxLength(10)]
        public string? Telephonecountrycode { get; set; }

        [MaxLength(25)]
        public string? Telephonenumber { get; set; }

        [MaxLength(100)]
        public string? Emailaddress { get; set; }

        public virtual ICollection<Disposition> Dispositions { get; set; } = new List<Disposition>();
    }
}
