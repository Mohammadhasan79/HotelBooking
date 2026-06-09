using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomService.Application.Common;
using RoomService.Application.DTOs;

namespace RoomService.Application.ServiceInterface
{
    public interface IRoomService
    {
        Task<Result<RoomDto>> AddAsync(CreateRoomDto Dto);
        Task<Result<RoomDto>> UpdateAsync(int id,UpdateRoomDto Dto);
        Task<Result<RoomDto>> DeleteAsync(int id);
        Task<Result<List<RoomDto>>> GetAllAsync();
        Task<Result<List<RoomDto>>> GetByHotelIdAsync(int hotelId);
        Task<Result<RoomDto>> GetByIdAsync(int id);
    }
}
