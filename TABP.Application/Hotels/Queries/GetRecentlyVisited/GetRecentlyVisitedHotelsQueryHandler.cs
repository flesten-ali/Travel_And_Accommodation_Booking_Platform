using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetRecentlyVisited;
public class GetRecentlyVisitedHotelsQueryHandler
    : IRequestHandler<GetRecentlyVisitedHotelsQuery, IEnumerable<RecentlyVisitedHotelsResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetRecentlyVisitedHotelsQueryHandler(
        IUserRepository userRepository,
         IBookingRepository bookingRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecentlyVisitedHotelsResponse>> Handle(
        GetRecentlyVisitedHotelsQuery request,
        CancellationToken cancellationToken = default)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.GuestId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        var recentlyVisitedHotels = await _bookingRepository
            .GetRecentlyVisitedHotelsAsync(request.GuestId, request.Limit, cancellationToken);

        return _mapper.Map<IEnumerable<RecentlyVisitedHotelsResponse>>(recentlyVisitedHotels);
    }
}
