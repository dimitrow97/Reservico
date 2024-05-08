using System.ComponentModel.DataAnnotations;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class TokenGetRequestModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [Required]
        public string GrantType { get; set; }
    }
}