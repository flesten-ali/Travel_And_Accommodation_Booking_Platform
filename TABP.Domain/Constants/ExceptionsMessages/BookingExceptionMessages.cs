namespace TABP.Domain.Constants.ExceptionsMessages;
public static class BookingExceptionMessages
{
    public const string NotFound = "The booking with the specified ID could not be found. Please verify the booking details.";
    public const string Exist = "A booking with the same ID or details already exists. Please check if this booking has been made before.";
    public const string InvalidDate = "The booking dates are invalid. Please ensure the check-in and check-out dates are correct.";
    public const string Overlapping = "The booking overlaps with an existing reservation. Please choose a different time or room.";
}
