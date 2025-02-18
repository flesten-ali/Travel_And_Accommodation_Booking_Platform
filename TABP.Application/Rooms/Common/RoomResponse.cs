namespace TABP.Application.Rooms.Common;
public class RoomResponse
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public Guid RoomClassId { get; set; }
}
