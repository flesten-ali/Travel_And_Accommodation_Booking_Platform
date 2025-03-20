using FluentValidation;
using TABP.Presentation.DTOs.Hotel;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Hotel;
public class GetHotelRoomClassesRequestValidator : AbstractValidator<GetHotelRoomClassesRequest>
{
    public GetHotelRoomClassesRequestValidator()
    {
        RuleFor(r => r.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}