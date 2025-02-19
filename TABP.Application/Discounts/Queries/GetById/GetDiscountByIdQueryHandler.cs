using AutoMapper;
using MediatR;
using TABP.Application.Discounts.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Discounts.Queries.GetById;
public class GetDiscountByIdQueryHandler : IRequestHandler<GetDiscountByIdQuery, DiscountResponse>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly IRoomClassRepository _roomClassRepository;

    public GetDiscountByIdQueryHandler(
        IDiscountRepository discountRepository,
        IMapper mapper,
        IRoomClassRepository roomClassRepository)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
        _roomClassRepository = roomClassRepository;
    }

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
