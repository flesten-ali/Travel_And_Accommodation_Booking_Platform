using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Commands.Update;
internal class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateReviewCommandHandler(
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        if (!await _reviewRepository
            .ExistsAsync(r => r.Id == request.Id && r.HotelId == request.HotelId && r.UserId == request.UserId, cancellationToken))
        {
            throw new NotFoundException(ReviewExceptionMessages.NotFoundForHotel);
        }

        var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(ReviewExceptionMessages.NotFound);

        _mapper.Map(request, review);

        _reviewRepository.Update(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

}
