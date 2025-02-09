using AutoMapper;
using TABP.Application.Reviews.Queries.GetDetails;
using TABP.Presentation.DTOs.Review;

namespace TABP.Presentation.Profiles;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<GetHotelReviewsRequest, GetHotelReviewsQuery>();
    }
}
