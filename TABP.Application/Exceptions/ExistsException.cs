using Raven.Client.Exceptions;

namespace TABP.Application.Exceptions;
public class ExistsException(string msg) : ConflictException(msg)
{
}
