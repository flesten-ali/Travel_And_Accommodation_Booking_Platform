using FluentValidation;
using TABP.Presentation.DTOs.Amenity;

namespace TABP.Presentation.Validators.Amenity;
public class AddAmenityRequestValidator : AbstractValidator<AddAmenityRequest>
{
    public AddAmenityRequestValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Amenity name is required.")
                .MaximumLength(100).WithMessage("Amenity name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");
    }
}
