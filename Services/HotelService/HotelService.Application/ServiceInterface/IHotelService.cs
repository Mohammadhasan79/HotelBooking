using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.Common;
using HotelService.Application.DTOs.Hotel;

namespace HotelService.Application.ServiceInterface
{
    public interface IHotelService
    {
        Task<Result<HotelDto>> CreateAsync(CreateHotelDto dto);
        Task<Result<List<HotelDto>>> GetAllAsync();
        Task<Result<HotelDto>> GetByIdAsync(int id);
    }
}
