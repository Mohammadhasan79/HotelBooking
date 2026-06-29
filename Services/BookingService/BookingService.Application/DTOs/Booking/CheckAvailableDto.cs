using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Application.DTOs.Booking
{
    public class CheckAvailableDto
    {
        public List<int> RoomId { get; set; } = [];
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
