using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Domain.Entities;

namespace BookingService.Application.RepositoryInterface
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task<List<Booking>> GetAllAsync();
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetByUserIdAsync(string userId);
        Task<List<Booking>> GetByRoomIdAsync(int roomId);
        Task<List<Booking>> GetBookingByIdList(List<int> roomId);
        Task SaveChangesAsync();
    }
}
