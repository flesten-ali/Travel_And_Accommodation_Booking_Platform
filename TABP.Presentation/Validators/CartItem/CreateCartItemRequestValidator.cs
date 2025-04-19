using FluentValidation;
using TABP.Presentation.DTOs.CartItem;

namespace TABP.Presentation.Validators.CartItem;
public class CreateCartItemRequestValidator : AbstractValidator<CreateCartItemRequest>
{
    public CreateCartItemRequestValidator()
    {
        RuleFor(x => x.RoomClassId)
            .NotEmpty().WithMessage("RoomClassId is required.");
    }
}
