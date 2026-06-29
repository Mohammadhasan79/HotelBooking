using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Application.RepositoryInterface;
using BookingService.Domain.Entities;
using BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;
        public BookingRepository(BookingDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }
        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings.ToListAsync();
        }
        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<Booking>> GetByRoomIdAsync(int roomId)
        {
            return await _context.Bookings.Where(a => a.RoomId == roomId).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetByUserIdAsync(string userId)
        {
            return await _context.Bookings.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task<List<Booking>> GetBookingByIdList(List<int> roomId)
        {
            return await _context.Bookings.Where(b => roomId.Contains(b.RoomId)).ToListAsync();
        }
    }
}
