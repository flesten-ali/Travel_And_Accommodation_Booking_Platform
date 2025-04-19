namespace TABP.Application.Reviews.Common;
public sealed record ReviewResponse(
    Guid Id,
    string Comment,
    int Rate,
    DateTime CreatedDate,
    DateTime? UpdatedDate,
    Guid HotelId);
