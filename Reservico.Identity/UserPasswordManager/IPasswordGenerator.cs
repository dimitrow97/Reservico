namespace Reservico.Identity.UserPasswordManager
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(int length);
    }
}