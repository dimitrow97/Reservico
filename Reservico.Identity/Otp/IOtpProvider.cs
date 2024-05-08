namespace Reservico.Identity.Otp
{
    public interface IOtpProvider
    {
        string GenerateTotp(string key, int step);

        string GenerateTotp(int step, params string[] keyParams);

        bool ValidateTotp(string key, int step, string totpCode);

        bool ValidateTotp(int step, string totpCode, params string[] keyParams);
    }
}