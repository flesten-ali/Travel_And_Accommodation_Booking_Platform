using AutoMapper;
using TABP.Application.Reviews.GetForHotel;
using TABP.Presentation.DTOs.Review;

namespace TABP.Presentation.Profiles;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<GetHotelReviewsRequest, GetHotelReviewsQuery>();
    }
}
