namespace TABP.Domain.Constants.ExceptionMessages;
public static class BookingExceptionMessages
{
    public const string NotFound = "The booking with the specified ID could not be found. Please verify the booking details.";
    public const string Exist = "A booking with the same ID or details already exists. Please check if this booking has been made before.";
}
