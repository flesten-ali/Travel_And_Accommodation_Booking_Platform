using FluentValidation;
using TABP.Presentation.DTOs.Room;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.Room;
public class GetRoomsForAdminRequestValidator : AbstractValidator<GetRoomsForAdminRequest>
{
    public GetRoomsForAdminRequestValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .SetValidator(new PaginationParametersValidator());
    }
}
