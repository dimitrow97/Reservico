namespace Reservico.Identity.UserManager.Models
{
    public class UserRegistrationResponseModel
    {
        public UserRegistrationResponseModel(
            UserRegisterStatus status)
        {
            Status = status;
        }

        public UserRegisterStatus Status { get; set; }
    }

    public enum UserRegisterStatus
    {
        Registered,
        Reactivated
    }
}