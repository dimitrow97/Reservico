using System.ComponentModel.DataAnnotations;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class TokenRefreshRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string GrantType { get; set; }
    }
}