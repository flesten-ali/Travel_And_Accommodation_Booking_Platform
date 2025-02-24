using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Users.Login;
using TABP.Application.Users.Register;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.User;

namespace TABP.Presentation.Controllers;

/// <summary>
/// Controller for managing user authentication and registration operations, including login and user creation.
/// </summary>
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/users")]
[ApiController]
public class UsersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Authenticates a user and generates an access token.
    /// </summary>
    /// <param name="request">The login request containing the user's credentials.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An access token if authentication is successful.</returns>
    /// <response code="200">Successfully authenticated the user and generated an access token.</response>
    /// <response code="400">The login request data is invalid.</response>
    /// <response code="401">The user is unauthorized (invalid credentials).</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<LoginUserCommand>(request);

        var result = await mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Registers a new user with the role of Guest.
    /// </summary>
    /// <param name="request">The registration request containing the user's details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created user account.</returns>
    /// <response code="201">Successfully created a new user with the role of Guest.</response>
    /// <response code="400">The registration request data is invalid.</response>
    /// <response code="409">A user with the same details already exists.</response>
    [HttpPost("register-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterUserCommand>(request);
        command.Role = Roles.Guest;

        await mediator.Send(command, cancellationToken);

        return Created();
    }

    /// <summary>
    /// Registers a new user with the role of Admin (admin privileges required).
    /// </summary>
    /// <param name="request">The registration request containing the admin's details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The created admin account.</returns>
    /// <response code="201">Successfully created a new admin user.</response>
    /// <response code="400">The registration request data is invalid.</response>
    /// <response code="409">An admin with the same details already exists.</response>
    /// <response code="401">The user is unauthorized to create an admin account.</response>
    /// <response code="403">The user does not have permission to register an admin.</response>
    [HttpPost("register-admin")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterUserCommand>(request);
        command.Role = Roles.Admin;

        await mediator.Send(command, cancellationToken);

        return Created();
    }
}
