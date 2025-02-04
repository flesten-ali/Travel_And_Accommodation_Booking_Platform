using MediatR;
using TABP.Domain.Enums;
using TABP.Domain.Models;
namespace TABP.Application.Hotels.Search;

public class SearchHotelCommand : IRequest<PaginatedList<SearchHotelResponse>>
{
    public string City { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int ChildrenCapacity { get; set; }
    public int AdultsCapacity { get; set; }
    public int NumberOfRooms { get; set; }
    public string? SortBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    public string? RoomType { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public int? StarRating { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public ICollection<string>? Amenities { get; set; } = [];
}
