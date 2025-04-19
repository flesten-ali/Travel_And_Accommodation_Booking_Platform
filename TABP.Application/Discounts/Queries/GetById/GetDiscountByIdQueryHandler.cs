using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Queries.GetById;

/// <summary>
/// Handles the query for retrieving a discount by its ID for a specific room class.
/// </summary>
public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountResponse>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetDiscountByIdQueryHandler(
        IDiscountRepository discountRepository,
        IRoomClassRepository roomClassRepository,
        IMapper mapper)
    {
        _discountRepository = discountRepository;
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the retrieval of a discount by its ID for a specific room class.
    /// </summary>
    /// <param name="request">The request containing the room class ID and discount ID to be fetched.</param>
    /// <param name="cancellationToken">A cancellation token for gracefully canceling the operation.</param>
    /// <returns>A task that represents the asynchronous operation, with a <see cref="DiscountResponse"/> as a result.</returns>
    /// <exception cref="NotFoundException">Thrown if the room class or discount does not exist.</exception>
    public async Task<DiscountResponse> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (!await _discountRepository
            .ExistsAsync(d => d.Id == request.DiscountId && d.RoomClassId == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(DiscountExceptionMessages.NotFoundForTheRoomClass);
        }

        var discount = await _discountRepository.GetByIdAsync(request.DiscountId, cancellationToken)
            ?? throw new NotFoundException(DiscountExceptionMessages.NotFound);

        return _mapper.Map<DiscountResponse>(discount);
    }
}
