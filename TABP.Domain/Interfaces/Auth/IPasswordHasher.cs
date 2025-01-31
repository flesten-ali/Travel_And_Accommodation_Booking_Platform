namespace TABP.Domain.Interfaces.Auth;

public interface IPasswordHasher
{
    string Hash(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}