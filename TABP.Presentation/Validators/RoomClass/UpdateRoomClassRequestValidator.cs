using FluentValidation;
using TABP.Presentation.DTOs.RoomClass;

namespace TABP.Presentation.Validators.RoomClass;
public class UpdateRoomClassRequestValidator : AbstractValidator<UpdateRoomClassRequest>
{
    public UpdateRoomClassRequestValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Room-class name is required.")
           .MaximumLength(100).WithMessage("Room-class name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

        RuleFor(x => x.ChildrenCapacity)
            .GreaterThanOrEqualTo(0).WithMessage("Children capacity cannot be negative.");

        RuleFor(x => x.AdultsCapacity)
            .GreaterThan(0).WithMessage("Adults capacity must be at least 1.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(b => b.RoomType)
            .NotNull().WithMessage("Room type os required.")
            .IsInEnum().WithMessage("Invalid room type.");

        RuleFor(x => x.AmenityIds)
           .Must(ids => ids.Distinct().Count() == ids.Count())
           .WithMessage("AmenityIds list contains duplicate values.")
           .Must(ids => ids.All(id => id != Guid.Empty))
           .WithMessage("AmenityIds must contain valid GUIDs.");
    }
}
