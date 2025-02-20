using FluentValidation;
using TABP.Presentation.DTOs.RoomClass;
using TABP.Presentation.Validators.Common;

namespace TABP.Presentation.Validators.RoomClass;
public class UploadRoomClassImageGalleryRequestValidator : AbstractValidator<UploadRoomClassImageGalleryRequest>
{
    public UploadRoomClassImageGalleryRequestValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new ImageFileValidator());
    }
}
