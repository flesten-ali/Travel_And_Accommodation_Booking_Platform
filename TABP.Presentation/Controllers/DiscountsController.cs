using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Discounts.Commands.Create;
using TABP.Application.Discounts.Commands.Delete;
using TABP.Application.Discounts.Queries.GetById;
using TABP.Application.Discounts.Queries.GetForRoomClass;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Discount;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing discounts related to specific room classes.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/room-classes/{roomClassId:guid}/discounts")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DiscountsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Creates a new discount for a specific room class.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class.</param>
    /// <param name="request">The details of the discount to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created discount details.</returns>
    /// <response code="201">The discount was successfully created.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to create a discount.</response>
    /// <response code="404">The specified room class was not found.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateDiscount(
    Guid roomClassId,
    CreateDiscountRequest request,
    CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateDiscountCommand>(request);
        command.RoomClassId = roomClassId;

        var createdDiscount = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetDiscount),
            new
            {
                roomClassId,
                id = createdDiscount.Id
            },
            createdDiscount);
    }

    /// <summary>
    /// Retrieves a discount by its ID for a specific room class.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class.</param>
    /// <param name="id">The unique identifier of the discount.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The details of the specified discount.</returns>
    /// <response code="200">Returns the discount details.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to view the discount.</response>
    /// <response code="404">The discount was not found for the specified room class.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDiscount(Guid roomClassId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDiscountByIdQuery(id, roomClassId);

        var discount = await mediator.Send(query, cancellationToken);

        return Ok(discount);
    }

    /// <summary>
    /// Deletes an existing discount for a specific room class.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class.</param>
    /// <param name="id">The unique identifier of the discount to delete.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">The discount was successfully deleted.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to delete the discount.</response>
    /// <response code="404">The discount to delete was not found.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDiscount(Guid roomClassId, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteDiscountCommand(id, roomClassId);

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Retrieves a list of discounts for a specific room class.
    /// </summary>
    /// <param name="roomClassId">The unique identifier of the room class.</param>
    /// <param name="request">The request containing pagination and filtering options.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of discounts for the specified room class.</returns>
    /// <response code="200">Returns a list of discounts for the room class.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to view the discounts.</response>
    /// <response code="404">No discounts found for the specified room class.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDiscountsForRoomClass(
    Guid roomClassId,
    [FromQuery] GetDiscountsForRoomClassRequest request,
    CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetDiscountsForRoomClassQuery>(request);
        query.RoomClassId = roomClassId;

        var discounts = await mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(discounts.PaginationMetaData);

        return Ok(discounts.Items);
    }
}
