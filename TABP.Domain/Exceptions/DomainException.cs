namespace TABP.Domain.Exceptions;
public abstract class DomainException(string message, string title) : Exception(message)
{
    public readonly string Title = title;
}