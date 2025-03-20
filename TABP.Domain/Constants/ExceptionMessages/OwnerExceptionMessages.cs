namespace TABP.Domain.Constants.ExceptionMessages;

public static class OwnerExceptionMessages
{
    public const string NotFound = "The owner with the specified ID or details could not be found. Please verify the owner information.";
    public const string ExistWithEmail = "An owner with the same email already exists. Ensure the owner is unique before adding it.";
    public const string EntityInUse = "The owner cannot be deleted because it has dependent hotels. Please remove or reassign the dependent hotels before deleting the owner.";
}
