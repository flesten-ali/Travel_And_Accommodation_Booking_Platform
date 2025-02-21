namespace TABP.Domain.Exceptions;
public class CartEmptyException(string message) : DomainException(message, "Cart is Empty")
{
}