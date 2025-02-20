using AutoMapper;
using TABP.Application.Reviews.Commands.Create;
using TABP.Application.Reviews.Commands.Update;
using TABP.Application.Reviews.Queries.GetForHotel;
using TABP.Presentation.DTOs.Review;

namespace TABP.Presentation.Profiles;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<GetHotelReviewsRequest, GetHotelReviewsQuery>();

        CreateMap<CreateReviewRequest, CreateReviewCommand>();

        CreateMap<UpdateReviewRequest, UpdateReviewCommand>();
    }
}
