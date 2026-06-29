using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Application.DTOs.HotelAndRoom
{
    public class SearchByCityAndDateDto
    {
        public string City { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
