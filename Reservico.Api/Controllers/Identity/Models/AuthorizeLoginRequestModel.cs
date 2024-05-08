using System.ComponentModel.DataAnnotations;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class AuthorizeLoginRequestModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string GrantType { get; set; }
    }
}
