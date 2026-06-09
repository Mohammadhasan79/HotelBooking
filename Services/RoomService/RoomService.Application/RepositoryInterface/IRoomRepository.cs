using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomService.Domain.Entity;

namespace RoomService.Application.RepositoryInterface
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        void Update(Room room);
        void Delete(Room room);
        Task<List<Room>> GetAllAsync();
        Task<List<Room>> GetByHotelIdAsync(int id);
        Task<Room?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
