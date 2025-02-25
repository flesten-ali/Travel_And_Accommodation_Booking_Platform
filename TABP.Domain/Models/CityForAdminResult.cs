namespace TABP.Domain.Models;

public sealed record CityForAdminResult(
    Guid Id,
    string Name,
    string Country,
    string PostOffice,
    int NumberOfHotels,
    DateTime CreatedDate,
    DateTime? UpdatedDate);
