using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using TABP.Application.Discounts.Commands.Create;
using TABP.Application.Discounts.Commands.Delete;
using TABP.Application.Discounts.Queries.GetById;
using TABP.Application.Discounts.Queries.GetForRoomClass;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Discount;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

[Route("api/room-classes/{roomClassId:guid}/discounts")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DiscountsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new discount for a specific room class",
        Description = "Create a new discount by providing necessary details."
    )]
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
        var command = _mapper.Map<CreateDiscountCommand>(request);
        command.RoomClassId = roomClassId;

        var createdDiscount = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetDiscount),
            new
            {
                roomClassId,
                id = createdDiscount.Id
            },
            createdDiscount);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a discount by ID",
        Description = "Gets discount details for a specific room class by providing the discount and room class IDs."
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDiscount(Guid roomClassId, Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDiscountByIdQuery
        {
            DiscountId = id,
            RoomClassId = roomClassId
        };

        var discount = await _mediator.Send(query, cancellationToken);

        return Ok(discount);
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Delete a discount",
        Description = "Delete an existing discount by providing the discount and room class IDs."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDiscount(Guid roomClassId, Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteDiscountCommand
        {
            DiscountId = id,
            RoomClassId = roomClassId
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(
    Summary = "Get discounts of room class",
    Description = "Retrieve a list of discounts for specific room class."
    )]
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
        var query = _mapper.Map<GetDiscountsForRoomClassQuery>(request);
        query.RoomClassId = roomClassId;

        var discounts = await _mediator.Send(query, cancellationToken);

        Response.AddPaginationHeader(discounts.PaginationMetaData);

        return Ok(discounts.Items);
    }
}
