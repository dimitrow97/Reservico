using System;
using System.Collections.Generic;

namespace Reservico.Data.Entities
{
    public partial class IdentityToken
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpirationDate { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int TokenType { get; set; }
    }
}
