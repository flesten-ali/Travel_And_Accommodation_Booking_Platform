namespace TABP.Domain.Constants.ExceptionMessages;
public static class ReviewExceptionMessages
{
    public const string NotFound = "No reviews found for the specified hotel. Please verify the hotel ID or check if reviews exist for this hotel.";
    public const string NotFoundForHotel = "The specified review does not exist for the provided hotel, or you do not have permission to modify it.";
}