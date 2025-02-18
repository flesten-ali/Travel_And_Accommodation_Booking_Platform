namespace TABP.Presentation.DTOs.Room;
public class UpdateRoomRequest
{
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public Guid RoomClassId { get; set; }
}