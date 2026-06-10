using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Application.DTOs.Booking;
using FluentValidation;

namespace BookingService.Application.Validator
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookDtoValidator() 
        {
            RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("RoomId is required");

            RuleFor(x => x.CheckInDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("CheckInDate cannot be in the past");

            RuleFor(x => x.CheckOutDate)
                .GreaterThan(x => x.CheckInDate)
                .WithMessage("CheckOutDate must be after CheckInDate");
        }
    }
}
