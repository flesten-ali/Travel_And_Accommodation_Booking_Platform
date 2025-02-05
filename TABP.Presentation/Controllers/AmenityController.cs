using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Amenities.Add;
using TABP.Presentation.DTOs.Amenity;

namespace TABP.Presentation.Controllers;
[Route("api/amenities")]
[ApiController]
public class AmenityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AmenityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("add-amenity")]
    public async Task<IActionResult> Add([FromBody] AddAmenityRequest request)
    {
        var command = _mapper.Map<AddAmenityCommand>(request);
        return Ok(await _mediator.Send(command));
    }
}
