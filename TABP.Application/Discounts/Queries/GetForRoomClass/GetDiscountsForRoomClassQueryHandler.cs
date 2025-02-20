using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Application.Helpers;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Discounts.Queries.GetForRoomClass;
public class GetDiscountsForRoomClassQueryHandler
    : IRequestHandler<GetDiscountsForRoomClassQuery, PaginatedList<DiscountResponse>>
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

    public async Task<PaginatedList<DiscountResponse>> Handle(
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

        return _mapper.Map<PaginatedList<DiscountResponse>>(discounts);
    }
}
