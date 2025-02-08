using FluentValidation;
using TABP.Presentation.DTOs;
using TABP.Presentation.Validators.Hotel.Common;

namespace TABP.Presentation.Validators.Hotel;
public class AddThumbnailRequestValidator : AbstractValidator<AddThumbnailRequest>
{
    public AddThumbnailRequestValidator()
    {
        RuleFor(x => x.HotelId)
             .NotEmpty().WithMessage("HotelId is required.");

        RuleFor(x => x.Thumbnail)
            .NotNull().WithMessage("Thumbnail file is required.")
            .SetValidator(new ImageFileValidator());
    }
}
