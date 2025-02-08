using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Hotels.Commands.AddHotel;
using TABP.Application.Hotels.Commands.AddImageGallery;
using TABP.Application.Hotels.Commands.AddThumbnail;
using TABP.Domain.Constants;
using TABP.Presentation.DTOs;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Controllers;

[Route("api/[controller]")]
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
    public async Task<IActionResult> Add([FromBody] AddHotelRequest request)
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

    [HttpPost("add-hotel-gallery")]
    public async Task<IActionResult> AddHotelGallery(AddImageGalleryRequest request)
    {
        var command = _mapper.Map<AddImageGalleryCommand>(request);
        await _mediator.Send(command);
        return Ok(command);
    }
}
