using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Amenities.Create;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Amenity;
namespace TABP.Presentation.Controllers;

[Route("api/amenities")]
[ApiController]
public class AmenityController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAmenity([FromBody] CreateAmenityRequest request)
    {
        var command = _mapper.Map<CreateAmenityCommand>(request);

        await _mediator.Send(command);

        return Created();
    }
}
