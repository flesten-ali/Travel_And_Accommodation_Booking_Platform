using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.SearchHotels;

public sealed record SearchHotelsQuery(
    string City,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    int ChildrenCapacity,
    int AdultsCapacity,
    int NumberOfRooms,
    string? RoomType,
    int? MinPrice,
    int? MaxPrice,
    int? StarRating,
    PaginationParameters PaginationParameters,
    IEnumerable<string>? Amenities) : IRequest<PaginatedResponse<SearchHotelResponse>>;
