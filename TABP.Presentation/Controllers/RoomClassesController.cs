using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.RoomClasses.Commands.Create;
using TABP.Application.RoomClasses.Commands.Delete;
using TABP.Application.RoomClasses.Commands.ImageGallery;
using TABP.Application.RoomClasses.Commands.Update;
using TABP.Application.RoomClasses.Queries.GetById;
using TABP.Application.RoomClasses.Queries.GetForAdmin;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.RoomClass;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing room classes, including creating, retrieving, updating, and deleting room classes.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/room-classes")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class RoomClassesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of room classes for administrative purposes.
    /// </summary>
    /// <param name="request">The request containing filtering and pagination options for the room classes.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of room classes.</returns>
    /// <response code="200">Successfully retrieved the list of room classes.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to view room classes.</response>
    [HttpGet("get-for-admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoomClassesForAdmin(
        [FromQuery] GetRoomClassesForAdminRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetRoomClassesForAdminQuery>(request);

        var roomClasses = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(roomClasses.PaginationMetaData);

        return Ok(roomClasses.Items);
    }

    /// <summary>
    /// Creates a new room class with the provided details.
    /// </summary>
    /// <param name="request">The details of the room class to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The newly created room class.</returns>
    /// <response code="201">Successfully created the room class.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to create a room class.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRoomClass(
          CreateRoomClassRequest request,
          CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateRoomClassCommand>(request);

        var createdRoomClass = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetRoomClass), new { id = createdRoomClass.Id }, createdRoomClass);
    }

    /// <summary>
    /// Retrieves a room class by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the room class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The room class details.</returns>
    /// <response code="200">Successfully retrieved the room class.</response>
    /// <response code="400">The room class ID is invalid.</response>
    /// <response code="404">No room class found with the specified ID.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomClass(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRoomClassByIdQuery(id);

        var roomClass = await mediator.Send(query, cancellationToken);

        return Ok(roomClass);
    }

    /// <summary>
    /// Updates an existing room class.
    /// </summary>
    /// <param name="id">The unique identifier of the room class to be updated.</param>
    /// <param name="request">The updated room class details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the room class is successfully updated.</returns>
    /// <response code="204">Successfully updated the room class.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to update the room class.</response>
    /// <response code="404">No room class found with the specified ID.</response>
    [HttpPut("{id:guid}")]
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
        var command = mapper.Map<UpdateRoomClassCommand>(request);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes an existing room class by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the room class to be deleted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the room class is successfully deleted.</returns>
    /// <response code="204">Successfully deleted the room class.</response>
    /// <response code="400">The room class ID is invalid.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to delete the room class.</response>
    /// <response code="404">No room class found with the specified ID.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomClass(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoomClassCommand(id);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Uploads gallery images for a room class.
    /// </summary>
    /// <param name="id">The unique identifier of the room class.</param>
    /// <param name="files">The gallery images to be uploaded.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A result indicating the success or failure of the upload operation.</returns>
    /// <response code="200">Successfully uploaded the gallery images.</response>
    /// <response code="400">The files are invalid or empty.</response>
    /// <response code="401">The user is not authorized.</response>
    /// <response code="403">The user does not have permission to upload gallery images.</response>
    [HttpPost("{id:guid}/gallery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UploadRoomClassGalleryImages(
    Guid id,
    [FromForm] UploadRoomClassImageGalleryRequest request,
    CancellationToken cancellationToken)
    {
        var command = mapper.Map<UploadRoomClassImageGalleryCommand>(request);
        command.RoomClassId = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
