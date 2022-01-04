using System.ComponentModel.DataAnnotations;

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

    public class CustomerCreateModel
    {
        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public string Givenname { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Streetaddress { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Zipcode { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string CountryCode { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        [Required]
        public string Emailaddress { get; set; } = null!;
    }

    public class CustomerEditModel
    {
        public int CustomerId { get; set; }

        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        public string Givenname { get; set; } = null!;
        [Required]
        public string Surname { get; set; } = null!;
        [Required]
        public string Streetaddress { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string Zipcode { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string CountryCode { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        [Required]
        public string Emailaddress { get; set; } = null!;
    }

    public class CustomerListDTOModel
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = null!;
        public string? TelephoneNumber { get; set; }
        public string? EmailAddress { get; set; }
    }

    public class CustomerDetailsDTOModel
    {
        public int CustomerId { get; set; }
        public string Gender { get; set; } = null!;

        public string Givenname { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Streetaddress { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Zipcode { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string CountryCode { get; set; } = null!;

        public DateTime? Birthday { get; set; }

        public string? NationalId { get; set; }

        public string? Telephonecountrycode { get; set; }

        public string? Telephonenumber { get; set; }

        public string Emailaddress { get; set; } = null!;
    }
}
