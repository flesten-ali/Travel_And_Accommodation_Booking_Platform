using FluentValidation;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Validators.CartItem;
public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
{
    public AddToCartRequestValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(c => c.RoomClassId)
         .NotEmpty().WithMessage("RoomClassId is required.");
    }
}