using AutoMapper;
using MediatR;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Queries.GetRecentlyVisited;

/// <summary>
/// Handles the query to retrieve a list of recently visited hotels by a guest.
/// </summary
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

    /// <summary>
    /// Handles the query to retrieve recently visited hotels by a guest.
    /// </summary>
    /// <param name="request">The query containing the guest ID and the limit for the number of hotels to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The result is a list of recently visited hotels as <see cref="RecentlyVisitedHotelsResponse"/>.
    /// </returns>
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
