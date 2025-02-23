using TABP.Domain.Enums;
namespace TABP.Application.Bookings.Common;

public sealed record BookingResponse(
    Guid Id,
    PaymentMethod PaymentMethod,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    DateTime BookingDate,
    string? Remarks,
    decimal TotalPrice,
    Guid UserId,
    IEnumerable<Guid> RoomIds);
