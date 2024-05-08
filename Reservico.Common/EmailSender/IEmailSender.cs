namespace Reservico.Common.EmailSender
{
    public interface IEmailSender
    {
        Task SendPasswordResetEmail(
            string to,
            string token);

        Task SendRegistrationEmail(
            string email,
            string password);
    }
}