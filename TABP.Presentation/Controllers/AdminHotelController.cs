﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Hotels.Add;
using TABP.Application.Hotels.AddThumbnail;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Controllers;

[Route("api/admin/hotels")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class AdminHotelController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AdminHotelController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("add-hotel")]
    public async Task<IActionResult> AddHotel([FromBody] AddHotelRequest request)
    {
        var command = _mapper.Map<AddHotelCommand>(request);
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("add-hotel-thumbnail")]
    public async Task<IActionResult> AddHotelThumbnail(AddThumbnailRequest request)
    {
        var command = _mapper.Map<AddThumbnailCommand>(request);
        await _mediator.Send(command);
        return Ok(command);
    }
}
