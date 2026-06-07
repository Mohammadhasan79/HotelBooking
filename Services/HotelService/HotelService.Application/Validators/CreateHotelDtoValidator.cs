using FluentValidation;
using HotelService.Application.DTOs.Hotel;

namespace HotelService.Application.Validators;

public class CreateHotelDtoValidator
    : AbstractValidator<CreateHotelDto>
{
    public CreateHotelDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);
    }
}