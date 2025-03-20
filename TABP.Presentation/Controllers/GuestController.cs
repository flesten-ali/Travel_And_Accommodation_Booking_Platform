using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Hotels.Queries.GetRecentlyVisited;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Guest;
using TABP.Presentation.Extensions;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing guest-related operations, including retrieving recently visited hotels.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/guests")]
[ApiController]
[Authorize(Roles = Roles.Guest)]
public class GuestController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of hotels recently visited by the specified guest.
    /// </summary>
    /// <param name="request">The request containing the filtering options for retrieving the hotels.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A list of hotels recently visited by the guest.</returns>
    /// <response code="200">Returns a list of recently visited hotels.</response>
    /// <response code="400">The request data is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the necessary permissions to access the resource.</response>
    /// <response code="404">No recently visited hotels were found for the guest.</response>
    [HttpGet("recently-visited-hotels")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecentlyVisitedHotels(
        [FromQuery] GetRecentlyVisitedHotelsRequest request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetRecentlyVisitedHotelsQuery>(request);
        query.GuestId = User.GetUserId();

        var hotels = await mediator.Send(query, cancellationToken);

        return Ok(hotels);
    }
}
