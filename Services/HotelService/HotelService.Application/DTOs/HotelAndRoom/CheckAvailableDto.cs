
using System;
using System.Collections.Generic;

namespace HotelService.Application.DTOs.HotelAndRoom
{
    public class CheckAvailableRequestDto
    {
        public List<int> RoomId { get; set; } = new();
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}