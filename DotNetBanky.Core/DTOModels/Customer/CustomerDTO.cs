using DotNetBanky.Core.DTOModels.Account;
using System.ComponentModel;
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
        [DisplayName("First Name")]
        public string Givenname { get; set; } = null!;
        [Required]
        [DisplayName("Last Name")]
        public string Surname { get; set; } = null!;
        [Required]
        [DisplayName("Street Address")]
        public string Streetaddress { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        [RegularExpression(@"(^\d{5}$)|(^\d{9}$)|(^\d{5}-\d{4}$)|(^\d{3} \d{2}$)|(^\d{4})", ErrorMessage = "The Zip code is not valid")]
        public string Zipcode { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [DisplayName("Country Code")]
        public string? CountryCode { get; set; } = null!;

        [DisplayName("Birth Year")]
        public DateTime? Birthday { get; set; }

        public string? NationalId { get; set; }

        [RegularExpression(
            @"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$",
            ErrorMessage = "The phone number entered is not valid")]
        [DisplayName("Phone Number")]
        public string? Telephonenumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Emailaddress { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirm password must match")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public class CustomerEditModel
    {
        public int CustomerId { get; set; }

        [Required]
        public string Gender { get; set; } = null!;
        [Required]
        [DisplayName("First Name")]
        public string Givenname { get; set; } = null!;
        [Required]
        [DisplayName("Last Name")]
        public string Surname { get; set; } = null!;
        [Required]
        [DisplayName("Street Address")]
        public string Streetaddress { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        [RegularExpression(@"(^\d{5}$)|(^\d{9}$)|(^\d{5}-\d{4}$)|(^\d{3} \d{2}$)|(^\d{4})", ErrorMessage = "The Zip code is not valid")]
        public string Zipcode { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [DisplayName("Country Code")]
        public string? CountryCode { get; set; } = null!;

        [DisplayName("Birth Year")]
        public DateTime? Birthday { get; set; }

        public string? NationalId { get; set; }

        [RegularExpression(
            @"^([+]?[\s0-9]+)?(\d{3}|[(]?[0-9]+[)])?([-]?[\s]?[0-9])+$",
            ErrorMessage = "The phone number entered is not valid")]
        [DisplayName("Phone Number")]
        public string? Telephonenumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
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

        public List<AccountSummeryDTO> Accounts { get; set; } = null!;

        public decimal TotalBalance { get; set; }
    }
}
