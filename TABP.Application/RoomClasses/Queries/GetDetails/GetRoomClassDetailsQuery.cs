﻿using MediatR;
using TABP.Domain.Models;

namespace TABP.Application.RoomClasses.Queries.GetDetails;
public class GetRoomClassDetailsQuery : IRequest<PaginatedList<GetRoomClassDetailsQueryResponse>>
{
    public Guid HotelId { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}