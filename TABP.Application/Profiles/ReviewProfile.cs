using AutoMapper;
using TABP.Application.Reviews.Commands.Create;
using TABP.Application.Reviews.Commands.Update;
using TABP.Application.Reviews.Common;
using TABP.Application.Reviews.Queries.GetForHotel;
using TABP.Domain.Entities;
using TABP.Domain.Models;

namespace TABP.Application.Profiles;
public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, HotelReviewsQueryReponse>()
            .ForMember(dest => dest.ReviwerName, opt => opt.MapFrom(src => src.User.UserName));

        CreateMap<PaginatedList<Review>, PaginatedList<HotelReviewsQueryReponse>>();

        CreateMap<CreateReviewCommand, Review>();

        CreateMap<Review, ReviewResponse>();

        CreateMap<UpdateReviewCommand, Review>();
    }
}