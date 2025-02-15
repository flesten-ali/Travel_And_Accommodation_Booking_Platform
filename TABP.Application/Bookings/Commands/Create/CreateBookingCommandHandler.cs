using AutoMapper;
using MediatR;
using TABP.Application.Bookings.Common;
using TABP.Domain.Constants;
using TABP.Domain.Entities;
using TABP.Domain.Enums;
using TABP.Domain.ExceptionMessages;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;
using TABP.Domain.Interfaces.Services.Email;
using TABP.Domain.Interfaces.Services.Html;
using TABP.Domain.Interfaces.Services.Pdf;
namespace TABP.Application.Bookings.Commands.Create;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPdfService _pdfService;
    private readonly IInvoiceHtmlGenerationService _invoiceHtmlGenerationService;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IMapper _mapper;

    public CreateBookingCommandHandler(
        IUserRepository userRepository,
        IRoomRepository roomRepository,
        IHotelRepository hotelRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IPdfService pdfService,
        IInvoiceHtmlGenerationService invoiceHtmlGenerationService,
        IEmailSenderService emailSenderService,
        IMapper mapper
    )
    {
        _userRepository = userRepository;
        _roomRepository = roomRepository;
        _hotelRepository = hotelRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pdfService = pdfService;
        _invoiceHtmlGenerationService = invoiceHtmlGenerationService;
        _emailSenderService = emailSenderService;
        _mapper = mapper;
    }

    public async Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(UserExceptionMessages.NotFound);

        if (user.Role != Roles.Guest)
        {
            throw new UserUnauthorizedException(UserExceptionMessages.UnauthorizedBooking);
        }

        var rooms = await _roomRepository.GetAllByIdAsync(request.RoomIds);
        if (rooms == null || rooms.Count() != request.RoomIds.Count())
        {
            throw new NotFoundException(RoomExceptionMessages.NotFound);
        }

        if (rooms.Any(r => r.RoomClass.HotelId != request.HotelId))
        {
            throw new RoomNotBelongToHotelException(RoomExceptionMessages.NotBelongToHotel);
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var booking = new Booking
            {
                BookingDate = DateTime.UtcNow,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                PaymentMethod = request.PaymentMethod,
                Remarks = request.Remarks,
                UserId = request.UserId,
                Rooms = rooms.ToList(),
                User = user,
                Invoice = new Invoice
                {
                    IssueDate = DateTime.UtcNow,
                    PaymentStatus = PaymentStatus.Completed,
                    TotalPrice = CalculateTotalPrice(rooms, request.CheckInDate, request.CheckOutDate),
                }
            };
            await _bookingRepository.CreateAsync(booking);

            var invoiceHtml = _invoiceHtmlGenerationService.GenerateHtml(booking);
            var invoicePdf = await _pdfService.GeneratePdfAsync(invoiceHtml);

            var emailAttachment = new EmailAttachment
            {
                FileName = "Invoice.pdf",
                FileContent = invoicePdf,
                ContentType = "application/pdf"
            };

            await _emailSenderService.SendEmailAsync(user.Email, "Your Booking Invoice", invoiceHtml, [emailAttachment]);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<BookingResponse>(booking);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private static double CalculateTotalPrice(IEnumerable<Room> rooms, DateTime checkInDate, DateTime checkOutDate)
    {
        var currentDate = DateTime.UtcNow;

        var total = rooms.Sum(room =>
        {
            var price = room.RoomClass.Price;
            var discount = room.RoomClass.Discounts
                               .Where(d => d.StartDate <= currentDate && d.EndDate > currentDate)
                               .Select(d => d.Percentage)
                               .DefaultIfEmpty(0)
                               .Max();

            var discoutedPrice = price * (1 - discount / 100);
            return discoutedPrice;
        });

        var stayDuration = (checkOutDate - checkInDate).Days;
        return stayDuration * total;
    }
}
