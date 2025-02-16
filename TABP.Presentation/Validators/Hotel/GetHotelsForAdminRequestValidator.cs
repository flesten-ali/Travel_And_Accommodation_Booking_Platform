using FluentValidation;
using TABP.Presentation.DTOs.Hotel;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Hotel;
public class GetHotelsForAdminRequestValidator : AbstractValidator<GetHotelsForAdminRequest>
{
    public GetHotelsForAdminRequestValidator()
    {
        RuleFor(r => r.PaginationParameters)
                .SetValidator(new PaginationParametersValidator());
    }
}
