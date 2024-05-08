using System.ComponentModel.DataAnnotations;

namespace Reservico.Identity.UserPasswordManager.Models
{
    public class ChangePasswordRequestModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}