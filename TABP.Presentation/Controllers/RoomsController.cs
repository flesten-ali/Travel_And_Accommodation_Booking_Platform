using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Rooms.Commands.Create;
using TABP.Application.Rooms.Commands.Delete;
using TABP.Application.Rooms.Commands.Update;
using TABP.Application.Rooms.Queries.GetById;
using TABP.Application.Rooms.Queries.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Room;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing rooms within a specific room class. Provides operations for creating, retrieving, updating, and deleting rooms.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/room-classes/{roomClassId:guid}/rooms")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class RoomsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of rooms for administrative purposes.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class to which the rooms belong.</param>
    /// <param name="request">The request containing filtering and pagination options for the rooms.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of rooms along with pagination metadata.</returns>
    /// <response code="200">Successfully retrieved the list of rooms.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to view rooms.</response>
    [HttpGet("get-for-admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomsForAdmin(
      Guid roomClassId,
      [FromQuery] GetRoomsForAdminRequest request,
      CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetRoomsForAdminQuery>(request);
        query.RoomClassId = roomClassId;

        var rooms = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(rooms.PaginationMetaData);

        return Ok(rooms.Items);
    }

    /// <summary>
    /// Creates a new room by providing necessary details such as room number, room class ID, and floor.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class to which the new room will belong.</param>
    /// <param name="request">The details of the room to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The newly created room.</returns>
    /// <response code="201">Successfully created the room.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to create a room.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateRoom(
    Guid roomClassId,
    CreateRoomRequest request,
    CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateRoomCommand>(request);
        command.RoomClassId = roomClassId;

        var createdRoom = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetRoom), new
        {
            roomClassId,
            id = createdRoom.Id
        }, createdRoom);
    }

    /// <summary>
    /// Retrieves room details by its unique ID.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class that the room belongs to.</param>
    /// <param name="id">The unique identifier of the room.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The room's details.</returns>
    /// <response code="200">Successfully retrieved the room details.</response>
    /// <response code="404">The room was not found.</response>
    /// <response code="400">The room ID is invalid.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoom(
        Guid roomClassId,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetRoomByIdQuery(id, roomClassId);

        var room = await mediator.Send(query, cancellationToken);

        return Ok(room);
    }

    /// <summary>
    /// Updates the details of an existing room.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class that the room belongs to.</param>
    /// <param name="id">The unique identifier of the room to be updated.</param>
    /// <param name="request">The updated room details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the room is successfully updated.</returns>
    /// <response code="204">Successfully updated the room.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to update the room.</response>
    /// <response code="404">The room was not found.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom(
        Guid roomClassId,
        Guid id,
        UpdateRoomRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateRoomCommand>(request);
        command.Id = id;
        command.RoomClassId = roomClassId;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes a room by its unique ID.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class that the room belongs to.</param>
    /// <param name="id">The unique identifier of the room to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the room is successfully deleted.</returns>
    /// <response code="204">Successfully deleted the room.</response>
    /// <response code="404">The room was not found.</response>
    /// <response code="400">The room ID is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to delete the room.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRoom(Guid roomClassId, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoomCommand(id, roomClassId);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
