using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetBanky.Core.DTOModels.User
{
    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UserCreateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [DisplayName("Name")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
        public SelectList? Roles { get; set; }
    }

    public class UserChangePasswordModel : IValidatableObject
    {
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OldPassword == NewPassword)
                yield return new ValidationResult("The new password and the old password cannot be the same");
        }
    }

    public class UserDTOModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [DisplayName("Name")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Role { get; set; }
        public SelectList? Roles { get; set; }
    }
}
