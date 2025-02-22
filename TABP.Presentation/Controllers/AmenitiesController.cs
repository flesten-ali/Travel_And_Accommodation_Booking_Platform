using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Amenities.Commands.Create;
using TABP.Application.Amenities.Commands.Delete;
using TABP.Application.Amenities.Commands.Update;
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
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new amenity",
        Description = "Creates a new amenity and returns the created amenity details."
    )]
    [ProducesResponseType(typeof(AmenityResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAmenity(CreateAmenityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateAmenityCommand>(request);

        var createdAmenity = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetAmenity), new { id = createdAmenity.Id }, createdAmenity);
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

        var amenity = await mediator.Send(query, cancellationToken);

        return Ok(amenity);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
    Summary = "Update amenity details",
    Description = "Update the details of an existing amenity using its ID."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAmenity(Guid id, UpdateAmenityRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateAmenityCommand>(request);
        command.Id = id;

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
    Summary = "Delete a amenity",
    Description = "Delete an existing amenity by its unique ID."
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAmenity(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteAmenityCommand { Id = id };

        await mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
