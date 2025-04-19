namespace TABP.Application.Cities.Queries.GetForAdmin;
public sealed record CityForAdminResponse(
    Guid Id, 
    string Name,
    string Country,
    string PostOffice,
    int NumberOfHotels,
    DateTime CreatedDate,
    DateTime? UpdatedDate);
