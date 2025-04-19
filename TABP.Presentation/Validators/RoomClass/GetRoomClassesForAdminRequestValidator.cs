using FluentValidation;
using TABP.Presentation.DTOs.RoomClass;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.RoomClass;
public class GetRoomClassesForAdminRequestValidator : AbstractValidator<GetRoomClassesForAdminRequest>
{
    public GetRoomClassesForAdminRequestValidator()
    {
        RuleFor(r => r.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}
