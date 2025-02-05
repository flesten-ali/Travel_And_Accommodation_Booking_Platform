using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TABP.Presentation.DTOs.Amenity;

namespace TABP.Presentation.Validators.Amenity;
public class AddAmenityRequestValidator : AbstractValidator<AddAmenityRequest>
{
    public AddAmenityRequestValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
    }
}
