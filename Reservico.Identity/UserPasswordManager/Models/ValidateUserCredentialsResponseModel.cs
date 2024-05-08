namespace Reservico.Identity.UserPasswordManager.Models
{
    public class ValidateUserCredentialsResponseModel
    {
        public ValidateUserCredentialsResponseModel(
            Guid userId,
            string userSecurityStamp,
            string userPhoneNumber,
            bool isUsersFirstLogin,
            IEnumerable<string> userRoles)
        {
            UserId = userId;
            UserSecurityStamp = userSecurityStamp;
            UserPhoneNumber = userPhoneNumber;
            IsUsersFirstLogin = isUsersFirstLogin;
            UserRoles = userRoles;
        }

        public Guid UserId { get; set; }

        public string UserSecurityStamp { get; set; }

        public string UserPhoneNumber { get; set; }

        public bool IsUsersFirstLogin { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}