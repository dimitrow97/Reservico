namespace Reservico.Identity.Code.Models
{
    public class GenerateCodeRequestModel
    {
        public GenerateCodeRequestModel(
            string clientId,
            int codeExpirationTime,
            ExpirationTimeType expirationTimeType,
            params string[] codeValues)
        {
            ClientId = clientId;
            CodeExpirationTime = codeExpirationTime;
            ExpirationTimeType = expirationTimeType;
            CodeValues = codeValues;
        }

        public string ClientId { get; set; }

        public int CodeExpirationTime { get; set; }

        public ExpirationTimeType ExpirationTimeType { get; set; }

        public IEnumerable<string> CodeValues { get; set; }
    }

    public enum ExpirationTimeType
    {
        Minutes = 0,
        Seconds,
        Hours
    }
}