namespace TABP.Domain.Models;

public sealed record RoomForAdminResult(Guid Id, int RoomNumber, int Floor, bool IsAvaiable);
