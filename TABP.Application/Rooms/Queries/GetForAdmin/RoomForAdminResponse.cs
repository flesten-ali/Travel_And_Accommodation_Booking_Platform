namespace TABP.Application.Rooms.Queries.GetForAdmin;
public class RoomForAdminResponse
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public bool IsAvaiable { get; set; }
}
