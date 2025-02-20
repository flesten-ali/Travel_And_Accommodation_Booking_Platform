using MediatR;
using TABP.Application.Shared;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Queries.SearchHotels;

public class SearchHotelsQuery : IRequest<PaginatedResponse<SearchHotelResponse>>
{
    public string City { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int ChildrenCapacity { get; set; }
    public int AdultsCapacity { get; set; }
    public int NumberOfRooms { get; set; }
    public string? RoomType { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public int? StarRating { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
    public ICollection<string>? Amenities { get; set; } = [];
}
