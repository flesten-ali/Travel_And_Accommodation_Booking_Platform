using AutoMapper;
using MediatR;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetDetails;
public class GetRoomClassDetailsQueryHandler
    : IRequestHandler<GetRoomClassDetailsQuery, PaginatedList<GetRoomClassDetailsQueryResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassDetailsQueryHandler(IRoomClassRepository roomClassRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<GetRoomClassDetailsQueryResponse>> Handle(
        GetRoomClassDetailsQuery request,
    CancellationToken cancellationToken
    )
    {
        var roomClasses = await _roomClassRepository
            .GetByHotelIdAsync(request.HotelId, request.PageSize, request.PageNumber);

        if (roomClasses == null)
        {
            throw new NotFoundException($"No room classes found for the hotel with ID {request.HotelId}");
        }

        return _mapper.Map<PaginatedList<GetRoomClassDetailsQueryResponse>>(roomClasses);
    }
}