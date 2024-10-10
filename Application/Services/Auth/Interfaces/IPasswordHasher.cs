namespace Application.Services.Auth.Interfaces
{
    public interface IPasswordHasher
    {
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
