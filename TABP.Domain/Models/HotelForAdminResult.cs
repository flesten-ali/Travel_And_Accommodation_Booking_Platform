namespace TABP.Domain.Models;

public sealed record HotelForAdminResult(
    Guid Id,
    string Name,
    int Rate,
    string OwnerName,
    string CityName,
    DateTime CreatedDate,
    DateTime? UpdatedDate);
