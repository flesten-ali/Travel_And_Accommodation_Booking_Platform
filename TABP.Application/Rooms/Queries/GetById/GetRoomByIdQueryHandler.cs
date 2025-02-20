using AutoMapper;
using MediatR;
using TABP.Application.Rooms.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Queries.GetById;
public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;
    private readonly IRoomClassRepository _roomClassRepository;

    public GetRoomByIdQueryHandler(
        IRoomRepository roomRepository,
        IMapper mapper,
        IRoomClassRepository roomClassRepository)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
        _roomClassRepository = roomClassRepository;
    }

    public async Task<RoomResponse> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        if (!await _roomRepository
            .ExistsAsync(r => r.Id == request.RoomId && r.RoomClassId == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomExceptionMessages.NotFoundForTheRoomClass);
        }

        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken)
                     ?? throw new NotFoundException(RoomExceptionMessages.NotFound);

        return _mapper.Map<RoomResponse>(room);
    }
}
