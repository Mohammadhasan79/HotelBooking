using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Application.ServiceInterface
{
    public interface IRoomApiClient
    {
        Task<bool> RoomExistAsync(int roomId);
        Task<decimal> GetRoomPriceAsync(int roomId);
    }
}
