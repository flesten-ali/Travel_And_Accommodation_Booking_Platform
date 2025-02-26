namespace TABP.Domain.Models;

public sealed class RoomForAdminResult
{
    public Guid Id { get; set; }
    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public bool IsAvaiable { get; set; }
}
