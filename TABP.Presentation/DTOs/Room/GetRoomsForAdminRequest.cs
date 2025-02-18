using TABP.Application.Shared;

namespace TABP.Presentation.DTOs.Room;
public class GetRoomsForAdminRequest
{
    public Guid RoomClassId { get; set; }
    public PaginationParameters PaginationParameters { get; set; }
}
