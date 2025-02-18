using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.RoomClasses.Commands.Create;
using TABP.Application.RoomClasses.Commands.Delete;
using TABP.Application.RoomClasses.Commands.Update;
using TABP.Application.RoomClasses.Queries.GetById;
using TABP.Application.RoomClasses.Queries.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.RoomClass;
namespace TABP.Presentation.Controllers;

[Route("api/room-classes")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
[SwaggerTag("Room Class Management")]
public class RoomClassesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpGet("get-for-admin")]
    [SwaggerOperation(
        Summary = "Get room classes for admin",
        Description = "Fetch a list of room classes for administrative purposes."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomClassesForAdmin(
        [FromQuery] GetRoomClassesForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetRoomClassesForAdminQuery>(request);

        var roomClasses = await _mediator.Send(query, cancellationToken);

        Response.Headers.Append("x-pagination",
            JsonSerializer.Serialize(roomClasses.PaginationMetaData));

        return Ok(roomClasses.Items);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new room class",
        Description = "Creates a new room class with the provided details and returns the created entity."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoomClass(
          CreateRoomClassRequest request,
          CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateRoomClassCommand>(request);

        var createdRoomClass = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetRoomClass), new { id = createdRoomClass.Id }, createdRoomClass);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get room class by ID",
        Description = "Retrieves the details of a specific room class by its unique identifier."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomClass(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRoomClassByIdQuery { Id = id };

        var roomClass = await _mediator.Send(query, cancellationToken);

        return Ok(roomClass);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing room class",
        Description = "Updates the details of a room class identified by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoomClass(
        Guid id,
        UpdateRoomClassRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateRoomClassCommand>(request);
        command.Id = id;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete a room class",
        Description = "Deletes the room class identified by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomClass(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoomClassCommand { Id = id };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
