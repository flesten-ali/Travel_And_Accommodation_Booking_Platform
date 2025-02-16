﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TABP.Application.RoomClasses.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.RoomClass;
namespace TABP.Presentation.Controllers;

[Route("api/room-classes")]
[ApiController]
//[Authorize(Roles = Roles.Admin)]
public class RoomClassesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomClassesForAdmin([FromQuery] GetRoomClassesForAdminRequest request)
    {
        var query = _mapper.Map<GetRoomClassesForAdminQuery>(request);

        var roomClasses = await _mediator.Send(query);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(roomClasses.PaginationMetaData));

        return Ok(roomClasses.Items);
    }
}
