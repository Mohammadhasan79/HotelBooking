using FluentValidation;
using IdentityService.Application.DTOs.Auth;

namespace IdentityService.Application.Validators;

public class LoginRequestDtoValidator
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}