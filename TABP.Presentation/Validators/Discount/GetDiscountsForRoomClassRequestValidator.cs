using FluentValidation;
using TABP.Presentation.DTOs.Discount;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Discount;
public class GetDiscountsForRoomClassRequestValidator : AbstractValidator<GetDiscountsForRoomClassRequest>
{
    public GetDiscountsForRoomClassRequestValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}