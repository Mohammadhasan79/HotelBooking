using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomService.Application.RepositoryInterface;
using RoomService.Domain.Entity;
using RoomService.Infrastructure.Persistence;

namespace RoomService.Infrastructure.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomDbContext _context;
        
        public RoomRepository(RoomDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }
        public void Update(Room room)
        {
             _context.Rooms.Update(room);
        }
        public void Delete(Room room)
        {
            _context.Rooms.Remove(room);
        }
        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
        public async Task<List<Room>> GetByHotelIdAsync(int id)
        {
            return await _context.Rooms.Where(a => a.HotelId == id).ToListAsync();
        }
        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
