using AutoMapper;
using MediatR;
using TABP.Application.Helper;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.Rooms.Queries.GetForAdmin;
public class GetRoomsForAdminQueryHandler : IRequestHandler<GetRoomsForAdminQuery, PaginatedList<RoomForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomsForAdminQueryHandler(
        IRoomClassRepository roomClassRepository,
        IRoomRepository roomRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoomForAdminResponse>> Handle(GetRoomsForAdminQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomExceptionMessages.NotFound);
        }

        var orderBy = SortBuilder.BuildRoomSort(request.PaginationParameters);

        var roomClasses = await _roomRepository.GetRoomsForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedList<RoomForAdminResponse>>(roomClasses);
    }
}
