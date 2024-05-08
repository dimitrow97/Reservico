using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserPasswordManager.Models
{
    public class ValidateUserCredentialsRequestModel
    {
        public ValidateUserCredentialsRequestModel(
            string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}