namespace TABP.Domain.Constants.ExceptionMessages;

public static class UserExceptionMessages
{
    public const string NotFound = "The user with the specified ID or email could not be found. Please verify the user details.";
    public const string UnauthorizedBooking = "Booking can only be made by a Guest. Please ensure that the user has the proper role or permissions to proceed.";
}
