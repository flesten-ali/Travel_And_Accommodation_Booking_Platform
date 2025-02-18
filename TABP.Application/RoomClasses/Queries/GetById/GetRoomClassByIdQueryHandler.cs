using AutoMapper;
using MediatR;
using TABP.Application.RoomClasses.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.RoomClasses.Queries.GetById;
public class GetRoomClassByIdQueryHandler : IRequestHandler<GetRoomClassByIdQuery, RoomClassResponse>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassByIdQueryHandler(IRoomClassRepository roomClassRepository, IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    public async Task<RoomClassResponse> Handle(GetRoomClassByIdQuery request, CancellationToken cancellationToken = default)
    {
        if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == request.Id, cancellationToken))
        {
            throw new NotFoundException(RoomClassExceptionMessages.NotFound);
        }

        var roomClass = await _roomClassRepository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<RoomClassResponse>(roomClass);
    }
}