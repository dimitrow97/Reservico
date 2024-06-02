namespace Reservico.Common.EmailSender
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string EmailFromAddress { get; set; }

        public string ApplicationUrl { get; set; }

        public string PublicApplicationUrl { get; set; }
    }
}