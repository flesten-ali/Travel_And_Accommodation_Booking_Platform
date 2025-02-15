namespace TABP.Domain.ExceptionMessages;

public static class CityExceptionMessages
{
    public const string NotFound = "The city with the specified name or ID could not be found. Please verify the details and try again.";
    public const string Exist = "A city with the same name or ID already exists. Ensure the city is unique before adding it.";
    public const string EntityInUse = "The city cannot be deleted because it has dependent hotels. Please remove or reassign the dependent hotels before deleting the city.";
}

public static class AmenityExceptionMessages
{
    public const string NotFound = "The specified amenity could not be found. Please verify the amenity details.";
    public const string Exist = "An amenity with the same name already exists. Please choose a unique name for the amenity.";
}

public static class HotelExceptionMessages
{
    public const string NotFound = "The hotel with the specified name or ID could not be found. Double-check the details or try searching again.";
    public const string ExistsInLocation = "A hotel already exists in the provided location. Please choose a different location or verify the details.";
    public const string EntityInUse = "The hotel cannot be deleted because it has dependent room classes. Please remove or reassign the dependent room classes before deleting the hotel.";
}

public static class UserExceptionMessages
{
    public const string NotFound = "The user with the specified ID or email could not be found. Please verify the user details.";
    public const string UnauthorizedBooking = "Booking can only be made by a Guest. Please ensure that the user has the proper role or permissions to proceed.";
}
public static class RoomExceptionMessages
{
    public const string NotFound = "One or more rooms could not be found. Please verify the room details and try again.";
    public const string NotBelongToHotel = "One or more rooms do not belong to the specified hotel. Please ensure the room IDs are correct for the selected hotel.";
}

public static class BookingExceptionMessages
{
    public const string NotFound = "The booking with the specified ID could not be found. Please verify the booking details.";
    public const string Exist = "A booking with the same ID or details already exists. Please check if this booking has been made before.";
    public const string InvalidDate = "The booking dates are invalid. Please ensure the check-in and check-out dates are correct.";
    public const string Overlapping = "The booking overlaps with an existing reservation. Please choose a different time or room.";
}

public static class RoomClassExceptionMessages
{
    public const string NotFound = "The specified room class could not be found. Please verify the room class details.";
}

public static class ValidationExceptionMessages
{
    public const string LimitGreaterThanZero = "The limit must be greater than zero. Please provide a valid limit.";
}

public static class OwnerExceptionMessages
{
    public const string NotFound = "The owner with the specified ID or details could not be found. Please verify the owner information.";
}

public static class ReviewExceptionMessages
{
    public const string NotFound = "No reviews found for the specified hotel. Please verify the hotel ID or check if reviews exist for this hotel.";
}