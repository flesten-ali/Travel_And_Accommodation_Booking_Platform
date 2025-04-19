namespace TABP.Application.Reviews.Queries.GetForHotel;
public sealed record HotelReviewsQueryReponse(
    Guid Id,
    string Comment, 
    int Rate, 
    DateTime CreatedDate,
    DateTime? UpdatedDate, 
    string ReviwerName);
