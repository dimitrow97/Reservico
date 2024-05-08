using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserPasswordManager.Models
{
    public class ResetPasswordRequestModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }
    }
}