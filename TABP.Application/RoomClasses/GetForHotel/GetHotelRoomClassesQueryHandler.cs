using AutoMapper;
using MediatR;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForHotel;
public class GetHotelRoomClassesQueryHandler
    : IRequestHandler<GetHotelRoomClassesQuery, PaginatedList<HotelRoomClassesQueryResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetHotelRoomClassesQueryHandler(IRoomClassRepository roomClassRepository,
        IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<HotelRoomClassesQueryResponse>> Handle(
        GetHotelRoomClassesQuery request,
        CancellationToken cancellationToken)
    {
        var roomClasses = await _roomClassRepository
            .GetByHotelIdAsync(request.HotelId, request.PageSize, request.PageNumber);

        return _mapper.Map<PaginatedList<HotelRoomClassesQueryResponse>>(roomClasses);
    }
}