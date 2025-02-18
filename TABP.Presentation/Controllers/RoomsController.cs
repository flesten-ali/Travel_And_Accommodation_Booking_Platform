using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Queries.GetById;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Presentation.DTOs.Room;
namespace TABP.Presentation.Controllers;

[Route("api/rooms")]
[ApiController]
//[Authorize(Roles = Roles.Admin)]
[SwaggerTag("Room Management")]
public class RoomsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("get-for-admin")]
    [SwaggerOperation(
      Summary = "Get rooms for admin",
      Description = "Fetch a list of rooms for administrative spurposes."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomsForAdmin(
      [FromQuery] GetRoomsForAdminRequest request,
      CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetRoomsForAdminQuery>(request);

        var rooms = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(rooms.PaginationMetaData));

        return Ok(rooms.Items);
    }

    [HttpPost]
    [SwaggerOperation(
    Summary = "Create a new room",
    Description = "Create a new room by providing necessary details such as room number, room class Id, and floor."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateRoom(
    CreateRoomRequest request,
    CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateRoomCommand>(request);

        var createdRoom = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetRoom), new { id = createdRoom.Id }, createdRoom);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
    Summary = "Get room by ID",
    Description = "Retrieve information of a room by its unique identifier."
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoom(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRoomByIdQuery { RoomId = id };

        var room = await _mediator.Send(query, cancellationToken);

        return Ok(room);
    }
}
