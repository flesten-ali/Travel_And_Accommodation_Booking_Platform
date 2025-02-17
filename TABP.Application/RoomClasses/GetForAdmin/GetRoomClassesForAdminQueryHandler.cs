using AutoMapper;
using MediatR;
using TABP.Application.Helper;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.GetForAdmin;
public class GetRoomClassesForAdminQueryHandler
    : IRequestHandler<GetRoomClassesForAdminQuery, PaginatedList<RoomClassForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassesForAdminQueryHandler(IRoomClassRepository roomClassRepository, IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RoomClassForAdminResponse>> Handle(
        GetRoomClassesForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildRoomClassSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository.GetRoomClassesForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedList<RoomClassForAdminResponse>>(roomClasses);
    }
}