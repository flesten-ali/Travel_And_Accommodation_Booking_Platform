using FluentValidation;
using TABP.Presentation.DTOs.Auth;

namespace TABP.Presentation.Validations.Auth;
public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(u => u.Email)
          .NotEmpty()
          .EmailAddress()
          .WithMessage("Email address is required!");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("password is required!");

    }
}
