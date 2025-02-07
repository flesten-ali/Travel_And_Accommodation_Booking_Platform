using TABP.Domain.Enums;

namespace TABP.Application.RoomClasses.Queries.GetDetails;
public class GetRoomClassDetailsQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public RoomType RoomType { get; set; }
    public int AdultsCapacity { get; set; }
    public int ChildrenCapacity { get; set; }
    public double Price { get; set; }
    public IEnumerable<string> GalleryUrls { get; set; } = [];
    public IEnumerable<DiscountResponse> DiscountResponses { get; set; } = [];
    public IEnumerable<AmenityResponse> AmenityResponses { get; set; } = [];
}

public class AmenityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class DiscountResponse
{
    public Guid Id { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid RoomClassId { get; set; }
}