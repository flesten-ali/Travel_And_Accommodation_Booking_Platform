using AutoMapper;
using MediatR;
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
        CancellationToken cancellationToken)
    {
        var roomClasses = await _roomClassRepository.GetRoomClassesForAdminAsync(request.PageSize, request.PageNumber);

        return _mapper.Map<PaginatedList<RoomClassForAdminResponse>>(roomClasses);
    }
}
//order parameter
//centrilize validations