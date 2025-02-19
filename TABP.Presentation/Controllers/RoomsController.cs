using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Commands.Delete;
using TABP.Application.Rooms.Commands.Update;
using TABP.Application.Rooms.Queries.GetById;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Room;
namespace TABP.Presentation.Controllers;

[Route("api/room-classes/{roomClassId:guid}/rooms")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
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
    Guid roomClassId,
    CreateRoomRequest request,
    CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateRoomCommand>(request);
        command.RoomClassId = roomClassId;

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

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update room details",
        Description = "Update the details of an existing room using its ID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom(Guid id, UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateRoomCommand>(request);
        command.Id = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
    Summary = "Delete a room",
    Description = "Delete an existing room by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRoom(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoomCommand { Id = id };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
