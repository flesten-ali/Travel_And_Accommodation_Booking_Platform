using AutoMapper;
using MediatR;
using TABP.Application.Helper;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForHotel;
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
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildRoomClassSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository
            .GetByHotelIdAsync(
            orderBy,
            request.HotelId,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedList<HotelRoomClassesQueryResponse>>(roomClasses);
    }
}