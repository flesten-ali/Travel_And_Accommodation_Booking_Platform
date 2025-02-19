using AutoMapper;
using MediatR;
using TABP.Application.Rooms.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Rooms.Commands.Create;
public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(
        IRoomClassRepository roomClassRepository,
        IMapper mapper,
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.RoomClassId, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var room = _mapper.Map<Room>(request);

        await _roomRepository.CreateAsync(room, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RoomResponse>(room);
    }
}
