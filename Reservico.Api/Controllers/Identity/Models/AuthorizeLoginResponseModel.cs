using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class AuthorizeLoginResponseModel
    {
        public string AuthorizationCode { get; set; }

        public static AuthorizeLoginResponseModel AuthCode(string authorizationCode)
            => new AuthorizeLoginResponseModel { AuthorizationCode = authorizationCode };        
    }
}