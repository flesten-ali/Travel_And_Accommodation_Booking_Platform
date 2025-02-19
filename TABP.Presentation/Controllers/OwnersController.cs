using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Owners.Commands.Create;
using TABP.Presentation.DTOs.Owner;

namespace TABP.Presentation.Controllers;
[Route("api/owners")]
[ApiController]
//[Authorize(Roles = Roles.Admin)]
public class OwnersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new owner",
        Description = "Create a new owner by providing necessary details."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateOwner(  
      [FromBody] CreateOwnerRequset request,
      CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateOwnerCommand>(request);

        var createdOwner = await _mediator.Send(command, cancellationToken);

        return Ok(createdOwner);
        //return CreatedAtAction(nameof(GetHotel), new { id = createdOwner.Id }, createdOwner);
    }
}
