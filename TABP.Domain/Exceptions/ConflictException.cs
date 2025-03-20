namespace TABP.Domain.Exceptions;
public class ConflictException(string message) : DomainException(message, "Conflict Detected")
{
}
