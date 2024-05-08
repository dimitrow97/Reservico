namespace Reservico.Data.Entities.Abstraction
{
    public abstract class BaseCode : BaseEntity
    {
        public string Code { get; set; }

        public string ClientId { get; set; }

        public bool IsUsed { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}