using bCrypt = BCrypt.Net.BCrypt;

namespace Reservico.Identity.UserPasswordManager
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password, string securityStamp)
            => bCrypt.EnhancedHashPassword(this.GeneratePreHashValue(password, securityStamp));

        public bool Verify(string input, string securityStamp, string passwordHash)
            => bCrypt.EnhancedVerify(
                this.GeneratePreHashValue(input, securityStamp),
                passwordHash);

        private string GeneratePreHashValue(
            string password,
            string securityStamp)
        {
            var separatingIndex = securityStamp.Length / 2;

            return $"{securityStamp.Substring(0, separatingIndex)}{password}{securityStamp.Substring(separatingIndex)}";
        }
    }
}