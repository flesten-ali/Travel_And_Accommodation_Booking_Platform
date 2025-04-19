using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Queries.GetForHotel;
public sealed record HotelRoomClassesQueryResponse(
    Guid Id,
    string Name,
    string? Description,
    RoomType RoomType,
    int AdultsCapacity,
    int ChildrenCapacity,
    double Price,
    IEnumerable<string> GalleryUrls,
    IEnumerable<DiscountResponse> DiscountResponses,
    IEnumerable<AmenityResponse> AmenityResponses);

public sealed record AmenityResponse(
    Guid Id,
    string Name,
    string? Description);

public sealed record DiscountResponse(
    Guid Id,
    double Percentage,
    DateTime StartDate,
    DateTime EndDate,
    Guid RoomClassId);
