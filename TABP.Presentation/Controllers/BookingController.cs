using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TABP.Application.Bookings.Commands.Add;
using TABP.Presentation.DTOs.Booking;

namespace TABP.Presentation.Controllers;
[Route("api/bookings")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BookingController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Add([FromBody] AddBookingRequest request)
    {
        var command = _mapper.Map<AddBookingCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
