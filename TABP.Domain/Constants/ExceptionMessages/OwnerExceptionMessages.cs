namespace TABP.Domain.Constants.ExceptionMessages;

public static class OwnerExceptionMessages
{
    public const string NotFound = "The owner with the specified ID or details could not be found. Please verify the owner information.";
    public const string ExistWithEmail = "An owner with the same email already exists. Ensure the owner is unique before adding it.";
}
