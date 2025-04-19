namespace TABP.Application.Discounts.Common;
public sealed record DiscountResponse(Guid Id, double Percentage, DateTime StartDate, DateTime EndDate, Guid RoomClassId);
