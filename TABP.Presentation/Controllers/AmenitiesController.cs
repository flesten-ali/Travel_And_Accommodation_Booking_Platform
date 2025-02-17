using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Amenities.Commands.Create;
using TABP.Application.Amenities.Common;
using TABP.Application.Amenities.Queries.GetById;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Amenity;
namespace TABP.Presentation.Controllers;

[Route("api/amenities")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
[SwaggerTag("Manage amenities in the system. Requires Admin access.")]
public class AmenitiesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new amenity",
        Description = "Creates a new amenity and returns the created amenity details."
    )]
    [ProducesResponseType(typeof(AmenityResponse),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateAmenityCommand>(request);

        var amenity = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetAmenity), new { id = amenity.Id }, amenity);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Get an amenity by ID",
        Description = "Retrieves the details of a specific amenity based on the provided ID."
    )]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAmenity(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAmenityByIdQuery { AmenityId = id };

        var amenity = await _mediator.Send(query, cancellationToken);

        return Ok(amenity);
    }
}
