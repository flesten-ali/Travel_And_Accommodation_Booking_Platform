namespace TABP.Application.Hotels.Queries.GetForAdmin;
public sealed record HotelForAdminResponse(
    Guid Id, 
    string Name,
    int Rate,
    string OwnerName, 
    string CityName,
    DateTime CreatedDate,
    DateTime? UpdatedDate);
