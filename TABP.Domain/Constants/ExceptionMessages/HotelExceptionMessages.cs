namespace TABP.Domain.Constants.ExceptionMessages;

public static class HotelExceptionMessages
{
    public const string NotFound = "The hotel with the specified name or ID could not be found. Double-check the details or try searching again.";
    public const string ExistsInLocation = "A hotel already exists in the provided location. Please choose a different location or verify the details.";
    public const string EntityInUseForRoomClasses = """
                                                       The hotel cannot be deleted because it has dependent room classes. 
                                                       Please remove or reassign the dependent room classes before deleting the hotel.
                                                    """;
}