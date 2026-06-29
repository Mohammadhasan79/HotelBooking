using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService.Application.DTOs
{
    public class CreateRoomDto
    {
        public int HotelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public decimal PricePernight { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
