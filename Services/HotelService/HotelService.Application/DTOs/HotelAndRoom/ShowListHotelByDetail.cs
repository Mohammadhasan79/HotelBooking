using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Application.DTOs.HotelAndRoom
{
    public class ShowListHotelByDetail
    {
        public int HotelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<RoomDto> Rooms { get; set; } = [];
    }
}
