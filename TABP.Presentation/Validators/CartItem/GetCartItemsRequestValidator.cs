using FluentValidation;
using TABP.Presentation.DTOs.CartItem;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.CartItem;
public class GetCartItemsRequestValidator :AbstractValidator<GetCartItemsRequest>
{
    public GetCartItemsRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x=>x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}
