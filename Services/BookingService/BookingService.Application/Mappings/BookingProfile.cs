using AutoMapper;
using BookingService.Application.DTOs.Booking;
using BookingService.Domain.Entities;

namespace BookingService.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, BookingDto>();
        }
    }
}
