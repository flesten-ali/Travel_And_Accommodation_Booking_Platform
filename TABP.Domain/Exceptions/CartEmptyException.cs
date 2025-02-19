namespace TABP.Domain.Exceptions;
public class CartEmptyException(string msg) : Exception(msg)
{
}