namespace TABP.Domain.Constants.ExceptionMessages;

public static class UserExceptionMessages
{
    public const string NotFound = "The user with the specified ID or email could not be found. Please verify the user details.";
    public const string Exist = "a user with the same email already exists. Please choose a unique email for the user.";
    public const string UnauthorizedBooking = "Booking can only be made by a Guest. Please ensure that the user has the proper role or permissions to proceed.";
    public const string UnauthorizedLogin = "Invalid email or password. Please verify your credentials and try again.";
}
