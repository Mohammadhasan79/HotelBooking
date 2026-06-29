using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelService.Application.ApiClientInterface
{
    public interface IBookingApiClient
    {
        Task<List<int>> CheckAvailableRoom(List<int> roomIds, DateTime checkInTime, DateTime checkOutTime);
    }
}