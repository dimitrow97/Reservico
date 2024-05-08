using OtpNet;
using System;
using System.Text;

namespace Reservico.Identity.Otp
{
    public class OtpProvider : IOtpProvider
    {
        public string GenerateTotp(string key, int step)
        {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var totp = new Totp(keyBytes, step: step);

            return totp.ComputeTotp(DateTime.UtcNow);
        }

        public string GenerateTotp(int step, params string[] keyParams)
            => GenerateTotp(GenerateKey(keyParams), step);

        public bool ValidateTotp(string key, int step, string totpCode)
        {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var totp = new Totp(keyBytes, step: step);

            return totp.VerifyTotp(
                totpCode,
                out var timeStepMatched,
                new VerificationWindow(previous: 1, future: 0));
        }

        public bool ValidateTotp(int step, string totpCode, params string[] keyParams)
            => ValidateTotp(GenerateKey(keyParams), step, totpCode);

        private string GenerateKey(string[] keyParams)
            => string.Join("/", keyParams);
    }
}