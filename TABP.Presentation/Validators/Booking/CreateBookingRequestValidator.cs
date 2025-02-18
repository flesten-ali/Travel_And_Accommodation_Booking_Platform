using FluentValidation;
using TABP.Presentation.DTOs.Booking;

namespace TABP.Presentation.Validators.Booking;
public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(b => b.HotelId)
           .NotNull().WithMessage("Hotel ID is required.");

        RuleFor(b => b.UserId)
            .NotNull().WithMessage("User ID is required.");

        RuleFor(b => b.RoomIds)
            .NotEmpty().WithMessage("At least one room must be selected.")
            .Must(ids => ids.Distinct().Count() == ids.Count())
            .WithMessage("RoomIds list contains duplicate values.")
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage("RoomIds must contain valid GUIDs.");

        RuleFor(x => x.CheckInDate)
            .NotEmpty().WithMessage("Check-in date is required. Format: MM-dd-yyyy.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Check-in date cannot be in the past.")
            .LessThan(x => x.CheckOutDate)
            .WithMessage("Check-in date must be before the check-out date.");

        RuleFor(x => x.CheckOutDate)
            .NotEmpty().WithMessage("Check-out date is required. Format: MM-dd-yyyy.")
            .GreaterThan(x => x.CheckInDate)
            .WithMessage("Check-out date must be after the check-in date.");

        RuleFor(b => b.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment  method.");

        RuleFor(b => b.Remarks)
            .MaximumLength(300).WithMessage("Remarks cannot exceed 300 characters.");
    }
}
