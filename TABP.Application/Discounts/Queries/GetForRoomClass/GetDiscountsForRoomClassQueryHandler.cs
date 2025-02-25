using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Application.Helpers;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Discounts.Queries.GetForRoomClass;

/// <summary>
/// Handles the query for retrieving a paginated list of discounts for a specific room class.
/// </summary>
public class GetDiscountsForRoomClassQueryHandler
    : IRequestHandler<GetDiscountsForRoomClassQuery, PaginatedResponse<DiscountResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public GetDiscountsForRoomClassQueryHandler(
        IRoomClassRepository roomClassRepository,
        IDiscountRepository discountRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the retrieval of a paginated list of discounts for a specific room class.
    /// </summary>
    /// <param name="request">The request containing the room class ID and pagination parameters.</param>
    /// <param name="cancellationToken">A cancellation token for gracefully canceling the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, with a <see cref="PaginatedResponse{DiscountResponse}"/> as a result.
    /// </returns>
    /// <exception cref="NotFoundException">Thrown if the room class does not exist.</exception>
    public async Task<PaginatedResponse<DiscountResponse>> Handle(
        GetDiscountsForRoomClassQuery request,
        CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(r => r.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var orderBy = SortBuilder.BuildDiscountSort(request.PaginationParameters);

        var discounts = await _discountRepository.GetDiscountsForRoomClass(
            orderBy,
            request.RoomClassId,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<DiscountResponse>>(discounts);
    }
}
