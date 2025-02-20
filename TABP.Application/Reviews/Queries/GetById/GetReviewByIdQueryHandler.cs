using AutoMapper;
using MediatR;
using TABP.Application.Reviews.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Queries.GetById;
public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewResponse>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetReviewByIdQueryHandler(
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<ReviewResponse> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _reviewRepository
            .ExistsAsync(r => r.Id == request.ReviewId && r.HotelId == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(ReviewExceptionMessages.NotFoundForHotel);
        }

        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        return _mapper.Map<ReviewResponse>(review);
    }
}
