using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService.Application.Interfaces
{
    public interface IHotelApiClient
    {
        Task<bool> HotelExistsAsync(int hotelId);
    }
}
