namespace TABP.Domain.Constants.ExceptionsMessages;

public static class CityExceptionMessages
{
    public const string NotFound = "The city with the specified name or ID could not be found. Please verify the details and try again.";
    public const string Exist = "A city with the same name or ID already exists. Ensure the city is unique before adding it.";
    public const string EntityInUse = "The city cannot be deleted because it has dependent hotels. Please remove or reassign the dependent hotels before deleting the city.";
}
