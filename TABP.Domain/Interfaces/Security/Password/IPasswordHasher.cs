namespace TABP.Domain.Interfaces.Security.Password;

public interface IPasswordHasher
{
    string Hash(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}