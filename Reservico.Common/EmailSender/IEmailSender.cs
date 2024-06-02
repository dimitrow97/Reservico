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

        Task ReservationCreatedEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName,
            Guid reservationId);

        Task ReservationEmailToLocation(
            string locationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName);

        Task ReservationConfirmedEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName);

        Task ReservationCancellationEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName,
            Guid reservationId);

        Task ReservationCancelledEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName);

        Task ReservationCancelledEmailToLocation(
            string locationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName);
    }
}