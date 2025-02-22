namespace TABP.Domain.Exceptions;
public class BadRequestException(string message) : DomainException(message, "Bad Request")
{
}
