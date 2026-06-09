using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService.Domain.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal PricePernight { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
