using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Hotels.Queries.GetRecentlyVisited;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Guest;
namespace TABP.Presentation.Controllers;

[Route("api/guests")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
[SwaggerTag("Manage guest-related operations including retrieving recently visited hotels.")]
public class GuestController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost("{id:guid}/recently-visited-hotels")]
    [SwaggerOperation(
        Summary = "Get recently visited hotels",
        Description = "Retrieves a list of hotels recently visited by the specified guest."
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecentlyVisitedHotels(
        Guid id,
        [FromQuery] GetRecentlyVisitedHotelsRequest request,
        CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetRecentlyVisitedHotelsQuery>(request);
        query.GuestId = id;

        var hotels = await _mediator.Send(query, cancellationToken);

        return Ok(hotels);
    }
}
