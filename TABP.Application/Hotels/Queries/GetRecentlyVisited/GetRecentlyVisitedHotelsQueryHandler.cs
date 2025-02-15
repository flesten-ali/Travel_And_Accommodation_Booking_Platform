using AutoMapper;
using MediatR;
using TABP.Domain.ExceptionMessages;
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
        CancellationToken cancellationToken)
    {
        if (!await _userRepository.ExistsAsync(u => u.Id == request.GuestId))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }

        var recentlyVisitedHotels = await _bookingRepository.GetRecentlyVisitedHotels(request.GuestId, request.Limit);

        return _mapper.Map<IEnumerable<RecentlyVisitedHotelsResponse>>(recentlyVisitedHotels);
    }
}
