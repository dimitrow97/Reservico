using System.Collections.Generic;

namespace Reservico.Identity.Code.Models
{
    public class ValidateCodeResponseModel
    {
        public ValidateCodeResponseModel(
            string clientId, IEnumerable<string> codeValues)
        {
            ClientId = clientId;
            CodeValues = codeValues;
        }

        public string ClientId { get; set; }

        public IEnumerable<string> CodeValues { get; set; }
    }
}