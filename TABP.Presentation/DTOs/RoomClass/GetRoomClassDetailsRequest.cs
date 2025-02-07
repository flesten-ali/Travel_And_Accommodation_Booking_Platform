namespace TABP.Presentation.DTOs.RoomClass;
public class GetRoomClassDetailsRequest
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
