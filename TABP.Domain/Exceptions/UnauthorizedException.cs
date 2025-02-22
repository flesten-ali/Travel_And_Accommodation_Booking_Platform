namespace TABP.Domain.Exceptions;
public class UnauthorizedException(string message) : DomainException(message, "Unauthorized Access")
{
}
