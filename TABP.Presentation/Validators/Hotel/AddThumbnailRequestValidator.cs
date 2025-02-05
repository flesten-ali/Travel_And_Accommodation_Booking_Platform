using FluentValidation;
using TABP.Presentation.DTOs;

namespace TABP.Presentation.Validators.Hotel;
public class AddThumbnailRequestValidator : AbstractValidator<AddThumbnailRequest>
{
    public AddThumbnailRequestValidator()
    {
        RuleFor(x => x.Thumbnail).NotEmpty().WithMessage("Thumbnail image is required");
        RuleFor(x => x.HotelId).NotEmpty().WithMessage("Hotel id is required");
    }
}
