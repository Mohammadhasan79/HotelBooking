using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.DTOs.HotelAndRoom;

namespace HotelService.Application.ApiClientInterface
{
    public interface IRoomApiClient
    {
        Task<List<RoomDto>> GetRooms(List<int> hotelIds);
    }
}
