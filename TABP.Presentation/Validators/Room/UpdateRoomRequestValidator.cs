using FluentValidation;
using TABP.Presentation.DTOs.Room;

namespace TABP.Presentation.Validators.Room;
public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomRequestValidator()
    {
        RuleFor(x => x.RoomNumber)
           .NotEmpty().WithMessage("Room number is required.")
           .GreaterThan(0).WithMessage("Room number must be greater than zero.");

        RuleFor(x => x.Floor)
         .NotEmpty().WithMessage("Floor is required.")
         .GreaterThanOrEqualTo(0).WithMessage("Floor cannot be negative.");
    }
}
