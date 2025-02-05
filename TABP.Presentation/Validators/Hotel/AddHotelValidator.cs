using FluentValidation;
using TABP.Presentation.DTOs.Hotel;
namespace TABP.Presentation.Validators.Hotels;

public class AddHotelValidator : AbstractValidator<AddHotelRequest>
{
    public AddHotelValidator()
    {
        RuleFor(h => h.Name).NotEmpty();
        RuleFor(h => h.OwnerId).NotEmpty();
        RuleFor(h => h.CityId).NotEmpty();
        RuleFor(h => h.LongitudeCoordinates).NotEmpty();
        RuleFor(h => h.LatitudeCoordinates).NotEmpty();
    }
}
