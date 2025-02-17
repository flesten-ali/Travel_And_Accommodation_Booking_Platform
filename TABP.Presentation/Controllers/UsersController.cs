using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TABP.Application.Users.Login;
using TABP.Application.Users.Register;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Auth;
namespace TABP.Presentation.Controllers;

[Route("api/users")]
[ApiController]
[SwaggerTag("User Authentication and Management")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login a user",
        Description = "Authenticate a user and generate an access token."
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<LoginUserCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("register-user")]
    [SwaggerOperation(
        Summary = "Register a new user",
        Description = "Register a new user with the role of Guest."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        command.Role = Roles.Guest;

        await _mediator.Send(command, cancellationToken);

        return Created();
    }

    [HttpPost("register-admin")]
    [SwaggerOperation(
        Summary = "Register a new admin",
        Description = "Register a new user with the role of Admin (admin privileges required)."
    )]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        command.Role = Roles.Admin;

        await _mediator.Send(command, cancellationToken);

        return Created();
    }
}
