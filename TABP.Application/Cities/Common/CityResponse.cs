using TABP.Domain.Entities;

namespace TABP.Application.Cities.Common;
public sealed record CityResponse(
    Guid Id,
    string Name, 
    string Country,
    string PostOffice, 
    DateTime CreatedDate, 
    DateTime? UpdatedDate);
