using Raven.Client.Exceptions;

namespace TABP.Domain.Exceptions;
public class ExistsException(string msg) : ConflictException(msg)
{
}
