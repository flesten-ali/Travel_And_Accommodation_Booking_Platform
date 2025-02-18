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

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<RoomResponse> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId, cancellationToken)
                     ?? throw new NotFoundException(RoomExceptionMessages.NotFound);

        return _mapper.Map<RoomResponse>(room);
    }
}
