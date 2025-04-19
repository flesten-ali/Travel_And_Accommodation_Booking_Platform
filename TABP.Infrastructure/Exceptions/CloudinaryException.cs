namespace TABP.Infrastructure.Exceptions;

public class CloudinaryException(string message) : Exception($"Cloudinary error occured:{message}")
{
}