﻿using AutoMapper;
using MediatR;
using TABP.Application.Helpers;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetForAdmin;
public class GetRoomClassesForAdminQueryHandler
    : IRequestHandler<GetRoomClassesForAdminQuery, PaginatedResponse<RoomClassForAdminResponse>>
{
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IMapper _mapper;

    public GetRoomClassesForAdminQueryHandler(IRoomClassRepository roomClassRepository, IMapper mapper)
    {
        _roomClassRepository = roomClassRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<RoomClassForAdminResponse>> Handle(
        GetRoomClassesForAdminQuery request,
        CancellationToken cancellationToken = default)
    {
        var orderBy = SortBuilder.BuildRoomClassSort(request.PaginationParameters);

        var roomClasses = await _roomClassRepository.GetRoomClassesForAdminAsync(
            orderBy,
            request.PaginationParameters.PageSize,
            request.PaginationParameters.PageNumber,
            cancellationToken);

        return _mapper.Map<PaginatedResponse<RoomClassForAdminResponse>>(roomClasses);
    }
}