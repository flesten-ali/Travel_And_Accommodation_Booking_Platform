namespace TABP.Domain.Constants.ExceptionMessages;

public static class RoomClassExceptionMessages
{
    public const string NotFound = "The specified room class could not be found. Please verify the room class details.";
    public const string EntityInUseForRooms = """
                                                   The room class cannot be deleted because it has dependent rooms.
                                                   Please remove or reassign the dependent rooms before deleting the room class.
                                              """;
}
