using FluentValidation;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Validators.Hotel;

public class CreateHotelRequestValidator : AbstractValidator<CreateHotelRequest>
{
    public CreateHotelRequestValidator()
    {
        RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Hotel name is required.")
              .MaximumLength(100).WithMessage("Hotel name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Description cannot exceed 300 characters.");

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 5).WithMessage("Rate must be between 1 and 5.");

        RuleFor(x => x.LongitudeCoordinates)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180 degrees.");

        RuleFor(x => x.LatitudeCoordinates)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90 degrees.");

        RuleFor(x => x.CityId)
            .NotEmpty().WithMessage("CityId is required.");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");
    }
}
