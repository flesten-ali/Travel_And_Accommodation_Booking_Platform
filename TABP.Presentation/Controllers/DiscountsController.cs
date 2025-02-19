using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Discounts.Commands.Create;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Discount;

namespace TABP.Presentation.Controllers;

[Route("api/room-classes/{roomClassId:guid}/discounts")]
[ApiController]
//[Authorize(Roles = Roles.Admin)]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public DiscountsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

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
    [FromBody] CreateDiscountRequest request,
    CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateDiscountCommand>(request);
        command.RoomClassId = roomClassId;

        var createdDiscount = await _mediator.Send(command, cancellationToken);

        return Ok(createdDiscount);
        //return CreatedAtAction(nameof(GetDiscount), new { id = createdDiscount.Id }, createdDiscount);
    } 

}
