using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Users.Register;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs.Auth;

namespace TABP.Presentation.Controllers;
[Route("api/admins")]
[ApiController]
[Authorize(Roles = Roles.Admin)]

public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AdminController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest request)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);
        command.Role = Roles.Admin;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
