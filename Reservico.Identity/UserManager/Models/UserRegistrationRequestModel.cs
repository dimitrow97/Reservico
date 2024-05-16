using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserManager.Models
{
    public class UserRegistrationRequestModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Guid? ClientId { get; set; }

        [Required]
        public IEnumerable<string> Roles { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}