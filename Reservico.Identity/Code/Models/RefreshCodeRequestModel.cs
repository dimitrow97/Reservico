namespace Reservico.Identity.Code.Models
{
    public class RefreshCodeRequestModel
    {
        public RefreshCodeRequestModel(
            string code,
            int codeExpirationTime,
            ExpirationTimeType expirationTimeType = ExpirationTimeType.Seconds)
        {
            Code = code;
            CodeExpirationTime = codeExpirationTime;
            ExpirationTimeType = expirationTimeType;
        }

        public string Code { get; set; }

        public int CodeExpirationTime { get; set; }

        public ExpirationTimeType ExpirationTimeType { get; set; }
    }
}