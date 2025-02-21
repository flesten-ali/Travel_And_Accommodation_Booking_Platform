namespace TABP.Domain.Constants.ExceptionMessages;

public static class RoomExceptionMessages
{
    public const string NotFound = "One or more rooms could not be found. Please verify the room details and try again.";
    public const string NotBelongToHotel = "One or more rooms do not belong to the specified hotel. Please ensure the room IDs are correct for the selected hotel.";
    public const string Exist = "A room with the same room number already exists. Please choose a unique room number for the room.";
    public const string EntityInUseForBookings = """
                                                       The room cannot be deleted because it has dependent bookings. 
                                                       Please remove or reassign the dependent bookings before deleting the room.
                                                    """;
    public const string NotFoundForTheRoomClass = "The specified room could not be found in the provided room class.";
    public const string NotFoundForHotel = "The specified room could not be found in the provided hotel.";
}