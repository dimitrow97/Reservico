using System.ComponentModel.DataAnnotations;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class UserForgotPasswordRequestModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid user email.")]
        public string Email { get; set; }
    }
}