using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.Common;
using HotelService.Domain.Entities;

namespace HotelService.Application.Interfaces
{
    public interface IHotelRepository
    {
        Task AddAsync(Hotel hotel);
        Task<List<Hotel>> GetAllAsync();
        Task<Hotel> GetByIdAsync(int id);
        Task SaveChangesAsync();    
    }
}
