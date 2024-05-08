using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Reservico.Api.Controllers.Identity.Models
{
    public class AuthorizeLoginResponseModel
    {
        const string OtpMessage = "One-Time-Password has been sent in a SMS to phone number: {0}";        

        public string OneTimeCode { get; set; }

        public string OneTimeMessage { get; set; }

        public string AuthorizationCode { get; set; }

        public IEnumerable<string> EnabledMfaOptions { get; set; }

        public static AuthorizeLoginResponseModel AuthCode(string authorizationCode)
            => new AuthorizeLoginResponseModel { AuthorizationCode = authorizationCode };

        public static AuthorizeLoginResponseModel OTC(string oneTimeCode, string phoneNumber, IEnumerable<string> enabledMfaOptions, bool sendMsg = true)
            => new AuthorizeLoginResponseModel 
            { 
                OneTimeCode = oneTimeCode, 
                OneTimeMessage = sendMsg ? string.Format(OtpMessage, SecurePhoneNumber(phoneNumber)) : string.Empty,
                EnabledMfaOptions = enabledMfaOptions
            };

        private static string SecurePhoneNumber(string phoneNumber)
        {
            var lastThreeDigits = phoneNumber.Substring(phoneNumber.Length - 3);
            var prefix = Regex.Replace(phoneNumber.Substring(0, phoneNumber.Length - 3), "\\d", "*");

            return $"{prefix}{lastThreeDigits}";
        }
    }
}