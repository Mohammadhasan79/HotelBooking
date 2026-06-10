using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Application.Common;
using BookingService.Application.DTOs.Booking;

namespace BookingService.Application.ServiceInterface
{
    public interface IBookingService
    {
        Task<Result<BookingDto>> CreateAsync(string userId, CreateBookingDto dto);
        Task<Result<List<BookingDto>>> GetAllAsync();
        Task<Result<BookingDto?>> GetByIdAsync(int id);
        Task<Result<List<BookingDto>>> GetMyBookingsAsync(string userId);
        Task<Result> CancelAsync(int bookingId,string userId);
    }
}
