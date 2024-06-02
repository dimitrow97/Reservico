using System.Net;
using System.Net.Mail;
using System.Web;
using Microsoft.Extensions.Options;

namespace Reservico.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration configuration;

        public EmailSender(
            IOptions<EmailConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public async Task SendPasswordResetEmail(
            string to,
            string token)
        {
            token = HttpUtility.UrlEncode(token);
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, to)
            {
                Body = $"<p>You can reset your password at <a href='{this.GetAppUrl()}auth/new-password?token={token}'>{this.GetAppUrl()}auth/new-password?token={token}</a></p>",
                Subject = "Reservico Reset Password",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task SendRegistrationEmail(
            string email,
            string password)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, email)
            {
                Body = $"<p>You have been registered to Reservico</p>\r\n<p>Your email is: {email}</p>\r\n<p>Your temporary password is: {password}</p>\r\n<p>You will need to change it after your first login.</p>\r\n<p>Please click <a href='{this.GetAppUrl()}'>Here</a> to login.</p>",
                Subject = "Reservico Registration",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationCreatedEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName,
            Guid reservationId)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, reservationEmail)
            {
                Body = $"<p>A Reservation in your name has been placed through Reservico</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>\r\n<p>Click <strong><a href=\"{this.GetPublicAppUrl()}/reservation-details/{reservationId}\">here</a></strong> to view the Reservation.</p>",
                Subject = "Reservico Reservation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationConfirmedEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, reservationEmail)
            {
                Body = $"<p>A Reservation from Reservico has been Confirmed</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>",
                Subject = "Reservico Reservation Confirmation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationCancellationEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName,
            Guid reservationId)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, reservationEmail)
            {
                Body = $"<p>Want to cancel your Reservation from Reservico?</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>\r\n<p>Click <strong><a href=\"{this.GetPublicAppUrl()}/reservation/cancel/{reservationId}\">here</a></strong> to cancel.</p>",
                Subject = "Reservico Reservation Cancellation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationCancelledEmail(
            string reservationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, reservationEmail)
            {
                Body = $"<p>You have Cancelled your Reservation from Reservico</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>",
                Subject = "Reservico Reservation Cancellation Confirmation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationCancelledEmailToLocation(
            string locationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, locationEmail)
            {
                Body = $"<p>A Reservation for your Location has been Cancelled on Reservico</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>",
                Subject = "Reservico Reservation Cancellation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        public async Task ReservationEmailToLocation(
            string locationEmail,
            DateTime guestsArrivingAt,
            int numberOfGuests,
            string locationName)
        {
            var emailFromAddress = this.GetEmailFromAddress();

            var message = new MailMessage(emailFromAddress, locationEmail)
            {
                Body = $"<p>New Reservation has been placed through Reservico</p>\r\n<p>Location: {locationName}</p>\r\n<p>Number of Guests: {numberOfGuests}</p>\r\n<p>Guests will be arriving at: {guestsArrivingAt}</p>",
                Subject = "Reservico Reservation",
                IsBodyHtml = true
            };

            await this.SendMailAsync(message);
        }

        private async Task SendMailAsync(
            MailMessage message)
        {
            using var client = new SmtpClient()
            {
                Port = configuration.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(configuration.Username, configuration.Password),
                Host = configuration.SmtpServer
            };

            await client.SendMailAsync(message);
        }

        private string GetEmailFromAddress()
            => this.configuration.EmailFromAddress;

        private string GetAppUrl()        
            => this.configuration.ApplicationUrl;

        private string GetPublicAppUrl()
            => this.configuration.PublicApplicationUrl;
    }
}
